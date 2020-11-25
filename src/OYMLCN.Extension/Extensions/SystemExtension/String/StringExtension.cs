/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.cs
Author: VicBilibily
Description: 
    本代码主要定义一些内置于 System.String 的常用方法扩展，以提升开发效率为目的。
    本代码扩展方法命名与 System.String 内的方法名称一致，不一致的另行创建代码文件。
    由于命名与内置方法命名一致，如果引用了其他程序集存在同名扩展，可能会存在扩展冲突。
*****************************************************************************/

using System;
using System.Collections.Generic;

namespace OYMLCN.Extensions
{
    /// <summary>
    ///   字符串 <see cref="string"/> 类型扩展
    /// </summary>
    public static partial class SystemStringExtension
    {
        /// <summary> 
        ///   检查当前字符串 是 <see langword="null" /> 或者是 空字符串 ("")
        /// </summary>
        /// <param name="value">要测试的字符串</param>
        /// <returns>
        ///   如果 <paramref name="value"/> 为 <see langword="null" /> 或空字符串 ("") 则为
        ///   <see langword="true" />，否则为 <see langword="false" />。
        /// </returns>
        public static bool IsNullOrEmpty(this string value)
            => string.IsNullOrEmpty(value);
        /// <summary> 
        ///   检查当前字符串 是 <see langword="null" />、空字符串 ("") 或是仅由空白字符组成
        /// </summary>
        /// <param name="value">要测试的字符串</param>
        /// <returns>
        ///   如果 <paramref name="value"/> 为 <see langword="null" />
        ///   或空字符串 ("") 或仅由空白字符组成则为
        ///   <see langword="true" />，否则为 <see langword="false" />。
        /// </returns>
        public static bool IsNullOrWhiteSpace(this string value)
            => string.IsNullOrWhiteSpace(value);

        #region Concat 扩展
        /// <summary>
        ///   串联类型为 <see cref="IEnumerable{T}" /> 的 <see cref="string" /> 构造集合的成员。
        /// </summary>
        /// <param name="values">
        ///   一个集合对象，该对象实现 <see cref="IEnumerable{T}" />，且其泛型类型参数为 <see cref="string" />。
        /// </param>
        /// <exception cref = "ArgumentNullException" >
        ///   <paramref name="values" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <returns>
        ///   由 <paramref name="values" /> 中的字符串集合串联而成的字符串，如果
        ///   <paramref name="values" /> 为空集合，则为 <see cref="string.Empty" />。
        /// </returns>
        public static string Concat(this IEnumerable<string> values)
            => string.Concat(values);
        /// <summary>
        ///   串联 <see cref="IEnumerable{T}" /> 实现的成员。
        /// </summary>
        /// <param name="values">
        ///   一个实现 <see cref="IEnumerable{T}" /> 接口的集合对象。
        /// </param>
        /// <typeparam name="T"> <paramref name="values" /> 成员的类型。 </typeparam>
        /// <exception cref="ArgumentNullException" >
        ///   <paramref name="values" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <returns>
        ///   <paramref name="values" /> 中的串联成员。
        /// </returns>
        public static string Concat<T>(this IEnumerable<T> values)
            => string.Concat(values);

        /// <summary>
        ///   创建指定对象的字符串表示形式。
        /// </summary>
        /// <param name="arg0">
        ///   要表示的对象，或 <see langword="null" />。
        /// </param>
        /// <returns>
        ///   <paramref name="arg0" /> 的值的字符串表示形式，
        ///   如果 <paramref name="arg0" /> 为 <see langword="null" />，
        ///   则为 <see cref="string.Empty" />。
        /// </returns>
        public static string Concat(this object arg0)
            => string.Concat(arg0);
        /// <summary>
        ///   连接两个指定对象的字符串表示形式。
        /// </summary>
        /// <param name="arg0">要连接的第一个对象。</param>
        /// <param name="arg1">要连接的第二个对象。</param>
        /// <returns>
        ///   <paramref name="arg0" /> 和 <paramref name="arg1" />
        ///   的值的串联字符串表示形式。
        /// </returns>
        public static string Concat(this object arg0, object arg1)
            => string.Concat(arg0, arg1);
        /// <summary>
        ///   连接三个指定对象的字符串表示形式。
        /// </summary>
        /// <param name="arg0">要连接的第一个对象。</param>
        /// <param name="arg1">要连接的第二个对象。</param>
        /// <param name="arg2">要连接的第三个对象。</param>
        /// <returns>
        ///   <paramref name="arg0" />、<paramref name="arg1" /> 和
        ///   <paramref name="arg2" /> 的值的串联字符串表示形式。
        /// </returns>
        public static string Concat(this object arg0, object arg1, object arg2)
            => string.Concat(arg0, arg1, arg2);
        /// <summary>
        ///   串联当前 <see cref="object" /> 数组中的元素的字符串表示形式。
        /// </summary>
        /// <param name="arg0">
        ///   要表示的初始对象，或 <see langword="null" />。
        /// </param>
        /// <param name="args">
        ///   一个对象数组，其中包含要连接的元素。
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="args" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="OutOfMemoryException">内存不足。</exception>
        /// <returns>
        ///   <paramref name="arg0" /> 的值的字符串表示形式 和 <paramref name="args" />
        ///   中元素的值的串联字符串表示形式 合并串联的字符串。
        /// </returns>
        public static string Concat(this object arg0, params object[] args)
            => string.Concat(arg0, string.Concat(args));

