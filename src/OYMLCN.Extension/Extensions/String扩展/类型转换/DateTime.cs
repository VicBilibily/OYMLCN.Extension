using System;
#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class StringConvertExtension
    {
        #region public static DateTime ConvertToDatetime(this string value)
        /// <summary>
        /// 将日期和时间的指定字符串表示形式转换为等效的日期和时间值的 <see cref="DateTime"/> 实例
        /// </summary>
        /// <param name="value"> 日期和时间的字符串表示形式 </param>
        /// <returns> 
        /// <para> 如果 <paramref name="value"/> 为 null，则返回 <see cref="DateTime.MinValue"/> 的值， </para>
        /// <para> 否则返回与 <paramref name="value"/> 的值的日期和时间等效的 <see cref="DateTime"/> 实例 </para>
        /// </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 不是格式正确的日期和时间字符串 或者为 <see cref="string.Empty"/> </exception>
        public static DateTime ConvertToDatetime(this string value)
            => Convert.ToDateTime(value);
#if Xunit
        [Fact]
        public static void ConvertToDatetimeTest()
        {
            string str = null;
            Assert.Equal(DateTime.MinValue, str.ConvertToDatetime());

            str = string.Empty;
            Assert.Throws<FormatException>(() => str.ConvertToDatetime());

            str = "Hello World!";
            Assert.Throws<FormatException>(() => str.ConvertToDatetime());

            var now = DateTime.Now;
            str = now.ToString();
            Assert.Equal(DateTime.Parse(str), str.ConvertToDatetime());
        }
#endif
        #endregion

        #region public static DateTime? ConvertToNullableDatetime(this string value)
        /// <summary>
        /// <para> 将日期和时间的指定字符串表示形式转换为等效的日期和时间值的 <see cref="DateTime"/> 实例 </para>
        /// <para> 当 <paramref name="value"/> 的值为 null、空、空格或格式不正确时返回 null </para>
        /// </summary>
        /// <param name="value"> 日期和时间的字符串表示形式 </param>
        /// <returns> 
        /// <para> 如果 <paramref name="value"/> 为 null、空、空格或格式不正确时返回 null， </para>
        /// <para> 否则返回与 <paramref name="value"/> 的值的日期和时间等效的 <see cref="DateTime"/> 实例 </para>
        /// </returns>
        public static DateTime? ConvertToNullableDatetime(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return null;
            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                return null;
            }
        }
#if Xunit
        [Fact]
        public static void ConvertToNullableDatetimeTest()
        {
            string str = null;
            Assert.Null(str.ConvertToNullableDatetime());

            str = string.Empty;
            Assert.Null(str.ConvertToNullableDatetime());

            str = "Hello World!";
            Assert.Null(str.ConvertToNullableDatetime());

            var now = DateTime.Now;
            str = now.ToString();
            Assert.Equal(DateTime.Parse(str), str.ConvertToNullableDatetime());
        }
#endif
        #endregion

        #region public static DateTime ConvertToDatetime(this string value, DateTime defaultValue)
        /// <summary>
        /// <para> 将日期和时间的指定字符串表示形式转换为等效的日期和时间值的 <see cref="DateTime"/> 实例 </para>
        /// <para> 如果转换失败或转换器输出了无效值 null，则返回设定的 <paramref name="defaultValue"/> 实例 </para>
        /// </summary>
        /// <param name="value"> 日期和时间的字符串表示形式 </param>
        /// <param name="defaultValue"> 当转换失败时返回的默认值 </param>
        /// <returns> 
        /// <para> 如果 <paramref name="value"/> 为 null、空、空格或格式不正确时返回 <paramref name="defaultValue"/>， </para>
        /// <para> 否则返回与 <paramref name="value"/> 的值的日期和时间等效的 <see cref="DateTime"/> 实例 </para>
        /// </returns>
        public static DateTime ConvertToDatetime(this string value, DateTime defaultValue)
            => value.ConvertToNullableDatetime() ?? defaultValue;
#if Xunit
        [Fact]
        public static void ConvertToDatetimeDefaultTest()
        {
            var today = DateTime.Today;
            string str = null;
            Assert.Equal(today, str.ConvertToDatetime(today));

            str = string.Empty;
            Assert.Equal(today, str.ConvertToDatetime(today));

            str = "Hello World!";
            Assert.Equal(today, str.ConvertToDatetime(today));

            var now = DateTime.Now;
            str = now.ToString();
            Assert.Equal(DateTime.Parse(str), str.ConvertToDatetime(today));
        }
#endif
        #endregion

    }
}
