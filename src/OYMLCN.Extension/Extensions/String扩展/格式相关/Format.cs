using OYMLCN.ArgumentChecker;
using System;
using System.Linq;
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
        #region public static bool IsNullOrEmpty(this string value)
        /// <summary> 
        /// 检查字符串 是 null 或者是 <see cref="string.Empty"/> 字符串
        /// </summary>
        /// <param name="value"> 要测试的字符串 </param>
        /// <returns> 如果 <paramref name="value"/> 为 null 或空字符串 ("") 则为 true，否则为 false。 </returns>
        public static bool IsNullOrEmpty(this string value)
            => string.IsNullOrEmpty(value);
#if Xunit
        [Fact]
        public static void IsNullOrEmptyTest()
        {
            string str = null;
            Assert.True(str.IsNullOrEmpty());
            str = string.Empty;
            Assert.True(str.IsNullOrEmpty());
            str = " ";
            Assert.False(str.IsNullOrEmpty());

            // 内置方法扩展，不需要深入测试
        }
#endif 
        #endregion

        #region public static bool IsNotNullOrEmpty(this string value)
        /// <summary> 
        /// 检查字符串 不是 null 且不是 <see cref="string.Empty"/> 字符串
        /// </summary>
        /// <param name="value"> 要测试的字符串 </param>
        /// <returns> 如果 <paramref name="value"/> 为 null 或空字符串 ("") 则为 false，否则为 true。 </returns>
        public static bool IsNotNullOrEmpty(this string value)
            => !string.IsNullOrEmpty(value);
#if Xunit
        [Fact]
        public static void IsNotNullOrEmptyTest()
        {
            string str = null;
            Assert.False(str.IsNotNullOrEmpty());
            str = string.Empty;
            Assert.False(str.IsNotNullOrEmpty());
            str = " ";
            Assert.True(str.IsNotNullOrEmpty());

            // 内置方法扩展，不需要深入测试
        }
#endif 
        #endregion


        #region public static bool IsNullOrWhiteSpace(this string value)
        /// <summary> 
        /// 检查字符串 是 null、空 <see cref="string.Empty"/> 或是仅由空白字符组成
        /// </summary>
        /// <param name="value"> 要测试的字符串 </param>
        /// <returns> 如果 <paramref name="value"/> 为 null 或空字符串 <see cref="string.Empty"/> 或仅由空白字符组成则为 true，否则为false 。 </returns>
        public static bool IsNullOrWhiteSpace(this string value)
            => string.IsNullOrWhiteSpace(value);
#if Xunit
        [Fact]
        public static void IsNullOrWhiteSpaceTest()
        {
            string str = null;
            Assert.True(str.IsNullOrWhiteSpace());
            str = string.Empty;
            Assert.True(str.IsNullOrWhiteSpace());
            str = " ";
            Assert.True(str.IsNullOrWhiteSpace());
            str = "　";
            Assert.True(str.IsNullOrWhiteSpace());
            str = "0";
            Assert.False(str.IsNullOrWhiteSpace());

            // 内置方法扩展，不需要深入测试
        }
#endif
        #endregion

        #region public static bool IsNotNullOrWhiteSpace(this string value)
        /// <summary> 
        /// 检查字符串 不是 null、空 <see cref="string.Empty"/> 或仅由空白字符组成 
        /// </summary>
        /// <param name="value"> 要测试的字符串 </param>
        /// <returns> 如果 <paramref name="value"/> 为 null 或空字符串 <see cref="string.Empty"/> 或仅由空白字符组成则为 false，否则为 true。 </returns>
        public static bool IsNotNullOrWhiteSpace(this string value)
            => !string.IsNullOrWhiteSpace(value);
#if Xunit
        [Fact]
        public static void IsNotNullOrWhiteSpaceTest()
        {
            string str = null;
            Assert.False(str.IsNotNullOrWhiteSpace());
            str = string.Empty;
            Assert.False(str.IsNotNullOrWhiteSpace());
            str = " ";
            Assert.False(str.IsNotNullOrWhiteSpace());
            str = "　";
            Assert.False(str.IsNotNullOrWhiteSpace());
            str = "0";
            Assert.True(str.IsNotNullOrWhiteSpace());

            // 内置方法扩展，不需要深入测试
        }
#endif
        #endregion


        #region public static string StringFormat(this string format, params object[] args)
        /// <summary>
        /// 将指定字符串中的格式项替换为指定参数中相应对象的字符串表示形式
        /// </summary>
        /// <param name="format"> 复合格式字符串 </param>
        /// <param name="args"> 一个对象数组，其中包含零个或多个要设置格式的对象 </param>
        /// <returns> <paramref name="format"/> 的副本，其中格式项已替换为 <paramref name="args"/> 中相应对象的字符串表示形式 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="format"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="args"/> 不能为 null </exception>
        /// <exception cref="FormatException"> <paramref name="format"/> 无效 或 格式项的索引小于零，或者大于或等于 <paramref name="args"/> 数组的长度 </exception>
        public static string StringFormat(this string format, params object[] args)
            => string.Format(format, args);
#if Xunit
        [Fact]
        public static void StringFormatTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.StringFormat());

            str = string.Empty;
            Assert.Throws<ArgumentNullException>(() => str.StringFormat(null));
            Assert.Equal(string.Empty, str.StringFormat());
            Assert.Equal(string.Empty, str.StringFormat("Hello"));

            str = "{-1} World!";
            Assert.Throws<FormatException>(() => str.StringFormat());

            str = "{0} {1}!";
            Assert.Throws<FormatException>(() => str.StringFormat("Hello"));
            Assert.Equal(" World!", str.StringFormat(null, "World"));
            Assert.Equal("Hello World!", str.StringFormat("Hello", "World"));
        }
