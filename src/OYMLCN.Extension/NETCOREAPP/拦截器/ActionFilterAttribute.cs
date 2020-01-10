using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OYMLCN.AspNetCore
{
    /// <summary>
    /// 过滤器扩展基类
    /// </summary>
    public abstract class ActionFilterAttribute : Attribute, IActionFilter
    {
        /// <summary>
        /// Action 已执行完毕，在返回数据之前拦截处理
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnActionExecuted(ActionExecutedContext context)
        {
            // TODO
        }
        /// <summary>
        /// Action 执行之前，数据模型绑定已完成
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnActionExecuting(ActionExecutingContext context)
        {
            // TODO
        }
    }
}
