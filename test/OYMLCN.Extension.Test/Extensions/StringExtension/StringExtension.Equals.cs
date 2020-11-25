/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.Contains.cs
Author: VicBilibily
Description: 单元测试代码，此文件主要测试 Equals 字符串相等判断的相关扩展方法。
*****************************************************************************/

using OYMLCN.Extensions;
using Xunit;

namespace OYMLCN.Tests.Extensions
{
    public partial class StringExtensionTest
    {
        [Fact]
        public void EqualsIgnoreCaseTest()
        {
            string str = null;
            Assert.True(str.EqualsIgnoreCase((string)null));
            Assert.False(str.EqualsIgnoreCase(string.Empty));
            str = string.Empty;
            Assert.True(str.EqualsIgnoreCase(string.Empty));
            Assert.False(str.EqualsIgnoreCase((string)null));
            str = "hello";
            Assert.True(str.EqualsIgnoreCase("Hello"));
            str = "Hello";
            Assert.True(str.EqualsIgnoreCase("Hello"));
            Assert.False(str.EqualsIgnoreCase("World"));
        }

        [Fact]
        public void EqualsWordsTest()
        {
            string str = null;
            Assert.True(str.EqualsWords(null));
            Assert.True(str.EqualsWords(string.Empty, null));
            str = string.Empty;
            Assert.True(str.EqualsWords(string.Empty, null));
            Assert.False(str.EqualsWords("Hello", "world", "!"));
            str = "hello";
            Assert.False(str.EqualsWords("Hello", "world", "!"));
            str = "Hello";
            Assert.True(str.EqualsWords("Hello", "world", "!"));
        }
        public void EqualsWordsIgnoreCaseTest()
        {
            string str = null;
            Assert.True(str.EqualsWordsIgnoreCase(null));
            Assert.True(str.EqualsWordsIgnoreCase(string.Empty, null));
            str = string.Empty;
            Assert.True(str.EqualsWordsIgnoreCase(string.Empty, null));
            Assert.False(str.EqualsWordsIgnoreCase("Hello", "world", "!"));
            str = "hello";
            Assert.True(str.EqualsWordsIgnoreCase("Hello", "world", "!"));
            str = "Hello";
            Assert.True(str.EqualsWordsIgnoreCase("Hello", "world", "!"));
        }

    }
}
