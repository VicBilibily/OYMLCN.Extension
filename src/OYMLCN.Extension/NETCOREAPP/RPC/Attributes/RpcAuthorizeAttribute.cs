using System;

namespace OYMLCN.RPC.Core
{
    /// <summary>
    /// 远程调用类型或目标方法需要进行Token身份认证
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RpcAuthorizeAttribute : Attribute
    {
    }
}
