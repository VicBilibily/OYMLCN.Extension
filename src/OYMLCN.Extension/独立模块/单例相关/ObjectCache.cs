using System;
using System.Collections.Concurrent;
using System.Threading;

namespace OYMLCN
{
    /// <summary>
    /// 取线程内唯一对象
    /// </summary>
    public static class CallContext
    {
        static ConcurrentDictionary<string, AsyncLocal<object>> state = new ConcurrentDictionary<string, AsyncLocal<object>>();

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="name"> 唯一键值 </param>
        /// <param name="data"> 数据对象 </param>
        public static void SetData(string name, object data)
            => state.GetOrAdd(name, _ => new AsyncLocal<object>()).Value = data;
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="name"> 唯一键值 </param>
        /// <returns> 数据对象 </returns>
        public static object GetData(string name)
            => state.TryGetValue(name, out AsyncLocal<object> data) ? data.Value : null;
    }

    /// <summary>
    /// 提供简单的不是线程安全的高性能的对象缓存
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public static class ObjectCache<TObject>
    {
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns> 对象实例 </returns>
        public static TObject Get()
            => Get(() => (TObject)(Activator.CreateInstance(typeof(TObject), false)));
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="createInstanceAction"> 自定义实例初始化方法 </param>
        /// <returns> 对象实例 </returns>
        public static TObject Get(Func<TObject> createInstanceAction)
            => Instance ?? (Instance = createInstanceAction());

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns> 对象实例 </returns>
        public static TObject GetFromCallContext()
            => GetFromCallContext(x => (TObject)Activator.CreateInstance(x));
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="createInstance"> 自定义实例初始化方法 </param>
        /// <returns> 对象实例 </returns>
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
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns> 对象实例 </returns>
        public static TObject Get()
            => Get(() => (TObject)Activator.CreateInstance(typeof(TObject), false));
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="createInstanceAction"> 自定义实例初始化方法 </param>
        /// <returns> 对象实例 </returns>
        public static TObject Get(Func<TObject> createInstanceAction)
        {
            if (Instance == null)
                lock (_locker)
                    Instance ??= createInstanceAction();
            return Instance;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns> 对象实例 </returns>
        public static TObject GetFromCallContext()
            => GetFromCallContext(x => (TObject)Activator.CreateInstance(x));
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="createInstance"> 自定义实例初始化方法 </param>
        /// <returns> 对象实例 </returns>
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
