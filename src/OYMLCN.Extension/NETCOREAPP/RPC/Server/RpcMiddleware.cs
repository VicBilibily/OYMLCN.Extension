using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OYMLCN.Extensions;
using OYMLCN.RPC.Core;
using OYMLCN.RPC.Core.RpcBuilder;
using System;
using System.Collections.Concurrent;
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
    /// 过程调用中间件
    /// </summary>
    public class RpcMiddleware
    {
        private readonly Stopwatch _stopwatch;
        private readonly ILogger _logger;

        private readonly RequestDelegate _next;
        private readonly RpcServerOptions _options;
        private readonly IDictionary<string, Type[]> _interface;
        private readonly IDictionary<string, Type> _types;
        private readonly IEnumerable<Type> _filterTypes;
        private readonly ConcurrentDictionary<string, List<RpcFilterAttribute>> _methodFilters = new ConcurrentDictionary<string, List<RpcFilterAttribute>>();

        /// <summary>
        /// 过程调用中间件
        /// </summary>
        public RpcMiddleware(RequestDelegate next, RpcServerOptions rpcServerOptions, ILoggerFactory loggerFactory)
        {
            _stopwatch = new Stopwatch();
            _logger = loggerFactory.CreateLogger<RpcMiddleware>();

            _next = next;
            _options = rpcServerOptions;
            _interface = rpcServerOptions.GetInterfaceTypes();
            _types = rpcServerOptions.GetTypes();
            _filterTypes = rpcServerOptions.GetFilterTypes();
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            bool isRootPath = context.Request.Path == "/";
            bool isMatchUrl = context.Request.Path.Equals(_options.RpcUrl, StringComparison.OrdinalIgnoreCase);
            // 不匹配入口路径时直接交由后续管道处理
            if (isMatchUrl == false)
            {
                await _next.Invoke(context);
                return;
            }

            // 一进入就开始计时
            _stopwatch.Restart();
            #region 一进来就开启同步IO，以读取Body内容
            var syncIOFeature = context.Features.Get<IHttpBodyControlFeature>();
            if (syncIOFeature != null)
                syncIOFeature.AllowSynchronousIO = true;
            #endregion

            #region 读取请求JSON数据并进行基础判断
            using var requestReader = new StreamReader(context.Request.Body);
            var requestContent = requestReader.ReadToEnd();
            RequestModel requestModel = null;

            ResponseModel responseModel = new ResponseModel { Code = -1 };
            // 过程调用通讯格式均为 JSON，一进来就固定返回内容类型为 JSON
            context.Response.ContentType = "application/json;charset=utf-8";

            if (HttpMethods.IsGet(context.Request.Method) && !isRootPath && isMatchUrl)
                responseModel.Message = "过程调用只接受POST请求调用";
            else
            {
                // 鬼知道前端会 POST 什么鬼东西进来，反序列化失败时就报格式错误
                try
                {
                    requestModel = requestContent.FromJson<RequestModel>();
                    // 反序列化能通过，但没内容时就返回示例 JSON 格式
                    if (requestContent.IsNullOrEmpty() || requestModel == null)
                        responseModel.Message = "读取请求调用数据失败，调用内容应为JSON格式的数据：{type:调用目标路径,method:调用方法名称,params:[调用参数集合]}";
                    // 检查调用目标是否为空
                    else if (requestModel.Target.IsNullOrWhiteSpace())
                        responseModel.Message = $"未指定调用目标";
                    // 检查调用方法名称是否为空
                    else if (requestModel.Action.IsNullOrWhiteSpace())
                        responseModel.Message = $"未指定调用方法";
                }
                catch
                {
                    responseModel.Message = "读取请求调用数据失败，请检查POST请求的JSON格式";
                    return;
                }
            }
            #endregion

            // 初步判断不符合过程调用数据条件，返回错误信息
            // 如果与指定路径不匹配则交由后续管道处理
            if (responseModel.Message.IsNotNullOrEmpty())
            {
                if (!isRootPath && isMatchUrl)
                    await context.Response.WriteAsync(responseModel.ToJson(), Encoding.UTF8);
                else await _next.Invoke(context);
                _logger.LogDebug("过程调用参数检查未通过：{0}", responseModel.Message);
                return;
            }

            // 将调用方法的 . 替换为 _ 
            requestModel.Action = requestModel.Action.Replace('.', '_');

            await HandleRequest(context, responseModel, requestModel);
            return;
        }

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <returns></returns>
        private async Task HandleRequest(HttpContext context, ResponseModel responseModel, RequestModel requestModel)
        {
            #region 通过请求参数找到要调用的目标类型
            Type targetType = null;
            // 如果没有指定接口，则向找一遍是否能够找到，找不到再找接口
            if (requestModel.Interface.IsNullOrWhiteSpace())
            {
                targetType = _types.Where(v => v.Key.EqualsIgnoreCase(requestModel.Target)).Select(v => v.Value).FirstOrDefault();
                if (targetType == null)
                    requestModel.Interface = _options.DefaultInterface;
            }
            if (targetType == null)
            {
                // 忽略大小写取已注册注入的接口
                var targets = _interface.Where(v => v.Key.EqualsIgnoreCase(requestModel.Interface)).Select(v => v.Value).FirstOrDefault();
                if (targets?.Any() ?? false)
                {
                    targetType = targets.Where(v => v.FullName.EqualsIgnoreCase(requestModel.Target)).FirstOrDefault();
                    if (targetType == null)
                    {
                        // 检查是否找到多个目标
                        targets = targets.Where(v => v.Name.EqualsIgnoreCase(requestModel.Target)).ToArray();
                        if (targets.Length > 1)
                            responseModel.Message = $"调用目标名称 {requestModel.Target} 指定不明确，当前已注册的目标有：{targets.Select(v => v.FullName).ToArray().Join("、")}";
                        else
                        {
                            targetType = targets.FirstOrDefault();
                            if (targetType == null)
                                responseModel.Message = $"在接口 {requestModel.Interface} 中未找到指定的目标名称 {requestModel.Target}";
                        }
                    }
                }
                else if (requestModel.Interface.IsNullOrWhiteSpace())
                    responseModel.Message = $"未指定调用目标接口或未设置默认接口名称";
                else
                    responseModel.Message = $"调用目标接口 {requestModel.Interface} 未注册";
            }
            if (responseModel.Message.IsNotNullOrEmpty())
            {
                await context.Response.WriteAsync(responseModel.ToJson(), Encoding.UTF8);
                _logger.LogDebug("未能匹配调用目标，调用接口：{0}，调用目标：{1}，查找结果：{2}", requestModel.Interface, requestModel.Target, responseModel.Message);
                return;
            }
            #endregion

            #region 初始化调用目标并使用依赖注入相关参数，然后找到要调用的方法
            var constructors = targetType.GetConstructors();
            if (constructors.Length > 1)
            {
                responseModel.Message = $"调用目标 {requestModel.Target} 包含多个构造函数，无法创建调用目标实例";
                await context.Response.WriteAsync(responseModel.ToJson(), Encoding.UTF8);
                return;
            }
            var constructor = constructors.First();
            var args = new List<object>();
            foreach (var param in constructor.GetParameters())
                args.Add(context.RequestServices.GetService(param.ParameterType));
            // 创建调用目标实例
            var instance = Activator.CreateInstance(targetType, args.ToArray());
            Utils.PropertieFromServicesInject(context.RequestServices, instance);
            var instanceType = instance.GetType();
            var method = instanceType.GetMethod(requestModel.Action);
            if (method == null) // 默认区分大小写，找不到再按不区分大小写找一遍
                method = instanceType.GetMethods().Where(v => v.Name.EqualsIgnoreCase(requestModel.Action)).FirstOrDefault();
            if (method == null)
            {
                responseModel.Message = $"调用目标方法 {requestModel.Action} 在 {targetType.FullName} 中未找到";
                await context.Response.WriteAsync(responseModel.ToJson(), Encoding.UTF8);
                return;
            }
            #endregion
            #region 初始化方法调用的参数
            var methodParamters = method.GetParameters();
            object GetStructInfoData() => new
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


            object[] paramters = new object[0];
            #region 传入的参数为数组时尝试过转换数据类型
            var requestParams = requestModel.Paramters as JArray;
            if (requestParams != null)
            {
                paramters = new object[requestParams.Count];
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
                            responseModel.Message = $"调用目标方法注入参数时发生类型转换错误，参数名称：{param.Name}，目标类型：{type.Name.FirstCharToLower()}";
                            responseModel.Data = GetStructInfoData();
                            await context.Response.WriteAsync(responseModel.ToJson(), Encoding.UTF8);
                            return;
                        }
                }
                // 判断必填的方法参数个数是否满足
                if (paramters.Length < methodParamters.Count(v => !v.HasDefaultValue))
                {
                    responseModel.Message = $"调用目标方法需要以下参数：{methodParamters.Select(v => v.HasDefaultValue ? $"[{v.Name}]" : v.Name).Join(",")}";
                    responseModel.Data = GetStructInfoData();
                    await context.Response.WriteAsync(responseModel.ToJson(), Encoding.UTF8);
                    return;
                }
                // 可选参数传入默认值
                if (paramters.Length < methodParamters.Length)
                {
                    var list = new List<object>();
                    list.AddRange(paramters);
                    int index = paramters.Length;
                    while (index < methodParamters.Length)
                        list.Add(methodParamters[index++].DefaultValue);
                    paramters = list.ToArray();
                }
                // 超出参数长度时尝试截断参数
                if (paramters.Length > methodParamters.Length)
                    paramters = paramters.Take(methodParamters.Length).ToArray();
            }
            #endregion
            #region 传入的参数为对象时尝试将对象的值转换为调用参数
            var requestObject = requestModel.Paramters as JObject;
            if (requestObject != null)
            {
                paramters = new object[methodParamters.Length];
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
            #endregion
            // 判断必填的方法参数个数是否满足
            if (paramters.Count() < methodParamters.Count(v => !v.HasDefaultValue))
            {
                responseModel.Message = $"调用目标方法需要以下参数：{methodParamters.Select(v => v.HasDefaultValue ? $"[{v.Name}]" : v.Name).Join(",")}";
                responseModel.Data = GetStructInfoData();
                await context.Response.WriteAsync(responseModel.ToJson(), Encoding.UTF8);
                return;
            }
            #endregion
            paramters.ForEach(param =>
            {
                var type = param.GetType();
                if (type.IsClass)
                    type.GetProperties().ForEach(p =>
                    {
                        var attr = p.GetAttribute<RpcPropertyAttribute>();
                        if (attr != null && attr.DefaultValue != null)
                            try { p.SetValue(param, attr.DefaultValue); } catch { }
                    });
            });

            RpcContext rpcContext = new RpcContext
            {
                Parameters = paramters,
                HttpContext = context,
                TargetType = instanceType,
                Method = method
            };
            TaskPiplineBuilder pipline = CreatPipleline(rpcContext);
            RpcRequestDelegate rpcRequestDelegate = pipline.Build(PiplineEndPoint(instance, rpcContext));
            try { await rpcRequestDelegate(rpcContext); }
            catch (Exception e)
            {
                responseModel.Message = context.RequestServices.IsDevelopment() ? e.InnerException?.StackTrace : e.InnerException?.Message;
                await context.Response.WriteAsync(responseModel.ToJson(), Encoding.UTF8);
                _logger.LogError("过程调用方法时发生未处理异常：{0}\r\n异常信息：{1}", e.InnerException?.Message, e.InnerException?.StackTrace);
                return;
            }
        }

        /// <summary>
        /// 创建任务执行管道
        /// </summary>
        private TaskPiplineBuilder CreatPipleline(RpcContext context)
        {
            TaskPiplineBuilder pipline = new TaskPiplineBuilder();
            //第一个中间件构建包装数据
            pipline.Use(async (rpcContext, next) =>
            {
                // 等待中间件过程调用返回数据
                await next(rpcContext);
                _stopwatch.Stop();
                // 使用新的数据对象返回调用结果
                ResponseModel responseModel = new ResponseModel
                {
                    Data = rpcContext.ReturnValue,
                    Code = 0,
                    Time = _stopwatch.ElapsedTicks / 10000d,
                };
                await context.HttpContext.Response.WriteAsync(responseModel.ToJson(), Encoding.UTF8);
                _logger.LogInformation("过程调用执行成功，调用目标：{1}，调用过程：{1}，执行耗时：{2}ms",
                    rpcContext.TargetType.FullName,
                    rpcContext.Method.Name,
                    responseModel.Time
                );
            });
            List<RpcFilterAttribute> interceptorAttributes = GetFilterAttributes(context);
            if (interceptorAttributes.Any())
                foreach (var item in interceptorAttributes)
                    pipline.Use(item.InvokeAsync);
            return pipline;
        }

        /// <summary>
        /// 过程调用管道终结点
        /// </summary>
        private static RpcRequestDelegate PiplineEndPoint(object instance, RpcContext rpcContext)
        {
            return rpcContext =>
            {
                try
                {
                    var returnValue = rpcContext.Method.Invoke(instance, rpcContext.Parameters);
                    if (returnValue != null)
                    {
                        var returnValueType = returnValue.GetType();
                        if (typeof(Task).IsAssignableFrom(returnValueType))
                        {
                            var resultProperty = returnValueType.GetProperty("Result");
                            rpcContext.ReturnValue = resultProperty.GetValue(returnValue);
                            return Task.CompletedTask;
                        }
                        rpcContext.ReturnValue = returnValue;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("过程调用发生未知错误", e.InnerException);
                }
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// 获取Attribute
        /// </summary>
        private List<RpcFilterAttribute> GetFilterAttributes(RpcContext rpcContext)
        {
            var methondInfo = rpcContext.Method;
            var methondInterceptorAttributes = _methodFilters.GetOrAdd($"{methondInfo.DeclaringType.FullName}#{methondInfo.Name}",
                key =>
                {
                    var methondAttributes = methondInfo.GetCustomAttributes(true)
                        .Where(i => typeof(RpcFilterAttribute).IsAssignableFrom(i.GetType()))
                        .Cast<RpcFilterAttribute>().ToList();
                    var classAttributes = methondInfo.DeclaringType.GetCustomAttributes(true)
                        .Where(i => typeof(RpcFilterAttribute).IsAssignableFrom(i.GetType()))
                        .Cast<RpcFilterAttribute>();
                    methondAttributes.AddRange(classAttributes);
                    var glableInterceptorAttribute = Utils.GetRpcFilterInstances(rpcContext.HttpContext.RequestServices, _filterTypes);
                    methondAttributes.AddRange(glableInterceptorAttribute);
                    return methondAttributes;
                });
            Utils.PropertiesFromServicesInject(rpcContext.HttpContext.RequestServices, methondInterceptorAttributes);
            return methondInterceptorAttributes;
        }
    }

}
