using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        }
    }
}
