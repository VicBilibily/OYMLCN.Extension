using System;
using System.ComponentModel;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        #region public static DateTime ConvertToDateTime(this string value)
        /// <summary>
        /// 将数字的指定字符串表示形式转换为等效的日期和时间
        /// </summary>
        /// <param name="value"> 日期和时间的字符串表示形式 </param>
        /// <returns> <paramref name="value"/> 的值的日期和时间等效项，如果 <paramref name="value"/> 为 null，则为 <see cref="DateTime.MinValue"/> 的日期和时间等效项 </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 不是格式正确的日期和时间字符串 </exception>
        public static DateTime ConvertToDateTime(this string value)
            => Convert.ToDateTime(value);
        #endregion
        #region public static DateTime ConvertToDateTime(this string value, IFormatProvider provider)
        /// <summary>
        /// 使用指定的区域性特定格式设置信息，将数字的指定字符串表示形式转换为等效的日期和时间
        /// </summary>
        /// <param name="value"> 包含要转换的日期和时间的字符串 </param>
        /// <param name="provider"> 一个提供区域性特定的格式设置信息的对象 </param>
        /// <returns> <paramref name="value"/> 的值的日期和时间等效项，如果 <paramref name="value"/> 为 null，则为 <see cref="DateTime.MinValue"/> 的日期和时间等效项 </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 不是格式正确的日期和时间字符串 </exception>
        public static DateTime ConvertToDateTime(this string value, IFormatProvider provider)
            => Convert.ToDateTime(value, provider);
        #endregion
        #region public static DateTime ConvertToDateTime(this object value)
        /// <summary>
        /// 将指定对象的值转换为 <see cref="DateTime"/> 对象
        /// </summary>
        /// <param name="value"> 用于实现 <see cref="IConvertible"/> 接口的对象，或为 null </param>
        /// <returns> <paramref name="value"/> 的值的日期和时间等效项，如果 <paramref name="value"/> 为 null，则为 <see cref="DateTime.MinValue"/> 的日期和时间等效项 </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 是无效的日期和时间值 </exception>
        /// <exception cref="InvalidCastException"> <paramref name="value"/> 不实现 <see cref="IConvertible"/> 接口 或 不支持该转换 </exception>
        public static DateTime ConvertToDateTime(this object value)
            => Convert.ToDateTime(value);
        #endregion
        #region public static DateTime ConvertToDateTime(this object value, IFormatProvider provider)
        /// <summary>
        /// 使用指定的区域性特定格式设置信息将指定对象的值转换为 <see cref="DateTime"/> 对象
        /// </summary>
        /// <param name="value"> 一个实现 <see cref="IConvertible"/> 接口的对象 </param>
        /// <param name="provider"> 一个提供区域性特定的格式设置信息的对象 </param>
        /// <returns> <paramref name="value"/> 的值的日期和时间等效项，如果 <paramref name="value"/> 为 null，则为 <see cref="DateTime.MinValue"/> 的日期和时间等效项 </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 是无效的日期和时间值 </exception>
        /// <exception cref="InvalidCastException"> <paramref name="value"/> 不实现 <see cref="IConvertible"/> 接口 或 不支持该转换 </exception>
        public static DateTime ConvertToDateTime(this object value, IFormatProvider provider)
            => Convert.ToDateTime(value, provider); 
        #endregion

    }
}
