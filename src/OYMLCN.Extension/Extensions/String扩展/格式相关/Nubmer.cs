using OYMLCN.ArgumentChecker;
using System;
using System.Text.RegularExpressions;
#if Xunit
using Xunit;
#endif


namespace OYMLCN.Extensions
{
    /// <summary>
    /// 格式相关扩展
    /// </summary>
    public static partial class StringFormatExtension
    {
        #region public static bool FormatIsNumeric(this string value)
        /// <summary>
        /// 验证文本是否是一个数值，即仅由 (+/-)号、数字和最多一个 . 组成
        /// </summary>
        /// <param name="value"> 要验证的字符串 </param>
        /// <returns> 如果 <paramref name="value"/> 为数值，则返回 true，否则为 false </returns>
        public static bool FormatIsNumeric(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
#if Xunit
        [Fact]
        public static void FormatIsNumericTest()
        {
            string str = null;
            Assert.False(str.FormatIsNumeric());
            str = string.Empty;
            Assert.False(str.FormatIsNumeric());
            str = "  ";
            Assert.False(str.FormatIsNumeric());

            str = " 20.20";
            Assert.False(str.FormatIsNumeric());

            str = "20.20";
            Assert.True(str.FormatIsNumeric());
            str = "+20.20";
            Assert.True(str.FormatIsNumeric());
            str = "-20.20";
            Assert.True(str.FormatIsNumeric());

            str = " 20";
            Assert.False(str.FormatIsNumeric());

            str = "20";
            Assert.True(str.FormatIsNumeric());
            str = "+20";
            Assert.True(str.FormatIsNumeric());
            str = "-20";
            Assert.True(str.FormatIsNumeric());
        }
#endif
        #endregion

        #region public static bool FormatIsUnsignNumeric(this string value)
        /// <summary>
        /// 验证文本是否是一个正数无符号数值，即由数字和最多一个 . 组成
        /// </summary>
        /// <param name="value"> 要验证的字符串 </param>
        /// <returns> 如果 <paramref name="value"/> 为无符号数值，则返回 true，否则为 false </returns>
        public static bool FormatIsUnsignNumeric(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, @"^\d*[.]?\d*$");
        }
#if Xunit
        [Fact]
        public static void FormatIsUnsignNumericTest()
        {
            string str = null;
            Assert.False(str.FormatIsUnsignNumeric());
            str = string.Empty;
            Assert.False(str.FormatIsUnsignNumeric());
            str = "  ";
            Assert.False(str.FormatIsUnsignNumeric());

            str = " 20.20";
            Assert.False(str.FormatIsUnsignNumeric());

            str = "20.20";
            Assert.True(str.FormatIsUnsignNumeric());
            str = "+20.20";
            Assert.False(str.FormatIsUnsignNumeric());
            str = "-20.20";
            Assert.False(str.FormatIsUnsignNumeric());

            str = " 20";
            Assert.False(str.FormatIsUnsignNumeric());

            str = "20";
            Assert.True(str.FormatIsUnsignNumeric());
            str = "+20";
            Assert.False(str.FormatIsUnsignNumeric());
            str = "-20";
            Assert.False(str.FormatIsUnsignNumeric());
        }
#endif
        #endregion

        #region public static bool FormatIsInteger(this string value)
        /// <summary>
        /// 验证文本是否是一个整数数值，即仅由 (+/-)号、数字组成
        /// </summary>
        /// <param name="value"> 要验证的字符串 </param>
        /// <returns> 如果 <paramref name="value"/> 为数值，则返回 true，否则为 false </returns>
        public static bool FormatIsInteger(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
#if Xunit
        [Fact]
        public static void FormatIsIntegerTest()
        {
            string str = null;
            Assert.False(str.FormatIsInteger());
            str = string.Empty;
            Assert.False(str.FormatIsInteger());
            str = "  ";
            Assert.False(str.FormatIsInteger());

            str = " 20.20";
            Assert.False(str.FormatIsInteger());

            str = "20.20";
            Assert.False(str.FormatIsInteger());
            str = "+20.20";
            Assert.False(str.FormatIsInteger());
            str = "-20.20";
            Assert.False(str.FormatIsInteger());

            str = " 20";
            Assert.False(str.FormatIsInteger());

            str = "20";
            Assert.True(str.FormatIsInteger());
            str = "+20";
            Assert.True(str.FormatIsInteger());
            str = "-20";
            Assert.True(str.FormatIsInteger());
        }
#endif
        #endregion


        #region public static string FormatAsNumeric(this string input)
        /// <summary>
        /// 获取文本中的第一个数值，即仅由 (+/-)号、数字和最多一个 . 组成
        /// </summary>
        /// <param name="input"> 要获取的字符串对象 </param>
        /// <returns> 如果 <paramref name="input"/> 包含数值，则返回第一个匹配的数值  </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static string FormatAsNumeric(this string input)
        {
            input.ThrowIfNull(nameof(input));
            return Regex.Match(input, @"[+-]?\d+(\.\d+)?").Value;
        }
#if Xunit
        [Fact]
        public static void FormatAsNumericTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.FormatAsNumeric());

            str = " ";
            Assert.Equal(string.Empty, str.FormatAsNumeric());

            str = " 100";
            Assert.Equal("100", str.FormatAsNumeric());

            str = "我是20.20";
            Assert.Equal("20.20", str.FormatAsNumeric());

            str = "-+20.20";
            Assert.Equal("+20.20", str.FormatAsNumeric());

            str = "+-20.20";
            Assert.Equal("-20.20", str.FormatAsNumeric());

            str = "你好呀";
            Assert.Equal(string.Empty, str.FormatAsNumeric());
        }
#endif 
        #endregion

        #region public static string FormatAsIntegerNumeric(this string input)
        /// <summary>
        /// 获取文本中的第一个整数数值，即仅由 (+/-)号、数字组成
        /// </summary>
        /// <param name="input"> 要获取的字符串对象 </param>
        /// <returns> 如果 <paramref name="input"/> 包含数值，则返回第一个匹配的整数数值  </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static string FormatAsIntegerNumeric(this string input)
        {
            input.ThrowIfNull(nameof(input));
            return Regex.Match(input, @"[+-]?\d+").Value;
        }
#if Xunit
        [Fact]
        public static void FormatAsIntegerNumericTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.FormatAsIntegerNumeric());

            str = " ";
            Assert.Equal(string.Empty, str.FormatAsIntegerNumeric());

            str = " 100";
            Assert.Equal("100", str.FormatAsIntegerNumeric());

            str = "我是20.20";
            Assert.Equal("20", str.FormatAsIntegerNumeric());

            str = "-+20.20";
            Assert.Equal("+20", str.FormatAsIntegerNumeric());

            str = "+-20.20";
            Assert.Equal("-20", str.FormatAsIntegerNumeric());

            str = "你好呀";
            Assert.Equal(string.Empty, str.FormatAsIntegerNumeric());
        }
#endif 
        #endregion

    }
}
