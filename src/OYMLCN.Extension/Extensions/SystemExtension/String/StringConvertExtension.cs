/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.cs
Author: VicBilibily
Description: 
    本代码主要定义一些内置于 System.String 的常用方法扩展，以提升开发效率为目的。
    本代码扩展方法命名与 System.String 内的私有 IConvertible 的类型转换方法名称一致，
    另外额外补充 System.Convert 内的对应名称的方法，其他不一致的另行创建代码文件。
*****************************************************************************/

using System;

namespace OYMLCN.Extensions
{
    public static partial class SystemStringExtension
    {
        /// <summary>
        ///   将逻辑值的指定字符串表示形式转换为其等效的布尔值。
        /// </summary>
        /// <param name="value">
        ///   包含 <see cref="bool.TrueString" /> 或
        ///   <see cref="bool.FalseString" /> 值的字符串。
        /// </param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不等于 <see cref="bool.TrueString" /> 或
        ///   <see cref="bool.FalseString" />。
        /// </exception>
        /// <returns>
        ///   <para>如果 <paramref name="value" /> 等于 <see cref="bool.TrueString" />，则为
        ///   <see langword="true" />，</para>
        ///   <para>如果 <paramref name="value" /> 等于 <see cref="bool.FalseString" /> 或
        ///   <see langword="null" />，则为 <see langword="false" />。</para>
        /// </returns>
        public static bool ToBoolean(this string value)
            => Convert.ToBoolean(value);

        /// <summary>
        ///   将数字的指定字符串表示形式转换为等效的 8 位无符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="byte.MinValue" />
        ///   或大于 <see cref="byte.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 等效的 8 位无符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为零。
        /// </returns>
        public static byte ToByte(this string value)
            => Convert.ToByte(value);
        /// <summary>
        ///   使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的 8 位无符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="byte.MinValue" />
        ///   或大于 <see cref="byte.MaxValue" /> 的数字。</exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 等效的 8 位无符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为零。
        /// </returns>
        public static byte ToByte(this string value, IFormatProvider provider)
            => Convert.ToByte(value, provider);
        /// <summary>
        ///   将指定基数的数字的字符串表示形式转换为等效的 8 位无符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="fromBase">
        ///   <paramref name="value" /> 中数字的基数，它必须是 2、8、10 或 16。
        /// </param>
        /// <exception cref="ArgumentException">
        ///   <paramref name="fromBase" /> 不是 2、8、10 或 16。  
        ///   或 
        ///   <paramref name="value" />（表示非 10 为基数的无符号数字）的前面带一个负号。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value" /> 上声明为默认值 <see cref="string.Empty" />。
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 包含一个字符，该字符不是由 <paramref name="fromBase" /> 指定的基数中的有效数字。
        ///   如果 <paramref name="value" /> 中的第一个字符无效，则该异常消息指示没有要转换的数字；
        ///   否则，该消息指示 <paramref name="value" /> 包含无效的尾随字符。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" />（表示 10 为基的无符号数字）的前面带一个负号。  
        ///   或 
        ///   <paramref name="value" /> 表示一个小于 <see cref="byte.MinValue" />
        ///   或大于 <see cref="byte.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   与 <paramref name="value" /> 中数字等效的 8 位无符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static byte ToByte(this string value, int fromBase)
            => Convert.ToByte(value, fromBase);

        /// <summary>
        ///   将指定字符串的第一个字符转换为 Unicode 字符。
        /// </summary>
        /// <param name="value">长度为 1 的字符串。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="value" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 的长度不是 1。
        /// </exception>
        /// <returns>
        ///   与 <paramref name="value" /> 中第一个且仅有的字符等效的 Unicode 字符。
        /// </returns>
        public static char ToChar(this string value)
            => Convert.ToChar(value);

