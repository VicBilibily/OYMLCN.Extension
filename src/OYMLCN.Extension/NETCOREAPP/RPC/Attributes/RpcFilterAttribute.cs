using System;
using System.Threading.Tasks;

namespace OYMLCN.RPC.Core
{
    /// <summary>
    /// 过程调用触发器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class RpcFilterAttribute : Attribute
    {
        /// <summary>
        /// 触发方法定义
        /// </summary>
        public abstract Task InvokeAsync(RpcContext context, RpcRequestDelegate next);
    }
}
