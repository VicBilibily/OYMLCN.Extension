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

    }
}
