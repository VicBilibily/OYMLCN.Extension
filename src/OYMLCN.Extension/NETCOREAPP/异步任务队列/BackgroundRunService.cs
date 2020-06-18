using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace OYMLCN.AspNetCore.TransferJob
{
    /// <summary>
    /// IBackgroundRunService
    /// </summary>
    public interface IBackgroundRunService
    {
        /// <summary>
        /// 异步任务调度方法
        /// </summary>
        Task Execute(CancellationToken cancellationToken);
        /// <summary>
        /// 推入后台异步任务
        /// </summary>
        void Transfer<T>(Expression<Func<T, Task>> expression);
        /// <summary>
        /// 推入后台异步任务
        /// </summary>
        /// <param name="expression"></param>
        void Transfer(Expression<Action> expression);
        /// <summary>
        /// 推入后台异步任务
        /// </summary>
        void Transfer<T>(Expression<Action<T>> expression);
    }
    /// <summary>
    /// BackgroundRunService
    /// </summary>
    public class BackgroundRunService : IBackgroundRunService
    {
        private readonly SemaphoreSlim _slim;
        private readonly ConcurrentQueue<LambdaExpression> queue;
        private ILogger<BackgroundRunService> _logger;
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// BackgroundRunService
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public BackgroundRunService(ILogger<BackgroundRunService> logger, IServiceProvider serviceProvider)
        {
            _slim = new SemaphoreSlim(1);
            _logger = logger;
            _serviceProvider = serviceProvider;
            queue = new ConcurrentQueue<LambdaExpression>();
        }
        /// <summary>
        /// 异步任务调度方法实现
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Execute(CancellationToken cancellationToken)
        {
            try
            {
                await _slim.WaitAsync(cancellationToken);
                if (queue.TryDequeue(out var job))
                {
                    using (var scope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        var action = job.Compile();
                        var parameters = job.Parameters;
                        var pars = new List<object>();
                        if (parameters.Any())
                            foreach (var parameter in parameters)
                            {
                                var type = parameter.Type;
                                var param = scope.ServiceProvider.GetRequiredService(type);
                                pars.Add(param);
                            }
                        // 如果异步调用的方法是一个异步方法，则采用异步等待
                        if (action.Method.ReturnType == typeof(Task))
                            await (Task)action.DynamicInvoke(pars.ToArray());
                        else
                            action.DynamicInvoke(pars.ToArray());
                    }
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception e)
            {
                _logger.LogError(e, e.ToString());
            }
        }
        /// <summary>
        /// 推入后台异步任务实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        public void Transfer<T>(Expression<Func<T, Task>> expression)
        {
            queue.Enqueue(expression);
            _slim.Release();
        }
        /// <summary>
        /// 推入后台异步任务实现
        /// </summary>
        /// <param name="expression"></param>
        public void Transfer(Expression<Action> expression)
        {
            queue.Enqueue(expression);
            _slim.Release();
        }
        /// <summary>
        /// 推入后台异步任务实现
        /// </summary>
        /// <param name="expression"></param>
        public void Transfer<T>(Expression<Action<T>> expression)
        {
            queue.Enqueue(expression);
            _slim.Release();
        }
    }
}