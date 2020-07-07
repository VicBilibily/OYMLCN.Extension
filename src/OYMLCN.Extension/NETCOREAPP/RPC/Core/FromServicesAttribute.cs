using System;

namespace OYMLCN.RPC.Core
{
    /// <summary>
    /// 标记属性实例从注入服务获取（在过程调用目标实例创建后自动注入类型）
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FromServicesAttribute : Attribute { }
}
