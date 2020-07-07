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
        /// 获取当前执行程序内容目录
        /// </summary>
        /// <param name="services"> 注入服务提供者 </param>
        /// <returns> 内容目录路径 </returns>
        public static string GetRootPath(this IServiceProvider services)
        {
            return services.GetRequiredService<IWebHostEnvironment>()?.ContentRootPath;
        }

        /// <summary>
        /// 获取当前网站的静态内容目录
        /// </summary>
        /// <param name="services"> 注入服务提供者 </param>
        /// <returns> 静态内容目录 </returns>
        public static string GetWebRootPath(this IServiceProvider services)
        {
            return services.GetRequiredService<IWebHostEnvironment>()?.WebRootPath;
        }

        internal static string CombinePath(in string filePath, in string sourceDir)
        {
            string fullPath = filePath ?? string.Empty;
            if (fullPath.StartsWith("~"))
                fullPath = fullPath.Substring(1);
            if (fullPath.StartsWith("/"))
                fullPath = fullPath.Substring(1);
            if (Path.DirectorySeparatorChar != '/')
                fullPath = fullPath.Replace('/', Path.DirectorySeparatorChar);
            return Path.Combine(sourceDir, fullPath);
        }

        /// <summary>
        /// 获取指定文件路径在服务端可用的完整路径
        /// <para> （比如将 ~/appsettings.json 映射为 c:\project\appsettings.json） </para>
        /// </summary>
        /// <param name="services"> 注入服务提供者 </param>
        /// <param name="filePath"> 文件路径 </param>
        /// <returns> 服务端完整路径 </returns>
        public static string CombineRootPath(this IServiceProvider services, string filePath)
            => CombinePath(filePath, services.GetRootPath());

        /// <summary>
        /// 获取指定文件路径在服务端可用的静态资源路径
        /// <para> （比如将 ~/css/site.css 映射为 c:\project\wwwroot\css\site.css） </para>
        /// </summary>
        /// <param name="services"> 注入服务提供者 </param>
        /// <param name="filePath"> 文件路径 </param>
        /// <returns> 服务端完整路径 </returns>
        public static string CombineWebRootPath(this IServiceProvider services, string filePath)
            => CombinePath(filePath, services.GetWebRootPath());

    }
}
