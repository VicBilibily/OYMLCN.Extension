using OYMLCN.ArgumentChecker;
using System;
using System.Linq;
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
        #region public static string[] Split(this string input, string separator, StringSplitOptions options = StringSplitOptions.None)
#if NET472 || NETSTANDARD2_0
        /// <summary>
        /// 基于指定的字符串将一个字符串拆分成最大数量的子字符串
        /// </summary>
        /// <param name="input"> 要拆分的字符串 </param>
        /// <param name="separator"> 拆分字符串的指定字符串 </param>
        /// <param name="options"> 要省略返回的数组中的空数组元素，则为 <see cref="StringSplitOptions.RemoveEmptyEntries"/>，要包含返回的数组中的空数组元素，则为 <see cref="StringSplitOptions.None"/> </param>
        /// <returns> 一个数组，其元素包含此字符串中的子字符串，这些子字符串由 separator 分隔 </returns>
        /// <exception cref="NullReferenceException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="options"/> 不是 <see cref="StringSplitOptions"/> 值之一 </exception>
        public static string[] Split(this string input, string separator, StringSplitOptions options = StringSplitOptions.None)
            => input.Split(new[] { separator }, options);
#endif
#if Xunit
        [Fact]
        public static void SplitTest()
        {
            string str = null;
            Assert.Throws<NullReferenceException>(() => str.Split("Demo"));

            str = "Hello World!";
            Assert.Equal(new[] { str }, str.Split((string)null));
            Assert.Equal(new[] { str }, str.Split(string.Empty));
            Assert.Equal(new[] { "Hello", "World!" }, str.Split(" "));

            Assert.Throws<ArgumentException>(() => str.Split(" ", options: (StringSplitOptions)(-1)));
        }
#endif
        #endregion

        #region public static string[] Split(this string input, params string[] separator)
        /// <summary>
        /// 基于指定的字符串将一个字符串拆分成最大数量的子字符串
        /// </summary>
        /// <param name="input"> 要拆分的字符串 </param>
        /// <param name="separator"> 分隔此字符串中子字符串的字符串数组 </param>
        /// <returns> 一个数组，其元素包含此字符串中的子字符串，这些子字符串由 separator 中的一个或多个字符串分隔 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="separator"/> 不能为 null </exception>
        public static string[] Split(this string input, params string[] separator)
        {
            input.ThrowIfNull(nameof(input));
            separator.ThrowIfNull(nameof(separator));
            // 加上 options 以调用内置方法，不加会造成循环调用
            return input.Split(separator, options: StringSplitOptions.None);
        }
#if Xunit
        [Fact]
        public static void SplitArrayTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.Split("Demo", "Test"));

            str = "Hello World!";
            string[] arr = null;
            Assert.Throws<ArgumentNullException>(() => Split(str, arr));
            Assert.Equal(new[] { "", "llo ", "rld!" }, str.Split("He", "Wo"));
            Assert.Equal(new[] { "Hello", "World", "" }, str.Split(null, string.Empty, " ", "!"));
        }
#endif
        #endregion


        #region public static string[] SplitRemoveEmpty(this string input, string separator)
        /// <summary>
        /// 基于指定的字符串将一个字符串拆分成最大数量的子字符串（不包含空字符串对象）
        /// </summary>
        /// <param name="input"> 要拆分的字符串 </param>
        /// <param name="separator"> 拆分字符串的指定字符串 </param>
        /// <returns> 一个数组，其元素包含此字符串中的子字符串，这些子字符串由 separator 分隔 </returns>
        /// <exception cref="NullReferenceException"> <paramref name="input"/> 不能为 null </exception>
        public static string[] SplitRemoveEmpty(this string input, string separator)
            => input.Split(separator, StringSplitOptions.RemoveEmptyEntries);
#if Xunit
        [Fact]
        public static void SplitStringRemoveEmptyTest()
        {
            string str = null;
            Assert.Throws<NullReferenceException>(() => str.SplitRemoveEmpty("Demo"));

            str = "Hello, ,World!";
            Assert.Equal(new[] { str }, str.SplitRemoveEmpty((string)null));
            Assert.Equal(new[] { str }, str.SplitRemoveEmpty(string.Empty));
            Assert.Equal(new[] { "Hello", " ", "World!" }, str.SplitRemoveEmpty(","));
        }
