using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 格式相关扩展
    /// </summary>
    public static partial class StringFormatExtension
    {
        /// <summary> 检查字符串是否与另一个指定字符串对象具有相同的值 </summary>
        /// <param name="str"> 字符串实例 </param>
        /// <param name="value"> 要与此实例进行比较的字符串 </param>
        /// <param name="comparisonType"> 枚举值之一，用于指定如何比较字符串 </param>
        /// <returns> 如果 <paramref name="str"/> 值与 <paramref name="value"/> 相同，则为 true；否则为 false。 </returns>
        /// <exception cref="ArgumentException"> <paramref name="comparisonType"/> 不是 <see cref="StringComparison"/> 值 </exception>
        public static bool IsEqual(this string str, string value, StringComparison comparisonType = StringComparison.Ordinal)
            => (str.IsNull() || value.IsNull()) ? false : str.Equals(value, comparisonType);

        /// <summary> 检查字符串是否与另一个指定字符串对象具有相同的值（忽略大小写） </summary>
        /// <param name="str"> 字符串实例 </param>
        /// <param name="value"> 要与此实例进行比较的字符串 </param>
        /// <returns> 如果 <paramref name="str"/> 值与 <paramref name="value"/> 相同，则为 true；否则为 false。 </returns>
        public static bool IsEqualIgnoreCase(this string str, string value)
            => str.IsEqual(value, StringComparison.OrdinalIgnoreCase);


        /// <summary> 检查字符串是否与另一个指定字符串数组内的对象具有相同的值 </summary>
        /// <param name="str"> 字符串实例 </param>
        /// <param name="values"> 要与此实例进行比较的字符串数组 </param>
        /// <param name="comparisonType"> 枚举值之一，用于指定如何比较字符串 </param>
        /// <returns> 如果 <paramref name="str"/> 值与 <paramref name="values"/> 内的任一对象相同，则为 true；否则为 false。 </returns>
        /// <exception cref="ArgumentException"> <paramref name="comparisonType"/> 不是 <see cref="StringComparison"/> 值 </exception>
        public static bool IsEqual(this string str, string[] values, StringComparison comparisonType = StringComparison.Ordinal)
        {
            if (str == null || (values == null || values.Length == 0)) return false;
            foreach (var value in values)
                if (str.Equals(value, comparisonType))
                    return true;
            return false;
        }

        /// <summary> 检查字符串是否与另一个指定字符串数组内的对象具有相同的值（忽略大小写） </summary>
        /// <param name="str"> 字符串实例 </param>
        /// <param name="values"> 要与此实例进行比较的字符串数组 </param>
        /// <returns> 如果 <paramref name="str"/> 值与 <paramref name="values"/> 内的任一对象相同，则为 true；否则为 false。 </returns>
        public static bool IsEqualIgnoreCase(this string str, string[] values)
            => str.IsEqual(values, StringComparison.OrdinalIgnoreCase);

    }
}
