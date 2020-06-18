using System;
using System.Collections.Concurrent;

namespace OYMLCN
{
    /// <summary>
    /// 简单的服务提供器
    /// </summary>
    public static class ServiceProvider
    {
        static ConcurrentDictionary<Type, object> _serviceDict = new ConcurrentDictionary<Type, object>();
        /// <summary>
        /// 获取指定类型的单例
        /// </summary>
        public static T GetService<T>()
        {
            Type typeFromHandle = typeof(T);
            return (T)_serviceDict.GetOrAdd(typeFromHandle, Activator.CreateInstance(typeFromHandle, false));
        }
    }
}
