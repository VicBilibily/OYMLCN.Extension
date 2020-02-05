using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型属性辅助扩展
    /// </summary>
    public static class AttributeExtension
    {
        /// <summary> 获取特定成员的指定属性 </summary>
        public static TAttribute GetAttribute<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
            => (TAttribute)(memberInfo.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault());

        /// <summary> 获取特定成员的指定属性 </summary>
        public static TAttribute[] GetAttributes<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
            => Array.ConvertAll(memberInfo.GetCustomAttributes(typeof(TAttribute), false), (object x) => (TAttribute)x);

    }
}
