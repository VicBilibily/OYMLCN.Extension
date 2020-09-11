using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OYMLCN.AspNetCore.TransferJob;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// ControllerExtension
    /// </summary>
    public static partial class ControllerExtension
    {
        /// <summary>
        /// 推入后台异步任务实现【使用时需要注入services.AddTransferJob();】
        /// </summary>
        public static void TransferJob<T>(this Controller controller, Expression<Func<T, Task>> expression)
        {
            var bgs = controller.GetService<IBackgroundRunService>();
            if (bgs == null) throw new Exception("未能获得后台异步处理服务，请检查是否注入了 services.AddTransferJob();");
            bgs.Transfer(expression);
        }
        /// <summary>
        /// 推入后台异步任务实现【使用时需要注入services.AddTransferJob();】
        /// </summary>
        public static void TransferJob(this Controller controller, Expression<Action> expression)
        {
            var bgs = controller.GetService<IBackgroundRunService>();
            if (bgs == null) throw new Exception("未能获得后台异步处理服务，请检查是否注入了 services.AddTransferJob();");
            bgs.Transfer(expression);
        }
        /// <summary>
        /// 推入后台异步任务实现【使用时需要注入services.AddTransferJob();】
        /// </summary>
        public static void TransferJob<T>(this Controller controller, Expression<Action<T>> expression)
        {
            var bgs = controller.GetService<IBackgroundRunService>();
            if (bgs == null) throw new Exception("未能获得后台异步处理服务，请检查是否注入了 services.AddTransferJob();");
            bgs.Transfer(expression);
        }
    }
}
