using System;
using System.Net;
using System.Text.RegularExpressions;
using OYMLCN.ArgumentChecker;

#if Xunit
using Xunit;
#endif


namespace OYMLCN.Extensions
{
    /// <summary>
    /// HTML 字符串相关扩展
    /// </summary>
    public static class StringHtmlExtension
    {
        #region public static string HtmlRemoveScript(this string html)
        /// <summary>
        /// 删除字符串内的 script 标签
        /// </summary>
        /// <param name="html"> HTML 字符串 </param>
        /// <returns> 已移除 script 标签的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="html"/> 不能为 null </exception>
        public static string HtmlRemoveScript(this string html)
        {
            html.ThrowIfNull(nameof(html));
            if (html.IsNullOrWhiteSpace()) return html;

            var result = Regex.Replace(html, @"(\<script(.+?)\</script\>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (html == result)
                result = Regex.Replace(html, @"(\<script(.+?)>|\</script\>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return result;
        }
#if Xunit
        [Fact]
        public static void HtmlRemoveScriptTest()
        {
            string html = null;
            Assert.Throws<ArgumentNullException>(() => html.HtmlRemoveScript());

            html = string.Empty;
            Assert.Equal(string.Empty, html.HtmlRemoveScript());

            html = " ";
            Assert.Equal(html, html.HtmlRemoveScript());

            html = @" <script type='test'> 
    alert('hi') 
</script> ";
            Assert.Equal("  ", html.HtmlRemoveScript());

            html = " <script type='demo'> alert('hi') ";
            Assert.Equal("  alert('hi') ", html.HtmlRemoveScript());
        }
#endif
        #endregion

        #region public static string HtmlRemoveStyleBlock(this string html)
        /// <summary>
        /// 使用正则表达式删除 style 标签块
        /// </summary>
        /// <param name="html"> HTML 字符串 </param>
        /// <returns> 已移除 style 标签块的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="html"/> 不能为 null </exception>
        public static string HtmlRemoveStyleBlock(this string html)
            => Regex.Replace(html, @"(\<style(.+?)\</style\>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
#if Xunit
        [Fact]
        public static void HtmlRemoveStyleBlockTest()
        {
            string html = null;
            Assert.Throws<ArgumentNullException>(() => html.HtmlRemoveStyleBlock());

            html = string.Empty;
            Assert.Equal(string.Empty, html.HtmlRemoveStyleBlock());

            html = " ";
            Assert.Equal(html, html.HtmlRemoveStyleBlock());

            html = @" <style type='test'> 
    .test {
        backgroup-color: red;
    }
</style> ";
            Assert.Equal("  ", html.HtmlRemoveStyleBlock());

            html = " <style type='test'> .test { ";
            Assert.Equal(html, html.HtmlRemoveStyleBlock());
        }
#endif
        #endregion


        #region public static string HtmlFormatNewLines(this string html)
        /// <summary>
        /// 替换回车和换行符为 <br/> 换行符
        /// </summary>
        /// <param name="html"> HTML 字符串 </param>
        /// <returns> 已将换行符或回车替换为 <br/> 换行符的字符串 </returns>
        public static string HtmlFormatNewLines(this string html)
        {
            html.ThrowIfNull(nameof(html));
            return html.Replace("\r\n", "<br/>").Replace("\n", "<br/>");
        }
#if Xunit
        [Fact]
        public static void HtmlFormatNewLinesTest()
        {
            string html = null;
            Assert.Throws<ArgumentNullException>(() => html.HtmlFormatNewLines());

            html = string.Empty;
            Assert.Equal(string.Empty, html.HtmlFormatNewLines());

            html = "Hello World!";
            Assert.Equal(html, html.HtmlFormatNewLines());
            html = @"Hello
World!";
            Assert.Equal("Hello<br/>World!", html.HtmlFormatNewLines());
            html = "Hello\nWorld!";
            Assert.Equal("Hello<br/>World!", html.HtmlFormatNewLines());
        }
#endif
        #endregion


        #region public static string HtmlDecode(this string html)
        /// <summary>
        /// 将已经为 HTTP 传输进行过 HTML 编码的字符串转换为已解码的字符串
        /// </summary>
        /// <param name="html"> 要解码的字符串 </param>
        /// <returns> 已解码的字符串 </returns>
        public static string HtmlDecode(this string html)
            => WebUtility.HtmlDecode(html);
#if Xunit
        [Fact]
        public static void HtmlDecodeTest()
        {
            // 调用内置方法，不需要过多测试
        }
        [Fact]
        public static void HtmlEncodeDecodeTest()
        {
            string html = @"<html id=""demo"">test</html>";
            Assert.Equal(html, html.HtmlEncode().HtmlDecode());
        }
#endif
        #endregion

        #region public static string HtmlEncode(this string html)
        /// <summary>
        /// 将字符串转换为 HTML 编码字符串
        /// </summary>
        /// <param name="html"> 要编码的字符串 </param>
        /// <returns> 已编码的字符串 </returns>
        public static string HtmlEncode(this string html)
            => WebUtility.HtmlEncode(html);
#if Xunit
        [Fact]
        public static void HtmlEncodeTest()
        {
            // 调用内置方法，不需要过多测试
        }
#endif 
        #endregion

    }
}