        /// <summary>
        ///   将数字的指定字符串表示形式转换为等效的十进制数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不是一个具有有效格式的数字。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="decimal.MinValue" />
        ///   或大于 <see cref="decimal.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   与 <paramref name="value" /> 中数字等效的十进制数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static decimal ToDecimal(this string value)
            => Convert.ToDecimal(value);
        /// <summary>
        ///   使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的十进制数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不是一个具有有效格式的数字。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="decimal.MinValue" />
        ///   或大于 <see cref="decimal.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   与 <paramref name="value" /> 中数字等效的十进制数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static decimal ToDecimal(this string value, IFormatProvider provider)
            => Convert.ToDecimal(value, provider);

        /// <summary>
        ///   将数字的指定字符串表示形式转换为等效的双精度浮点数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不是一个具有有效格式的数字。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="double.MinValue" />
        ///   或大于 <see cref="double.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   与 <paramref name="value" /> 中数字等效的双精度浮点数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static double ToDouble(this string value)
            => Convert.ToDouble(value);
        /// <summary>
        ///   使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的双精度浮点数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不是一个具有有效格式的数字。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="double.MinValue" />
        ///   或大于 <see cref="double.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   与 <paramref name="value" /> 中数字等效的双精度浮点数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static double ToDouble(this string value, IFormatProvider provider)
            => Convert.ToDouble(value, provider);

        /// <summary>
        ///   将数字的指定字符串表示形式转换为等效的 16 位带符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。</exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="short.MinValue" />
        ///   或大于 <see cref="short.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 16 位带符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static short ToInt16(this string value)
            => Convert.ToInt16(value);
        /// <summary>
        ///   使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的 16 位带符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="short.MinValue" />
        ///   或大于 <see cref="short.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 16 位带符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static short ToInt16(this string value, IFormatProvider provider)
            => Convert.ToInt16(value, provider);
        /// <summary>
        ///   将指定基数的数字的字符串表示形式转换为等效的 16 位有符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="fromBase">
        ///   <paramref name="value" /> 中数字的基数，它必须是 2、8、10 或 16。
        /// </param>
        /// <exception cref="ArgumentException">
        ///   <paramref name="fromBase" /> 不是 2、8、10 或 16。  
        ///   或 
        ///   <paramref name="value" />（表示基数不为 10 的符号数字）的前缀为负号。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value" /> 上声明的默认值为 <see cref="string.Empty" />。
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 包含一个字符，该字符不是由 <paramref name="fromBase" />
        ///   指定的基数中的有效数字。 如果 <paramref name="value" />
        ///   中的第一个字符无效，则该异常消息指示没有要转换的数字；否则，该消息指示
        ///   <paramref name="value" /> 包含无效的尾随字符。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" />（表示基数不为 10 的符号数字）的前缀为负号。  
        ///   或 
        ///   <paramref name="value" /> 表示一个小于 <see cref="short.MinValue" />
        ///   或大于 <see cref="short.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 16 位带符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static short ToInt16(this string value, int fromBase)
            => Convert.ToInt16(value, fromBase);

        /// <summary>
        ///   将数字的指定字符串表示形式转换为等效的 32 位带符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。</exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="int.MinValue" />
        ///   或大于 <see cref="int.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 32 位带符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static int ToInt32(this string value)
            => Convert.ToInt32(value);
        /// <summary>
        ///   使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的 32 位带符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="int.MinValue" />
        ///   或大于 <see cref="int.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 32 位带符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static int ToInt32(this string value, IFormatProvider provider)
            => Convert.ToInt32(value, provider);
        /// <summary>
        ///   将指定基数的数字的字符串表示形式转换为等效的 32 位有符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="fromBase">
        ///   <paramref name="value" /> 中数字的基数，它必须是 2、8、10 或 16。
        /// </param>
        /// <exception cref="ArgumentException">
        ///   <paramref name="fromBase" /> 不是 2、8、10 或 16。  
        ///   或 
        ///   <paramref name="value" />（表示基数不为 10 的符号数字）的前缀为负号。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value" /> 上声明的默认值为 <see cref="string.Empty" />。
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 包含一个字符，该字符不是由 <paramref name="fromBase" />
        ///   指定的基数中的有效数字。如果 <paramref name="value" />
        ///   中的第一个字符无效，则该异常消息指示没有要转换的数字；否则，该消息指示
        ///   <paramref name="value" /> 包含无效的尾随字符。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" />（表示基数不为 10 的符号数字）的前缀为负号。  
        ///   或 
        ///   <paramref name="value" /> 表示一个小于 <see cref="int.MinValue" />
        ///   或大于 <see cref="int.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 32 位带符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static int ToInt32(this string value, int fromBase)
            => Convert.ToInt32(value, fromBase);

