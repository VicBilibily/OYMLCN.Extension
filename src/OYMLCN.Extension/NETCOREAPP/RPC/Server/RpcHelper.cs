using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OYMLCN.AspNetCore.TransferJob;
using OYMLCN.Extensions;
using System;

namespace OYMLCN.RPC.Server
{
    /// <summary>
    /// 远程调用辅助类
    /// </summary>
    public partial class RpcHelper
    {
        /// <summary>
        /// 注入服务提供器
        /// </summary>
        public IServiceProvider RequestServices { get; }
        /// <summary>
        /// 远程调用辅助类
        /// </summary>
        public RpcHelper(IServiceProvider requestServices)
        {
            this.RequestServices = requestServices;
        }

        private IWebHostEnvironment _hostEnvironment;
        private IConfiguration _configuration;
        private IMemoryCache _memoryCache;
        private IHttpContextAccessor _httpContextAccessor;
        private HttpContext _httpContext;

        private IBackgroundRunService _backgroundRunService;

        /// <summary>
        /// 主机环境信息
        /// </summary>
        public IWebHostEnvironment HostEnvironment
            => _hostEnvironment ??= RequestServices.GetRequiredService<IWebHostEnvironment>();
        /// <summary>
        /// 程序运行配置
        /// </summary>
        public IConfiguration Configuration
           => _configuration ??= RequestServices.GetRequiredService<IConfiguration>();
        /// <summary>
        /// 内存缓存实例
        /// </summary>
        public IMemoryCache MemoryCache
            => _memoryCache ??= RequestServices.GetRequiredService<IMemoryCache>();
        private IHttpContextAccessor HttpContextAccessor
            => _httpContextAccessor ??= RequestServices.GetRequiredService<IHttpContextAccessor>();
        /// <summary>
        /// 请求上下文信息
        /// </summary>
        public HttpContext HttpContext
            => _httpContext ??= HttpContextAccessor.HttpContext;

        /// <summary>
        /// 后台任务提供器
        /// </summary>
        public IBackgroundRunService BackgroundRunService
           => _backgroundRunService ??= RequestServices.GetRequiredService<IBackgroundRunService>();




    }
}
