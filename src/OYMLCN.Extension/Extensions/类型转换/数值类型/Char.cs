﻿using System;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        #region public static char ConvertToChar(this string value)
        /// <summary>
        /// 将指定字符串的第一个字符转换为 Unicode 字符
        /// </summary>
        /// <param name="value"> 长度为 1 的字符串 </param>
        /// <returns> 与 <paramref name="value"/> 中第一个且仅有的字符等效的 Unicode 字符 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 为 null </exception>
        /// <exception cref="FormatException"> <paramref name="value"/> 的长度不是 1 </exception>
        public static char ConvertToChar(this string value)
            => Convert.ToChar(value);
        #endregion
        #region public static char ToChar(this byte value)
        /// <summary>
        /// 将指定的 8 位无符号整数的值转换为其等效的 Unicode 字符
        /// </summary>
        /// <param name="value"> 要转换的 8 位无符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的 Unicode 字符 </returns>
        public static char ToChar(this byte value)
            => Convert.ToChar(value);
        #endregion
        #region public static char ToChar(this sbyte value)
        /// <summary>
        /// 将指定的 8 位有符号整数的值转换为它的等效 Unicode 字符
        /// </summary>
        /// <param name="value"> 要转换的 8 位带符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的 Unicode 字符 </returns>
        /// <exception cref="OverflowException"> <paramref name="value"/> 小于 <see cref="char.MinValue"/> </exception>
        public static char ToChar(this sbyte value)
            => Convert.ToChar(value);
        #endregion
        #region public static char ToChar(this short value)
        /// <summary>
        /// 将指定的 16 位有符号整数的值转换为它的等效 Unicode 字符
        /// </summary>
        /// <param name="value"> 要转换的 16 位带符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的 Unicode 字符 </returns>
        /// <exception cref="OverflowException"> <paramref name="value"/> 小于 <see cref="char.MinValue"/> </exception>
        public static char ToChar(this short value)
            => Convert.ToChar(value);
        #endregion
        #region public static char ToChar(this int value)
        /// <summary>
        /// 将指定的 32 位有符号整数的值转换为它的等效 Unicode 字符
        /// </summary>
        /// <param name="value"> 要转换的 32 位带符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的 Unicode 字符 </returns>
        /// <exception cref="OverflowException"> <paramref name="value"/> 小于 <see cref="char.MinValue"/> 或大于 <see cref="char.MaxValue"/> </exception>
        public static char ToChar(this int value)
            => Convert.ToChar(value);
        #endregion
        #region public static char ToChar(this long value)
        /// <summary>
        /// 将指定的 64 位有符号整数的值转换为它的等效 Unicode 字符
        /// </summary>
        /// <param name="value"> 要转换的 64 位带符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的 Unicode 字符 </returns>
        /// <exception cref="OverflowException"> <paramref name="value"/> 小于 <see cref="char.MinValue"/> 或大于 <see cref="char.MaxValue"/> </exception>
        public static char ToChar(this long value)
            => Convert.ToChar(value);
        #endregion
        #region public static char ToChar(this ushort value)
        /// <summary>
        /// 将指定的 16 位无符号整数的值转换为其等效的 Unicode 字符
        /// </summary>
        /// <param name="value"> 要转换的 16 位无符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的 Unicode 字符 </returns>
        public static char ToChar(this ushort value)
            => Convert.ToChar(value);
        #endregion
        #region public static char ToChar(this uint value)
        /// <summary>
        /// 将指定的 32 位无符号整数的值转换为其等效的 Unicode 字符
        /// </summary>
        /// <param name="value"> 要转换的 32 位无符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的 Unicode 字符 </returns>
        /// <exception cref="OverflowException"> <paramref name="value"/> 大于 <see cref="char.MaxValue"/> </exception>
        public static char ToChar(this uint value)
            => Convert.ToChar(value);
        #endregion
        #region public static char ToChar(this ulong value)
        /// <summary>
        /// 将指定的 64 位无符号整数的值转换为其等效的 Unicode 字符
        /// </summary>
        /// <param name="value"> 要转换的 64 位无符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的 Unicode 字符 </returns>
        /// <exception cref="OverflowException"> <paramref name="value"/> 大于 <see cref="char.MaxValue"/> </exception>
        public static char ToChar(this ulong value)
            => Convert.ToChar(value); 
        #endregion

    }
}
