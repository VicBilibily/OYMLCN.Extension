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
        [TestMethod]
        public void DemoTestMethod()
        {
            var calendar = new ChineseCalendar(new DateTime(2020, 1, 6));
            var d1 = calendar.ChineseTwentyFourPrevDay;
            var d2 = calendar.ChineseTwentyFourNextDay;

            OYMLCN.Helpers.SevenZipHelper.Zip(new List<string>
                {
                @"V:\VicBilibily\OYMLCN.Extension\OYMLCN.Extension.Test\DateTime.cs",
@"V:\VicBilibily\OYMLCN.Extension\OYMLCN.Extension.Test\Demo.cs"}, @"C:\Users\Vic\Desktop\test.7z");

        }
    }
}
