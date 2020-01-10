using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 类型属性辅助
    /// </summary>
    public class AttributeHelper
    {
        /// <summary>
        /// 获取指定属性
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(MemberInfo memberInfo)
            => (TAttribute)(memberInfo.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault());
        /// <summary>
        /// 获取指定属性
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        public static TAttribute[] GetAttributes<TAttribute>(MemberInfo memberInfo)
            => Array.ConvertAll(memberInfo.GetCustomAttributes(typeof(TAttribute), false), (object x) => (TAttribute)x);

    }
}
