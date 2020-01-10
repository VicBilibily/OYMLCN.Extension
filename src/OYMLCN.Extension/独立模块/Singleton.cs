using OYMLCN.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN
{
    /// <summary>
    /// 单例模式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T>
    {
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T GetInstance(params object[] parameters)
        {
            if (_instance == null)
            {
                Type typeFromHandle = typeof(T);
                object obj = _lockers.GetValueOrSetDefaultFunc(typeFromHandle, (Type x) => new object());
                lock (obj)
                    if (_instance == null)
                    {
                        ConstructorInfo constructorInfo = typeFromHandle.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault();
                        _instance = (T)constructorInfo.Invoke(parameters);
                    }
            }
            return _instance;
        }

        private static Dictionary<Type, object> _lockers = new Dictionary<Type, object>();
        private static T _instance;
    }
}
