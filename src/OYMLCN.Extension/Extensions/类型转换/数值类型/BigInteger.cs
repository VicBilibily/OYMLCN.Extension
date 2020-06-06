using System;
using System.Numerics;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        #region public static BigInteger ConvertToBigInteger(this string value)
        /// <summary>
        /// 将数字的字符串表示形式转换为它的等效 <see cref="BigInteger"/> 表示形式
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <returns> 一个值，等于 <paramref name="value"/> 参数中指定的数字 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        /// <exception cref="FormatException"> <paramref name="value"/> 的格式不正确 </exception>
        public static BigInteger ConvertToBigInteger(this string value)
            => BigInteger.Parse(value); 
        #endregion
        #region public static BigInteger ConvertToBigInteger(this string value, IFormatProvider provider)
        /// <summary>
        /// 将指定的区域性特定格式的数字字符串表示形式转换为它的等效 <see cref="BigInteger"/>
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <param name="provider"> 一个对象，提供有关 <paramref name="value"/> 的区域性特定格式设置信息 </param>
        /// <returns> 一个值，等于 <paramref name="value"/> 参数中指定的数字 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        /// <exception cref="FormatException"> <paramref name="value"/> 的格式不正确 </exception>
        public static BigInteger ConvertToBigInteger(this string value, IFormatProvider provider)
            => BigInteger.Parse(value, provider);
        #endregion
        #region public static BigInteger ToBigInteger(this byte[] value)
        /// <summary>
        /// 使用字节数组中的值初始化 <see cref="BigInteger"/> 结构的新实例
        /// </summary>
        /// <param name="value"> 顺序为 little-endian 的字节值的数组 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        public static BigInteger ToBigInteger(this byte[] value)
            => new BigInteger(value);
        #endregion
        #region public static BigInteger ToBigInteger(this int value)
        /// <summary>
        /// 使用 32 位带符号整数值初始化 <see cref="BigInteger"/> 结构的新实例
        /// </summary>
        /// <param name="value"> 32 位带符号整数 </param>
        public static BigInteger ToBigInteger(this int value)
            => new BigInteger(value);
        #endregion
        #region public static BigInteger ToBigInteger(this long value)
        /// <summary>
        /// 使用 64 位带符号整数值初始化 <see cref="BigInteger"/> 结构的新实例
        /// </summary>
        /// <param name="value"> 64 位带符号整数 </param>
        public static BigInteger ToBigInteger(this long value)
            => new BigInteger(value);
        #endregion
        #region public static BigInteger ToBigInteger(this uint value)
        /// <summary>
        /// 使用 32 位无符号整数值初始化 <see cref="BigInteger"/> 结构的新实例
        /// </summary>
        /// <param name="value"> 32 位带符号整数 </param>
        public static BigInteger ToBigInteger(this uint value)
            => new BigInteger(value);
        #endregion
        #region public static BigInteger ToBigInteger(this ulong value)
        /// <summary>
        /// 使用 64 位无符号整数值初始化 <see cref="BigInteger"/> 结构的新实例
        /// </summary>
        /// <param name="value"> 64 位带符号整数 </param>
        public static BigInteger ToBigInteger(this ulong value)
            => new BigInteger(value);
        #endregion
        #region public static BigInteger ToBigInteger(this float value)
        /// <summary>
        /// 使用单精度浮点值初始化 <see cref="BigInteger"/> 结构的新实例
        /// </summary>
        /// <param name="value"> 单精度浮点值 </param>
        /// <exception cref="OverflowException"> <paramref name="value"/> 为 <see cref="float.NaN"/>、<see cref="float.NegativeInfinity"/> 或 <see cref="float.PositiveInfinity"/> </exception>
        public static BigInteger ToBigInteger(this float value)
            => new BigInteger(value);
        #endregion
        #region public static BigInteger ToBigInteger(this double value)
        /// <summary>
        /// 使用双精度浮点值初始化 <see cref="BigInteger"/> 结构的新实例
        /// </summary>
        /// <param name="value"> 一个双精度浮点值 </param>
        /// <exception cref="OverflowException"> <paramref name="value"/> 为 <see cref="double.NaN"/>、<see cref="double.NegativeInfinity"/> 或 <see cref="double.PositiveInfinity"/> </exception>
        public static BigInteger ToBigInteger(this double value)
            => new BigInteger(value);
        #endregion
        #region public static BigInteger ToBigInteger(this decimal value)
        /// <summary>
        /// 使用 <see cref="decimal"/> 结构初始化 <see cref="BigInteger"/> 结构的新实例
        /// </summary>
        /// <param name="value"> 十进制数 </param>
        public static BigInteger ToBigInteger(this decimal value)
            => new BigInteger(value);
        #endregion

    }
}
