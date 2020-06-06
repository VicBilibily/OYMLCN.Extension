using System;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        #region public static bool ConvertToBoolean(this string value)
        /// <summary>
        /// 将逻辑值的指定字符串表示形式转换为其等效的布尔值
        /// </summary>
        /// <param name="value"> 包含 <see cref="bool.TrueString"/> 或 <see cref="bool.FalseString"/> 值的字符串 </param>
        /// <returns> 
        /// 如果 <paramref name="value"/> 等于 <see cref="bool.TrueString"/> 则为 true，
        /// 如果 <paramref name="value"/> 等于 <see cref="bool.FalseString"/> 或 null 时则为 fasle。
        /// </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 不是 <see cref="bool.TrueString"/> 或 <see cref="bool.FalseString"/ </exception>
        public static bool ConvertToBoolean(this string value)
            => Convert.ToBoolean(value);
        #endregion
        #region public static bool ToBoolean(this byte value)
        /// <summary>
        /// 将指定的 8 位无符号整数的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 要转换的 8 位无符号整数 </param>
        /// <returns> 如果 <paramref name="value"/> 不为零，则为 true；否则，为 false。 </returns>
        public static bool ToBoolean(this byte value)
            => Convert.ToBoolean(value);
        #endregion
        #region public static bool ToBoolean(this sbyte value)
        /// <summary>
        /// 将指定的 8 位有符号整数的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 要转换的 8 位带符号整数 </param>
        /// <returns> 如果 <paramref name="value"/> 不为零，则为 true；否则，为 false。 </returns>
        public static bool ToBoolean(this sbyte value)
            => Convert.ToBoolean(value);
        #endregion
        #region public static bool ToBoolean(this short value)
        /// <summary>
        /// 将指定的 16 位有符号整数的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 要转换的 16 位带符号整数 </param>
        /// <returns> 如果 <paramref name="value"/> 不为零，则为 true；否则，为 false。 </returns>
        public static bool ToBoolean(this short value)
            => Convert.ToBoolean(value);
        #endregion
        #region public static bool ToBoolean(this int value)
        /// <summary>
        /// 将指定的 32 位有符号整数的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 要转换的 32 位带符号整数 </param>
        /// <returns> 如果 <paramref name="value"/> 不为零，则为 true；否则，为 false。 </returns>
        public static bool ToBoolean(this int value)
            => Convert.ToBoolean(value);
        #endregion
        #region public static bool ToBoolean(this long value)
        /// <summary>
        /// 将指定的 64 位有符号整数的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 要转换的 64 位带符号整数 </param>
        /// <returns> 如果 <paramref name="value"/> 不为零，则为 true；否则，为 false。 </returns>
        public static bool ToBoolean(this long value)
            => Convert.ToBoolean(value);
        #endregion
        #region public static bool ToBoolean(this ushort value)
        /// <summary>
        /// 将指定的 16 位无符号整数的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 要转换的 16 位无符号整数 </param>
        /// <returns> 如果 <paramref name="value"/> 不为零，则为 true；否则，为 false。 </returns> 
        public static bool ToBoolean(this ushort value)
            => Convert.ToBoolean(value);
        #endregion
        #region public static bool ToBoolean(this uint value)
        /// <summary>
        /// 将指定的 32 位无符号整数的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 要转换的 32 位无符号整数 </param>
        /// <returns> 如果 <paramref name="value"/> 不为零，则为 true；否则，为 false。 </returns> 
        public static bool ToBoolean(this uint value)
            => Convert.ToBoolean(value);
        #endregion
        #region public static bool ToBoolean(this ulong value)
        /// <summary>
        /// 将指定的 64 位无符号整数的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 要转换的 64 位无符号整数 </param>
        /// <returns> 如果 <paramref name="value"/> 不为零，则为 true；否则，为 false。 </returns> 
        public static bool ToBoolean(this ulong value)
            => Convert.ToBoolean(value);
        #endregion
        #region public static bool ToBoolean(this float value)
        /// <summary>
        /// 将指定的单精度浮点数的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 要转换的单精度浮点数 </param>
        /// <returns> 如果 <paramref name="value"/> 不为零，则为 true；否则，为 false。 </returns>
        public static bool ToBoolean(this float value)
            => Convert.ToBoolean(value);
        #endregion
        #region public static bool ToBoolean(this double value)
        /// <summary>
        /// 将指定的双精度浮点数的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 要转换的双精度浮点数 </param>
        /// <returns> 如果 <paramref name="value"/> 不为零，则为 true；否则，为 false。 </returns>
        public static bool ToBoolean(this double value)
            => Convert.ToBoolean(value);
        #endregion
        #region public static bool ToBoolean(this decimal value)
        /// <summary>
        /// 将指定的十进制数字的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 要转换的数字 </param>
        /// <returns> 如果 <paramref name="value"/> 不为零，则为 true；否则，为 false。 </returns>
        public static bool ToBoolean(this decimal value)
            => Convert.ToBoolean(value); 
        #endregion

    }
}
