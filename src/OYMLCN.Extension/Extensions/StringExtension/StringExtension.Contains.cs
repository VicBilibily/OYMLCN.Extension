/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.Contains.cs
Author: VicBilibily
Description: 
    本代码所在文件夹的文件主要定义一些的常用的字符串操作相关方法扩展，以提升开发效率为目的。
    此文件主要定义 Contains 字符串包含判断的相关扩展方法。
*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace OYMLCN.Extensions
{
    public static partial class StringExtension
    {
        /// <summary>
        ///   返回一个值，该值指示指定的子串是否出现在此字符串中（忽略大小写）。
        /// </summary>
        /// <param name="str">被搜寻的字符串。</param>
        /// <param name="value">要搜寻的字符串。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="value" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <returns>
        ///   <para>如果当前字符串 <paramref name="str"/> 为 <see langword="null" />
        ///   或者空字符串 ("")，则始终为 false。</para>
        ///   <para>如果 <paramref name="value" /> 参数出现在此字符串中（忽略大小写），或者
        ///   <paramref name="value" /> 为空字符串 ("")，则为
        ///   <see langword="true" />；否则为 <see langword="false" />。</para>
        /// </returns>
        public static bool ContainsIgnoreCase(this string str, string value)
        {
            if (string.IsNullOrEmpty(str)) return false;
            return str.Contains(value, StringComparison.CurrentCultureIgnoreCase);
        }
        /// <summary>
        ///   返回一个值，该值指示指定的字符是否出现在此字符串中（忽略大小写）。
        /// </summary>
        /// <param name="str">被搜寻的字符串。</param>
        /// <param name="value">要查找的字符。</param>
        /// <returns>
        ///   <para>如果当前字符串 <paramref name="str"/> 为 <see langword="null" />
        ///   或者空字符串 ("")，则始终为 false。</para>
        ///   <para>如果 <paramref name="value" /> 参数在此字符串中出现（忽略大小写），则为
        ///   <see langword="true" />；否则为 <see langword="false" />。</para>
        /// </returns>
        public static bool ContainsIgnoreCase(this string str, char value)
        {
            if (string.IsNullOrEmpty(str)) return false;
            return str.Contains(value, StringComparison.CurrentCultureIgnoreCase);
        }


        /// <summary>
        ///   返回一个值，该值指示此字符串中是否出现指定的字符串集合的任意值
        /// </summary>
        /// <param name="str">被搜寻的字符串</param>
        /// <param name="words">要搜寻的字符串</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="words"/> 上声明的值包含 <see langword="null"/>
        /// </exception>
        /// <returns>
        ///   <para>如果被搜寻的字符串 <paramref name="str"/> 为
        ///   <see langword="null"/> 或 空字符串 ("") ，或者
        ///   <paramref name="words"/> 为空，则始终返回 <see langword="false"/>。</para>
        ///   <para>如果要搜寻的字符串集合的任意值出现在被搜寻的字符串中，或者
        ///   <paramref name="words"/> 包含空字符串 ("")，则为
        ///   <see langword="true"/>；否则为 <see langword="false"/>。</para>
        /// </returns>
        public static bool ContainsWords(this string str, params string[] words)
        {
            if (string.IsNullOrEmpty(str)) return false;
            return words.Any(word => str.Contains(word));
        }
        /// <summary>
        ///   返回一个值，该值指示此字符串中是否出现指定的字符串集合的任意值（忽略大小写）
        /// </summary>
        /// <param name="str">被搜寻的字符串</param>
        /// <param name="words">要搜寻的字符串</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="words"/> 上声明的值包含 <see langword="null"/>
        /// </exception>
        /// <returns>
        ///   <para>如果被搜寻的字符串 <paramref name="str"/> 为 <see langword="null"/>
        ///   或 空字符串 ("") ，或者 <paramref name="words"/>
        ///   为空，则始终返回 <see langword="false"/>。</para>
        ///   <para>如果要搜寻的字符串集合的任意值出现在被搜寻的字符串中（忽略大小写），或者
        ///   <paramref name="words"/> 包含空字符串 ("")，则为
        ///   <see langword="true"/>；否则为 <see langword="false"/>。</para>
        /// </returns>
        public static bool ContainsWordsIgnoreCase(this string str, params string[] words)
        {
            if (string.IsNullOrEmpty(str)) return false;
            return words.Any(word => str.Contains(word, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        ///   返回一个值，该值指示此字符串中是否出现指定的任意字符。</summary>
        /// <param name="str">被搜寻的字符串</param>
        /// <param name="chars">要查找的字符</param>
        /// <returns>
        ///   <para>如果被搜寻的字符串 <paramref name="str"/> 为
        ///   <see langword="null"/> 或 空字符串 ("") ，或者
        ///   <paramref name="chars"/> 为空，则始终返回
        ///   <see langword="false"/>。</para>
        ///   
        ///   <para>如果 <paramref name="chars" /> 任意字符在此字符串中出现，则为
        ///   <see langword="true" />，否则为 <see langword="false" />。</para>
        /// </returns>
        public static bool ContainsChars(this string str, params char[] chars)
        {
            if (string.IsNullOrEmpty(str)) return false;
            return chars.Any(@char => str.Contains(@char));
        }
        /// <summary>
        ///   返回一个值，该值指示此字符串中是否出现指定的任意字符（忽略大小写）。</summary>
        /// <param name="str">被搜寻的字符串</param>
        /// <param name="chars">要查找的字符</param>
        /// <returns>
        ///   <para>如果被搜寻的字符串 <paramref name="str"/> 为 <see langword="null"/>
        ///   或 空字符串 ("") ，或者 <paramref name="chars"/>
        ///   为空，则始终返回 <see langword="false"/>。</para>
        ///   
        ///   <para>如果 <paramref name="chars" /> 任意字符在此字符串中出现（忽略大小写），则为
        ///   <see langword="true" />，否则为 <see langword="false" />。</para>
        /// </returns>
        public static bool ContainsCharsIgnoreCase(this string str, params char[] chars)
        {
            if (string.IsNullOrEmpty(str)) return false;
            return chars.Any(@char => str.Contains(@char, StringComparison.CurrentCultureIgnoreCase));
        }


        /// <summary>
        ///   返回一个值，该值指示此字符串集合中是否出现指定的字符串集合的任意值
        /// </summary>
        /// <param name="values">被搜寻字符串集合</param>
        /// <param name="words">要搜寻的字符串对象</param>
        /// <returns>
        ///   如果要搜寻的字符串在搜寻对象字符串中，则为
        ///   <see langword="true"/>，否则为 <see langword="false"/>。
        /// </returns>
        public static bool ContainsWords(this IEnumerable<string> values, params string[] words)
        {
            if (values?.Any() == false) return false;
            return words?.Any(word => values.Any(val => val == word)) ?? false;
        }

    }
}