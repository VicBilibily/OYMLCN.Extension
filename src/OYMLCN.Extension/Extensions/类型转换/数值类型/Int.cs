using System;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        #region public static int ConvertToInt(this string value)
        /// <summary>
        /// 将数字的指定字符串表示形式转换为等效的 32 位带符号整数
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <returns> 一个与 <paramref name="value"/> 中数字等效的 32 位带符号整数，如果 <paramref name="value"/> 为 null，则为 0（零） </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 不由一个可选符号后跟一系列数字 (0-9) 组成 </exception>
        /// <exception cref="OverflowException"> <paramref name="value"/> 表示一个小于 <see cref="int.MinValue"/> 或大于 <see cref="int.MaxValue"/> 的数字 </exception>
        public static int ConvertToInt(this string value)
            => Convert.ToInt32(value);
        #endregion
        #region public static int ConvertToInt(this string value, int fromBase)
        /// <summary>
        /// 将指定基数的数字的字符串表示形式转换为等效的 32 位有符号整数
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <param name="fromBase"> <paramref name="value"/> 中数字的基数，它必须是 2、8、10 或 16 </param>
        /// <returns> 一个与 <paramref name="value"/> 中数字等效的 32 位带符号整数，如果 <paramref name="value"/> 为 null，则为 0（零） </returns>
        /// <exception cref="ArgumentException"> <paramref name="value"/>（表示基数不为 10 的符号数字）的前缀为负号 </exception>
        /// <exception cref="ArgumentException"> <paramref name="fromBase"/> 不是 2、8、10 或 16。 </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="value"/> 为 <see cref="string.Empty"/> </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="value"/> 包含一个字符，该字符不是由 <paramref name="fromBase"/> 指定的基数中的有效数字。
        ///   如果 <paramref name="value"/> 中的第一个字符无效，则该异常消息指示没有要转换的数字；
        ///   否则，该消息指示 <paramref name="value"/> 包含无效的尾随字符
        /// </exception>
        /// <exception cref="OverflowException"> <paramref name="value"/>（表示基数不为 10 的符号数字）的前缀为负号 </exception>
        /// <exception cref="OverflowException"> <paramref name="value"/> 表示一个小于 <see cref="int.MinValue"/> 或大于 <see cref="int.MaxValue"/> 的数字 </exception>
        public static int ConvertToInt(this string value, int fromBase)
            => Convert.ToInt32(value, fromBase);
        #endregion
        #region public static int ConvertToInt(this string value, IFormatProvider provider)
        /// <summary>
        /// 使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的 32 位带符号整数
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <param name="provider"> 一个提供区域性特定的格式设置信息的对象 </param>
        /// <returns> 一个与 <paramref name="value"/> 中数字等效的 32 位带符号整数，如果 <paramref name="value"/> 为 null，则为 0（零） </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 不由一个可选符号后跟一系列数字 (0-9) 组成 </exception>
        /// <exception cref="OverflowException"> <paramref name="value"/> 表示一个小于 <see cref="int.MinValue"/> 或大于 <see cref="int.MaxValue"/> 的数字 </exception>
        public static int ConvertToInt(this string value, IFormatProvider provider)
            => Convert.ToInt32(value, provider);
        #endregion
        #region public static int ConvertToInt(this object value)
        /// <summary>
        /// 将指定对象的值转换为 32 位带符号整数
        /// </summary>
        /// <param name="value"> 用于实现 <see cref="IConvertible"/> 接口的对象，或为 null </param>
        /// <returns> 一个与 <paramref name="value"/> 中数字等效的 32 位带符号整数，如果 <paramref name="value"/> 为 null，则为 0（零） </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 的格式不正确 </exception>
        /// <exception cref="InvalidCastException"> <paramref name="value"/> 不实现 <see cref="IConvertible"/> 接口。 或 不支持该转换 </exception>
        /// <exception cref="OverflowException"> <paramref name="value"/> 表示一个小于 <see cref="int.MinValue"/> 或大于 <see cref="int.MaxValue"/> 的数字 </exception>
        public static int ConvertToInt(this object value)
            => Convert.ToInt32(value);
        #endregion
        #region public static int ConvertToInt(this object value, IFormatProvider provider)
        /// <summary>
        /// 使用指定的区域性特定格式信息，将指定对象的值转换为 32 位带符号整数
        /// </summary>
        /// <param name="value"> 一个实现 <see cref="IConvertible"/> 接口的对象 </param>
        /// <param name="provider"> 一个提供区域性特定的格式设置信息的对象 </param>
        /// <returns> 一个与 <paramref name="value"/> 中数字等效的 32 位带符号整数，如果 <paramref name="value"/> 为 null，则为 0（零） </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 的格式不正确 </exception>
        /// <exception cref="InvalidCastException"> <paramref name="value"/> 不实现 <see cref="IConvertible"/> 接口。 或 不支持该转换 </exception>
        /// <exception cref="OverflowException"> <paramref name="value"/> 表示一个小于 <see cref="int.MinValue"/> 或大于 <see cref="int.MaxValue"/> 的数字 </exception>
        public static int ConvertToInt(this object value, IFormatProvider provider)
            => Convert.ToInt32(value, provider);
        #endregion

        #region public static int ToInt(this bool value)
        /// <summary>
        /// 将指定的布尔值转换为等效的 32 位带符号整数
        /// </summary>
        /// <param name="value"> 要转换的布尔值 </param>
        /// <returns> 如果 <paramref name="value"/> 为 true，则为数字 1；否则，为 0 </returns>
        public static int ToInt(this bool value)
            => Convert.ToInt32(value);
        #endregion
        #region public static int ToInt(this byte value)
        /// <summary>
        /// 将指定的 8 位无符号整数的值转换为等效的 32 位有符号整数
        /// </summary>
        /// <param name="value"> 要转换的 8 位无符号整数 </param>
        /// <returns> 一个与 <paramref name="value"/> 等效的 32 位带符号整数 </returns>
        public static int ToInt(this byte value)
            => Convert.ToInt32(value); 
        #endregion
        #region public static int ToInt(this char value)
        /// <summary>
        /// 将指定的 Unicode 字符的值转换为等效的 32 位有符号整数
        /// </summary>
        /// <param name="value"> 要转换的 Unicode 字符 </param>
        /// <returns> 一个与 <paramref name="value"/> 等效的 32 位带符号整数 </returns>
        public static int ToInt(this char value)
            => Convert.ToInt32(value); 
        #endregion
        #region public static int ToInt(this sbyte value)
        /// <summary>
        /// 将指定的 8 位带符号整数的值转换为等效的 32 位带符号整数
        /// </summary>
        /// <param name="value"> 要转换的 8 位带符号整数 </param>
        /// <returns> 一个与 <paramref name="value"/> 等效的 32 位带符号整数 </returns>
        public static int ToInt(this sbyte value)
            => Convert.ToInt32(value); 
        #endregion
        #region public static int ToInt(this short value)
        /// <summary>
        /// 将指定的 16 位有符号整数的值转换为等效的 32 位有符号整数
        /// </summary>
        /// <param name="value"> 要转换的 16 位带符号整数 </param>
        /// <returns> 一个与 <paramref name="value"/> 等效的 32 位带符号整数 </returns>
        public static int ToInt(this short value)
            => Convert.ToInt32(value); 
        #endregion
        #region public static int ToInt(this long value)
        /// <summary>
        /// 将指定的 64 位有符号整数的值转换为等效的 32 位有符号整数
        /// </summary>
        /// <param name="value"> 要转换的 64 位带符号整数 </param>
        /// <returns> 一个与 <paramref name="value"/> 等效的 32 位带符号整数 </returns>
        /// <exception cref="OverflowException"> <paramref name="value"/> 大于 <see cref="int.MaxValue"/> 或小于 <see cref="int.MinValue"/> </exception>
        public static int ToInt(this long value)
            => Convert.ToInt32(value); 
        #endregion
        #region public static int ToInt(this ushort value)
        /// <summary>
        /// 将指定的 16 位无符号整数的值转换为等效的 32 位有符号整数
        /// </summary>
        /// <param name="value"> 要转换的 16 位无符号整数 </param>
        /// <returns> 一个与 <paramref name="value"/> 等效的 32 位带符号整数 </returns>
        public static int ToInt(this ushort value)
            => Convert.ToInt32(value); 
        #endregion
        #region public static int ToInt(this uint value)
        /// <summary>
        /// 将指定的 32 位无符号整数的值转换为等效的 32 位有符号整数
        /// </summary>
        /// <param name="value"> 要转换的 32 位无符号整数 </param>
        /// <returns> 一个与 <paramref name="value"/> 等效的 32 位带符号整数 </returns>
        /// <exception cref="OverflowException"> <paramref name="value"/> 大于 <see cref="int.MaxValue"/> </exception>
        public static int ToInt(this uint value)
            => Convert.ToInt32(value); 
        #endregion
        #region public static int ToInt(this ulong value)
        /// <summary>
        /// 将指定的 64 位无符号整数的值转换为等效的 32 位有符号整数
        /// </summary>
        /// <param name="value"> 要转换的 64 位无符号整数 </param>
        /// <returns> 一个与 <paramref name="value"/> 等效的 32 位带符号整数 </returns>
        /// <exception cref="OverflowException"> <paramref name="value"/> 大于 <see cref="int.MaxValue"/> </exception>
        public static int ToInt(this ulong value)
            => Convert.ToInt32(value);
        #endregion
        #region public static int ToInt(this float value)
        /// <summary>
        /// 将指定的单精度浮点数的值转换为等效的 32 位带符号整数
        /// </summary>
        /// <param name="value"> 要转换的单精度浮点数 </param>
        /// <returns>
        ///   <paramref name="value"/>，舍入为最接近的 32 位有符号整数。
        ///   如果 <paramref name="value"/> 为两个整数中间的数字，则返回二者中的偶数；即 4.5 转换为 4，而 5.5 转换为 6。
        /// </returns>
        /// <exception cref="OverflowException"> <paramref name="value"/> 大于 <see cref="int.MaxValue"/> 或小于 <see cref="int.MinValue"/> </exception>
        public static int ToInt(this float value)
            => Convert.ToInt32(value); 
        #endregion
        #region public static int ToInt(this double value)
        /// <summary>
        /// 将指定的双精度浮点数的值转换为等效的 32 位带符号整数
        /// </summary>
        /// <param name="value"> 要转换的双精度浮点数 </param>
        /// <returns>
        ///   <paramref name="value"/>，舍入为最接近的 32 位有符号整数。
        ///   如果 <paramref name="value"/> 为两个整数中间的数字，则返回二者中的偶数；即 4.5 转换为 4，而 5.5 转换为 6。
        /// </returns>
        /// <exception cref="OverflowException"> <paramref name="value"/> 大于 <see cref="int.MaxValue"/> 或小于 <see cref="int.MinValue"/> </exception>
        public static int ToInt(this double value)
            => Convert.ToInt32(value); 
        #endregion
        #region public static int ToInt(this decimal value)
        /// <summary>
        /// 将指定的十进制数的值转换为等效的 32 位带符号整数
        /// </summary>
        /// <param name="value"> 要转换的十进制数 </param>
        /// <returns>
        ///   <paramref name="value"/>，舍入为最接近的 32 位有符号整数。
        ///   如果 <paramref name="value"/> 为两个整数中间的数字，则返回二者中的偶数；即 4.5 转换为 4，而 5.5 转换为 6。
        /// </returns>
        /// <exception cref="OverflowException"> <paramref name="value"/> 大于 <see cref="int.MaxValue"/> 或小于 <see cref="int.MinValue"/> </exception>
        public static int ToInt(this decimal value)
            => Convert.ToInt32(value);  
        #endregion

    }
}
