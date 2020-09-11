using System.Net;

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
        #region public static Cookie ConvertToCookie(this string cookie)
        /// <summary>
        /// 将单个 Cookie 字符串信息转换为 <see cref="Cookie"/> 类型
        /// </summary>
        /// <param name="cookie"> 单个 Cookie 信息的字符串对象 </param>
        /// <returns> <paramref name="cookie"/> 所表示的 <see cref="Cookie"/> 实例 </returns>
        /// <exception cref="CookieException"> Cookie 字符串信息不是有效的内容 </exception>
        public static Cookie ConvertToCookie(this string cookie)
        {
            if (cookie.IsNullOrWhiteSpace()) return null;
            var index = cookie.IndexOf('=');
            index = index < 0 ? 0 : index;
            return new Cookie(cookie.Substring(0, index).Trim(), cookie.Substring(++index));
        }
#if Xunit
        [Fact]
        public static void ConvertToCookieTest()
        {
            string str = null;
            Assert.Null(str.ConvertToCookie());

            str = string.Empty;
            Assert.Null(str.ConvertToCookie());

            str = "session";
            Assert.Throws<CookieException>(() => str.ConvertToCookie());

            str = "session=";
            var cookie = str.ConvertToCookie();
            Assert.Equal("session", cookie.Name);
            Assert.Equal(string.Empty, cookie.Value);

            str = "session=usersession=11";
            cookie = str.ConvertToCookie();
            Assert.NotNull(cookie);
            Assert.Equal("session", cookie.Name);
            Assert.Equal("usersession=11", cookie.Value);
        }
#endif
        #endregion

        #region public static CookieCollection ConvertToCookieCollection(this string cookies)
        /// <summary>
        /// 将包含 Cookies 信息的字符串转换为 <see cref="CookieCollection"/> 实例
        /// </summary>
        /// <param name="cookies"> 包含 Cookies 信息的字符串对象 </param>
        /// <returns> <paramref name="cookies"/> 所表示的 <see cref="CookieCollection"/> 实例 </returns>
        /// <exception cref="CookieException"> Cookie 字符串信息不是有效的内容 </exception>
        public static CookieCollection ConvertToCookieCollection(this string cookies)
        {
            var result = new CookieCollection();
            if (cookies.IsNullOrWhiteSpace())
                return result;

            foreach (var cookie in cookies.SplitRemoveEmpty(";"))
                result.Add(cookie.ConvertToCookie());
            return result;
        }
#if Xunit
        [Fact]
        public static void ConvertToCookieCollectionTest()
        {
            string str = null;
            Assert.Empty(str.ConvertToCookieCollection());

            str = string.Empty;
            Assert.Empty(str.ConvertToCookieCollection());

            str = "session";
            Assert.Throws<CookieException>(() => str.ConvertToCookieCollection());

            str = "session=";
            var cookie = str.ConvertToCookieCollection();
            Assert.Equal("session", cookie[0].Name);
            Assert.Equal(string.Empty, cookie[0].Value);

            str = "session=usersession=11;;aspnetcore=3.1;version=;";
            cookie = str.ConvertToCookieCollection();
            Assert.Equal("session", cookie[0].Name);
            Assert.Equal("usersession=11", cookie[0].Value);
            Assert.Equal("aspnetcore", cookie[1].Name);
            Assert.Equal("3.1", cookie[1].Value);
            Assert.Equal("version", cookie[2].Name);
            Assert.Equal(string.Empty, cookie[2].Value);
        }
#endif  
        #endregion

    }
}
