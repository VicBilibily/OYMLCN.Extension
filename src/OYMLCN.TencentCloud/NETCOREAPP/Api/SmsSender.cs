using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OYMLCN.Extensions;
using OYMLCN.TencentCloud;

namespace OYMLCN.AspNetCore.TencentCloud
{
    /// <summary>
    /// 腾讯云短信
    /// </summary>
    public class SmsSender : SmsSenderPackage
    {
        /// <summary>
        /// 腾讯云短信发送封装
        /// </summary>
        public SmsSender(IOptions<TencentCloudOptions> options) : base(options?.Value)
        {

        }
    }

    /// <summary>
    /// InjectionServiceCollectionExtensions
    /// </summary>
    public static partial class InjectionServiceCollectionExtensions
    {
        /// <summary>
        /// 注入腾讯云短信发送模块封装
        /// </summary>
        public static IServiceCollection AddTencentCloudSmsSender(this IServiceCollection services, string key = "TencentCloud")
        {
            var section = services.GetRequiredService<IConfiguration>().GetSection(key);
            services.Configure<TencentCloudOptions>(section);
            services.TryAddScoped<SmsSender>();
            return services;
        }
    }
}

