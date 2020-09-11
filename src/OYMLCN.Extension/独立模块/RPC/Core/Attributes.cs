using System;

namespace OYMLCN.RPC.Core
{
    /// <summary>
    /// 标记属性实例从注入服务获取（在过程调用目标实例创建后自动注入类型）
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FromServicesAttribute : Attribute { }

    /// <summary>
    /// 标记过程调用对象属性（用于序列化参数判断）
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RpcPropertyAttribute : Attribute
    {
        /// <summary>
        /// 标记过程调用对象属性（用于序列化参数判断）
        /// </summary>
        public RpcPropertyAttribute() { }
        /// <summary>
        /// 标记过程调用对象属性（用于序列化参数判断）
        /// </summary>
        public RpcPropertyAttribute(string description)
            => Description = description;
        /// <summary>
        /// 描述文本
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 必填项目
        /// </summary>
        public bool Require { get; set; }
        /// <summary>
        /// 默认值初始化
        /// </summary>
        public object DefaultValue { get; set; }
        /// <summary>
        /// 提示消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 最大长度限制(0表示无限制)
        /// </summary>
        public int MaxLength { get; set; }
    }
    /// <summary>
    /// 调用过程返回属性字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class RpcResponsePropertyAttribute : Attribute
    {
        /// <summary>
        /// 调用过程返回属性字段
        /// </summary>
        public RpcResponsePropertyAttribute() { }
        /// <summary>
        /// 调用过程返回属性字段
        /// </summary>
        public RpcResponsePropertyAttribute(string description)
            => Description = description;

        /// <summary>
        /// 描述文本
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }
        /// <summary>
        /// 提示消息
        /// </summary>
        public string Message { get; set; }
    }


    [AttributeUsage(AttributeTargets.Field)]
    public class RpcEnumAttribute : Attribute
    {
        public RpcEnumAttribute() { }
        public RpcEnumAttribute(string description)
            => Description = description;

        /// <summary>
        /// 描述文本
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }
        /// <summary>
        /// 提示消息
        /// </summary>
        public string Message { get; set; }
    }
}
