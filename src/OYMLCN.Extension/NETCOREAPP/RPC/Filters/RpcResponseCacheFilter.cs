﻿using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OYMLCN.Extensions;
using OYMLCN.RPC.Core;
using System;
using System.Threading.Tasks;

namespace OYMLCN.RPC.Server
{
    internal class RpcResponseCacheFilter : RpcFilterAttribute
    {
        [FromServices]
        public RpcHelper Helper { get; set; }
        [FromServices]
        public ILoggerFactory LoggerFactory { get; set; }

        private static IMemoryCache RspCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));

        public override async Task InvokeAsync(RpcContext context, RpcRequestDelegate next)
        {
            var cacheKey = Helper.GetRpcResponseCacheKey();
            if (cacheKey.IsNotNullOrEmpty() &&
                RspCache.TryGetValue(cacheKey, out ResponseModel rpcResult))
            {
                Helper.RpcResponse = rpcResult;
                await Helper.WriteRpcResponseAsync();
                var _logger = LoggerFactory.CreateLogger<RpcMiddleware>();
                _logger.LogDebug("过程调用缓存命中，调用目标：{0}，调用过程：{1}，执行耗时：{2}ms",
                    context.TargetType.FullName,
                    context.Method.Name,
                    Helper.RpcResponseTime
                );
            }
            else
            {
                await next(context);
                if (cacheKey.IsNotNullOrEmpty())
                {
                    var cacheTime = Helper.GetRpcResponseCacheTime();
                    if (cacheTime > 0 && Helper.RpcResponseCode == 0)
                        RspCache.Set(cacheKey, Helper.RpcResponse, TimeSpan.FromSeconds(cacheTime));
                }
            }
        }
    }
}
