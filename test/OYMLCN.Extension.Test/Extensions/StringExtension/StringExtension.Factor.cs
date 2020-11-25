/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.Factor.cs
Author: VicBilibily
Description: 单元测试代码，此文件主要测试字符串条件判断相关的扩展方法。
*****************************************************************************/

using OYMLCN.Extensions;
using Xunit;

namespace OYMLCN.Tests.Extensions
{
    public partial class StringExtensionTest
    {
        [Fact]
        public void IfNullOrEmptyTest()
        {
            string str = null;
            Assert.Null(str.IfNullOrEmpty(null));
            Assert.Equal(string.Empty, str.IfNullOrEmpty(string.Empty));
            Assert.Empty(str.IfNullOrEmpty(string.Empty));
            Assert.Equal("Hello", str.IfNullOrEmpty("Hello"));
            str = string.Empty;
            Assert.Empty(str.IfNullOrEmpty(string.Empty));
            Assert.Equal("Hello", str.IfNullOrEmpty("Hello"));
            str = " ";
            Assert.Equal(" ", str.IfNullOrEmpty("Hello"));
        }
        [Fact]
        public void IfNullOrWhiteSpaceTest()
        {
            string str = null;
            Assert.Null(str.IfNullOrWhiteSpace(null));
            Assert.Equal(string.Empty, str.IfNullOrWhiteSpace(string.Empty));
            Assert.Empty(str.IfNullOrWhiteSpace(string.Empty));
            Assert.Equal("Hello", str.IfNullOrWhiteSpace("Hello"));
            str = string.Empty;
            Assert.Empty(str.IfNullOrWhiteSpace(string.Empty));
            Assert.Equal("Hello", str.IfNullOrWhiteSpace("Hello"));
            str = " ";
            Assert.Equal("Hello", str.IfNullOrWhiteSpace("Hello"));
        }

    }
}
