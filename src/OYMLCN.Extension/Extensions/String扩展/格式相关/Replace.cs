using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
        #region public static string KeepOneLineBreak(this string input)
        /// <summary>
        /// 去除字符串内过多的换行符，多个换行仅保留一个
        /// </summary>
        /// <param name="input"> 要处理的字符串 </param>
        /// <returns> 处理后的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static string KeepOneLineBreak(this string input)
        {
            input.ThrowIfNull(nameof(input));

            string[] param = { "\r\n", "\n" };
            char[] chars = input.ReplaceValuesRegexMatches("\n", param).ToCharArray();
            char preChar = default;
            StringBuilder sb = new StringBuilder();
            foreach (var @char in chars)
            {
                if (preChar != '\n' && @char == '\n')
                    sb.Append("\r\n");
                else if (@char != '\n')
                    sb.Append(@char);
                preChar = @char;
            }
            return sb.ToString()/*.TrimEnd('\r', '\n')*/;
        }
#if Xunit
        [Fact]
        public static void KeepOneLineBreakTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.KeepOneLineBreak());

            str = string.Empty;
            Assert.Equal(str, str.KeepOneLineBreak());

            str = "你好，\r\n\r\n\n\n世界\n\n\n！\r\n\n";
            Assert.Equal("你好，\r\n世界\r\n！\r\n", str.KeepOneLineBreak());
        }
#endif
        #endregion

        #region public static string RemoveAllBlank(this string input)
        /// <summary>
        /// 去除字符串内的所有空格、换行、制表符
        /// </summary>
        /// <param name="input"> 要处理的字符串 </param>
        /// <returns> 已处理的结果 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static string RemoveAllBlank(this string input)
        {
            input.ThrowIfNull(nameof(input));
            return Regex.Replace(input, @"\s", "");
        }
#if Xunit
        [Fact]
        public static void RemoveAllBlankTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.RemoveAllBlank());

            str = string.Empty;
            Assert.Equal(str, str.RemoveAllBlank());

            str = "你\r 好\n，\r\n　世\t 界 ！  ";
            Assert.Equal("你好，世界！", str.RemoveAllBlank());
        }
#endif
        #endregion


        #region public static string[] RegexMatches(this string input, string pattern, RegexOptions options = RegexOptions.None)
        /// <summary>
        /// 使用正则表达式匹配字符串内的所有匹配项
        /// </summary>
        /// <param name="input"> 要搜索匹配项的字符串 </param>
        /// <param name="pattern"> 要匹配的正则表达式模式 </param>
        /// <param name="options"> 枚举值的按位组合，这些枚举值指定用于匹配的选项 </param>
        /// <returns> 已匹配到的字符串序列，如果没有匹配项，则返回一个空序列。 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="pattern"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="pattern"/> 正则表达式含有错误 </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="options"/> 不是 <see cref="RegexOptions"/> 值的有效按位组合 </exception>
        public static string[] RegexMatches(this string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            input.ThrowIfNull(nameof(input));
            pattern.ThrowIfNull(nameof(pattern));

            var result = new List<string>();
            if (!input.IsNullOrEmpty())
            {
                var data = Regex.Matches(input, pattern, options);
                foreach (var item in data)
                    result.Add(item.ToString());
            }
            return result.ToArray();
        }
#if Xunit
        [Fact]
        public static void RegexMatchesTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.RegexMatches(""));

            str = "Hello World";
            Assert.Throws<ArgumentNullException>(() => str.RegexMatches(null));
            Assert.ThrowsAny<Exception>(() => str.RegexMatches(@"^\d{5}-([a-zA-Z]\){4}$")); // RegexParseException
            Assert.Equal(new[] { "lo", "rl" }, str.RegexMatches("(lo)|(rl)"));
        }
