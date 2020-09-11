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
        #region public static float ConvertToFloat(this string value)
        /// <summary>
        /// 将数字的指定字符串表示形式转换为等效的单精度浮点数
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <returns> 与 <paramref name="value"/> 中数字等效的单精度浮点数，如果 <paramref name="value"/> 为 null，则为 0（零） </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 不是一个具有有效格式的数字 </exception>
        /// <exception cref="OverflowException"> <paramref name="value"/> 表示一个小于 <see cref="float.MinValue"/> 或大于 <see cref="float.MaxValue"/> 的数字 </exception>
        public static float ConvertToFloat(this string value)
            => Convert.ToSingle(value);
        #endregion
        #region public static float ConvertToFloat(this string value, IFormatProvider provider)
        /// <summary>
        /// 使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的单精度浮点数
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <param name="provider"> 一个提供区域性特定的格式设置信息的对象 </param>
        /// <returns> 与 <paramref name="value"/> 中数字等效的单精度浮点数，如果 <paramref name="value"/> 为 null，则为 0（零） </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 不是一个具有有效格式的数字 </exception>
        /// <exception cref="OverflowException"> <paramref name="value"/> 表示一个小于 <see cref="float.MinValue"/> 或大于 <see cref="float.MaxValue"/> 的数字 </exception>
        public static float ConvertToFloat(this string value, IFormatProvider provider)
            => Convert.ToSingle(value, provider);
        #endregion
        #region public static float ToFloat(this bool value)
        /// <summary>
        /// 将指定的布尔值转换为等效的单精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的布尔值 </param>
        /// <returns> 如果 <paramref name="value"/> 为 true，则为数字 1；否则，为 0 </returns>
        public static float ToFloat(this bool value)
            => Convert.ToSingle(value);
        #endregion
        #region public static float ToFloat(this byte value)
        /// <summary>
        /// 将指定的 8 位无符号整数的值转换为等效的单精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 8 位无符号整数 </param>
        /// <returns>  一个等于 <paramref name="value"/> 的单精度浮点数 </returns>
        public static float ToFloat(this byte value)
            => Convert.ToSingle(value);
        #endregion
        #region public static float ToFloat(this sbyte value)
        /// <summary>
        /// 将指定的 8 位带符号整数的值转换为等效的单精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 8 位带符号整数 </param>
        /// <returns></returns>
        public static float ToFloat(this sbyte value)
            => Convert.ToSingle(value);
        #endregion
        #region public static float ToFloat(this short value)
        /// <summary>
        /// 将指定的 16 位带符号整数的值转换为等效的单精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 16 位带符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的单精度浮点数 </returns>
        public static float ToFloat(this short value)
           => Convert.ToSingle(value); 
        #endregion
        #region public static float ToFloat(this int value)
        /// <summary>
        /// 将指定的 32 位带符号整数的值转换为等效的单精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 32 位带符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的单精度浮点数 </returns>
        public static float ToFloat(this int value)
            => Convert.ToSingle(value); 
        #endregion
        #region public static float ToFloat(this long value)
        /// <summary>
        /// 将指定的 64 位带符号整数的值转换为等效的单精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 64 位带符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的单精度浮点数 </returns>
        public static float ToFloat(this long value)
            => Convert.ToSingle(value);
        #endregion
        #region public static float ToFloat(this ushort value)
        /// <summary>
        /// 将指定的 16 位无符号整数的值转换为等效的单精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 16 位无符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的单精度浮点数 </returns>
        public static float ToFloat(this ushort value)
            => Convert.ToSingle(value); 
        #endregion
        #region public static float ToFloat(this uint value)
        /// <summary>
        /// 将指定的 32 位无符号整数的值转换为等效的单精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 32 位无符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的单精度浮点数 </returns>
        public static float ToFloat(this uint value)
            => Convert.ToSingle(value); 
        #endregion
        #region public static float ToFloat(this ulong value)
        /// <summary>
        /// 将指定的 64 位无符号整数的值转换为等效的单精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的 64 位无符号整数 </param>
        /// <returns> 一个等于 <paramref name="value"/> 的单精度浮点数 </returns>
        public static float ToFloat(this ulong value)
            => Convert.ToSingle(value); 
        #endregion
        #region public static float ToFloat(this double value)
        /// <summary>
        /// 将指定的双精度浮点数的值转换为等效的单精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的双精度浮点数 </param>
        /// <returns>
        ///   一个等于 <paramref name="value"/> 的单精度浮点数。
        ///   使用“舍入为最接近的数字”规则对 <paramref name="value"/> 进行舍入。
        ///   例如，当舍入为两位小数时，值 2.345 变成 2.34，而值 2.355 变成 2.36。
        /// </returns>
        public static float ToFloat(this double value)
            => Convert.ToSingle(value);
        #endregion
        #region public static float ToFloat(this decimal value)
        /// <summary>
        /// 将指定的十进制数的值转换为等效的单精度浮点数
        /// </summary>
        /// <param name="value"> 要转换的十进制数 </param>
        /// <returns>
        ///   一个等于 <paramref name="value"/> 的单精度浮点数。
        ///   使用“舍入为最接近的数字”规则对 <paramref name="value"/> 进行舍入。
        ///   例如，当舍入为两位小数时，值 2.345 变成 2.34，而值 2.355 变成 2.36。
        /// </returns>
        public static float ToFloat(this decimal value)
            => Convert.ToSingle(value); 
        #endregion

    }
}