        /// <summary>
        ///   将数字的指定字符串表示形式转换为等效的 64 位带符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="long.MinValue" />
        ///   或大于 <see cref="long.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 64 位带符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static long ToInt64(this string value)
            => Convert.ToInt64(value);
        /// <summary>
        ///   使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的 64 位带符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="long.MinValue" />
        ///   或大于 <see cref="long.MaxValue" /> 的数字。</exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 64 位带符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static long ToInt64(this string value, IFormatProvider provider)
            => Convert.ToInt64(value, provider);
        /// <summary>
        ///   将指定基数的数字的字符串表示形式转换为等效的 64 位有符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="fromBase">
        ///   <paramref name="value" /> 中数字的基数，它必须是 2、8、10 或 16。
        /// </param>
        /// <exception cref="ArgumentException">
        ///   <paramref name="fromBase" /> 不是 2、8、10 或 16。  
        ///   或 
        ///   <paramref name="value" />（表示基数不为 10 的符号数字）的前缀为负号。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value" /> 上声明的默认值为 <see cref="string.Empty" />。
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 包含一个字符，该字符不是由 <paramref name="fromBase" />
        ///   指定的基数中的有效数字。 如果 <paramref name="value" />
        ///   中的第一个字符无效，则该异常消息指示没有要转换的数字；否则，该消息指示
        ///   <paramref name="value" /> 包含无效的尾随字符。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" />（表示基数不为 10 的符号数字）的前缀为负号。  
        ///   或 
        ///   <paramref name="value" /> 表示一个小于 <see cref="long.MinValue" />
        ///   或大于 <see cref="long.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 64 位带符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static long ToInt64(this string value, int fromBase)
            => Convert.ToInt64(value, fromBase);

        /// <summary>
        ///   将数字的指定字符串表示形式转换为等效的 8 位带符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="sbyte.MinValue" />
        ///   或大于 <see cref="sbyte.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   与 <paramref name="value" /> 中数字等效的 8 位带符号整数，如果值为
        ///   <see langword="null" />，则为 0（零）。
        /// </returns>
        public static sbyte ToSByte(this string value)
            => Convert.ToSByte(value);
        /// <summary>
        ///   使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的 8 位带符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="value" /> 上声明的默认值为 <see langword="null" />。
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="sbyte.MinValue" />
        ///   或大于 <see cref="sbyte.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 等效的 8 位带符号整数。
        /// </returns>
        public static sbyte ToSByte(this string value, IFormatProvider provider)
            => Convert.ToSByte(value, provider);
        /// <summary>
        ///   将指定基数的数字的字符串表示形式转换为等效的 8 位有符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="fromBase">
        ///   <paramref name="value" /> 中数字的基数，它必须是 2、8、10 或 16。
        /// </param>
        /// <exception cref="ArgumentException">
        ///   <paramref name="fromBase" /> 不是 2、8、10 或 16。  
        ///   或 
        ///   <paramref name="value" />（表示基数不为 10 的符号数字）的前缀为负号。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value" /> 上声明的默认值为 <see cref="string.Empty" />。
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 包含一个字符，该字符不是由 <paramref name="fromBase" />
        ///   指定的基数中的有效数字。如果 <paramref name="value" />
        ///   中的第一个字符无效，则该异常消息指示没有要转换的数字；否则，该消息指示
        ///   <paramref name="value" /> 包含无效的尾随字符。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" />（表示基数不为 10 的符号数字）的前缀为负号。  
        ///   或 
        ///   <paramref name="value" /> 表示一个小于 <see cref="sbyte.MinValue" />
        ///   或大于 <see cref="sbyte.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   与 <paramref name="value" /> 中数字等效的 8 位带符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static sbyte ToSByte(this string value, int fromBase)
            => Convert.ToSByte(value, fromBase);

