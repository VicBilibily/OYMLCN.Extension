using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 字符串相关扩展
    /// </summary>
    public static partial class StringExtension
    {
        #region public static bool Contains(this string str, params string[] words)
        /// <summary>
        /// 返回一个值，该值指示指定的子串是否出现在此字符串中
        /// </summary>
        /// <param name="str"> 搜寻对象 </param>
        /// <param name="words"> 要搜寻的字符串 </param>
        /// <returns> 如果要搜寻的字符串参数出现在搜寻对象字符串中，或者 <paramref name="words"/> 包含空字符串 ("")，则为 true；否则为 false。 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="words"/> 不能含有 null </exception>
        public static bool Contains(this string str, params string[] words)
        {
            if (str.IsNullOrEmpty()) return false;
            return words.Any(word => str.Contains(word));
        }
#if Xunit
        [Fact]
        public static void ContainsWordsTest()
        {
            string str = null;
            Assert.False(str.Contains("0", "1"));

            str = "20200202";
            Assert.False(str.Contains("10", "11"));
            Assert.True(str.Contains("020", "100"));
            Assert.True(str.Contains("20", "00"));
            Assert.True(str.Contains("110", string.Empty));
            Assert.Throws<ArgumentNullException>(() => str.Contains(null, string.Empty));
        }
#endif
        #endregion


        #region public static string Join(this IEnumerable<string> values, string separator = "")
        /// <summary>
        /// 串联类型为 <see cref="IEnumerable{T}"/>，T为 <see cref="string"/> 构造集合的成员，其中在每个成员之间使用指定的分隔符
        /// </summary>
        /// <param name="values"> 一个包含要串联的字符串的集合 </param>
        /// <param name="separator"> 要用作分隔符的字符串。只有在 <paramref name="values"/> 具有多个元素时，<paramref name="separator"/> 才包括在返回的字符串中 </param>
        /// <returns>
        /// <para> 一个由 <paramref name="values"/> 的成员组成的字符串，这些成员以 <paramref name="separator"/> 字符串分隔。 </para>
        /// <para> 如果 <paramref name="values"/> 没有成员，则该方法返回 <see cref="string.Empty"/>。 </para>
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="values"/> 不能为 null </exception>
        public static string Join(this IEnumerable<string> values, string separator = "")
            => string.Join(separator, values);
#if Xunit
        [Fact]
        public static void JoinValuesTest()
        {
            string[] list = null;
            Assert.Throws<ArgumentNullException>(() => list.Join());

            list = new string[0];
            Assert.Equal(string.Empty, list.Join());

            list = new[] { "Hello" };
            Assert.Equal("Hello", list.Join());
            Assert.Equal("Hello", list.Join(","));

            list = new[] { "Hello", "World" };
            Assert.Equal("HelloWorld", list.Join());
            Assert.Equal("Hello,World", list.Join(","));

            list = new[] { "Hello", null, "World" };
            Assert.Equal("HelloWorld", list.Join());
            Assert.Equal("Hello,,World", list.Join(","));
        }
#endif
        #endregion


        #region public static IEnumerable<string> WhereContains(this IEnumerable<string> source, params string[] words)
        /// <summary>
        /// 基于提供的关键词筛选包含提供值的序列
        /// </summary>
        /// <param name="source"> 需要筛选的字符串序列 </param>
        /// <param name="words"> 要搜寻的字符串 </param>
        /// <returns> 
        /// <para> 如果要筛选的字符串序列含有字符串包含在 <paramref name="words"/> 中，则返回对应的字符串对象序列 </para> 
        /// <para> 如果 <paramref name="words"/> 包含空字符串 ("")，则返回已排除 null 对象的 <paramref name="source"/> 序列 </para>
        /// <para> 如果 <paramref name="words"/> 为空，不包含任何值，则返回一个空的字符串对象序列 </para>
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="words"/> 不能含有 null </exception>
        public static IEnumerable<string> WhereContains(this IEnumerable<string> source, params string[] words)
            => source.Where(item => item.IsNotNull() && item.Contains(words));
#if Xunit
        [Fact]
        public static void WhereContainsTest()
        {
            string[] arr = null;
            Assert.Throws<ArgumentNullException>(() => arr.WhereContains(""));
            arr = new[] { "Hello", " ", null, "World!" };
            Assert.Throws<ArgumentNullException>(() => arr.WhereContains(null).ToArray());

            Assert.Equal(new[] { " " }, arr.WhereContains(" "));
            Assert.Equal(new[] { " ", "World!" }, arr.WhereContains(" ", "!"));
            Assert.Equal(new[] { "Hello", " ", "World!" }, arr.WhereContains(" ", "o"));
            Assert.Empty(arr.WhereContains());
        }
#endif
        #endregion

        #region public static IEnumerable<string> WhereStartsWith(this IEnumerable<string> source, string value)
        /// <summary>
        /// 基于提供的关键词筛选包含此字符串实例的开头是否与指定的字符串匹配的序列
        /// </summary>
        /// <param name="source"> 需要筛选的字符串序列 </param>
        /// <param name="value"> 要比较的字符串 </param>
        /// <returns>
        /// <para> 如果要筛选的字符串序列含有字符串实例的开头是否与指定的字符串 <paramref name="value"/> 匹配，则返回对应的字符串对象序列 </para>
        /// <para> 如果 <paramref name="value"/> 为空字符串 ("")，则返回已排除 null 对象的 <paramref name="source"/> 序列 </para>
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        public static IEnumerable<string> WhereStartsWith(this IEnumerable<string> source, string value)
            => source.Where(item => item.IsNotNull() && item.StartsWith(value));
#if Xunit
        [Fact]
        public static void WhereStartsWithTest()
        {
            string[] arr = null;
            Assert.Throws<ArgumentNullException>(() => arr.WhereStartsWith(""));
            arr = new[] { "Hello", " ", null, "World!" };
            Assert.Throws<ArgumentNullException>(() => arr.WhereStartsWith(null).ToArray());

            Assert.Equal(new[] { " " }, arr.WhereStartsWith(" "));
            Assert.Equal(new[] { "World!" }, arr.WhereStartsWith("W"));
            Assert.Equal(new[] { "Hello", " ", "World!" }, arr.WhereStartsWith(string.Empty));
            Assert.Empty(arr.WhereStartsWith("Y"));
        }
#endif 
        #endregion

        #region public static IEnumerable<string> WhereEndsWith(this IEnumerable<string> source, string value)
        /// <summary>
        /// 基于提供的关键词筛选包含此字符串实例的结尾是否与指定的字符串匹配的序列
        /// </summary>
        /// <param name="source"> 需要筛选的字符串的序列 </param>
        /// <param name="value"> 要与此序列实例末尾的子字符串进行比较的字符串 </param>
        /// <returns>
        /// <para> 如果要筛选的字符串序列含有字符串实例的结尾是否与指定的字符串 <paramref name="value"/> 匹配，则返回对应的字符串对象序列 </para>
        /// <para> 如果 <paramref name="value"/> 为空字符串 ("")，则返回已排除 null 对象的 <paramref name="source"/> 序列 </para>
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        public static IEnumerable<string> WhereEndsWith(this IEnumerable<string> source, string value)
            => source.Where(item => item.IsNotNull() && item.EndsWith(value));
#if Xunit
        [Fact]
        public static void WhereEndsWithTest()
        {
            string[] arr = null;
            Assert.Throws<ArgumentNullException>(() => arr.WhereEndsWith(string.Empty));
            arr = new[] { "Hello", " ", null, "World!" };
            Assert.Throws<ArgumentNullException>(() => arr.WhereEndsWith(null).ToArray());

            Assert.Equal(new[] { " " }, arr.WhereEndsWith(" "));
            Assert.Equal(new[] { "World!" }, arr.WhereEndsWith("!"));
            Assert.Equal(new[] { "Hello", " ", "World!" }, arr.WhereEndsWith(string.Empty));
            Assert.Empty(arr.WhereEndsWith("Y"));
        }
#endif
        #endregion


        #region public static IEnumerable<string> WhereIsNotNullOrEmpty(this IEnumerable<string> source)
        /// <summary>
        /// 筛选不是空或由空格组成的字符串实例
        /// </summary>
        /// <param name="source"> 需要筛选的字符串的序列 </param>
        /// <returns>  从 <paramref name="source"/> 筛选出来的不是空或由空格组成的字符串序列 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source"/> 不能为 null </exception>
        public static IEnumerable<string> WhereIsNotNullOrEmpty(this IEnumerable<string> source)
            => source.Where(item => item.IsNotNullOrEmpty());
#if Xunit
        [Fact]
        public static void WhereIsNotNullOrEmptyTest()
        {
            string[] arr = null;
            Assert.Throws<ArgumentNullException>(() => arr.WhereIsNotNullOrEmpty());

            arr = new[] { "Hello", "", null, "World!" };
            Assert.Equal(new[] { "Hello", "World!" }, arr.WhereIsNotNullOrEmpty());
            arr = new[] { "Hello", " ", "World!" };
            Assert.Equal(arr, arr.WhereIsNotNullOrEmpty());
        }
#endif
        #endregion

        #region public static IEnumerable<string> WhereIsNotNullOrWhiteSpace(this IEnumerable<string> source)
        /// <summary>
        /// 筛选不是空或由空格组成的字符串实例
        /// </summary>
        /// <param name="source"> 需要筛选的字符串的序列 </param>
        /// <returns>  从 <paramref name="source"/> 筛选出来的不是空或由空格组成的字符串序列 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source"/> 不能为 null </exception>
        public static IEnumerable<string> WhereIsNotNullOrWhiteSpace(this IEnumerable<string> source)
            => source.Where(item => item.IsNotNullOrWhiteSpace());
#if Xunit
        [Fact]
        public static void WhereIsNotNullOrWhiteSpaceTest()
        {
            string[] arr = null;
            Assert.Throws<ArgumentNullException>(() => arr.WhereIsNotNullOrWhiteSpace());

            arr = new[] { "Hello", "", " ", null, "World!" };
            Assert.Equal(new[] { "Hello", "World!" }, arr.WhereIsNotNullOrWhiteSpace());
        }
#endif 
        #endregion
    }
}
