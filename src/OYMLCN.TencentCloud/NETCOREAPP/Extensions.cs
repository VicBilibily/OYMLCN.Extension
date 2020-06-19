using OYMLCN.AspNetCore.TencentCloud;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// InjectionServiceCollectionExtensions
    /// </summary>
    public static partial class InjectionServiceCollectionExtensions
    {
        /// <summary>
        /// 注入腾讯云所有已实现的模块封装
        /// </summary>
        public static IServiceCollection AddTencentCloud(this IServiceCollection services, string key = "TencentCloud")
        {
            services.AddTencentCloudSmsSender(key);
            return services;
        }
    }
}
