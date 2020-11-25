/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.With.cs
Author: VicBilibily
Description: 
    本代码所在文件夹的文件主要定义一些的常用的字符串操作相关方法扩展，以提升开发效率为目的。
    此文件主要定义字符串开始或结尾的条件判断相关扩展方法。
*****************************************************************************/

using System.Linq;
using System;

namespace OYMLCN.Extensions
{
    public static partial class StringExtension
    {
        /// <summary>
        ///   确定此字符串实例的开头是否与指定的字符串匹配（不区分大小写）。
        /// </summary>
        /// <param name="str">被比较的字符串。</param>
        /// <param name="value">要比较的字符串。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="value" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <returns>
        ///   如果此实例以 <paramref name="value" /> 开头，则为
        ///   <see langword="true" />，否则为 <see langword="false" />。
        /// </returns>
        public static bool StartsWithIgnoreCase(this string str, string value)
        {
            if (str.IsNullOrEmpty()) return false;
            return str.StartsWith(value, StringComparison.CurrentCultureIgnoreCase);
        }
        /// <summary>
        ///   确定此字符串实例的结尾是否与指定的字符串匹配（不区分大小写）。
        /// </summary>
        /// <param name="str">被比较的字符串。</param>
        /// <param name="value">要与此实例末尾的子字符串进行比较的字符串。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="value" /> 为默认值 <see langword="null" />。
        /// </exception>
        /// <returns>
        ///   如果 <paramref name="value" /> 与此实例的末尾匹配（不区分大小写），则为
        ///   <see langword="true" />，否则为 <see langword="false" />。
        /// </returns>
        public static bool EndsWithIgnoreCase(this string str, string value)
        {
            if (str.IsNullOrEmpty()) return false;
            return str.EndsWith(value, StringComparison.CurrentCultureIgnoreCase);
        }



        /// <summary>
        ///   确定此字符串实例的开头是否与指定的字符串集合的任意值匹配。
        /// </summary>
        /// <param name="str">被测试的字符串。</param>
        /// <param name="values">要比较的字符串集合。</param>
        /// <returns>
        ///   如果 <paramref name="values" /> 的任意值与此字符串的开头匹配，则为
        ///   <see langword="true" />，否则为 <see langword="false" />。
        /// </returns>
        public static bool StartsWithWords(this string str, params string[] values)
        {
            if (string.IsNullOrEmpty(str)) return false;
            if (values == null) return false;
            if (values.Contains(null))
                values = values.Where(v => v != null).ToArray();
            return values.Any(value => str.StartsWith(value));
        }
        /// <summary>
        ///   确定此字符串实例的开头是否与指定的字符集合的任意值匹配。
        /// </summary>
        /// <param name="str">被测试的字符串。</param>
        /// <param name="values">要比较的字符集合。</param>
        /// <returns>
        ///   如果 <paramref name="values" /> 的任意字符与此字符串的开头匹配，则为
        ///   <see langword="true" />，否则为 <see langword="false" />。
        /// </returns>
        public static bool StartsWithChars(this string str, params char[] values)
        {
            if (string.IsNullOrEmpty(str)) return false;
            if (values == null) return false;
            return values.Any(value => str.StartsWith(value));
        }

        /// <summary>
        ///   确定此字符串实例的结尾是否与指定的字符串集合的任意值匹配。
        /// </summary>
        /// <param name="str">被测试的字符串。</param>
        /// <param name="values">要比较的字符串集合。</param>
        /// <returns>
        ///   如果 <paramref name="values" /> 的任意值与此字符串的末尾匹配，则为
        ///   <see langword="true" />，否则为 <see langword="false" />。
        /// </returns>
        public static bool EndsWithWords(this string str, params string[] values)
        {
            if (string.IsNullOrEmpty(str)) return false;
            if (values == null) return false;
            if (values.Contains(null))
                values = values.Where(v => v != null).ToArray();
            return values.Any(value => str.EndsWith(value));
        }
        /// <summary>
        ///   确定此字符串实例的结尾是否与指定的字符集合的任意值匹配。
        /// </summary>
        /// <param name="str">被测试的字符串。</param>
        /// <param name="values">要比较的字符集合。</param>
        /// <returns>
        ///   如果 <paramref name="values" /> 的任意字符与此字符串的末尾匹配，则为
        ///   <see langword="true" />，否则为 <see langword="false" />。
        /// </returns>
        public static bool EndsWithChars(this string str, params char[] values)
        {
            if (string.IsNullOrEmpty(str)) return false;
            if (values == null) return false;
            return values.Any(value => str.EndsWith(value));
        }


    }
}
