using OYMLCN.ArgumentChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
        #region public static string UrlEncode(this string value)
        /// <summary>
        /// 将文本字符串转换为 URL 编码的字符串
        /// </summary>
        /// <param name="value"> 要进行 URL 编码的文本 </param>
        /// <returns> URL 编码的字符串 </returns>
        public static string UrlEncode(this string value)
            => WebUtility.UrlEncode(value);
#if Xunit
        [Fact]
        public static void UrlEncodeTest()
        {
            string str = null;
            Assert.Null(str.UrlEncode());

            str = string.Empty;
            Assert.Empty(str.UrlEncode());

            str = " ";
            Assert.Equal("+", str.UrlEncode());

            // 调用内置方法，不再详细写单元测试
        }
#endif
        #endregion

        #region public static string UrlDecode(this string encodedValue)
        /// <summary>
        /// 将已编码用于 URL 传输的字符串转换为解码的字符串
        /// </summary>
        /// <param name="encodedValue"> 要解码的 URL 编码的字符串 </param>
        /// <returns> 已解码的字符串 </returns>
        public static string UrlDecode(this string encodedValue)
            => WebUtility.UrlDecode(encodedValue);
#if Xunit
        [Fact]
        public static void UrlDecodeTest()
        {
            string str = null;
            Assert.Null(str.UrlDecode());

            str = string.Empty;
            Assert.Empty(str.UrlDecode());

            str = "+";
            Assert.Equal(" ", str.UrlDecode());

            // 调用内置方法，不再详细写单元测试
        }
#endif
        #endregion


        #region public static string EscapeUriString(this string stringToEscape)
        /// <summary>
        /// 将 URL 字符串转换为它的转义表示形式的参数名称/值编码合法的格式
        /// </summary>
        /// <param name="stringToEscape"> 要转义的字符串 </param>
        /// <returns> 一个 <paramref name="stringToEscape"/> 转义表示形式的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="stringToEscape"/> 不能为 null </exception>
        /// <exception cref="UriFormatException"> <paramref name="stringToEscape"/> 的长度超过 32766 个字符 </exception>
        public static string EscapeUriString(this string stringToEscape)
            => Uri.EscapeUriString(stringToEscape);
#if Xunit
        [Fact]
        public static void EscapeUriStringTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.EscapeUriString());

            str = string.Empty;
            Assert.Empty(str.EscapeUriString());

            // 调用内置方法，不再详细写单元测试
        }
#endif 
        #endregion

        #region public static string UriEscapeDataString(this string stringToEscape)
        /// <summary>
        /// 将 URL 字符串转换为它的转义表示形式的参数名称/值编码合法的格式
        /// </summary>
        /// <param name="stringToEscape"> 要转义的字符串 </param>
        /// <returns> 一个 <paramref name="stringToEscape"/> 转义表示形式的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="stringToEscape"/> 不能为 null </exception>
        /// <exception cref="UriFormatException"> <paramref name="stringToEscape"/> 的长度超过 32766 个字符 </exception>
        public static string UriEscapeDataString(this string stringToEscape)
            => Uri.EscapeDataString(stringToEscape);
#if Xunit
        [Fact]
        public static void UriEscapeDataStringTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.UriEscapeDataString());

            str = string.Empty;
            Assert.Empty(str.UriEscapeDataString());

            // 调用内置方法，不再详细写单元测试
        }
#endif 
        #endregion


        #region public static string GetUrlHost(this string uriString)
        /// <summary>
        /// 获取 URL 字符串的的域名地址（eg：www.qq.com）
        /// </summary>
        /// <param name="uriString"> URL 字符串 </param>
        /// <returns> 域名地址（eg：www.qq.com） </returns>
        public static string GetUrlHost(this string uriString)
            => uriString.ConvertToUri()?.Host;
#if Xunit
        [Fact]
        public static void GetUrlHostTest()
        {
            string str = null;
            Assert.Null(str.GetUrlHost());

            str = "https://www.qq.com/index.shtml";
            Assert.Equal("www.qq.com", str.GetUrlHost());
        }
#endif 
        #endregion

        #region public static string GetUrlSchemeHost(this string uriString)
        /// <summary>
        /// 获取 URL 字符串的的协议域名地址（eg：https://www.qq.com）
        /// </summary>
        /// <param name="uriString"> URL 字符串 </param>
        /// <returns> 协议域名地址（eg：https://www.qq.com） </returns>
        public static string GetUrlSchemeHost(this string uriString)
            => uriString.ConvertToUri()?.GetSchemeHost();
