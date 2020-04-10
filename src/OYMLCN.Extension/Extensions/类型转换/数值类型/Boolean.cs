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
        #region public static bool ConvertToBoolean(this object value)
        /// <summary>
        /// 将指定对象的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 用于实现 <see cref="IConvertible"/> 接口的对象，或为 null。 </param>
        /// <returns>
        ///   true 或 false，它将反映通过对  的基础类型调用 <see cref="IConvertible.ToBoolean(IFormatProvider)"/> 方法而返回的值。
        ///   如果 <paramref name="value"/> 为 null，则此方法返回 false。
        /// </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 是一个不等于 <see cref="bool.TrueString"/> 或 <see cref="bool.FalseString"/> 的字符串 </exception>
        /// <exception cref="InvalidCastException"> <paramref name="value"/> 不实现 <see cref="IConvertible"/> 接口 或 不支持 <paramref name="value"/> 到 <see cref="bool"/> 的转换 </exception>
        public static bool ConvertToBoolean(this object value)
            => Convert.ToBoolean(value);
        #endregion
        #region public static bool ConvertToBoolean(this object value, IFormatProvider provider)
        /// <summary>
        ///  使用指定的区域性特定格式设置信息，将指定对象的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 用于实现 <see cref="IConvertible"/> 接口的对象，或为 null。 </param>
        /// <param name="provider"> 一个提供区域性特定的格式设置信息的对象 </param>
        /// <returns>
        ///   true 或 false，它将反映通过对  的基础类型调用 <see cref="IConvertible.ToBoolean(IFormatProvider)"/> 方法而返回的值。
        ///   如果 <paramref name="value"/> 为 null，则此方法返回 false。
        /// </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 是一个不等于 <see cref="bool.TrueString"/> 或 <see cref="bool.FalseString"/> 的字符串 </exception>
        /// <exception cref="InvalidCastException"> <paramref name="value"/> 不实现 <see cref="IConvertible"/> 接口 或 不支持 <paramref name="value"/> 到 <see cref="bool"/> 的转换 </exception>
        public static bool ConvertToBoolean(this object value, IFormatProvider provider)
            => Convert.ToBoolean(value, provider);
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
        #region public static bool ToBoolean(ushort value)
        /// <summary>
        /// 将指定的 16 位无符号整数的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 要转换的 16 位无符号整数 </param>
        /// <returns> 如果 <paramref name="value"/> 不为零，则为 true；否则，为 false。 </returns> 
        public static bool ToBoolean(ushort value)
            => Convert.ToBoolean(value);
        #endregion
        #region public static bool ToBoolean(uint value)
        /// <summary>
        /// 将指定的 32 位无符号整数的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 要转换的 32 位无符号整数 </param>
        /// <returns> 如果 <paramref name="value"/> 不为零，则为 true；否则，为 false。 </returns> 
        public static bool ToBoolean(uint value)
            => Convert.ToBoolean(value);
        #endregion
        #region public static bool ToBoolean(ulong value)
        /// <summary>
        /// 将指定的 64 位无符号整数的值转换为等效的布尔值
        /// </summary>
        /// <param name="value"> 要转换的 64 位无符号整数 </param>
        /// <returns> 如果 <paramref name="value"/> 不为零，则为 true；否则，为 false。 </returns> 
        public static bool ToBoolean(ulong value)
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
