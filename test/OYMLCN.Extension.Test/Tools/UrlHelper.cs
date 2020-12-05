/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: UrlHelper.cs
Author: VicBilibily
Description: 单元测试代码，测试 URL 地址相关的帮助方法。
*****************************************************************************/

using OYMLCN.Tools;
using Xunit;

namespace OYMLCN.Tests.Tools
{
    public class UrlHelperTest
    {
        [Fact]
        public void GetIPAddressesTest()
        {
            string str = null;
            Assert.Empty(UrlHelper.GetIPAddresses(str));
            str = string.Empty;
            Assert.Empty(UrlHelper.GetIPAddresses(str));

            str = "192.168.1.1:8060";
            Assert.Equal(new[] { "192.168.1.1" }, UrlHelper.GetIPAddresses(str));
            str = "http://192.168.1.1:8060/test.aspx";
            Assert.Equal(new[] { "192.168.1.1" }, UrlHelper.GetIPAddresses(str));

            str = "2001:0DB8:02de::0e13";
            Assert.Equal(new[] { "2001:db8:2de::e13" }, UrlHelper.GetIPAddresses(str));
            str = "[2001:0DB8:02de::0e13]:9010";
            Assert.Equal(new[] { "2001:db8:2de::e13" }, UrlHelper.GetIPAddresses(str));

            str = "http://localhost/test.aspx";
            Assert.Equal(new[] { "::1", "127.0.0.1" }, UrlHelper.GetIPAddresses(str));
        }
        [Fact]
        public static void CheckIPAddressTest()
        {
            string str = null;
            Assert.False(UrlHelper.CheckIPAddress(str));

            str = "localhost";
            Assert.False(UrlHelper.CheckIPAddress(str));

            str = "192.168.1.1";
            Assert.True(UrlHelper.CheckIPAddress(str));
            str = "192.168.1.1:8060";
            Assert.False(UrlHelper.CheckIPAddress(str));

            str = "2001:0DB8:02de::0e13";
            Assert.True(UrlHelper.CheckIPAddress(str));
            str = "[2001:0DB8:02de::0e13]:9010";
            Assert.True(UrlHelper.CheckIPAddress(str));
        }
        public static void CheckIPv4Test()
        {
            string str = null;
            Assert.False(UrlHelper.CheckIPv4(str));

            str = "localhost";
            Assert.False(UrlHelper.CheckIPv4(str));

            str = "192.168.1.1";
            Assert.True(UrlHelper.CheckIPv4(str));
            str = "192.168.1.1:8060";
            Assert.False(UrlHelper.CheckIPv4(str));

            str = "2001:0DB8:02de::0e13";
            Assert.False(UrlHelper.CheckIPv4(str));
            str = "[2001:0DB8:02de::0e13]:9010";
            Assert.False(UrlHelper.CheckIPv4(str));
        }
        public static void CheckIPv6Test()
        {
            string str = null;
            Assert.False(UrlHelper.CheckIPv6(str));

            str = "localhost";
            Assert.False(UrlHelper.CheckIPv6(str));

            str = "192.168.1.1";
            Assert.False(UrlHelper.CheckIPv6(str));
            str = "192.168.1.1:8060";
            Assert.False(UrlHelper.CheckIPv6(str));

            str = "2001:0DB8:02de::0e13";
            Assert.True(UrlHelper.CheckIPv6(str));
            str = "[2001:0DB8:02de::0e13]:9010";
            Assert.False(UrlHelper.CheckIPv6(str));
        }

    }
}
