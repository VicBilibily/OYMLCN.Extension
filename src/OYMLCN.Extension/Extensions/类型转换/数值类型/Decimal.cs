using System;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        #region public static decimal ConvertToDecimal(this string value)
        /// <summary>
        /// 将数字的指定字符串表示形式转换为等效的十进制数
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <returns> 与 <paramref name="value"/> 中数字等效的十进制数，如果 <paramref name="value"/> 为 null，则为 0（零） </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 不是一个具有有效格式的数字 </exception>
        /// <exception cref="OverflowException"> <paramref name="value"/> 表示一个小于 <see cref="decimal.MinValue"/> 或大于 <see cref="decimal.MaxValue"/> 的数字 </exception>
        public static decimal ConvertToDecimal(this string value)
            => Convert.ToDecimal(value);
        #endregion
        #region public static decimal ConvertToDecimal(this string value, IFormatProvider provider)
        /// <summary>
        /// 使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的十进制数
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <param name="provider"> 一个提供区域性特定的格式设置信息的对象 </param>
        /// <returns> 与 <paramref name="value"/> 中数字等效的十进制数，如果 <paramref name="value"/> 为 null，则为 0（零） </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 不是一个具有有效格式的数字 </exception>
        /// <exception cref="OverflowException"> <paramref name="value"/> 表示一个小于 <see cref="decimal.MinValue"/> 或大于 <see cref="decimal.MaxValue"/> 的数字 </exception>
        public static decimal ConvertToDecimal(this string value, IFormatProvider provider)
            => Convert.ToDecimal(value, provider);
        #endregion
        #region public static decimal ToDecimal(this bool value)
        /// <summary>
        /// 将指定的布尔值转换为等效的十进制数
        /// </summary>
        /// <param name="value"> 要转换的布尔值 </param>
        /// <returns> 如果 value 为 true，则为数字 1；否则，为 0。 </returns>
        public static decimal ToDecimal(this bool value)
            => Convert.ToDecimal(value); 
        #endregion
        #region public static decimal ToDecimal(this byte value)
        /// <summary>
        /// 将指定的 8 位无符号整数的值转换为等效的十进制数
        /// </summary>
        /// <param name="value"> 要转换的 8 位无符号整数 </param>
        /// <returns> 与 <paramref name="value"/> 等效的十进制数 </returns>
        public static decimal ToDecimal(this byte value)
            => Convert.ToDecimal(value);
        #endregion
        #region public static decimal ToDecimal(this sbyte value)
        /// <summary>
        /// 将指定的 8 位带符号整数的值转换为等效的十进制数
        /// </summary>
        /// <param name="value"> 要转换的 8 位带符号整数 </param>
        /// <returns> 与 <paramref name="value"/> 等效的十进制数 </returns>
        public static decimal ToDecimal(this sbyte value)
            => Convert.ToDecimal(value);
        #endregion
        #region public static decimal ToDecimal(this short value)
        /// <summary>
        /// 将指定的 16 位带符号整数的值转换为等效的十进制数
        /// </summary>
        /// <param name="value"> 要转换的 16 位带符号整数 </param>
        /// <returns> 与 <paramref name="value"/> 等效的十进制数 </returns>
        public static decimal ToDecimal(this short value)
            => Convert.ToDecimal(value);
        #endregion
        #region public static decimal ToDecimal(this int value)
        /// <summary>
        /// 将指定的 32 位带符号整数的值转换为等效的十进制数
        /// </summary>
        /// <param name="value"> 要转换的 32 位带符号整数 </param>
        /// <returns> 与 <paramref name="value"/> 等效的十进制数 </returns>
        public static decimal ToDecimal(this int value)
            => Convert.ToDecimal(value);
        #endregion
        #region public static decimal ToDecimal(this long value)
        /// <summary>
        /// 将指定的 64 位带符号整数的值转换为等效的十进制数
        /// </summary>
        /// <param name="value"> 要转换的 64 位带符号整数 </param>
        /// <returns> 与 <paramref name="value"/> 等效的十进制数 </returns>
        public static decimal ToDecimal(this long value)
            => Convert.ToDecimal(value);
        #endregion
        #region public static decimal ToDecimal(this ushort value)
        /// <summary>
        /// 将指定的 16 位无符号整数的值转换为等效的十进制数
        /// </summary>
        /// <param name="value"> 要转换的 16 位无符号整数 </param>
        /// <returns> 与 <paramref name="value"/> 等效的十进制数 </returns>
        public static decimal ToDecimal(this ushort value)
            => Convert.ToDecimal(value);
        #endregion
        #region public static decimal ToDecimal(this uint value)
        /// <summary>
        /// 将指定的 32 位无符号整数的值转换为等效的十进制数
        /// </summary>
        /// <param name="value"> 要转换的 32 位无符号整数 </param>
        /// <returns> 与 <paramref name="value"/> 等效的十进制数 </returns>
        public static decimal ToDecimal(this uint value)
            => Convert.ToDecimal(value);
        #endregion
        #region public static decimal ToDecimal(this ulong value)
        /// <summary>
        /// 将指定的 64 位无符号整数的值转换为等效的十进制数
        /// </summary>
        /// <param name="value"> 要转换的 64 位无符号整数 </param>
        /// <returns> 与 <paramref name="value"/> 等效的十进制数 </returns>
        public static decimal ToDecimal(this ulong value)
            => Convert.ToDecimal(value);
        #endregion
        #region public static decimal ToDecimal(this float value)
        /// <summary>
        /// 将指定的单精度浮点数的值转换为等效的十进制数
        /// </summary>
        /// <param name="value"> 要转换的单精度浮点数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的十进制数 </returns>
        /// <exception cref="OverflowException"> <paramref name="value"/> 大于 <see cref="decimal.MaxValue"/> 或 小于 <see cref="decimal.MinValue"/> </exception>
        public static decimal ToDecimal(this float value)
            => Convert.ToDecimal(value); 
        #endregion
        #region public static decimal ToDecimal(this double value)
        /// <summary>
        /// 将指定的双精度浮点数的值转换为等效的十进制数
        /// </summary>
        /// <param name="value"> 要转换的双精度浮点数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的十进制数 </returns>
        /// <exception cref="OverflowException"> <paramref name="value"/> 大于 <see cref="decimal.MaxValue"/> 或 小于 <see cref="decimal.MinValue"/> </exception>
        public static decimal ToDecimal(this double value)
            => Convert.ToDecimal(value); 
        #endregion

    }
}
