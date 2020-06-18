using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using OYMLCN.AspNetCore.TransferJob;

namespace OYMLCN.AspNetCore.TransferJob
{
    /// <summary>
    /// TransferJobHostedService
    /// </summary>
    public class TransferJobHostedService : BackgroundService
    {
        private IBackgroundRunService _runService;
        /// <summary>
        /// 后台任务处理服务
        /// </summary>
        public TransferJobHostedService(IBackgroundRunService runService)
        {
            _runService = runService;
        }
        /// <summary>
        /// 重写执行方法
        /// </summary>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
                await _runService.Execute(stoppingToken);
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// InjectionServiceCollectionExtensions
    /// </summary>
    public static partial class InjectionServiceCollectionExtensions
    {
        /// <summary>
        /// 注入后台处理任务依赖
        /// </summary>
        public static IServiceCollection AddTransferJob(this IServiceCollection services)
        {
            services.AddSingleton<IBackgroundRunService, BackgroundRunService>();
            services.AddHostedService<TransferJobHostedService>();
            return services;
        }
    }
}
