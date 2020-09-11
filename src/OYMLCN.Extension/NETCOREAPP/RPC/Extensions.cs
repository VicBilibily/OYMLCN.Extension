using System;
using Microsoft.AspNetCore.Http;
using OYMLCN.RPC.Server;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// ServiceCollectionExtensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册过程调用方法
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        public static IServiceCollection AddRpcServer(this IServiceCollection services, Action<RpcServerOptions> options)
        {
            services.AddMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransferJob();

            RpcServerOptions rpcServerOptions = new RpcServerOptions(services);
            options.Invoke(rpcServerOptions);
            services.AddSingleton(rpcServerOptions);
            if (rpcServerOptions.RpcHelperType?.BaseType == typeof(RpcHelper) ||
                rpcServerOptions.RpcHelperType == typeof(RpcHelper))
                services.AddScoped(rpcServerOptions.RpcHelperType);
            return services;
        }
    }
}

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// RpcIApplicationBuilderExtensions
    /// </summary>
    public static class RpcIApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用过程调用处理中间件
        /// </summary>
        /// <param name="applicationBuilder"></param>
        public static IApplicationBuilder UseRpcMiddleware(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.UseMiddleware<RpcMiddleware>();
    }
}
