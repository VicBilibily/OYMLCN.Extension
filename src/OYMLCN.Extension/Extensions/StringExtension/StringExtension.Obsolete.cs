/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.Obsolete.cs
Author: VicBilibily
Description: 
    本代码所在文件夹的文件主要定义一些的常用的字符串操作相关方法扩展，以提升开发效率为目的。
    本代码主要定义一些旧版本中存在，但并非常用或习惯用法的扩展方法，将在下一版本(V6)中移除。
*****************************************************************************/

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

using System;
using System.Collections.Generic;
using System.Linq;

namespace OYMLCN.Extensions
{
    public static partial class StringExtension
    {
        [Obsolete("将在下一主要版本(V6)中移除")]
        public static IEnumerable<string> WhereContains(this IEnumerable<string> source, params string[] words)
            => source.Where(item => item != null && item.ContainsWords(words));
        [Obsolete("将在下一主要版本(V6)中移除")]
        public static IEnumerable<string> WhereStartsWith(this IEnumerable<string> source, string value)
            => source.Where(item => item != null && item.StartsWith(value));
        [Obsolete("将在下一主要版本(V6)中移除")]
        public static IEnumerable<string> WhereEndsWith(this IEnumerable<string> source, string value)
            => source.Where(item => item != null && item.EndsWith(value));
        [Obsolete("将在下一主要版本(V6)中移除")]
        public static IEnumerable<string> WhereIsNotNullOrEmpty(this IEnumerable<string> source)
            => source.Where(item => item.IsNotNullOrEmpty());
        [Obsolete("将在下一主要版本(V6)中移除")]
        public static IEnumerable<string> WhereIsNotNullOrWhiteSpace(this IEnumerable<string> source)
            => source.Where(item => item.IsNotNullOrWhiteSpace());



        [Obsolete("建议改用推荐用法" + nameof(EqualsWords) + "，将在下一主要版本(V6)中移除。")]
        public static bool Equals(this string value, params string[] values)
            => EqualsWords(value, values);
        [Obsolete("建议改用推荐用法" + nameof(EqualsWordsIgnoreCase) + "，将在下一主要版本(V6)中移除。")]
        public static bool EqualsIgnoreCase(this string value, params string[] values)
            => EqualsWordsIgnoreCase(value, values);



        [Obsolete("建议改用 obj ??= value 语法糖写法，将在下一主要版本(V6)中移除。")]
        public static string IfNull(this string value, string defVal)
            => value ?? defVal;
        [Obsolete("建议改用标准用法" + nameof(SystemStringExtension.Format) + "，将在下一主要版本(V6)中移除。")]
        public static string StringFormat(this string format, params object[] args)
            => string.Format(format, args);

        [Obsolete("建议改用标准用法" + nameof(string.Substring) + "，将在下一主要版本(V6)中移除。")]
        public static string TakeSubString(this string value, int skipLength, int subLength = int.MaxValue)
            => new string(value.Skip(skipLength).Take(subLength).ToArray());



        [Obsolete("建议改用推荐用法" + nameof(StartsWithWords) + "，将在下一主要版本(V6)中移除。")]
        public static bool StartsWith(this string input, params string[] args)
            => StartsWithWords(input, args);
        [Obsolete("建议改用推荐用法" + nameof(StartsWithChars) + "，将在下一主要版本(V6)中移除。")]
        public static bool StartsWith(this string input, params char[] args)
            => StartsWithChars(input, args);
        [Obsolete("建议改用推荐用法" + nameof(EndsWithWords) + "，将在下一主要版本(V6)中移除。")]
        public static bool EndsWith(this string input, params string[] args)
            => EndsWithWords(input, args);
        [Obsolete("建议改用推荐用法" + nameof(EndsWithChars) + "，将在下一主要版本(V6)中移除。")]
        public static bool EndsWith(this string input, params char[] args)
            => EndsWithChars(input, args);



    }
}