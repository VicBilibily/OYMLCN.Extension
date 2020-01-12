using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// AspNetCoreExtension
    /// </summary>
    public static class AspNetCoreExtension
    {
        #region GetService/GetOptions
        /// <summary>
        /// 获取已注入的服务实例 <typeparamref name="T" /> 基于 <see cref="T:System.IServiceProvider" />.
        /// </summary>
        public static T GetService<T>(this Controller controller)
            => controller.HttpContext.RequestServices.GetService<T>();
        /// <summary>
        /// 获取已注入的服务实例 <typeparamref name="T" /> 基于 <see cref="T:System.IServiceProvider" />.
        /// </summary>
        public static T GetRequiredService<T>(this Controller controller)
            => controller.HttpContext.RequestServices.GetRequiredService<T>();
        /// <summary>
        /// 获取已注入的服务实例
        /// </summary>
        public static T GetOptions<T>(this Controller context) where T : class, new()
            => context.GetService<IOptions<T>>()?.Value ?? new T();
        /// <summary>
        /// 获取已注入的服务实例
        /// </summary>
        public static T GetRequiredOptions<T>(this Controller context) where T : class, new()
            => context.GetRequiredService<IOptions<T>>().Value;

        /// <summary>
        /// 获取已注入的服务实例 <typeparamref name="T" /> 基于 <see cref="T:System.IServiceProvider" />.
        /// </summary>
        public static T GetService<T>(this ActionContext context)
            => context.HttpContext.RequestServices.GetService<T>();
        /// <summary>
        /// 获取已注入的服务实例 <typeparamref name="T" /> 基于 <see cref="T:System.IServiceProvider" />.
        /// </summary>
        public static T GetRequiredService<T>(this ActionContext context)
            => context.HttpContext.RequestServices.GetRequiredService<T>();
        /// <summary>
        /// 获取已注入的服务实例
        /// </summary>
        public static T GetOptions<T>(this ActionContext context) where T : class, new()
            => context.GetService<IOptions<T>>()?.Value ?? new T();
        /// <summary>
        /// 获取已注入的服务实例
        /// </summary>
        public static T GetRequiredOptions<T>(this ActionContext context) where T : class, new()
            => context.GetRequiredService<IOptions<T>>().Value;

        /// <summary>
        /// 获取已注入的服务实例 <typeparamref name="T" /> 基于 <see cref="T:System.IServiceProvider" />.
        /// </summary>
        public static T GetService<T>(this IApplicationBuilder app)
            => app.ApplicationServices.GetService<T>();
        /// <summary>
        /// 获取已注入的服务实例 <typeparamref name="T" /> 基于 <see cref="T:System.IServiceProvider" />.
        /// </summary>
        public static T GetRequiredService<T>(this IApplicationBuilder app)
            => app.ApplicationServices.GetRequiredService<T>();
        /// <summary>
        /// 获取已注入的服务实例
        /// </summary>
        public static T GetOptions<T>(this IApplicationBuilder app) where T : class, new()
            => app.GetService<IOptions<T>>()?.Value ?? new T();
        /// <summary>
        /// 获取已注入的服务实例
        /// </summary>
        public static T GetRequiredOptions<T>(this IApplicationBuilder app) where T : class, new()
            => app.GetRequiredService<IOptions<T>>().Value;

        /// <summary>
        /// 获取已注入的服务实例
        /// </summary>
        public static T GetService<T>(this IServiceCollection services)
            => services.BuildServiceProvider().GetService<T>();
        /// <summary>
        /// 获取已注入的服务实例
        /// </summary>
        public static T GetRequiredService<T>(this IServiceCollection services)
            => services.BuildServiceProvider().GetRequiredService<T>();
        /// <summary>
        /// 获取已注入的服务实例
        /// </summary>
        public static T GetOptions<T>(this IServiceCollection services) where T : class, new()
            => services.GetService<IOptions<T>>()?.Value ?? new T();
        /// <summary>
        /// 获取已注入的服务实例
        /// </summary>
        public static T GetRequiredOptions<T>(this IServiceCollection services) where T : class, new()
            => services.GetRequiredService<IOptions<T>>().Value;
        #endregion



    }
}
