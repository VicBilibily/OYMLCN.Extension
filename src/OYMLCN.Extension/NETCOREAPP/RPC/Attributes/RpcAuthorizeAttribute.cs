using System;

namespace OYMLCN.RPC.Core
{
    /// <summary>
    /// 远程调用类型或目标方法需要进行Token身份认证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RpcAuthorizeAttribute : Attribute
    {
        /// <summary>
        /// 目标方法允许匿名访问（仅对目标方法有效）
        /// </summary>
        public bool AllowAnonymous { get; set; }
    }
}
