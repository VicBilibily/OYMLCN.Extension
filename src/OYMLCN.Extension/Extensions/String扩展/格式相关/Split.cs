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
        /// <summary> 基于指定的字符串将一个字符串拆分成最大数量的子字符串，可以指定子字符串是否包含空数组元素 </summary>
        /// <param name="str"></param>
        /// <param name="separator"> 分隔此字符串中子字符串的字符串数组，不包含分隔符的空数组或 null </param>
        /// <param name="count"> 要返回的子字符串的最大数量 </param>
        /// <param name="options"> 要省略返回的数组中的空数组元素，则为 <see cref="StringSplitOptions.RemoveEmptyEntries"/>，要包含返回的数组中的空数组元素，则为 <see cref="StringSplitOptions.None"/> </param>
        /// <returns> 一个数组，其元素包含此字符串中的子字符串，这些子字符串由 separator 中的一个或多个字符串分隔 </returns>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="count"/> 为负数 </exception>
        /// <exception cref="ArgumentException"> <paramref name="options"/> 不是 <see cref="StringSplitOptions"/> 值之一 </exception>
        public static string[] SplitBySign(this string str, string separator, int count = int.MaxValue, StringSplitOptions options = StringSplitOptions.None)
        {
            if (str.IsNullOrWhiteSpace()) return new string[0];
            return str.Split(new[] { separator }, count, options);
        }

        /// <summary> 基于指定的字符串将一个字符串拆分成最大数量的子字符串（不包含空字符串对象） </summary>
        /// <param name="str"></param>
        /// <param name="separator"> 分隔此字符串中子字符串的字符串数组，不包含分隔符的空数组或 null </param>
        /// <param name="count"> 要返回的子字符串的最大数量 </param>
        /// <returns> 一个数组，其元素包含此字符串中的子字符串，这些子字符串由 separator 中的一个或多个字符串分隔 </returns>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="count"/> 为负数 </exception>
        public static string[] SplitBySignRemoveEmpty(this string str, string separator, int count = int.MaxValue)
            => str.SplitBySign(separator, count, StringSplitOptions.RemoveEmptyEntries);


        /// <summary> 基于指定的字符串将一个字符串拆分子字符串后获得第一个字符串对象 </summary>
        /// <param name="str"></param>
        /// <param name="separator"> 分隔此字符串中子字符串的字符串数组，不包含分隔符的空数组或 null </param>
        /// <param name="options"> 要省略返回的数组中的空数组元素，则为 <see cref="StringSplitOptions.RemoveEmptyEntries"/>，要包含返回的数组中的空数组元素，则为 <see cref="StringSplitOptions.None"/> </param>
        /// <returns> 基于指定的字符串将一个字符串拆分子字符串后获得第一个字符串对象 </returns>
        /// <exception cref="ArgumentException"> <paramref name="options"/> 不是 <see cref="StringSplitOptions"/> 值之一 </exception>
        public static string SplitThenGetFirst(this string str, string separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
            => str.SplitBySign(separator, 1, options).FirstOrDefault();

        /// <summary> 基于指定的字符串将一个字符串拆分子字符串后获得最后一个字符串对象 </summary>
        /// <param name="str"></param>
        /// <param name="separator"> 分隔此字符串中子字符串的字符串数组，不包含分隔符的空数组或 null </param>
        /// <param name="options"> 要省略返回的数组中的空数组元素，则为 <see cref="StringSplitOptions.RemoveEmptyEntries"/>，要包含返回的数组中的空数组元素，则为 <see cref="StringSplitOptions.None"/> </param>
        /// <returns> 基于指定的字符串将一个字符串拆分子字符串后获得最后一个字符串对象 </returns>
        /// <exception cref="ArgumentException"> <paramref name="options"/> 不是 <see cref="StringSplitOptions"/> 值之一 </exception>
        public static string SplitThenGetLast(this string str, string separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
            => str.SplitBySign(separator, int.MaxValue, options).LastOrDefault();


        /// <summary> 基于多个字符串将一个字符串拆分成最大数量的子字符串（不包含空字符串对象） </summary>
        /// <param name="str"></param>
        /// <param name="separator"> 分隔此字符串中子字符串的字符串数组，不包含分隔符的空数组或 null </param>
        /// <returns></returns>
        public static string[] SplitByMultiSign(this string str, params string[] separator)
        {
            if (str.IsNotNullOrEmpty() || separator.IsNullOrEmpty())
                return new string[0];
            else
                return str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary> 根据 | \ / 、 ， , 空格 中文空格 制表符空格换行 分割字符串 </summary>
        public static string[] SplitAuto(this string str)
            => str.SplitByMultiSign("|", "\\", "/", "、", ":", "：", "，", ",", "　", " ", "\t");

        /// <summary> 根据换行符拆分字符串 </summary>
        public static string[] SplitLines(this string str)
            => str.SplitByMultiSign("\r\n", "\r", "\n");

    }
}
