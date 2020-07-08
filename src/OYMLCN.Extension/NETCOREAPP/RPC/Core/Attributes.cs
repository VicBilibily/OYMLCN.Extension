﻿using System;

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
    }
}