#endif
        #endregion


        #region public static string TakeSubString(this string value, int skipLength, int subLength = int.MaxValue)
        /// <summary> 
        /// 从此实例检索子字符串。子字符串忽略指定的长度字符，然后返回具有指定长度的后续字符。
        /// </summary>
        /// <param name="value"> 字符串实例 </param>
        /// <param name="skipLength"> 要忽略的字符数量 </param>
        /// <param name="subLength"> 要取得的字符数量 </param>
        /// <returns> 基于 <paramref name="value"/> 字符串忽略指定的长度字符，然后根据指定长度返回后续的字符拼接而成的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="skipLength"/> 不能小于 0 </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="subLength"/> 必须大于 0 </exception>
        public static string TakeSubString(this string value, int skipLength, int subLength = int.MaxValue)
        {
            value.ThrowIfNull(nameof(value));
            skipLength.ThrowIfNegative(nameof(skipLength));
            subLength.ThrowIfNegativeOrZero(nameof(subLength));
            return new string(value.Skip(skipLength).Take(subLength).ToArray());
        }
#if Xunit
        [Fact]
        public static void TakeSubStringTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.TakeSubString(0));

            str = "Hello World!";
            Assert.Throws<ArgumentOutOfRangeException>(() => str.TakeSubString(-1));
            Assert.Equal("Hello", str.TakeSubString(0, 5));
            Assert.Throws<ArgumentOutOfRangeException>(() => str.TakeSubString(0, 0));
            Assert.Equal("World", str.TakeSubString(6, 5));
        }
#endif
        #endregion


        #region public static bool StartsWith(this string input, params string[] args)
        /// <summary>
        /// 确定此字符串实例的开头是否与指定的字符串匹配
        /// </summary>
        /// <param name="input"> 要比较的字符串 </param>
        /// <param name="args"> 要匹配的字符串 </param>
        /// <returns> 如果 <paramref name="input"/> 的开头与 <paramref name="args"/> 任意一项匹配，则为 true，否则为 false </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static bool StartsWith(this string input, params string[] args)
        {
            input.ThrowIfNull(nameof(input));
            return args.Any(value => input.StartsWith(value));
        }
#if Xunit
        [Fact]
        public static void StartsWithStringTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.StartsWith("a", "b"));

            str = "Hello World!";
            Assert.True(str.StartsWith("Hello", "World"));
            Assert.False(str.StartsWith("Yes", "World"));
        }
#endif
        #endregion

        #region public static bool StartsWith(this string input, params char[] args)
        /// <summary>
        /// 确定此字符串实例的开头是否与指定的字符串匹配
        /// </summary>
        /// <param name="input"> 要比较的字符串 </param>
        /// <param name="args"> 要匹配的字符串 </param>
        /// <returns> 如果 <paramref name="input"/> 的开头与 <paramref name="args"/> 任意一项匹配，则为 true，否则为 false </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static bool StartsWith(this string input, params char[] args)
        {
            input.ThrowIfNull(nameof(input));
            return args.Any(value => input.StartsWith(value));
        }
#if Xunit
        [Fact]
        public static void StartsWithCharTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.StartsWith('a', 'b'));

            str = "Hello World!";
            Assert.True(str.StartsWith(' ', 'H', 'W'));
            Assert.False(str.StartsWith('W', 'o', 'W'));
        }
#endif
        #endregion

        #region public static bool EndsWith(this string input, params string[] args)
        /// <summary>
        /// 确定此字符串实例的末尾是否与指定的字符串匹配
        /// </summary>
        /// <param name="input"> 要比较的字符串 </param>
        /// <param name="args"> 要匹配的字符串 </param>
        /// <returns> 如果 <paramref name="input"/> 的末尾与 <paramref name="args"/> 任意一项匹配，则为 true，否则为 false </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static bool EndsWith(this string input, params string[] args)
        {
            input.ThrowIfNull(nameof(input));
            return args.Any(value => input.EndsWith(value));
        }
#if Xunit
        [Fact]
        public static void EndsWithStringTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.EndsWith("a", "b"));

            str = "Hello World!";
            Assert.True(str.EndsWith("Hello", "World!"));
            Assert.False(str.EndsWith("Hello", "World"));
        }
#endif
        #endregion

        #region public static bool EndsWith(this string input, params char[] args)
        /// <summary>
        /// 确定此字符串实例的末尾是否与指定的字符串匹配
        /// </summary>
        /// <param name="input"> 要比较的字符串 </param>
        /// <param name="args"> 要匹配的字符串 </param>
        /// <returns> 如果 <paramref name="input"/> 的末尾与 <paramref name="args"/> 任意一项匹配，则为 true，否则为 false </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static bool EndsWith(this string input, params char[] args)
        {
            input.ThrowIfNull(nameof(input));
            return args.Any(value => input.EndsWith(value));
        }
#if Xunit
        [Fact]
        public static void EndsWithCharTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.EndsWith('a', 'b'));

            str = "Hello World!";
            Assert.True(str.EndsWith('H', 'W', '!'));
            Assert.False(str.EndsWith('W', 'o', 'W'));
        }
#endif 
        #endregion


    }
}
