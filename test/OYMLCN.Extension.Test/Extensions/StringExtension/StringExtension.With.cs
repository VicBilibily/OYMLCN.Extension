/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.With.cs
Author: VicBilibily
Description: 单元测试代码，此文件测试 StartWith_ 和 EndWith_ 扩展方法。
*****************************************************************************/

using OYMLCN.Extensions;
using System;
using Xunit;

namespace OYMLCN.Tests.Extensions
{
    public partial class StringExtensionTest
    {
        [Fact]
        public void StartsWithTest()
        {
            string str = null;
            Assert.False(str.StartsWithIgnoreCase(null));
            Assert.False("".StartsWithIgnoreCase(null));
            Assert.Throws<ArgumentNullException>(() => "Abc".StartsWithIgnoreCase(null));
            Assert.True("Abc".StartsWithIgnoreCase("a"));

            Assert.False(str.StartsWithWords(string.Empty));
            Assert.False(str.StartsWithWords(null));
            Assert.False(str.StartsWithWords(string.Empty, null));
            Assert.False(string.Empty.StartsWithWords(string.Empty, null));
            Assert.True(" ".StartsWithWords(string.Empty, null));
            Assert.True(" Abc ".StartsWithWords(string.Empty, " "));

            char cha = default;
            Assert.False(str.StartsWithChars(cha));
            Assert.False(string.Empty.StartsWithChars(cha));
            Assert.False(" ".StartsWithChars(cha));
            Assert.True(" ".StartsWithChars(' '));
            Assert.True(" Abc ".StartsWithChars(cha, ' '));
        }

        [Fact]
        public void EndsWithTest()
        {
            string str = null;
            Assert.False(str.EndsWithIgnoreCase(null));
            Assert.False("".EndsWithIgnoreCase(null));
            Assert.Throws<ArgumentNullException>(() => "Abc".EndsWithIgnoreCase(null));
            Assert.True("Abc".EndsWithIgnoreCase("C"));

            Assert.False(str.EndsWithWords(string.Empty));
            Assert.False(str.EndsWithWords(null));
            Assert.False(str.EndsWithWords(string.Empty, null));
            Assert.False(string.Empty.EndsWithWords(string.Empty, null));
            Assert.True(" ".EndsWithWords(string.Empty, null));
            Assert.True(" Abc ".EndsWithWords(string.Empty, " "));

            char cha = default;
            Assert.False(str.EndsWithChars(cha));
            Assert.False(string.Empty.EndsWithChars(cha));
            Assert.False(" ".EndsWithChars(cha));
            Assert.True(" ".EndsWithChars(' '));
            Assert.True(" Abc ".EndsWithChars(cha, ' '));
        }
    }
}
