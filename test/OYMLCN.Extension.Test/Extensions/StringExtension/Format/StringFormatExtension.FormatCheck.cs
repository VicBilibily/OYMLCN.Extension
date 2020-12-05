/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringFormatExtension.FormatCheck.cs
Author: VicBilibily
Description: 单元测试代码，此文件测试字符串格式验证与转换相关扩展方法。
*****************************************************************************/

using OYMLCN.Extensions;
using System;
using Xunit;

namespace OYMLCN.Tests.Extensions
{
    public partial class StringFormatExtensionTest
    {
        [Fact]
        public static void FormatIsChineseIDCardTest()
        {
            string str = null;
            Assert.False(str.FormatIsChineseIDCard());

            str = "320311770706001";
            Assert.True(str.FormatIsChineseIDCard());
            str = "3203117707060001";
            Assert.False(str.FormatIsChineseIDCard());

            // 测试值均为模拟值
            str = "440102198001021230";
            Assert.True(str.FormatIsChineseIDCard());
            str = "440102198001011230";
            Assert.False(str.FormatIsChineseIDCard());
        }
        [Fact]
        public static void ChineseIDCard15UpTo18Test()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.ChineseIDCard15UpTo18());

            // 测试值均为模拟值
            str = "3203117707060001";
            Assert.Throws<ArgumentException>(() => str.ChineseIDCard15UpTo18());

            str = "380311770706001";
            Assert.Throws<ArgumentException>(() => str.ChineseIDCard15UpTo18());

            str = "320311770706001";
            Assert.Equal(18, str.ChineseIDCard15UpTo18().Length);
        }

        [Fact]
        public static void FormatIsEmailAddressTest()
        {
            string str = null;
            Assert.False(str.FormatIsEmailAddress());
            str = " ";
            Assert.False(str.FormatIsEmailAddress());
            str = "qq.com";
            Assert.False(str.FormatIsEmailAddress());
            str = "10000@qq.com";
            Assert.True(str.FormatIsEmailAddress());
            str = "www.测试@域名.中国";
            Assert.True(str.FormatIsEmailAddress());
        }
        [Fact]
        public static void FormatIsUrlTest()
        {
            string str = null;
            Assert.False(str.FormatIsUrl());
            str = " ";
            Assert.False(str.FormatIsUrl());
            str = "Test.aspx";
            Assert.False(str.FormatIsUrl());
            str = "/MyResource/Test.aspx";
            Assert.False(str.FormatIsUrl());

            str = "http://www.qq.com/MyResource/Test.aspx";
            Assert.True(str.FormatIsUrl());
            str = "https://www.qq.com/Test.aspx";
            Assert.True(str.FormatIsUrl());
        }

        [Fact]
        public static void FormatIsTelephoneTest()
        {
            string str = null;
            Assert.False(str.FormatIsTelephone());
            str = "13800138000";
            Assert.False(str.FormatIsTelephone());
            str = "010-10086";
            Assert.False(str.FormatIsTelephone());
            str = "020-88668866";
            Assert.True(str.FormatIsTelephone());
        }
        [Fact]
        public static void FormatIsMobilePhoneNumberTest()
        {
            string str = null;
            Assert.False(str.FormatIsMobilePhone());
            Assert.False("14123456789".FormatIsMobilePhone());
            Assert.True("13800138000".FormatIsMobilePhone());
        }
        [Fact]
        public static void FormatIsPostalCodeTest()
        {
            string str = null;
            Assert.False(str.FormatIsPostalCode());
            Assert.False(" ".FormatIsPostalCode());
            Assert.False("abc123".FormatIsPostalCode());
            Assert.True("102200".FormatIsPostalCode());
        }
        [Fact]
        public static void FormatHasChineseCharactersTest()
        {
            string str = null;
            Assert.False(str.FormatHasChineseCharacters());
            str = "Hello World！";
            Assert.False(str.FormatHasChineseCharacters());
            str = "你好，世界！";
            Assert.True(str.FormatHasChineseCharacters());
        }

    }
}
