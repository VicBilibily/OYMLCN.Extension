/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.cs
Author: VicBilibily
Description: 单元测试代码，此文件只作为测试覆盖引用，并未实现完整的过程测试。
*****************************************************************************/

// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable CheckNamespace

using Xunit;
using OYMLCN.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OYMLCN.Tests.Extensions
{
    public partial class SystemStringExtensionTest
    {
        [Fact]
        public void IsNullOrEmptyTest()
        {
            Assert.True(SystemStringExtension.IsNullOrEmpty(null));
            Assert.True(string.Empty.IsNullOrEmpty());
            Assert.False("101".IsNullOrEmpty());
        }
        [Fact]
        public void IsNullOrWhiteSpaceTest()
        {
            Assert.True(SystemStringExtension.IsNullOrWhiteSpace(null));
            Assert.True(string.Empty.IsNullOrWhiteSpace());
            Assert.True(" ".IsNullOrWhiteSpace());
            Assert.False("101".IsNullOrWhiteSpace());
        }

        #region Concat 扩展
        [Fact]
        public void ConcatIEnumerableStringTest()
        {
            List<string> list = null;
            Assert.Throws<ArgumentNullException>(() => list.Concat());
            list = new List<string>();
            SystemStringExtension.Concat(list);
            Assert.Empty(list.Concat());
        }
        [Fact]
        public void ConcatIEnumerableGenericTest()
        {
            List<object> list = null;
            Assert.Throws<ArgumentNullException>(() => list.Concat());
            list = new List<object>();
            SystemStringExtension.Concat(list);
            Assert.Empty(list.Concat());
        }
        [Fact]
        public void ConcatObjectTest()
        {
            object test = null;
            Assert.Empty(test.Concat());
            Assert.Empty(test.Concat(new object[0]));
            Assert.Equal("1", 1.Concat());
            Assert.Equal("1", 1.Concat(new object[0]));
            Assert.Equal("12", 1.Concat(2));
            Assert.Equal("12", 1.Concat(new object[] { 2 }));
            Assert.Equal("123", 1.Concat(2, 3));
            Assert.Equal("123456", 1.Concat(2, 3, 4, 5, 6));
        }
        [Fact]
        public void ConcatStringTest()
        {
            string test = null;
            Assert.Empty(test.Concat(new string[0]));
            Assert.Equal("1", "1".Concat());
            Assert.Equal("1", "1".Concat(new string[0]));
            Assert.Equal("12", "1".Concat("2"));
            Assert.Equal("12", "1".Concat(new[] { "2" }));
            Assert.Equal("123", "1".Concat("2", "3"));
            Assert.Equal("1234", "1".Concat("2", "3", "4"));
            Assert.Equal("123456", "1".Concat("2", "3", "4", "5", "6"));
        }
        #endregion

        [Fact]
        public void FormatTest()
        {
            "Hello {0}".Format("World");
            "Hello {0}{1}".Format("World", "!");
            "Hello {0}{1}{2}".Format("1", 0, 1);
            "Hello {0}{1}{2}, I {3}".Format("1", 0, 1, "want");
            "Hello {0}{1}{2}, I {3} {4}".Format("1", 0, 1, "want", "it");
        }

        [Fact]
        public void JoinTest()
        {
            var strArr = new[] { "Aa", "Bb", "Cc" };
            Assert.Equal("AaBbCc", strArr.Join());
            Assert.Equal("Aa-Bb-Cc", strArr.Join('-'));
            Assert.Equal("Bb-Cc", strArr.Join('-', 1, 2));
            Assert.Equal("Aa-Bb-Cc", strArr.Join("-"));
            Assert.Equal("Bb-Cc", strArr.Join("-", 1, 2));
            var objArr = new object[] { "1", 2, '3' };
            Assert.Equal("1-2-3", objArr.Join('-'));
            Assert.Equal("1-2-3", objArr.Join("-"));
            var strList = new List<string> { "Aa", "Bb", "Cc" };
            Assert.Equal("AaBbCc", strList.Join());
            Assert.Equal("Aa-Bb-Cc", strList.Join('-'));
            Assert.Equal("Aa-Bb-Cc", strList.Join("-"));
        }


        #region Obsolete
        [Obsolete, Fact]
        public void IsNotNullOrEmptyTest()
        {
            SystemStringExtension.IsNotNullOrEmpty(null);
            string.Empty.IsNotNullOrEmpty();
        }
        [Obsolete, Fact]
        public void IsNotNullOrWhiteSpaceTest()
        {
            SystemStringExtension.IsNotNullOrWhiteSpace(null);
            string.Empty.IsNotNullOrWhiteSpace();
        }
        #endregion

    }
}