#endif
        #endregion


        #region public static string ReplaceValues(this string input, string newValue, params string[] oldValues)
        /// <summary>
        /// 返回一个新字符串，其中当前实例中出现的所有指定字符串都替换为另一个指定的字符串
        /// </summary>
        /// <param name="input"> 要处理的字符串实例 </param>
        /// <param name="newValue"> 要替换 <paramref name="oldValues"/> 的所有匹配项的字符串 </param>
        /// <param name="oldValues"> 要替换的字符串 </param>
        /// <returns> 
        /// <para> 等效于当前字符串（除了 <paramref name="oldValues"/> 的所有实例都已替换为 <paramref name="newValue"/> 外）的字符串。 </para>
        /// <para> 如果在当前实例中找不到 <paramref name="oldValues"/>，此方法返回未更改的当前实例。 </para>
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="newValue"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="oldValues"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="oldValues"/> 包含空字符串 ("") </exception>
        public static string ReplaceValues(this string input, string newValue, params string[] oldValues)
        {
            input.ThrowIfNull(nameof(input));
            newValue.ThrowIfNull(nameof(newValue));
            oldValues.ThrowIfNull(nameof(oldValues));
            if (input.IsNullOrEmpty()) return input;

            foreach (var old in oldValues)
                input = input.Replace(old, newValue);
            return input;
        }
#if Xunit
        [Fact]
        public static void ReplaceValuesTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.ReplaceValues("demo", "test"));

            str = "Hello World!";
            Assert.Throws<ArgumentNullException>(() => str.ReplaceValues(null, "test"));
            Assert.Throws<ArgumentNullException>(() => str.ReplaceValues("demo", null));
            Assert.Throws<ArgumentException>(() => str.ReplaceValues("demo", string.Empty));

            Assert.Equal("Helll Wlrldl", str.ReplaceValues("l", "o", "!"));
        }
#endif
        #endregion

        #region public static string ReplaceValuesIgnoreCase(this string input, string newValue, params string[] oldValues)
#if NETSTANDARD2_1 || NETCOREAPP3_1
        /// <summary>
        /// 返回一个新字符串，其中当前实例中出现的所有指定字符串都替换为另一个指定的字符串（忽略匹配项的大小写）
        /// </summary>
        /// <param name="input"> 要处理的字符串实例 </param>
        /// <param name="newValue"> 要替换 <paramref name="oldValues"/> 的所有匹配项的字符串 </param>
        /// <param name="oldValues"> 要替换的字符串 </param>
        /// <returns> 
        /// <para> 等效于当前字符串（除了 <paramref name="oldValues"/> 的所有实例都已替换为 <paramref name="newValue"/> 外）的字符串。 </para>
        /// <para> 如果在当前实例中找不到 <paramref name="oldValues"/>，此方法返回未更改的当前实例。 </para>
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="newValue"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="oldValues"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="oldValues"/> 包含空字符串 ("") </exception>
        public static string ReplaceValuesIgnoreCase(this string input, string newValue, params string[] oldValues)
        {
            input.ThrowIfNull(nameof(input));
            newValue.ThrowIfNull(nameof(newValue));
            oldValues.ThrowIfNull(nameof(oldValues));
            if (input.IsNullOrEmpty()) return input;

            foreach (var old in oldValues)
                input = input.Replace(old, newValue, StringComparison.OrdinalIgnoreCase);
            return input;
        }
#if Xunit
        [Fact]
        public static void ReplaceValuesIgnoreCaseTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.ReplaceValuesIgnoreCase("demo", "test"));

            str = "Hello World!";
            Assert.Throws<ArgumentNullException>(() => str.ReplaceValuesIgnoreCase(null, "test"));
            Assert.Throws<ArgumentNullException>(() => str.ReplaceValuesIgnoreCase("demo", null));
            Assert.Throws<ArgumentException>(() => str.ReplaceValuesIgnoreCase("demo", string.Empty));

            Assert.Equal("Helll Wlrldl", str.ReplaceValuesIgnoreCase("l", "O", "!"));
        }
#endif
#endif
        #endregion

        #region public static string ReplaceValuesRegexMatches(this string input, string newValue, params string[] oldValues)
        /// <summary>
        /// 返回一个新字符串，其中当前实例中出现的所有指定字符串或与正则表达式匹配的项都替换为另一个指定的字符串
        /// </summary>
        /// <param name="input"> 要处理的字符串实例 </param>
        /// <param name="newValue"> 要替换 <paramref name="oldValues"/> 的所有匹配项的字符串 </param>
        /// <param name="oldValues"> 要替换的字符串或正则表达式 </param>
        /// <returns> 
        /// <para> 等效于当前字符串（除了 <paramref name="oldValues"/> 的所有实例都已替换为 <paramref name="newValue"/> 外）的字符串。 </para>
        /// <para> 如果在当前实例中找不到 <paramref name="oldValues"/>，此方法返回未更改的当前实例。 </para>
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="newValue"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="oldValues"/> 不能为 null </exception>
        public static string ReplaceValuesRegexMatches(this string input, string newValue, params string[] oldValues)
        {
            input.ThrowIfNull(nameof(input));
            newValue.ThrowIfNull(nameof(newValue));
            oldValues.ThrowIfNull(nameof(oldValues));
            if (input.IsNullOrEmpty()) return input;

            return Regex.Replace(input, $"({oldValues.Join("|")})", newValue);
        }