        /// <summary>
        ///   连接 <see cref="string" /> 的两个指定实例。
        /// </summary>
        /// <param name="str0">要串联的第一个字符串。</param>
        /// <param name="str1">要串联的第二个字符串。</param>
        /// <returns>
        ///   <paramref name="str0" /> 和 <paramref name="str1" /> 的串联。
        /// </returns>
        public static string Concat(this string str0, string str1)
            => string.Concat(str0, str1);

        /// <summary>
        ///   连接 <see cref="string" /> 的三个指定实例。
        /// </summary>
        /// <param name="str0">要串联的第一个字符串。</param>
        /// <param name="str1">要串联的第二个字符串。</param>
        /// <param name="str2">要比较的第三个字符串。</param>
        /// <returns>
        ///   <paramref name="str0" />、<paramref name="str1" /> 和
        ///   <paramref name="str2" /> 的串联。
        /// </returns>
        public static string Concat(this string str0, string str1, string str2)
            => string.Concat(str0, str1, str2);
        /// <summary>
        ///   连接 <see cref="string" /> 的四个指定实例。
        /// </summary>
        /// <param name="str0">要串联的第一个字符串。</param>
        /// <param name="str1">要串联的第二个字符串。</param>
        /// <param name="str2">要比较的第三个字符串。</param>
        /// <param name="str3">要比较的第四个字符串。</param>
        /// <returns>
        ///   <paramref name="str0" />、<paramref name="str1" />、<paramref name="str2" />
        ///   和 <paramref name="str3" /> 的串联。
        /// </returns>
        public static string Concat(this string str0, string str1, string str2, string str3)
            => string.Concat(str0, str1, str2, str3);
        /// <summary>
        ///   连接指定的 <see cref="string" /> 数组的元素。
        /// </summary>
        /// <param name="str0">要串联的第一个初始字符串。</param>
        /// <param name="values">字符串实例的数组。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="values" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="OutOfMemoryException">内存不足。</exception>
        /// <returns>
        ///   <paramref name="str0" /> 和 <paramref name="values" /> 的串联元素 的串联。
        /// </returns>
        public static string Concat(this string str0, params string[] values)
            => string.Concat(str0, string.Concat(values));
        #endregion