#if Xunit
        [Fact]
        public static void GetUrlSchemeHostTest()
        {
            string str = null;
            Assert.Null(str.GetUrlSchemeHost());

            str = "https://www.qq.com/index.shtml";
            Assert.Equal("https://www.qq.com", str.GetUrlSchemeHost());
        }
#endif
        #endregion


        #region public static Dictionary<string, string[]> QueryStringToDictionary(this string queryString)
        /// <summary>
        /// 把 QueryString 拆解为字典
        /// </summary>
        /// <param name="queryString"> Url 字符串或查询字符串 </param>
        /// <returns> 将 <paramref name="queryString"/> 提取后的字典 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="queryString"/> 不能为 null </exception>
        public static Dictionary<string, string[]> QueryStringToDictionary(this string queryString)
        {
            queryString.ThrowIfNull(nameof(queryString));

            var dic = new Dictionary<string, List<string>>();
            if (queryString.IsNotNullOrWhiteSpace())
            {
                queryString = WebUtility.HtmlDecode(queryString);
                queryString = queryString.SplitThenGetLast("?");
                var queryGroup = queryString.Split('&')
                    .Select(item =>
                    {
                        var key = item.SplitThenGetFirst("=");
                        // 可能值中也会有 = 号，除了第一个 = 号认作参数分割，其他均认为是值
                        var value = item.TakeSubString(key.Length + 1);
                        value = WebUtility.UrlDecode(value);
                        return new { key, value };
                    })
                    .GroupBy(v => v.key)
                    .ToDictionary(
                        v => v.Key,
                        v => v.Select(s => s.value).WhereIsNotNullOrEmpty().ToArray());
                foreach (var item in queryGroup)
                {
                    var key = item.Key.SplitThenGetFirst("[");
                    if (dic.ContainsKey(key))
                        dic[key].AddRange(item.Value);
                    else
                        dic[key] = item.Value.ToList();
                }
            }
            return dic.ToDictionary(v => v.Key, v => v.Value.ToArray());
        }
#if Xunit
        [Fact]
        public static void QueryStringToDictionaryTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.QueryStringToDictionary());

            str = "";
            Assert.Empty(str.QueryStringToDictionary());

            str = "date&time=0&time[1]=1";
            Assert.Equal(
                new Dictionary<string, string[]> {
                    { "date", new string[0] },
                    { "time", new[] { "0", "1" } },
                }, str.QueryStringToDictionary());

            str = "date=2020-02-02&time[0]=0&time[1]=1";
            Assert.Equal(
                new Dictionary<string, string[]> {
                    { "date", new[] { "2020-02-02" } },
                    { "time", new[] { "0", "1" } },
                }, str.QueryStringToDictionary());
        }
#endif
        #endregion

        #region public static string ToQueryString(this Dictionary<string, string> formData)
        /// <summary>
        /// 组装 QueryString，参数之间用 &amp; 连接，首位没有符号，如：a=1&amp;b=2&amp;c=3
        /// </summary>
        /// <param name="formData"> 要处理的字典 </param>
        /// <returns> 由字典 <paramref name="formData"/> 组装的 QueryString </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="formData"/> 不能为 null </exception>
        public static string ToQueryString(this Dictionary<string, string> formData)
        {
            formData.ThrowIfNull(nameof(formData));
            if (formData.IsEmpty()) return string.Empty;

            var paramArr = new List<string>();
            foreach (var kv in formData.Where(v => v.Key.IsNotNullOrWhiteSpace() && v.Value.IsNotNullOrEmpty()))
                paramArr.Add(string.Format("{0}={1}", kv.Key, kv.Value?.ToString().UriEscapeDataString()));
            return paramArr.Join("&");
        }
#if Xunit
        [Fact]
        public static void DictionaryToQueryStringTest()
        {
            Dictionary<string, string> dic = null;
            Assert.Throws<ArgumentNullException>(() => dic.ToQueryString());

            dic = new Dictionary<string, string>();
            dic[string.Empty] = null;
            dic["empty[0]"] = null;
            dic["empty[1]"] = string.Empty;
            Assert.Equal(string.Empty, dic.ToQueryString());

            dic = new Dictionary<string, string>();
            dic["date"] = "2020-02-02";
            dic["time[0]"] = "0";
            dic["time[1]"] = "1";
            Assert.Equal("date=2020-02-02&time[0]=0&time[1]=1", dic.ToQueryString());
        }
#endif 
        #endregion

    }
}
