#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
using System;
using System.Collections.Concurrent;
using System.Threading;
//using System.Runtime.Remoting.Messaging;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 取线程内唯一对象
    /// </summary>
    public static class CallContext
    {
        static ConcurrentDictionary<string, AsyncLocal<object>> state = new ConcurrentDictionary<string, AsyncLocal<object>>();

        public static void SetData(string name, object data)
            => state.GetOrAdd(name, _ => new AsyncLocal<object>()).Value = data;
        public static object GetData(string name)
            => state.TryGetValue(name, out AsyncLocal<object> data) ? data.Value : null;
    }

    /// <summary>
    /// 提供简单的不是线程安全的高性能的对象缓存
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public static class ObjectCache<TObject>
    {
        public static TObject Get()
            => Get(() => (TObject)(Activator.CreateInstance(typeof(TObject), false)));
        public static TObject Get(Func<TObject> createInstanceAction)
            => Instance ?? (Instance = createInstanceAction());

        public static TObject GetFromCallContext()
            => GetFromCallContext((Type x) => (TObject)Activator.CreateInstance(x));
        public static TObject GetFromCallContext(Func<Type, TObject> createInstance)
        {
            Type typeFromHandle = typeof(TObject);
            TObject obj = (TObject)CallContext.GetData(typeFromHandle.FullName);
            if (obj == null)
            {
                obj = createInstance(typeFromHandle);
                CallContext.SetData(typeFromHandle.FullName, obj);
            }
            return obj;
        }

        private static TObject Instance { get; set; }
    }

    /// <summary>
    /// 提供高性能线程安全的对象缓存
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public class ObjectCacheLocked<TObject>
    {
        public static TObject Get()
            => Get(() => (TObject)Activator.CreateInstance(typeof(TObject), false));
        public static TObject Get(Func<TObject> createInstanceAction)
        {
            if (Instance == null)
                lock (_locker)
                    if (Instance == null)
                        Instance = createInstanceAction();
            return Instance;
        }

        public static TObject GetFromCallContext()
            => GetFromCallContext((Type x) => (TObject)Activator.CreateInstance(x));
        public static TObject GetFromCallContext(Func<Type, TObject> createInstance)
        {
            Type typeFromHandle = typeof(TObject);
            TObject tobject = (TObject)CallContext.GetData(typeFromHandle.FullName);
            if (tobject == null)
                lock (_locker)
                {
                    tobject = (TObject)CallContext.GetData(typeFromHandle.FullName);
                    CallContext.SetData(typeFromHandle.FullName, tobject ?? (tobject = createInstance(typeFromHandle)));
                }
            return tobject;
        }

        private static TObject Instance { get; set; }
        private static object _locker = new object();
    }
}
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