        #region Format 扩展
        /// <summary>
        ///   将字符串中的一个或多个格式项替换为指定对象的字符串表示形式。
        /// </summary>
        /// <param name="format">复合格式字符串。</param>
        /// <param name="arg0">要设置格式的对象。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="format" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="format" /> 中的格式项无效 或 格式项的索引不为0。
        /// </exception>
        /// <returns>
        ///   <paramref name="format" /> 的副本，其中的任何格式项均替换为
        ///   <paramref name="arg0" /> 的字符串表示形式。
        /// </returns>
        public static string Format(this string format, object arg0)
            => string.Format(format, arg0);
        /// <summary>
        ///   将字符串中的格式项替换为两个指定对象的字符串表示形式。
        /// </summary>
        /// <param name="format">复合格式字符串。</param>
        /// <param name="arg0">要设置格式的第一个对象。</param>
        /// <param name="arg1">要设置格式的第二个对象。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="format" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="format" /> 无效 或 格式项的索引不为0或1。
        /// </exception>
        /// <returns>
        ///   <paramref name="format" /> 的副本，其中的格式项替换为
        ///   <paramref name="arg0" /> 和 <paramref name="arg1" /> 的字符串表示形式。
        /// </returns>
        public static string Format(this string format, object arg0, object arg1)
            => string.Format(format, arg0, arg1);
        /// <summary>
        ///   将字符串中的格式项替换为三个指定对象的字符串表示形式。
        /// </summary>
        /// <param name="format">复合格式字符串。</param>
        /// <param name="arg0">要设置格式的第一个对象。</param>
        /// <param name="arg1">要设置格式的第二个对象。</param>
        /// <param name="arg2">要设置格式的第三个对象。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="format" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="format" /> 无效 或 格式项的索引小于0，或者大于2。
        /// </exception>
        /// <returns>
        ///   <paramref name="format" /> 的副本，其中的格式项已替换为
        ///   <paramref name="arg0" />、<paramref name="arg1" />
        ///   和 <paramref name="arg2" /> 的字符串表示形式。
        /// </returns>
        public static string Format(this string format, object arg0, object arg1, object arg2)
            => string.Format(format, arg0, arg1, arg2);
        /// <summary>
        ///   将指定字符串中的格式项替换为指定数组中相应对象的字符串表示形式。
        /// </summary>
        /// <param name="format">复合格式字符串。</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="format" /> 或 <paramref name="args" /> 为 <see langword="null" />。
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="format" /> 无效 或 格式项的索引小于0，
        ///   或者大于或等于 <paramref name="args" /> 数组的长度。
        /// </exception>
        /// <returns>
        ///   <paramref name="format" /> 的副本，其中格式项已替换为
        ///   <paramref name="args" /> 中相应对象的字符串表示形式。
        /// </returns>
        public static string Format(this string format, params object[] args)
            => string.Format(format, args);
        #endregion

