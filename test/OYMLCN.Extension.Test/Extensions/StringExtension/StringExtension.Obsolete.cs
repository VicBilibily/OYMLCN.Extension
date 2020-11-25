/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.Obsolete.cs
Author: VicBilibily
Description: 单元测试代码，此文件只作为测试覆盖引用，并未实现完整的过程测试。
*****************************************************************************/

using OYMLCN.Extensions;
using System;
using Xunit;

namespace OYMLCN.Tests.Extensions
{
    public partial class StringExtensionTest
    {
        [Obsolete, Fact]
        public void ContainsObsoleteTest()
        {
            var source = new string[0];
            source.WhereContains();
            source.WhereStartsWith(null);
            source.WhereEndsWith(null);
            source.WhereIsNotNullOrEmpty();
            source.WhereIsNotNullOrWhiteSpace();
        }
        [Obsolete, Fact]
        public void EqualsTest()
        {
            var str = string.Empty;
            str.Equals(null, null);
            str.EqualsIgnoreCase(null, null);
        }
        [Obsolete, Fact]
        public void NormalTest()
        {
            var str = string.Empty;
            str.IfNull(null);
            str.StringFormat(null, null);
            str.TakeSubString(1, int.MaxValue);
            str.StartsWith(str, str);
            str.StartsWith(' ', ' ');
            str.EndsWith(str, str);
            str.EndsWith(' ', ' ');
        }
    }
}
