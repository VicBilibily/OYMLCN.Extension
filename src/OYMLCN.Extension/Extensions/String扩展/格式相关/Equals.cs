using System;
using System.Linq;
using OYMLCN.ArgumentChecker;

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
        #region public static bool Equals(this string value, params string[] values)
        /// <summary>
        /// 检查字符串是否与另一个指定字符串数组内的对象具有相同的值
        /// </summary>
        /// <param name="value"> 字符串实例 </param>
        /// <param name="values"> 要与此实例进行比较的字符串数组 </param>
        /// <returns> 如果 <paramref name="value"/> 值与 <paramref name="values"/> 内的任一对象相同，则为 true；否则为 false。 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="values"/> 不能为 null </exception>
        public static bool Equals(this string value, params string[] values)
        {
            values.ThrowIfNull(nameof(values));
            if (values.Length == 0) return false;
            return values.Any(v => string.Equals(value, v));
        }
#if Xunit
        [Fact]
        public static void EqualsStringsTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => StringFormatExtension.Equals(str, null));
            Assert.False(str.Equals("Hello", "World", "!"));
            Assert.True(str.Equals(null, "!"));

            str = "!";
            Assert.True(str.Equals("Hello", "!"));
            Assert.False(str.Equals("Hello", "World"));
        }
#endif
        #endregion


        #region public static bool EqualsIgnoreCase(this string input, string value)
        /// <summary>
        /// 检查字符串是否与另一个指定字符串对象具有相同的值（忽略大小写）
        /// </summary>
        /// <param name="input"> 字符串实例 </param>
        /// <param name="value"> 要与此实例进行比较的字符串 </param>
        /// <returns> 如果 <paramref name="input"/> 值与 <paramref name="value"/> 相同，则为 true；否则为 false </returns>
        public static bool EqualsIgnoreCase(this string input, string value)
            => string.Equals(input, value, StringComparison.OrdinalIgnoreCase);
#if Xunit
        [Fact]
        public static void EqualsIgnoreCaseTest()
        {
            string str = null;
            Assert.False(str.EqualsIgnoreCase(string.Empty));
            Assert.True(str.EqualsIgnoreCase(str));

            str = string.Empty;
            Assert.True(str.EqualsIgnoreCase(string.Empty));
            Assert.False(str.EqualsIgnoreCase(" "));

            str = "Hello World!";
            Assert.True(str.EqualsIgnoreCase("HELLO WORLD!"));
            Assert.True(str.EqualsIgnoreCase("Hello WORLD!"));
            Assert.False(str.EqualsIgnoreCase(string.Empty));
        }
#endif
        #endregion

        #region public static bool EqualsIgnoreCase(this string value, params string[] values)
        /// <summary>
        /// 检查字符串是否与另一个指定字符串数组内的对象具有相同的值（忽略大小写）
        /// </summary>
        /// <param name="value"> 字符串实例 </param>
        /// <param name="values"> 要与此实例进行比较的字符串数组 </param>
        /// <returns> 如果 <paramref name="value"/> 值与 <paramref name="values"/> 内的任一对象相同，则为 true；否则为 false。 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="values"/> 不能为 null </exception>
        public static bool EqualsIgnoreCase(this string value, params string[] values)
        {
            values.ThrowIfNull(nameof(values));
            if (values.Length == 0) return false;
            return values.Any(v => string.Equals(value, v, StringComparison.OrdinalIgnoreCase));
        }
#if Xunit
        [Fact]
        public static void EqualsIgnoreCaseStringsTest()
        {
            string str = null;
            string[] arr = null;
            Assert.Throws<ArgumentNullException>(() => StringFormatExtension.EqualsIgnoreCase(str, arr));
            Assert.False(str.EqualsIgnoreCase("Hello", "World", "!"));
            Assert.True(str.EqualsIgnoreCase(null, "!"));

            str = "World";
            Assert.True(str.EqualsIgnoreCase("WORLD", "!"));
            Assert.False(str.EqualsIgnoreCase("HELLO", "!"));
        }
#endif  
        #endregion

    }
}