        #region Join 扩展
        /// <summary>
        ///   串联字符串数组的所有元素，其中在每个元素之间没有分隔符。
        /// </summary>
        /// <param name="values">一个数组，其中包含要连接的元素。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="values" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///   生成的字符串长度超出了允许的最大长度 (<see cref="int.MaxValue" />)。
        /// </exception>
        /// <returns>
        ///   <para>一个由 <paramref name="values" /> 中的元素组成的字符串。</para>
        ///   <para>如果 <paramref name="values" /> 为空数组，该方法将返回 <see cref="string.Empty" />。</para>
        /// </returns>
        public static string Join(this string[] values)
            => string.Join(string.Empty, values);
        /// <summary>
        ///   连接字符串数组，其中在每个成员之间使用指定的分隔符。
        /// </summary>
        /// <param name="separator">
        ///   要用作分隔符的字符。只有在 <paramref name="values" />
        ///   具有多个元素时，<paramref name="separator" /> 才包括在返回的字符串中。
        /// </param>
        /// <param name="values">要连接的字符串数组。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="values" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///   生成的字符串长度超出了允许的最大长度 (<see cref="int.MaxValue" />)。
        /// </exception>
        /// <returns>
        ///   <para>一个由 <paramref name="values" /> 的元素组成的字符串，这些元素以
        ///   <paramref name="separator" /> 字符分隔。</para>
        ///   <para>如果 <paramref name="values" /> 包含零个元素或
        ///   <paramref name="values" /> 的所有元素都为
        ///   <see langword="null" />，则为 <see cref="string.Empty" />。</para>
        /// </returns>
        public static string Join(this string[] values, char separator)
            => string.Join(separator, values);
        /// <summary>
        ///   连接字符串数组，其中在每个成员之间使用指定的分隔符，并且从数组
        ///   <paramref name="values" /> 位于 <paramref name="startIndex" />
        ///   中的元素开始，并连接多达 <paramref name="count" /> 个元素。
        /// </summary>
        /// <param name="separator">连接字符串数组，在每个成员之间使用的分隔符。</param>
        /// <param name="values">要连接的字符串数组。</param>
        /// <param name="startIndex">要连接的 <paramref name="values" /> 中的第一个项的索引。</param>
        /// <param name="count">
        ///   要连接的 <paramref name="values" /> 中的元素数，从位于
        ///   <paramref name="startIndex" /> 位置的元素开始。
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="values" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="startIndex" /> 或 <paramref name="count" /> 为负。
        ///   或
        ///   <paramref name="startIndex" /> 加上 <paramref name="count" />
        ///   大于 <paramref name="values" />中的元素数。
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///   生成的字符串长度超出了允许的最大长度 (<see cref="int.MaxValue" />)。
        /// </exception>
        /// <returns>
        ///   <para>一个由 <paramref name="values" /> 的元素组成的字符串，这些元素以
        ///   <paramref name="separator" /> 字符分隔。</para>
        ///   <para>如果 <paramref name="count" /> 为零，<paramref name="values" />
        ///   没有元素，或 <paramref name="values" /> 的全部元素均为 <see langword="null" />
        ///   或 <see cref="string.Empty" />，则为 <see cref="string.Empty" />。</para>
        /// </returns>
        public static string Join(this string[] values, char separator, int startIndex, int count)
            => string.Join(separator, values, startIndex, count);
        /// <summary>
        ///   串联字符串数组的所有元素，其中在每个元素之间使用指定的分隔符。
        /// </summary>
        /// <param name="separator">
        ///   要用作分隔符的字符串。只有在 <paramref name="values" />
        ///   具有多个元素时，<paramref name="separator" /> 才包括在返回的字符串中。
        /// </param>
        /// <param name="values">一个数组，其中包含要连接的元素。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="values" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///   生成的字符串长度超出了允许的最大长度 (<see cref="int.MaxValue" />)。
        /// </exception>
        /// <returns>
        ///   <para>一个由 <paramref name="values" /> 中的元素组成的字符串，这些元素以
        ///   <paramref name="separator" /> 字符串分隔。</para>
        ///   <para>如果 <paramref name="values" /> 为空数组，该方法将返回 <see cref="string.Empty" />。</para>
        /// </returns>
        public static string Join(this string[] values, string separator)
            => string.Join(separator, values);
        /// <summary>
        ///   串联字符串数组的指定元素，其中在每个元素之间使用指定的分隔符。
        /// </summary>
        /// <param name="separator">
        ///   要用作分隔符的字符串。只有在 <paramref name="values" />
        ///   具有多个元素时，<paramref name="separator" /> 才包括在返回的字符串中。
        /// </param>
        /// <param name="values">一个数组，其中包含要连接的元素。</param>
        /// <param name="startIndex">
        ///   <paramref name="values" /> 中要使用的第一个元素的索引。
        /// </param>
        /// <param name="count">要使用的 <paramref name="values" /> 的元素数。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="values" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="startIndex" /> 或 <paramref name="count" /> 小于 0。
        ///   或
        ///   <paramref name="startIndex" /> 加上 <paramref name="count" />
        ///   大于 <paramref name="values" />中的元素数。
        /// </exception>
        /// <exception cref="OutOfMemoryException">内存不足。</exception>
        /// <returns>
        ///   <para>由 <paramref name="values" /> 中的字符串组成的字符串，这些字符串以
        ///   <paramref name="separator" /> 字符串分隔。</para>
        ///   <para>如果 <paramref name="count" /> 为零，<paramref name="values" />
        ///   没有元素，或 <paramref name="separator" /> 以及 <paramref name="values" />
        ///   的全部元素均为 <see cref="string.Empty" />，则为 <see cref="string.Empty" />。</para>
        /// </returns>
        public static string Join(this string[] values, string separator, int startIndex, int count)
            => string.Join(separator, values, startIndex, count);
        /// <summary>
        ///   连接对象数组的字符串表示形式，其中在每个成员之间使用指定的分隔符。
        /// </summary>
        /// <param name="separator">
        ///   要用作分隔符的字符。只有在 <paramref name="values" />
        ///   具有多个元素时，<paramref name="separator" /> 才包括在返回的字符串中。
        /// </param>
        /// <param name="values">将连接其字符串表示形式的对象数组。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="values" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///   生成的字符串长度超出了允许的最大长度 (<see cref="int.MaxValue" />)。
        /// </exception>
        /// <returns>
        ///   <para>一个由 <paramref name="values" /> 的元素组成的字符串，这些元素以
        ///   <paramref name="separator" /> 字符分隔。</para>
        ///   <para>如果 <paramref name="values" /> 包含零个元素或 <paramref name="values" />
        ///   的所有元素都为 <see langword="null" />，则为 <see cref="string.Empty" />。</para>
        /// </returns>
        public static string Join(this object[] values, char separator)
            => string.Join(separator, values);
        /// <summary>
        ///   串联对象数组的各个元素，其中在每个元素之间使用指定的分隔符。
        /// </summary>
        /// <param name="separator">
        ///   要用作分隔符的字符串。只有在 <paramref name="values" />
        ///   具有多个元素时，<paramref name="separator" /> 才包括在返回的字符串中。
        /// </param>
        /// <param name="values">一个数组，其中包含要连接的元素。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="values" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///   生成的字符串长度超出了允许的最大长度 (<see cref="int.MaxValue" />)。
        /// </exception>
        /// <returns>
        ///   <para>一个由 <paramref name="values" /> 的元素组成的字符串，这些元素以
        ///   <paramref name="separator" /> 字符串分隔。</para>
        ///   <para>如果 <paramref name="values" /> 为空数组，该方法将返回
        ///   <see cref="string.Empty" />。</para>
        /// </returns>
        public static string Join(this object[] values, string separator)
            => string.Join(separator, values);

