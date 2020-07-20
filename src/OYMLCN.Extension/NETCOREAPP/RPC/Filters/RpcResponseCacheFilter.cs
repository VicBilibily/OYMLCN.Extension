using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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

        public override async Task InvokeAsync(RpcContext context, RpcRequestDelegate next)
        {
            var cacheKey = Helper.GetRpcResponseCacheKey();
            if (cacheKey.IsNotNullOrEmpty() &&
                Helper.MemoryCache.TryGetValue(cacheKey, out ResponseModel rpcResult))
            {
                Helper.RpcResponse = rpcResult;
                await Helper.WriteRpcResponseAsync();
                var _logger = LoggerFactory.CreateLogger<RpcMiddleware>();
                _logger.LogInformation("过程调用缓存命中，调用目标：{0}，调用过程：{1}，执行耗时：{2}ms",
                    context.TargetType.FullName,
                    context.Method.Name,
                    Helper.RpcResponse.Time
                );
            }
            else
            {
                await next(context);
                var cacheTime = Helper.GetRpcResponseCacheTime();
                if (cacheTime > 0 && Helper.RpcResponse?.Code == 0)
                    Helper.MemoryCache.Set(cacheKey, Helper.RpcResponse, TimeSpan.FromSeconds(cacheTime));
            }
        }
    }
}
