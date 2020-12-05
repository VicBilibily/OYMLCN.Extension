/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringFormatExtension.FormatConvert.cs
Author: VicBilibily
Description: 单元测试代码，此文件测试字符串格式转换相关扩展方法。
*****************************************************************************/

using OYMLCN.Extensions;
using System;
using Xunit;

namespace OYMLCN.Tests.Extensions
{
    public partial class StringFormatExtensionTest
    {
        [Fact]
        public static void ToYesOrNoTest()
        {
            Assert.Equal("Yes", true.ToYesOrNo());
            Assert.Equal("No", false.ToYesOrNo());
            Assert.Equal("是", true.ToYesOrNo(true));
            Assert.Equal("否", false.ToYesOrNo(true));
        }


    }
}