#if Xunit
        [Fact]
        public static void ReplaceValuesRegexMatchesTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.ReplaceValuesRegexMatches("demo", "test"));

            str = "Hello World!";
            Assert.Throws<ArgumentNullException>(() => str.ReplaceValuesRegexMatches(null, "test"));
            Assert.Throws<ArgumentNullException>(() => str.ReplaceValuesRegexMatches("demo", null));

            Assert.Equal("HellllWlrldl", str.ReplaceValuesRegexMatches("l", " ", "o", "!"));
            Assert.Equal("Helll Wlrldl", str.ReplaceValuesRegexMatches("l", "[ho]", "!"));
        }
#endif 
        #endregion

        #region public static string ReplaceValuesRegexMatchesIgnoreCase(this string input, string newValue, params string[] oldValues)
        /// <summary>
        /// 返回一个新字符串，其中当前实例中出现的所有指定字符串或与正则表达式匹配的项都替换为另一个指定的字符串（忽略匹配项的大小写）
        /// </summary>
        /// <param name="input"> 要处理的字符串实例 </param>
        /// <param name="newValue"> 要替换 <paramref name="oldValues"/> 的所有匹配项的字符串 </param>
        /// <param name="oldValues"> 要替换的字符串或正则表达式 </param>
        /// <returns> 
        /// <para> 等效于当前字符串（除了 <paramref name="oldValues"/> 的所有实例都已替换为 <paramref name="newValue"/> 外）的字符串。 </para>
        /// <para> 如果在当前实例中找不到 <paramref name="oldValues"/>，此方法返回未更改的当前实例。 </para>
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="newValue"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="oldValues"/> 不能为 null </exception>
        public static string ReplaceValuesRegexMatchesIgnoreCase(this string input, string newValue, params string[] oldValues)
        {
            input.ThrowIfNull(nameof(input));
            newValue.ThrowIfNull(nameof(newValue));
            oldValues.ThrowIfNull(nameof(oldValues));
            if (input.IsNullOrEmpty()) return input;

            return Regex.Replace(input, $"({oldValues.Join("|")})", newValue, RegexOptions.IgnoreCase);
        }
#if Xunit
        [Fact]
        public static void ReplaceValuesRegexMatchesIgnoreCaseTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.ReplaceValuesRegexMatchesIgnoreCase("demo", "test"));

            str = "Hello World!";
            Assert.Throws<ArgumentNullException>(() => str.ReplaceValuesRegexMatchesIgnoreCase(null, "test"));
            Assert.Throws<ArgumentNullException>(() => str.ReplaceValuesRegexMatchesIgnoreCase("demo", null));

            Assert.Equal("HellllWlrldl", str.ReplaceValuesRegexMatchesIgnoreCase("l", " ", "O", "!"));
            Assert.Equal("lelll Wlrldl", str.ReplaceValuesRegexMatchesIgnoreCase("l", "[Ho]", "!"));
        }
#endif
        #endregion


        #region public static string RemoveValues(this string input, params string[] values)
        /// <summary>
        /// 返回一个新字符串，其中当前实例中出现的所有指定字符串都会被移除
        /// </summary>
        /// <param name="input"> 要处理的字符串实例 </param>
        /// <param name="values"> 要移除的字符 </param>
        /// <returns> 由 <paramref name="input"/> 已移除所有 <paramref name="values"/> 指定字符串的新实例 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="values"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="values"/> 包含空字符串 ("") </exception>
        public static string RemoveValues(this string input, params string[] values)
            => ReplaceValues(input, string.Empty, values);
#if Xunit
        [Fact]
        public static void RemoveValuesTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.RemoveValues("demo"));

            str = "Hello World!";
            Assert.Throws<ArgumentNullException>(() => str.RemoveValues(null));
            Assert.Throws<ArgumentException>(() => str.RemoveValues(string.Empty));

            Assert.Equal("He Wrd", str.RemoveValues("l", "o", "!"));
        }
