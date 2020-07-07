using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// EnvironmentExtension
    /// </summary>
    public static partial class EnvironmentExtension
    {
        /// <summary>
        /// 检查当前运行环境是否是开发环境
        /// </summary>
        public static bool IsDevelopment(this IServiceProvider services)
        {
            return services.GetRequiredService<IWebHostEnvironment>()?.IsDevelopment() ?? false;
        }
    }
}
