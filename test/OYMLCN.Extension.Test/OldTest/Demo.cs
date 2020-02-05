using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OYMLCN.Extensions;
namespace OYMLCN.Extension.Test
{
    [TestClass]
    public class DemoTest
    {
        public class Demo
        {
            public string DemoStr { get; set; }
        }
        [TestMethod]
        public void DemoTestMethod()
        {
            var calendar = new ChineseCalendar(new DateTime(2020, 1, 6));
            var d1 = calendar.ChineseTwentyFourPrevDay;
            var d2 = calendar.ChineseTwentyFourNextDay;

            var demo = new Demo();
            demo.DemoStr = "0ss";
            var dm1 = demo.DeepClone();
            var dm2 = demo.XmlDeepClone();
            var dm3 = demo.JsonDeepClone();
            var dm4 = demo.Clone();

            var ipV4 = "192.168.1.1";
            var ipV6 = "2001:0DB8:02de::0e13";
            var iv4 = ipV4.FormatIsIPAddress();
            var iv6 = ipV6.FormatIsIPAddress();
            iv4 = iv6 = false;
            iv4 = ipV4.FormatIsIPv4();
            iv6 = ipV6.FormatIsIPv6();
            var ipV4Add = "192.168.1.1:8060";
            var ipV6Add = "[2001:0DB8:2de::e13]:9010";
            ipV4Add = ipV4Add.GetIPAddress();
            ipV6Add = ipV6Add.GetIPAddress();

            //var date = new System.Globalization.ChineseLunisolarCalendar();

            var age = new DateTime(1994, 12, 25).GetAge();
            var date = "20200130".ConvertToDatetime("yyyyMMdd");

            var id18 = "34052419800101001".ChineseIDCard15UpTo18();
            var id182 = "429005811009091".ChineseIDCard15UpTo18();
            // 验证密码强度
            // 操作ini/xml

            OYMLCN.Helpers.SevenZipHelper.Zip(new List<string>
                {
                @"V:\VicBilibily\OYMLCN.Extension\OYMLCN.Extension.Test\DateTime.cs",
@"V:\VicBilibily\OYMLCN.Extension\OYMLCN.Extension.Test\Demo.cs"}, @"C:\Users\Vic\Desktop\test.7z");

        }
    }
}
