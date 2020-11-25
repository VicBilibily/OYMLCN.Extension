/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.Equals.cs
Author: VicBilibily
Description: 
    本代码所在文件夹的文件主要定义一些的常用的字符串操作相关方法扩展，以提升开发效率为目的。
    此文件主要定义 Equals 字符串相等判断的相关扩展方法。
*****************************************************************************/

using System;
using System.Linq;

namespace OYMLCN.Extensions
{
    public static partial class StringExtension
    {
        /// <summary>
        ///   确定两个指定的 <see cref="string" /> 对象是否具有相同的值（忽略大小写）。
        /// </summary>
        /// <param name="str">要比较的第一个字符串，或 <see langword="null" />。</param>
        /// <param name="value">要比较的第二个字符串，或 <see langword="null" />。</param>
        /// <returns>
        ///   如果 <paramref name="str" /> 参数的值与 <paramref name="value" />
        ///   参数的值相同，则为 <see langword="true" />，否则为 <see langword="false" />。
        /// </returns>
        public static bool EqualsIgnoreCase(this string str, string value)
            => string.Equals(str, value, StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        ///   确定此字符串是否与另一个指定字符串数组内的对象具有相同的值
        /// </summary>
        /// <param name="str">检测的字符串</param>
        /// <param name="values">要进行比较的字符串数组</param>
        /// <returns>
        ///   <para>如果 <paramref name="str"/> 值与 <paramref name="values"/>
        ///   内的任一字符串相等，则为 <see langword="true"/>，否则为 <see langword="false"/>。</para>
        ///   <para>如果 <paramref name="str"/> 值与 <paramref name="values"/> 均为
        ///   <see langword="null"/>，则始终返回 <see langword="true"/>。</para>
        /// </returns>
        public static bool EqualsWords(this string str, params string[] values)
        {
            if (str == null && values == null) return true;
            if (values?.Any() == false) return false;
            return values.Any(v => string.Equals(str, v));
        }
        /// <summary>
        ///   确定此字符串是否与另一个指定字符串数组内的对象具有相同的值（忽略大小写）
        /// </summary>
        /// <param name="str">检测的字符串</param>
        /// <param name="values">要进行比较的字符串数组</param>
        /// <returns>
        ///   <para>如果 <paramref name="str"/> 值与 <paramref name="values"/>
        ///   内的任一字符串相等（忽略大小写），则为 <see langword="true"/>，否则为
        ///   <see langword="false"/>。</para>
        ///   <para>如果 <paramref name="str"/> 值与 <paramref name="values"/> 均为
        ///   <see langword="null"/>，则始终返回 <see langword="true"/>。</para>
        /// </returns>
        public static bool EqualsWordsIgnoreCase(this string str, params string[] values)
        {
            if (str == null && values == null) return true;
            if (values?.Any() == false) return false;
            return values.Any(v => string.Equals(str, v, StringComparison.OrdinalIgnoreCase));
        }

    }
}
