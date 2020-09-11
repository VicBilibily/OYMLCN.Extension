using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 实例相关辅助方法
    /// </summary>
    public static class InstanceHelper
    {
        /// <summary>
        /// 从指定程序集创建继承指定类型或接口的类型实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<T> CreateAllInstancesOf<T>(Assembly assembly)
            => assembly.GetTypes()
                .Where(t => typeof(T).IsAssignableFrom(t))
                // 获取非抽象类 排除接口继承
                .Where(t => !t.IsAbstract && t.IsClass)
                .Select(t => (T)Activator.CreateInstance(t));
    }
}
