using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.RPC.Server
{
    /// <summary>
    /// 过程调用中间件
    /// </summary>
    public class RpcMiddleware
    {
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
            #region 一进来就开启同步IO，以读取Body内容
            var syncIOFeature = context.Features.Get<IHttpBodyControlFeature>();
            if (syncIOFeature != null)
                syncIOFeature.AllowSynchronousIO = true;
            #endregion

            var rpcHelper = context.RequestServices.GetRequiredService(_options.RpcHelperType) as RpcHelper;
            var rpcContext = rpcHelper.RpcContext = new RpcContext() { HttpContext = context };
            rpcContext.Stopwatch.Start();

            bool isRootPath = context.Request.Path == "/";
            bool isMatchUrl = context.Request.Path.StartsWithSegments(_options.RpcUrl, StringComparison.OrdinalIgnoreCase);
            rpcHelper.IsMatchRpcUrl = !isRootPath && isMatchUrl;
            // 不匹配入口路径时直接交由后续管道处理
            if (isMatchUrl == false)
            {
                await _next.Invoke(context);
                rpcContext.Stopwatch.Stop();
                return;
            }

            // 初步判断不符合过程调用数据条件，返回错误信息
            // 如果与指定路径不匹配则交由后续管道处理
            if (!rpcHelper.RpcRequestCheck())
            {
                rpcContext.Stopwatch.Stop();
                var rpcResponse = rpcHelper.RpcResponse;
                if (!isRootPath && isMatchUrl)
                    await rpcHelper.WriteRpcResponseAsync();
                else await _next.Invoke(context);
                _logger.LogDebug("过程调用参数检查未通过：{0}", rpcResponse.Message);
                return;
            }

            await HandleRequest(rpcHelper);
            return;
        }

        /// <summary>
        /// 处理请求
        /// </summary>
        /// <returns></returns>
        private async Task HandleRequest(RpcHelper rpcHelper)
        {
            if (!rpcHelper.RpcCheckTarget())
            {
                await rpcHelper.WriteRpcResponseAsync();
                _logger.LogDebug("未能匹配调用目标，调用接口：{0}，调用目标：{1}，查找结果：{2}", rpcHelper.RpcRequest.Interface, rpcHelper.RpcRequest.Target, rpcHelper.RpcResponse.Message);
                return;
            }
            if (!rpcHelper.RpcCheckAction() || !rpcHelper.RpcInitParameters())
            {
                await rpcHelper.WriteRpcResponseAsync();
                return;
            }

            var rpcContext = rpcHelper.RpcContext;
            // 创建调用目标实例
            var constructors = rpcContext.TargetType.GetConstructors();
            var constructor = constructors.First();
            var args = new List<object>();
            foreach (var param in constructor.GetParameters())
                args.Add(rpcHelper.RequestServices.GetService(param.ParameterType));
            var instance = Activator.CreateInstance(rpcContext.TargetType, args.ToArray());
            Utils.PropertieFromServicesInject(rpcHelper.RequestServices, instance);
            // 创建远程调用任务处理管道
            TaskPiplineBuilder pipline = CreatPipleline(rpcContext);
            RpcRequestDelegate rpcRequestDelegate = pipline.Build(PiplineEndPoint(instance, rpcContext));
            try { await rpcRequestDelegate(rpcContext); }
            catch (Exception e)
            {
                rpcHelper.RpcResponse.Message = rpcHelper.RequestServices.IsDevelopment() ? e.InnerException?.Message + e.InnerException?.StackTrace : e.InnerException?.Message;
                await rpcHelper.WriteRpcResponseAsync();
                _logger.LogError(e.InnerException ?? e, "过程调用方法时发生未处理异常：{0}\r\n异常信息：{1}", e.InnerException?.Message + e.InnerException?.Message, e.InnerException?.StackTrace);
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
                rpcContext.Stopwatch.Stop();

                object returnData = rpcContext.ReturnValue;
                // 如果返回的类型为元组，则将元组项目转换为数组结果返回
                if (returnData != null && returnData.GetType().GetInterfaces().Contains(typeof(ITuple)))
                {
                    var data = returnData as ITuple;
                    var obj = new object[data.Length];
                    for (var i = 0; i < data.Length; i++)
                        obj[i] = data[i];
                    returnData = obj;
                }
                // 使用新的数据对象返回调用结果
                ResponseModel responseModel = new ResponseModel
                {
                    Data = returnData,
                    Code = 0,
                    Time = rpcContext.Stopwatch.ElapsedTicks / 10000d,
                };
                context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
                await context.HttpContext.Response.WriteAsync(responseModel.ToJson(), Encoding.UTF8);
                _logger.LogInformation("过程调用成功，调用目标：{0}，调用过程：{1}，执行耗时：{2}ms",
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
                    throw new Exception("过程调用发生未知错误", e.InnerException ?? e);
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
