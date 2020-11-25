/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.Contains.cs
Author: VicBilibily
Description: 单元测试代码，此文件主要测试 Contains 字符串包含判断的相关扩展方法。
*****************************************************************************/
/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.Contains.cs
Author: VicBilibily
Description: 单元测试代码，此文件主要测试 Contains 字符串包含判断的相关扩展方法。
*****************************************************************************/

using OYMLCN.Extensions;
using System;
using Xunit;

namespace OYMLCN.Tests.Extensions
{
    public partial class StringExtensionTest
    {
        [Fact]
        public void ContainsIgnoreCaseTest()
        {
            string str = null;
            Assert.False(str.ContainsIgnoreCase(null));
            str = "ABC";
            Assert.Throws<ArgumentNullException>(() => str.ContainsIgnoreCase(null));
            Assert.True(str.ContainsIgnoreCase(string.Empty));
            Assert.True(str.ContainsIgnoreCase("b"));
            Assert.False(str.ContainsIgnoreCase("0"));
            Assert.True(str.ContainsIgnoreCase('b'));
            Assert.False(str.ContainsIgnoreCase('0'));
        }
        [Fact]
        public void ContainsWordsTest()
        {
            Assert.False(string.Empty.ContainsWords());
            Assert.False("101".ContainsWords("2"));
            Assert.True("101".ContainsWords("0"));
            Assert.True("101".ContainsWords(string.Empty));

            Assert.False(string.Empty.ContainsWordsIgnoreCase());
            Assert.False("AbC".ContainsWordsIgnoreCase("cd"));
            Assert.True("AbC".ContainsWordsIgnoreCase("ab"));
            Assert.True("AbC".ContainsWordsIgnoreCase(string.Empty));
        }
        [Fact]
        public void ContainsCharsTest()
        {
            Assert.False(string.Empty.ContainsChars());
            Assert.False("101".ContainsChars('2'));
            Assert.True("101".ContainsChars('0'));

            Assert.False(string.Empty.ContainsCharsIgnoreCase());
            Assert.False("AbC".ContainsCharsIgnoreCase('d'));
            Assert.True("AbC".ContainsCharsIgnoreCase('B'));
        }

        [Fact]
        public void ContainsWordsIEnumerableTest()
        {
            string[] test = null;
            Assert.False(test.ContainsWords());
            test = new[] { "101", "110", "010" };
            Assert.True(test.ContainsWords("110"));
            Assert.False(test.ContainsWords("000"));
            Assert.False(test.ContainsWords(string.Empty));
            Assert.False(test.ContainsWords(null));
            Assert.True(test.ContainsWords("110", null));
        }

    }
}
