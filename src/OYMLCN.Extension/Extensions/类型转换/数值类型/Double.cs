using System;

#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        #region public static double ConvertToDouble(this string value)
        /// <summary>
        /// 将表示形式为数字的字符串转换为等效的双精度浮点数，如果为 null，则为 0（零）。
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <returns> 与 <paramref name="value"/> 中数字等效的双精度浮点数，如果 <paramref name="value"/> 为 null，则为 0（零）。 </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 不是一个具有有效格式的数字 </exception>
        /// <exception cref="OverflowException"> <paramref name="value"/> 表示一个小于 <see cref="double.MinValue"/> 或大于 <see cref="double.MaxValue"/> 的数字 </exception>
        public static double ConvertToDouble(this string value)
            => Convert.ToDouble(value);
        #endregion
        #region public static double ConvertToDouble(this string value, IFormatProvider provider)
        /// <summary>
        /// 使用指定的区域性特定格式设置信息，将表示形式为数字的字符串转换为等效的双精度浮点数，如果为 null，则为 0（零）。
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <param name="provider"> 一个提供区域性特定的格式设置信息的对象 </param>
        /// <returns> 与 <paramref name="value"/> 中数字等效的双精度浮点数，如果 <paramref name="value"/> 为 null，则为 0（零）。 </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 不是一个具有有效格式的数字 </exception>
        /// <exception cref="OverflowException"> <paramref name="value"/> 表示一个小于 <see cref="double.MinValue"/> 或大于 <see cref="double.MaxValue"/> 的数字 </exception>
        public static double ConvertToDouble(this string value, IFormatProvider provider)
            => Convert.ToDouble(value, provider);
        #endregion
        #region public static double ToDouble(this bool value)
        /// <summary>
        /// 将指定的布尔值转换为等效的双精度浮点数，如果 <paramref name="value"/> 为 true，则为数字 1；否则，为 0。
        /// </summary>
        /// <param name="value"> 要转换的布尔值 </param>
        /// <returns> 如果 <paramref name="value"/> 为 true，则为数字 1；否则，为 0。 </returns>
        public static double ToDouble(this bool value)
            => Convert.ToDouble(value);
        #endregion
        #region public static double ToDouble(this byte value)
        /// <summary>
        /// 将指定的 8 位无符号整数的值转换为等效的双精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 8 位无符号整数 </param>
        /// <returns> 与 <paramref name="value"/> 等效的双精度浮点数 </returns>
        public static double ToDouble(this byte value)
            => Convert.ToDouble(value);
        #endregion
        #region public static double ToDouble(this sbyte value)
        /// <summary>
        /// 将指定的 8 位带符号整数的值转换为等效的双精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 8 位带符号整数 </param>
        /// <returns> 与 <paramref name="value"/> 等效的 8 位带符号整数 </returns>
        public static double ToDouble(this sbyte value)
            => Convert.ToDouble(value);
        #endregion
        #region public static double ToDouble(this short value)
        /// <summary>
        /// 将指定的 16 位带符号整数的值转换为等效的双精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 16 位带符号整数 </param>
        /// <returns> 与 <paramref name="value"/> 等效的双精度浮点数 </returns>
        public static double ToDouble(this short value)
            => Convert.ToDouble(value);
        #endregion
        #region public static double ToDouble(this int value)
        /// <summary>
        /// 将指定的 32 位带符号整数的值转换为等效的双精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 32 位带符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的双精度浮点数 </returns>
        public static double ToDouble(this int value)
            => Convert.ToDouble(value);
        #endregion
        #region public static double ToDouble(this long value)
        /// <summary>
        /// 将指定的 64 位带符号整数的值转换为等效的双精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 64 位带符号整数 </param>
        /// <returns>  一个等于 <paramref name="value"/> 的双精度浮点数 </returns>
        public static double ToDouble(this long value)
            => Convert.ToDouble(value);
        #endregion
        #region public static double ToDouble(this ushort value)
        /// <summary>
        /// 将指定的 16 位无符号整数的值转换为等效的双精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 16 位无符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的双精度浮点数 </returns>
        public static double ToDouble(this ushort value)
            => Convert.ToDouble(value);
        #endregion
        #region public static double ToDouble(this uint value)
        /// <summary>
        /// 将指定的 32 位无符号整数的值转换为等效的双精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 32 位无符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的双精度浮点数 </returns>
        public static double ToDouble(this uint value)
            => Convert.ToDouble(value);
        #endregion
        #region public static double ToDouble(this ulong value)
        /// <summary>
        /// 将指定的 64 位无符号整数的值转换为等效的双精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 64 位无符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的双精度浮点数 </returns>
        public static double ToDouble(this ulong value)
            => Convert.ToDouble(value);
        #endregion
        #region public static double ToDouble(this float value)
        /// <summary>
        /// 将指定的单精度浮点数的值转换为等效的双精度浮点数
        /// </summary>
        /// <param name="value"> 单精度浮点数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的双精度浮点数 </returns>
        public static double ToDouble(this float value)
            => Convert.ToDouble(value);
        #endregion
        #region public static double ToDouble(this decimal value)
        /// <summary>
        /// 将指定的十进制数的值转换为等效的双精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的十进制数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的双精度浮点数 </returns>
        public static double ToDouble(this decimal value)
            => Convert.ToDouble(value);
        #endregion

    }
}