        /// <summary>
        ///   将数字的指定字符串表示形式转换为等效的单精度浮点数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不是一个具有有效格式的数字。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="float.MinValue" />
        ///   或大于 <see cref="float.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   与 <paramref name="value" /> 中数字等效的单精度浮点数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static float ToSingle(this string value)
            => Convert.ToSingle(value);
        /// <summary>
        ///   使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的单精度浮点数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不是一个具有有效格式的数字。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="float.MinValue" />
        ///   或大于 <see cref="float.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   与 <paramref name="value" /> 中数字等效的单精度浮点数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static float ToSingle(this string value, IFormatProvider provider)
            => Convert.ToSingle(value, provider);

        /// <summary>
        ///   将数字的指定字符串表示形式转换为等效的 16 位无符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="ushort.MinValue" />
        ///   或大于 <see cref="ushort.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 16 位无符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static ushort ToUInt16(this string value)
           => Convert.ToUInt16(value);
        /// <summary>
        ///   使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的 16 位无符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="ushort.MinValue" />
        ///   或大于 <see cref="ushort.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 16 位无符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static ushort ToUInt16(this string value, IFormatProvider provider)
            => Convert.ToUInt16(value, provider);
        /// <summary>
        ///   将指定基数的数字的字符串表示形式转换为等效的 16 位无符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="fromBase">
        ///   <paramref name="value" /> 中数字的基数，它必须是 2、8、10 或 16。
        /// </param>
        /// <exception cref="ArgumentException">
        ///   <paramref name="fromBase" /> 不是 2、8、10 或 16。  
        ///   或 
        ///   <paramref name="value" />（表示非 10 为基数的无符号数字）的前面带一个负号。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value" /> 上声明的默认值为 <see cref="string.Empty" />。
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 包含一个字符，该字符不是由 <paramref name="fromBase" />
        ///   指定的基数中的有效数字。如果 <paramref name="value" />
        ///   中的第一个字符无效，则该异常消息指示没有要转换的数字；否则，该消息指示
        ///   <paramref name="value" /> 包含无效的尾随字符。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" />（表示非 10 为基数的无符号数字）的前面带一个负号。  
        ///   或 
        ///   <paramref name="value" /> 表示一个小于 <see cref="ushort.MinValue" />
        ///   或大于 <see cref="ushort.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 16 位无符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static ushort ToUInt16(this string value, int fromBase)
            => Convert.ToUInt16(value, fromBase);

        /// <summary>
        ///   将数字的指定字符串表示形式转换为等效的 32 位无符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="uint.MinValue" />
        ///   或大于 <see cref="uint.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 32 位无符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static uint ToUInt32(this string value)
           => Convert.ToUInt32(value);
        /// <summary>
        ///   使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的 32 位无符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="uint.MinValue" />
        ///   或大于 <see cref="uint.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 32 位无符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static uint ToUInt32(this string value, IFormatProvider provider)
            => Convert.ToUInt32(value, provider);
        /// <summary>
        ///   将指定基数的数字的字符串表示形式转换为等效的 32 位无符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="fromBase">
        ///   <paramref name="value" /> 中数字的基数，它必须是 2、8、10 或 16。
        /// </param>
        /// <exception cref="ArgumentException">
        ///   <paramref name="fromBase" /> 不是 2、8、10 或 16。  
        ///   或 
        ///   <paramref name="value" />（表示非 10 为基数的无符号数字）的前面带一个负号。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value" /> 上声明的默认值为 <see cref="string.Empty" />。
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 包含一个字符，该字符不是由 <paramref name="fromBase" />
        ///   指定的基数中的有效数字。如果 <paramref name="value" />
        ///   中的第一个字符无效，则该异常消息指示没有要转换的数字；否则，该消息指示
        ///   <paramref name="value" /> 包含无效的尾随字符。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" />（表示非 10 为基数的无符号数字）的前面带一个负号。  
        ///   或 
        ///   <paramref name="value" /> 表示一个小于 <see cref="uint.MinValue" />
        ///   或大于 <see cref="uint.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 32 位无符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static uint ToUInt32(this string value, int fromBase)
            => Convert.ToUInt32(value, fromBase);

