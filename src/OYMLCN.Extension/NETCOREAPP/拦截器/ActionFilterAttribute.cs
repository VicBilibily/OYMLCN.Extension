using Microsoft.AspNetCore.Mvc.Filters;

namespace OYMLCN.AspNetCore
{
    /// <summary>
    /// 过滤器扩展基类
    /// </summary>
    public abstract class ActionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// Action 已执行完毕，在返回数据之前拦截处理
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            // TODO
        }
        /// <summary>
        /// Action 执行之前，数据模型绑定已完成
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            // TODO
        }
    }
}
