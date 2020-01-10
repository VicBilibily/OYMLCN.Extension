using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// AspNetCoreExtension
    /// </summary>
    public static class AspNetCoreExtension
    {
        #region GetService
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
        /// 获取已注入的服务实例 <typeparamref name="T" /> 基于 <see cref="T:System.IServiceProvider" />.
        /// </summary>
        public static T GetService<T>(this IApplicationBuilder app)
            => app.ApplicationServices.GetService<T>();
        /// <summary>
        /// 获取已注入的服务实例 <typeparamref name="T" /> 基于 <see cref="T:System.IServiceProvider" />.
        /// </summary>
        public static T GetRequiredService<T>(this IApplicationBuilder app)
            => app.ApplicationServices.GetRequiredService<T>();
        #endregion



    }
}
