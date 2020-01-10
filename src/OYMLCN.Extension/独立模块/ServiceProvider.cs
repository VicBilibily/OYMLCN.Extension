#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
using OYMLCN.Helpers;
using System;
using System.Collections.Generic;

namespace OYMLCN
{
    public class ServiceProvider
    {
        public static T GetService<T>()
        {
            Type typeFromHandle = typeof(T);
            object obj = null;
            if (!_serviceDict.TryGetValue(typeFromHandle, out obj))
                _serviceDict.Add(typeFromHandle, obj = Activator.CreateInstance(typeFromHandle, false));
            return (T)obj;
        }
        public static T GetServiceFromCallContext<T>()
        {
            Type typeFromHandle = typeof(T);
            object obj = CallContext.GetData(typeFromHandle.FullName);
            if (obj == null)
                CallContext.SetData(typeFromHandle.FullName, obj = Activator.CreateInstance(typeFromHandle, false));
            return (T)obj;
        }

        private static Dictionary<Type, object> _serviceDict = new Dictionary<Type, object>();
    }
}
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