#endif
        #endregion

        #region public static string[] SplitRemoveEmpty(this string input, params string[] separator)
        /// <summary>
        /// 基于指定的字符串将一个字符串拆分成最大数量的子字符串（不包含空字符串对象）
        /// </summary>
        /// <param name="input"></param>
        /// <param name="separator"> 拆分字符串的指定字符串 </param>
        /// <returns> 一个数组，其元素包含此字符串中的子字符串，这些子字符串由 separator 中的一个或多个字符串分隔 </returns>
        /// <exception cref="NullReferenceException"> <paramref name="input"/> 不能为 null </exception>
        public static string[] SplitRemoveEmpty(this string input, params string[] separator)
            => input.Split(separator, StringSplitOptions.RemoveEmptyEntries);
#if Xunit
        [Fact]
        public static void SplitStringsRemoveEmptyTest()
        {
            string str = null;
            Assert.Throws<NullReferenceException>(() => str.SplitRemoveEmpty("Demo", "Test"));

            str = "Hello, ,World!";
            Assert.Equal(new[] { str }, str.SplitRemoveEmpty((string)null, "Demo"));
            Assert.Equal(new[] { str }, str.SplitRemoveEmpty(string.Empty, "Demo"));
            Assert.Equal(new[] { "Hello", "World!" }, str.SplitRemoveEmpty(",", " "));
        }
#endif
        #endregion


        #region public static string SplitThenGetFirst(this string input, string separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        /// <summary>
        /// 基于指定的字符串将一个字符串拆分子字符串后获得第一个字符串对象
        /// </summary>
        /// <param name="input"> 要拆分的字符串 </param>
        /// <param name="separator"> 拆分字符串的指定字符串 </param>
        /// <param name="options"> 要省略返回的数组中的空数组元素，则为 <see cref="StringSplitOptions.RemoveEmptyEntries"/>，要包含返回的数组中的空数组元素，则为 <see cref="StringSplitOptions.None"/> </param>
        /// <returns> 基于指定的字符串将一个字符串拆分子字符串后获得第一个字符串对象 </returns>
        /// <exception cref="NullReferenceException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="options"/> 不是 <see cref="StringSplitOptions"/> 值之一 </exception>
        public static string SplitThenGetFirst(this string input, string separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
            => input.Split(separator, options).FirstOrDefault();
#if Xunit
        [Fact]
        public static void SplitThenGetFirstTest()
        {
            string str = null;
            Assert.Throws<NullReferenceException>(() => str.SplitThenGetFirst("Demo"));

            str = "Hello, ,World!";
            Assert.Equal(str, str.SplitThenGetFirst(null));
            Assert.Equal(str, str.SplitThenGetFirst(string.Empty));
            Assert.Equal("Hello,", str.SplitThenGetFirst(" "));
            Assert.Equal("Hello", str.SplitThenGetFirst(","));

            Assert.Throws<ArgumentException>(() => str.SplitThenGetFirst("Demo", (StringSplitOptions)(-1)));
        }
#endif
        #endregion

        #region public static string SplitThenGetLast(this string str, string separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        /// <summary>
        /// 基于指定的字符串将一个字符串拆分子字符串后获得最后一个字符串对象
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"> 拆分字符串的指定字符串 </param>
        /// <param name="options"> 要省略返回的数组中的空数组元素，则为 <see cref="StringSplitOptions.RemoveEmptyEntries"/>，要包含返回的数组中的空数组元素，则为 <see cref="StringSplitOptions.None"/> </param>
        /// <returns> 基于指定的字符串将一个字符串拆分子字符串后获得最后一个字符串对象 </returns>
        /// <exception cref="ArgumentException"> <paramref name="options"/> 不是 <see cref="StringSplitOptions"/> 值之一 </exception>
        public static string SplitThenGetLast(this string str, string separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
            => str.Split(separator, options).LastOrDefault();
#if Xunit
        [Fact]
        public static void SplitThenGetLastTest()
        {
            string str = null;
            Assert.Throws<NullReferenceException>(() => str.SplitThenGetLast("Demo"));

            str = "Hello, ,World!";
            Assert.Equal(str, str.SplitThenGetLast(null));
            Assert.Equal(str, str.SplitThenGetLast(string.Empty));
            Assert.Equal(",World!", str.SplitThenGetLast(" "));
            Assert.Equal("World!", str.SplitThenGetLast(","));

            Assert.Throws<ArgumentException>(() => str.SplitThenGetFirst("Demo", (StringSplitOptions)(-1)));
        }
#endif 
        #endregion


        #region public static string[] RegexSplit(this string input, string pattern, RegexOptions options = RegexOptions.None)
        /// <summary>
        /// 在由指定正则表达式模式定义的位置将输入字符串拆分为一个子字符串数组
        /// </summary>
        /// <param name="input"> 要拆分的字符串 </param>
        /// <param name="pattern"> 要匹配的正则表达式模式 </param>
        /// <param name="options"> 枚举值的一个按位组合，这些枚举值提供匹配选项 </param>
        /// <returns> 字符串数组 </returns>
        /// <exception cref="ArgumentException"> 出现正则表达式分析错误 </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="pattern"/> 不能为 null </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="options"/> 不是 <see cref="RegexOptions"/> 值的有效按位组合 </exception>
        /// <exception cref="RegexMatchTimeoutException"> 发生超时 </exception>
        public static string[] RegexSplit(this string input, string pattern, RegexOptions options = RegexOptions.None)
            => Regex.Split(input, pattern, options);
#if Xunit
        [Fact]
        public static void RegexSplitTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.RegexSplit(""));

            str = "Hello World";
            Assert.Throws<ArgumentNullException>(() => str.RegexSplit(null));
            Assert.ThrowsAny<Exception>(() => str.RegexSplit(@"^\d{5}-([a-zA-Z]\){4}$")); // RegexParseException
            Assert.Throws<ArgumentOutOfRangeException>(() => str.RegexSplit("lo|WO", (RegexOptions)(-1)));

            Assert.Equal(new[] { "Hel", " ", "rld" }, str.RegexSplit("lo|WO", RegexOptions.IgnoreCase));
        }
