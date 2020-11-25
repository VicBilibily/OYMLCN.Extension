/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.Contains.cs
Author: VicBilibily
Description: 
    本代码所在文件夹的文件主要定义一些的常用的字符串操作相关方法扩展，以提升开发效率为目的。
    此文件主要定义字符串条件判断相关的值返回扩展方法。
*****************************************************************************/

namespace OYMLCN.Extensions
{
    public static partial class StringExtension
    {
        /// <summary> 
        ///   如果当前字符串 <paramref name="str"/> 是 <see langword="null" />
        ///   或者是空字符串 ("") 时返回 <paramref name="value"/> 的值。
        /// </summary>
        /// <param name="str">被测试的字符串</param>
        /// <param name="value">
        ///   当 <paramref name="str"/> 为 <see langword="null"/>
        ///   或空字符串 ("") 时返回的值
        /// </param>
        /// <returns>
        ///   如果 <paramref name="str"/> 为 <see langword="null" /> 或空字符串 ("") 则为
        ///   <paramref name="value"/> 的值，否则返回原值 <paramref name="str"/>。
        /// </returns>
        public static string IfNullOrEmpty(this string str, string value)
            => string.IsNullOrEmpty(str) ? value : str;
        /// <summary> 
        ///   如果当前字符串 <paramref name="str"/> 是
        ///   <see langword="null" />、空字符串 ("") 或是仅由空白字符组成时返回
        ///   <paramref name="value"/> 的值。
        /// </summary>
        /// <param name="str">被测试的字符串</param>
        /// <param name="value">
        ///   当 <paramref name="str"/> 为
        ///   <see langword="null"/>、空字符串 ("") 或是仅由空白字符组成时返回空字符串 ("") 时返回的值
        /// </param>
        /// <returns>
        ///   如果 <paramref name="value"/> 为 <see langword="null" />
        ///   或空字符串 ("") 或仅由空白字符组成则为
        ///   <paramref name="value"/> 的值，否则返回原值 <paramref name="str"/>。
        /// </returns>
        public static string IfNullOrWhiteSpace(this string str, string value)
            => string.IsNullOrWhiteSpace(str) ? value : str;


    }
}