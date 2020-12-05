/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringFormatExtension.Obsolete.cs
Author: VicBilibily
Description: 单元测试代码，此文件只作为测试覆盖引用，并未实现完整的过程测试。
*****************************************************************************/

using OYMLCN.Extensions;
using System;
using Xunit;

namespace OYMLCN.Tests.Extensions
{
    public partial class StringFormatExtensionTest
    {
        [Obsolete, Fact]
        public void IPAddressTest()
        {
            string.Empty.GetIPAddresses();
            string.Empty.FormatIsIPAddress();
            string.Empty.FormatIsIPv4();
            string.Empty.FormatIsIPv6();
        }


    }
}