#endif
        #endregion

        #region public static string[] SplitAuto(this string input)
        /// <summary>
        /// 根据 | \ / 、 ， , 空格 中文空格 制表符 换行 拆分字符串
        /// </summary>
        /// <param name="input"> 要拆分的字符串 </param>
        /// <returns>
        /// 根据 | \ / 、 ， , 空格 中文空格 制表符 换行 拆分字符串后的序列
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static string[] SplitAuto(this string input)
        {
            input.ThrowIfNull(nameof(input));
            return input.RegexSplit(@"<br\s*/?>|\s|[|\\/、:：,，\t]", RegexOptions.IgnoreCase)
                        .Select(value => value.Trim())
                        .Where(value => !value.StartsWith("<br", StringComparison.OrdinalIgnoreCase))
                        .WhereIsNotNullOrEmpty()
                        .ToArray();
        }
#if Xunit
        [Fact]
        public static void SplitAutoTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.SplitAuto());

            str = string.Empty;
            Assert.Empty(str.SplitAuto());

            str = "You:Are：New|Hello/World\\!";
            Assert.Equal(new[] { "You", "Are", "New", "Hello", "World", "!" }, str.SplitAuto());

            str = "中国、美国\t英国";
            Assert.Equal(new[] { "中国", "美国", "英国" }, str.SplitAuto());

            str = " It is My New <br /> Work   <br    >，<br> Hello \n World \r  ! \r\n ";
            Assert.Equal(new[] { "It", "is", "My", "New", "Work", "Hello", "World", "!" }, str.SplitAuto());
        }
#endif 
        #endregion

        #region public static string[] SplitByLines(this string input)
        /// <summary>
        /// 根据换行符拆分字符串，且清除前后空格和空白项
        /// </summary>
        /// <param name="input"> 要拆分的字符串 </param>
        /// <returns>
        /// <para> 基于换行符拆分处理，且清除前后空格和空白项的拆分序列 </para>
        /// <para> 如果 <paramref name="input"/> 仅由换行或空格组成，则返回空序列 </para>
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static string[] SplitByLines(this string input)
        {
            input.ThrowIfNull(nameof(input));
            return input.RegexSplit(@"<br\s*/?>|\r|\n", RegexOptions.IgnoreCase)
                        .Select(value => value.Trim())
                        .Where(value => !value.StartsWith("<br", StringComparison.OrdinalIgnoreCase))
                        .WhereIsNotNullOrEmpty()
                        .ToArray();
        }
#if Xunit
        [Fact]
        public static void SplitByLinesTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.SplitByLines());

            str = string.Empty;
            Assert.Empty(str.SplitByLines());

            str = " \r\n  \n  ";
            Assert.Empty(str.SplitByLines());

            str = " It is My New <br /> Work   <br    >，<br> Hello \n World \r  ! \r\n ";
            Assert.Equal(new[] { "It is My New", "Work", "，", "Hello", "World", "!" }, str.SplitByLines());
        }
#endif 
        #endregion

    }
}
