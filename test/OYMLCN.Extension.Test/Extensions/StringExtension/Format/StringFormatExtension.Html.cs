/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringFormatExtension.Html.cs
Author: VicBilibily
Description: 单元测试代码，此文件测试HTML字符串相关扩展方法。
*****************************************************************************/

using OYMLCN.Extensions;
using Xunit;

namespace OYMLCN.Tests.Extensions
{
    public partial class StringFormatExtensionTest
    {
        [Fact]
        public static void HtmlRemoveScriptTest()
        {
            string html = null;
            Assert.Null(html.HtmlRemoveScript());

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
        [Fact]
        public static void HtmlRemoveStyleBlockTest()
        {
            string html = null;
            Assert.Null(html.HtmlRemoveStyleBlock());

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

        [Fact]
        public static void HtmlFormatNewLinesTest()
        {
            string html = null;
            Assert.Null(html.HtmlRemoveStyleBlock());

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

        [Fact]
        public static void HtmlEncodeTest()
        {
            // 扩展方法调用WebUtility内置方法，不过多测试，仅作为测试覆盖
            string.Empty.HtmlDecode().HtmlEncode();
        }

    }
}