        /// <summary>
        ///   将数字的指定字符串表示形式转换为等效的 64 位无符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="ulong.MinValue" />
        ///   或大于 <see cref="ulong.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 64 位带符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static ulong ToUInt64(this string value)
          => Convert.ToUInt64(value);
        /// <summary>
        ///   使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的 64 位无符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不由一个可选符号后跟一系列数字 (0-9) 组成。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" /> 表示一个小于 <see cref="ulong.MinValue" />
        ///   或大于 <see cref="ulong.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 64 位无符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static ulong ToUInt64(this string value, IFormatProvider provider)
            => Convert.ToUInt64(value, provider);
        /// <summary>
        ///   将指定基数的数字的字符串表示形式转换为等效的 64 位无符号整数。
        /// </summary>
        /// <param name="value">包含要转换的数字的字符串。</param>
        /// <param name="fromBase">
        ///   <paramref name="value" /> 中数字的基数，它必须是 2、8、10 或 16。
        /// </param>
        /// <exception cref="ArgumentException">
        ///   <paramref name="fromBase" /> 不是 2、8、10 或 16。  
        ///   或 
        ///   <paramref name="value" />（表示非 10 为基数的无符号数字）的前面带一个负号。
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   <paramref name="value" /> 上声明的默认值为 <see cref="string.Empty" />。
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 包含一个字符，该字符不是由 <paramref name="fromBase" />
        ///   指定的基数中的有效数字。如果 <paramref name="value" />
        ///   中的第一个字符无效，则该异常消息指示没有要转换的数字；否则，该消息指示
        ///   <paramref name="value" /> 包含无效的尾随字符。
        /// </exception>
        /// <exception cref="OverflowException">
        ///   <paramref name="value" />（表示非 10 为基数的无符号数字）的前面带一个负号。  
        ///   或 
        ///   <paramref name="value" /> 表示一个小于 <see cref="ulong.MinValue" />
        ///   或大于 <see cref="ulong.MaxValue" /> 的数字。
        /// </exception>
        /// <returns>
        ///   一个与 <paramref name="value" /> 中数字等效的 64 位无符号整数，如果
        ///   <paramref name="value" /> 为 <see langword="null" />，则为 0（零）。
        /// </returns>
        public static ulong ToUInt64(this string value, int fromBase)
            => Convert.ToUInt64(value, fromBase);

        /// <summary>
        ///   将日期和时间的指定字符串表示形式转换为等效的日期和时间值。
        /// </summary>
        /// <param name="value">日期和时间的字符串表示形式。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不是格式正确的日期和时间字符串。
        /// </exception>
        /// <returns>
        ///   <paramref name="value" /> 的值的日期和时间等效项，如果 <paramref name="value" /> 为
        ///   <see langword="null" />，则为 <see cref="DateTime.MinValue" /> 的日期和时间等效项。
        /// </returns>
        public static DateTime ToDateTime(this string value)
            => Convert.ToDateTime(value);
        /// <summary>
        ///   使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的日期和时间。
        /// </summary>
        /// <param name="value">包含要转换的日期和时间的字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <exception cref="FormatException">
        ///   <paramref name="value" /> 不是格式正确的日期和时间字符串。
        /// </exception>
        /// <returns>
        ///   <paramref name="value" /> 的值的日期和时间等效项，如果 <paramref name="value" /> 为
        ///   <see langword="null" />，则为 <see cref="DateTime.MinValue" /> 的日期和时间等效项。
        /// </returns>
        public static DateTime ToDateTime(this string value, IFormatProvider provider)
            => Convert.ToDateTime(value, provider);



    }
}