        /// <summary>
        ///   串联集合的成员，其中在每个成员之间没有分隔符。
        /// </summary>
        /// <param name="values">一个包含要串联的对象的集合。</param>
        /// <typeparam name="T">
        ///   <paramref name="values" /> 成员的类型。
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="values" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///   生成的字符串长度超出了允许的最大长度 (<see cref="int.MaxValue" />)。
        /// </exception>
        /// <returns>
        ///   <para>一个由 <paramref name="values" /> 的成员组成的字符串。</para>
        ///   <para>如果 <paramref name="values" /> 没有成员，则该方法返回
        ///   <see cref="string.Empty" />。</para>
        /// </returns>
        public static string Join<T>(this IEnumerable<T> values)
            => string.Join(string.Empty, values);
        /// <summary>
        ///   串联集合的成员，其中在每个成员之间使用指定的分隔符。
        /// </summary>
        /// <param name="separator">
        ///   要用作分隔符的字符。只有在 <paramref name="separator" />
        ///   具有多个元素时，<paramref name="values" /> 才包括在返回的字符串中。
        /// </param>
        /// <param name="values">一个包含要串联的对象的集合。</param>
        /// <typeparam name="T">
        ///   <paramref name="values" /> 成员的类型。
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="values" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///   生成的字符串长度超出了允许的最大长度 (<see cref="int.MaxValue" />)。
        /// </exception>
        /// <returns>
        ///   <para>一个由 <paramref name="values" /> 的成员组成的字符串，这些成员以
        ///   <paramref name="separator" /> 字符分隔。</para>
        ///   <para>如果 <paramref name="values" /> 没有成员，则该方法返回
        ///   <see cref="string.Empty" />。</para>
        /// </returns>
        public static string Join<T>(this IEnumerable<T> values, char separator)
            => string.Join(separator, values);
        /// <summary>
        ///   串联集合的成员，其中在每个成员之间使用指定的分隔符。
        /// </summary>
        /// <param name="separator">
        ///   要用作分隔符的字符串。只有在 <paramref name="separator" />
        ///   具有多个元素时，<paramref name="values" /> 才包括在返回的字符串中。
        /// </param>
        /// <param name="values">一个包含要串联的对象的集合。</param>
        /// <typeparam name="T">
        ///   <paramref name="values" /> 成员的类型。
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="values" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///   生成的字符串长度超出了允许的最大长度 (<see cref="int.MaxValue" />)。
        /// </exception>
        /// <returns>
        ///   <para>一个由 <paramref name="values" /> 的成员组成的字符串，这些成员以
        ///   <paramref name="separator" /> 字符串分隔。</para>
        ///   <para>如果 <paramref name="values" /> 没有成员，则该方法返回
        ///   <see cref="string.Empty" />。</para>
        /// </returns>
        public static string Join<T>(this IEnumerable<T> values, string separator)
            => string.Join(separator, values); 
        #endregion





        #region Obsolete
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        [Obsolete("建议改用标准用法" + nameof(IsNullOrEmpty) + "，将在下一主要版本(V6)中移除。")]
        public static bool IsNotNullOrEmpty(this string value)
            => string.IsNullOrEmpty(value) == false;
        [Obsolete("建议改用标准用法" + nameof(IsNullOrWhiteSpace) + "，将在下一主要版本(V6)中移除。")]
        public static bool IsNotNullOrWhiteSpace(this string value)
            => !string.IsNullOrWhiteSpace(value);



#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        #endregion

    }
}