#endif
        #endregion

        #region public static string RemoveValuesIgnoreCase(this string input, params string[] values)
#if NETSTANDARD2_1 || NETCOREAPP3_1
        /// <summary>
        /// 返回一个新字符串，其中当前实例中出现的所有指定字符串都会被移除（忽略匹配项的大小写）
        /// </summary>
        /// <param name="input"> 要处理的字符串实例 </param>
        /// <param name="values"> 要移除的字符 </param>
        /// <returns> 由 <paramref name="input"/> 已移除所有 <paramref name="values"/> 指定字符串的新实例 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="values"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="values"/> 包含空字符串 ("") </exception>
        public static string RemoveValuesIgnoreCase(this string input, params string[] values)
            => ReplaceValuesIgnoreCase(input, string.Empty, values);
#if Xunit
        [Fact]
        public static void RemoveValuesIgnoreCaseTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.RemoveValuesIgnoreCase("demo"));

            str = "Hello World!";
            Assert.Throws<ArgumentNullException>(() => str.RemoveValuesIgnoreCase(null));
            Assert.Throws<ArgumentException>(() => str.RemoveValuesIgnoreCase(string.Empty));

            Assert.Equal("He Wrd", str.RemoveValuesIgnoreCase("l", "O", "!"));
        }
#endif
#endif
        #endregion

        #region public static string RemoveValuesRegexMatches(this string input, params string[] values)
        /// <summary>
        /// 返回一个新字符串，其中当前实例中出现的所有指定字符串或与正则表达式匹配的字符都会被移除
        /// </summary>
        /// <param name="input"> 要处理的字符串实例 </param>
        /// <param name="values"> 要移除的字符 </param>
        /// <returns> 由 <paramref name="input"/> 已移除所有 <paramref name="values"/> 指定字符串或与正则表达式匹配的新实例 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="values"/> 不能为 null </exception>
        public static string RemoveValuesRegexMatches(this string input, params string[] values)
        {
            input.ThrowIfNull(nameof(input));
            values.ThrowIfNull(nameof(values));
            return input.ReplaceValuesRegexMatches(string.Empty, values);
        }
#if Xunit
        [Fact]
        public static void RemoveValuesRegexMatchesTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.RemoveValuesRegexMatches(""));

            str = "Hello World";
            Assert.Throws<ArgumentNullException>(() => str.RemoveValuesRegexMatches(null));
            Assert.ThrowsAny<Exception>(() => str.RemoveValuesRegexMatches(@"^\d{5}-([a-zA-Z]\){4}$")); // RegexParseException

            Assert.Equal("Hel Wod", str.RemoveValuesRegexMatches("lo", "(rl)"));
        }
#endif
        #endregion

        #region public static string RemoveValuesRegexMatchesIgnoreCase(this string input, params string[] values)
        /// <summary>
        /// 返回一个新字符串，其中当前实例中出现的所有指定字符串或与正则表达式匹配的字符都会被移除（忽略匹配项的大小写）
        /// </summary>
        /// <param name="input"> 要处理的字符串实例 </param>
        /// <param name="values"> 要移除的字符 </param>
        /// <returns> 由 <paramref name="input"/> 已移除所有 <paramref name="values"/> 指定字符串或与正则表达式匹配的新实例 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="values"/> 不能为 null </exception>
        public static string RemoveValuesRegexMatchesIgnoreCase(this string input, params string[] values)
        {
            input.ThrowIfNull(nameof(input));
            values.ThrowIfNull(nameof(values));
            return input.ReplaceValuesRegexMatchesIgnoreCase(string.Empty, values);
        }
#if Xunit
        [Fact]
        public static void RemoveValuesRegexMatchesIgnoreCaseTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.RemoveValuesRegexMatchesIgnoreCase(""));

            str = "Hello World";
            Assert.Throws<ArgumentNullException>(() => str.RemoveValuesRegexMatchesIgnoreCase(null));
            Assert.ThrowsAny<Exception>(() => str.RemoveValuesRegexMatchesIgnoreCase(@"^\d{5}-([a-zA-Z]\){4}$")); // RegexParseException

            Assert.Equal("Hel rld", str.RemoveValuesRegexMatchesIgnoreCase("lo", "(WO)"));
        }
#endif
        #endregion

    }
}
