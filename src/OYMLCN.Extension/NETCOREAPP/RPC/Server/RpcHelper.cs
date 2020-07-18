using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OYMLCN.AspNetCore.TransferJob;
using OYMLCN.Extensions;
using OYMLCN.RPC.Core;
using OYMLCN.RPC.Core.RpcBuilder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.RPC.Server
{
    /// <summary>
    /// 远程调用辅助类
    /// </summary>
    public class RpcHelper
    {
        /// <summary>
        /// 注入服务提供器
        /// </summary>
        public IServiceProvider RequestServices { get; }
        /// <summary>
        /// 远程调用辅助类
        /// </summary>
        public RpcHelper(IServiceProvider requestServices)
        {
            this.RequestServices = requestServices;
        }

        private IWebHostEnvironment _hostEnvironment;
        private IConfiguration _configuration;
        private IMemoryCache _memoryCache;
        private IHttpContextAccessor _httpContextAccessor;
        private HttpContext _httpContext;

        private IBackgroundRunService _backgroundRunService;

        /// <summary>
        /// 主机环境信息
        /// </summary>
        public IWebHostEnvironment HostEnvironment
            => _hostEnvironment ??= RequestServices.GetRequiredService<IWebHostEnvironment>();
        /// <summary>
        /// 程序运行配置
        /// </summary>
        public IConfiguration Configuration
           => _configuration ??= RequestServices.GetRequiredService<IConfiguration>();
        /// <summary>
        /// 内存缓存实例
        /// </summary>
        public IMemoryCache MemoryCache
            => _memoryCache ??= RequestServices.GetRequiredService<IMemoryCache>();
        private IHttpContextAccessor HttpContextAccessor
            => _httpContextAccessor ??= RequestServices.GetRequiredService<IHttpContextAccessor>();
        /// <summary>
        /// 请求上下文信息
        /// </summary>
        public HttpContext HttpContext
            => _httpContext ??= HttpContextAccessor.HttpContext;

        /// <summary>
        /// 后台任务提供器
        /// </summary>
        public IBackgroundRunService BackgroundRunService
           => _backgroundRunService ??= RequestServices.GetRequiredService<IBackgroundRunService>();


        #region RPC处理辅助相关内容
        private RpcServerOptions _rpcServerOptions;
        /// <summary>
        /// RPC远程调用服务配置信息
        /// </summary>
        public RpcServerOptions RpcServerOptions => _rpcServerOptions ??= RequestServices.GetRequiredService<RpcServerOptions>();

        /// <summary>
        /// 是否通过匹配的RPC地址进入调用过程（对于使用根目录处理的会一直返回false）
        /// </summary>
        public bool IsMatchRpcUrl { get; internal set; }
        /// <summary>
        /// 处理上下文内容
        /// </summary>
        public RpcContext RpcContext { get; internal set; }
        /// <summary>
        /// 请求内容主体
        /// </summary>
        public RequestModel RpcRequest { get; internal set; }
        /// <summary>
        /// 响应内容主体
        /// </summary>
        public ResponseModel RpcResponse { get; set; }

        /// <summary>
        /// 将响应内容写入响应流
        /// </summary>
        internal async Task WriteRpcResponseAsync()
        {
            RpcContext.Stopwatch.Stop();
            RpcResponse.Time = RpcContext.Stopwatch.ElapsedTicks / 10000d;
            RpcContext.HttpContext.Response.ContentType = "application/json;charset=utf-8";
            await RpcContext.HttpContext.Response.WriteAsync(RpcResponse.ToJson(), Encoding.UTF8);
        }
        /// <summary>
        /// 读取请求JSON数据并进行基础判断
        /// </summary>
        internal bool RpcRequestCheck()
        {
            var httpRequest = HttpContext.Request;
            var requestReader = new StreamReader(httpRequest.Body);
            var requestContent = requestReader.ReadToEnd();

            RpcResponse = new ResponseModel { Code = -1 };
            // 过程调用通讯格式均为 JSON，一进来就固定返回内容类型为 JSON
            if (HttpMethods.IsGet(httpRequest.Method))
                RpcResponse.Message = "过程调用只接受POST请求调用";
            else
            {
                // 鬼知道前端会 POST 什么鬼东西进来，反序列化失败时就报格式错误
                try
                {
                    RpcRequest = requestContent.FromJson<RequestModel>();
                    // 反序列化能通过，但没内容时就返回示例 JSON 格式
                    if (requestContent.IsNullOrEmpty() || RpcRequest == null)
                        RpcResponse.Message = "读取请求调用数据失败，调用内容应为JSON格式的数据：{type:调用目标路径,method:调用方法名称,params:[调用参数集合]}";
                    // 检查调用目标是否为空
                    else if (RpcRequest.Target.IsNullOrWhiteSpace())
                        RpcResponse.Message = $"未指定调用目标";
                    // 检查调用方法名称是否为空
                    else if (RpcRequest.Action.IsNullOrWhiteSpace())
                        RpcResponse.Message = $"未指定调用方法";
                }
                catch
                {
                    RpcResponse.Message = "读取请求调用数据失败，请检查POST请求的JSON格式";
                }
            }
            if (!RpcResponse.Message.IsNullOrWhiteSpace())
                return false;
            // 将调用方法的 . 替换为 _ 
            RpcRequest.Action = RpcRequest.Action.Replace('.', '_');
            return true;
        }
        /// <summary>
        /// 检查远程调用的接口目标类型是否存在
        /// </summary>
        internal bool RpcCheckTarget()
        {
            var _interface = RpcServerOptions.GetInterfaceTypes();
            var _types = RpcServerOptions.GetTypes();
            Type targetType = null;
            // 如果没有指定接口，则向找一遍是否能够找到，找不到再找接口
            if (RpcRequest.Interface.IsNullOrWhiteSpace())
            {
                targetType = _types.Where(v => v.Key.EqualsIgnoreCase(RpcRequest.Target)).Select(v => v.Value).FirstOrDefault();
                if (targetType == null)
                    RpcRequest.Interface = RpcServerOptions.DefaultInterface;
            }
            if (targetType == null)
            {
                // 忽略大小写取已注册注入的接口
                Type[] targets = null;
                var interfaces = _interface.Where(v => v.Key.ToLower().EndsWith(RpcRequest.Interface.ToLower()));
                if (interfaces.Count() == 0)
                    RpcResponse.Message = $"调用目标接口 {RpcRequest.Interface} 未注册";
                else if (interfaces.Count() == 1)
                    targets = interfaces.Select(v => v.Value).First();
                else if (interfaces.Count() > 1)
                    RpcResponse.Message = $"调用目标接口 {RpcRequest.Interface} 指定不明确，已注册的接口有：{interfaces.Select(v => v.Key).ToArray().Join("、")}";
                targets ??= new Type[0];

                if (RpcResponse.Message.IsNullOrEmpty())
                {
                    if (targets.Any())
                    {
                        targetType = targets.Where(v => v.FullName.EqualsIgnoreCase(RpcRequest.Target)).FirstOrDefault();
                        if (targetType == null)
                        {
                            // 检查是否找到多个目标
                            targets = targets.Where(v => v.Name.EqualsIgnoreCase(RpcRequest.Target)).ToArray();
                            if (targets.Length > 1)
                                RpcResponse.Message = $"调用目标名称 {RpcRequest.Target} 指定不明确，当前已注册的目标有：{targets.Select(v => v.FullName).ToArray().Join("、")}";
                            else
                            {
                                targetType = targets.FirstOrDefault();
                                if (targetType == null)
                                    RpcResponse.Message = $"在接口 {RpcRequest.Interface} 中未找到指定的目标名称 {RpcRequest.Target}";
                            }
                        }
                    }
                    else if (RpcRequest.Interface.IsNullOrWhiteSpace())
                        RpcResponse.Message = $"未指定调用目标接口或未设置默认接口名称";
                    else if (targets.Length == 0)
                        RpcResponse.Message = $"调用目标接口 {RpcRequest.Interface} 无实现";
                }
            }
            RpcContext.TargetType = targetType;
            return targetType != null;
        }
        /// <summary>
        /// 检查远程调用的目标类型方法是否存在
        /// </summary>
        internal bool RpcCheckAction()
        {
            var constructors = RpcContext.TargetType.GetConstructors();
            if (constructors.Length > 1)
            {
                RpcResponse.Message = $"调用目标 {RpcRequest.Target} 包含多个构造函数，无法创建调用目标实例";
                return false;
            }
            var method = RpcContext.TargetType.GetMethod(RpcRequest.Action);
            if (method == null) // 默认区分大小写，找不到再按不区分大小写找一遍
                method = RpcContext.TargetType.GetMethods().Where(v => v.Name.EqualsIgnoreCase(RpcRequest.Action)).FirstOrDefault();
            if (method == null)
            {
                RpcResponse.Message = $"调用目标方法 {RpcRequest.Action} 在 {RpcContext.TargetType.FullName} 中未找到";
                return false;
            }
            RpcContext.Method = method;
            return true;
        }
        private object GetStructInfoData(ParameterInfo[] methodParamters)
        {
            return new
            {
                @params = methodParamters.ToDictionary(v => v.Name + (v.HasDefaultValue ? string.Empty : "*"), v =>
                {
                    if (v.ParameterType.IsValueType || v.ParameterType == typeof(string) || v.ParameterType.IsEnum)
                    {
                        if (v.HasDefaultValue)
                            return new
                            {
                                type = v.ParameterType.Name.FirstCharToLower(),
                                @default = v.DefaultValue
                            };
                        else
                            return (object)v.ParameterType.Name.FirstCharToLower();
                    }
                    else
                        return new
                        {
                            type = v.ParameterType.Name.CamelCaseToUnderline(),
                            @struct = v.ParameterType.GetProperties()
                                .Select(p =>
                                {
                                    var rpcPropertyAttribute = p.GetAttribute<RpcPropertyAttribute>();
                                    return new
                                    {
                                        p.Name,
                                        Type = p.PropertyType.Name.FirstCharToLower(),
                                        Require = rpcPropertyAttribute?.Require ?? false,
                                        rpcPropertyAttribute?.DefaultValue,
                                        rpcPropertyAttribute?.Message,
                                    };
                                })
                                .ToDictionary(
                                    p => p.Name.FirstCharToLower() + (p.Require ? "*" : string.Empty),
                                    p =>
                                    {
                                        var dict = new Dictionary<string, object>();
                                        dict["type"] = p.Type.CamelCaseToUnderline();
                                        if (p.DefaultValue != null) dict["default"] = p.DefaultValue;
                                        if (p.Message != null) dict["msg"] = p.Message;

                                        if (dict.Count == 1) return dict["type"];
                                        else return dict;
                                    })
                        };
                })
            };
        }
        /// <summary>
        /// 检查并修正调用目标参数，无法修正则返回false
        /// </summary>
        internal bool RpcInitParameters()
        {
            var method = RpcContext.Method;
            var methodParamters = method.GetParameters();

            RpcContext.Parameters = new object[0];
            if (!this.RpcInitArrayParameters(methodParamters)) return false;
            this.RpcFixObjectParameters(methodParamters);
            if (!this.RpcFixInvokeParameters(methodParamters)) return false;
            return true;
        }
        private bool RpcInitArrayParameters(ParameterInfo[] methodParamters)
        {
            var requestParams = RpcRequest.Paramters as JArray;
            if (requestParams != null)
            {
                var paramters = RpcContext.Parameters = new object[requestParams.Count];
                for (int i = 0; i < requestParams.Count; i++)
                {
                    if (i >= methodParamters.Length) break;
                    var param = methodParamters[i];
                    var type = param.ParameterType;
                    if (type.IsValueType || type == typeof(string) || type.IsEnum)
                        try { paramters[i] = Convert.ChangeType(requestParams[i], type); }
                        catch { paramters[i] = param.HasDefaultValue ? param.DefaultValue : default; }
                    else if (type.IsClass)
                        try { paramters[i] = requestParams[i].ToObject(type); }
                        catch
                        {
                            RpcResponse.Message = $"调用目标方法注入参数时发生类型转换错误，参数名称：{param.Name}，目标类型：{type.Name.FirstCharToLower()}";
                            RpcResponse.Data = GetStructInfoData(methodParamters);
                            return false;
                        }
                }
                // 判断必填的方法参数个数是否满足
                if (paramters.Length < methodParamters.Count(v => !v.HasDefaultValue))
                {
                    RpcResponse.Message = $"调用目标方法需要以下参数：{methodParamters.Select(v => v.HasDefaultValue ? $"[{v.Name}]" : v.Name).Join(",")}";
                    RpcResponse.Data = GetStructInfoData(methodParamters);
                    return false;
                }
                // 可选参数传入默认值
                if (paramters.Length < methodParamters.Length)
                {
                    var list = new List<object>();
                    list.AddRange(paramters);
                    int index = paramters.Length;
                    while (index < methodParamters.Length)
                        list.Add(methodParamters[index++].DefaultValue);
                    RpcContext.Parameters = paramters = list.ToArray();
                }
                // 超出参数长度时尝试截断参数
                if (paramters.Length > methodParamters.Length)
                    RpcContext.Parameters = paramters.Take(methodParamters.Length).ToArray();
            }
            return true;
        }
        private void RpcFixObjectParameters(ParameterInfo[] methodParamters)
        {
            var requestObject = RpcRequest.Paramters as JObject;
            if (requestObject != null)
            {
                var paramters = RpcContext.Parameters = new object[methodParamters.Length];
                for (var i = 0; i < methodParamters.Count(); i++)
                {
                    var param = methodParamters[i];
                    var type = param.ParameterType;
                    if (requestObject.TryGetValue(param.Name, StringComparison.OrdinalIgnoreCase, out JToken value))
                    {
                        if (type.IsClass)
                            try { paramters[i] = value.ToObject(type); }
                            catch { paramters[i] = default; }
                        else if (type.IsValueType || type == typeof(string) || type.IsEnum)
                            try { paramters[i] = Convert.ChangeType(value, type); }
                            catch { paramters[i] = param.HasDefaultValue ? param.DefaultValue : Activator.CreateInstance(type); }
                    }
                    else
                        paramters[i] = param.HasDefaultValue ? param.DefaultValue : default;
                }
                if (paramters.All(v => v == null))
                    for (var i = 0; i < methodParamters.Count(); i++)
                    {
                        var param = methodParamters[i];
                        var type = param.ParameterType;
                        if (paramters[i] == null)
                        {
                            if (type.IsClass)
                                try { paramters[i] = requestObject.ToObject(type); } catch { }
                            else if (type.IsValueType || type == typeof(string) || type.IsEnum)
                                try { paramters[i] = param.HasDefaultValue ? param.DefaultValue : Activator.CreateInstance(type); } catch { }
                        }
                    }
            }
        }
        private bool RpcFixInvokeParameters(ParameterInfo[] methodParamters)
        {
            var paramters = RpcContext.Parameters;
            // 判断必填的方法参数个数是否满足
            if (paramters.Count() < methodParamters.Count(v => !v.HasDefaultValue))
            {
                RpcResponse.Message = $"调用目标方法需要以下参数：{methodParamters.Select(v => v.HasDefaultValue ? $"[{v.Name}]" : v.Name).Join(",")}";
                RpcResponse.Data = GetStructInfoData(methodParamters);
                return false;
            }
            paramters.ForEach(param =>
            {
                if (param == null) return;
                var type = param.GetType();
                if (type.IsClass)
                    type.GetProperties().ForEach(p =>
                    {
                        var attr = p.GetAttribute<RpcPropertyAttribute>();
                        if (attr != null && attr.DefaultValue != null)
                            try { p.SetValue(param, attr.DefaultValue); } catch { }
                    });
            });
            return true;
        }
        #endregion


    }
}
