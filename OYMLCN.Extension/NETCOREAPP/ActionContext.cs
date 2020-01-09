using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// ActionContextExtension
    /// </summary>
    public static class ActionContextExtension
    {
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
    }
}
