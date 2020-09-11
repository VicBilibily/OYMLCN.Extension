using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OYMLCN.Extensions;
using OYMLCN.RPC.Core;
using OYMLCN.RPC.Core.RpcBuilder;

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
            var rpcContext = rpcHelper.RpcContext = new RpcContext { HttpContext = context };
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
                if (!isRootPath && isMatchUrl)
                {
                    await rpcHelper.WriteRpcResponseAsync();
                    _logger.LogDebug("过程调用参数检查未通过：{0}", rpcHelper.RpcResponse?.Message);
                }
                else await _next.Invoke(context);
                return;
            }

            await HandleRequest(rpcHelper);
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
                args.Add(rpcHelper.ServiceProvider.GetService(param.ParameterType));
            var instance = Activator.CreateInstance(rpcContext.TargetType, args.ToArray());
            rpcHelper.PropertieFromServicesInject(instance);
            // 创建远程调用任务处理管道
            TaskPiplineBuilder pipline = CreatPipleline(rpcHelper);
            RpcRequestDelegate rpcRequestDelegate = pipline.Build(PiplineEndPoint(instance, rpcHelper));
            try
            {
                await rpcRequestDelegate(rpcContext);
                // 如果有中间件过滤器拦截提前返回响应，则不再处理
                if (!rpcContext.HttpContext.Response.HasStarted)
                {
                    await rpcHelper.WriteRpcResponseAsync();
                    _logger.LogInformation("过程调用成功，调用目标：{0}，调用过程：{1}，执行耗时：{2}ms",
                        rpcContext.TargetType.FullName,
                        rpcContext.Method.Name,
                        rpcHelper.RpcResponseTime
                    );
                }
            }
            catch (Exception e)
            {
                rpcHelper.RpcResponse.Message = rpcHelper.ServiceProvider.IsDevelopment() ? e.InnerException?.Message + e.InnerException?.StackTrace : e.InnerException?.Message;
                await rpcHelper.WriteRpcResponseAsync();
                _logger.LogError(e.InnerException ?? e, "过程调用方法时发生未处理异常：{0}\r\n异常信息：{1}", e.InnerException?.Message + e.InnerException?.Message, e.InnerException?.StackTrace);
            }
        }

        /// <summary>
        /// 创建任务执行管道
        /// </summary>
        private TaskPiplineBuilder CreatPipleline(RpcHelper rpcHelper)
        {
            TaskPiplineBuilder pipline = new TaskPiplineBuilder();
            //第一个中间件构建包装数据
            pipline.Use(async (rpcContext, next) => await next(rpcContext));
            List<RpcFilterAttribute> interceptorAttributes = GetFilterAttributes(rpcHelper);
            if (interceptorAttributes.Any())
                foreach (var item in interceptorAttributes)
                    pipline.Use(item.InvokeAsync);
            return pipline;
        }

        /// <summary>
        /// 过程调用管道终结点
        /// </summary>
        private static RpcRequestDelegate PiplineEndPoint(object instance, RpcHelper rpcHelper)
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
                            returnValue = resultProperty.GetValue(returnValue);
                            rpcHelper.RpcPiplineEndPointResultHandler(returnValue);
                            return Task.CompletedTask;
                        }
                        rpcHelper.RpcPiplineEndPointResultHandler(returnValue);
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
        private List<RpcFilterAttribute> GetFilterAttributes(RpcHelper rpcHelper)
        {
            var methondInfo = rpcHelper.RpcContext.Method;
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
                    var glableInterceptorAttribute = rpcHelper.GetRpcFilterInstances(_filterTypes);
                    methondAttributes.AddRange(glableInterceptorAttribute);
                    return methondAttributes;
                });
            rpcHelper.PropertiesFromServicesInject(methondInterceptorAttributes);
            return methondInterceptorAttributes;
        }
    }

}
