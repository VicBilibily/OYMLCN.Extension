using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using OYMLCN.Extensions;

namespace OYMLCN.Extension.Test
{
    [TestClass]
    public class DateTimeTest
    {
        [TestMethod]
        public void TimestampTest()
        {
            var datetime = new DateTime(2017, 1, 1, 0, 0, 0);
            long timestamp;
            Assert.AreEqual(timestamp = datetime.ToTimestamp(), 1483200000);
            Assert.AreEqual(timestamp.TimestampToDateTime(), datetime);

            Assert.AreEqual(datetime.ToCnMonthString(), "2017年01月");
            Assert.AreEqual(datetime.ToCnDateString(), "2017年01月01日");
            Assert.AreEqual(datetime.ToCnDatetimeString(), "2017年01月01日 00:00");
            Assert.AreEqual(datetime.ToCnDatetimeString(true), "2017年01月01日 00:00:00");

            Assert.AreEqual(datetime.ToTimeString(), "00:00");
            Assert.AreEqual(datetime.ToTimeString(true), "00:00:00");
            Assert.AreEqual(datetime.ToDayTimeString(), "01 00:00");
            Assert.AreEqual(datetime.ToDayTimeString(true), "01 00:00:00");
        }
        [TestMethod]
        public void DatetimeStartEndTest()
        {
            var now = DateTime.Now;
            Assert.AreEqual(now.ToCnIntervalString(), "刚刚");
            Assert.AreEqual(now.AddMinutes(-3).ToCnIntervalString(), "3分钟前");
            Assert.AreEqual(now.AddHours(-3).ToCnIntervalString(), "3小时前");
            Assert.AreEqual(now.AddHours(-6).ToCnIntervalString(), "6小时前");
            //Assert.AreEqual(now.AddHours(-7).ToCnIntervalString(), "今天");
            Assert.AreEqual(now.AddDays(-1).ToCnIntervalString(), "昨天");
            Assert.AreEqual(now.AddDays(-3).ToCnIntervalString(), "3天前");
            Assert.AreEqual(now.AddDays(-7).ToCnIntervalString(), "1周前");
            Assert.AreEqual(now.AddDays(-13).ToCnIntervalString(), "1周前");
            Assert.AreEqual(now.AddDays(-14).ToCnIntervalString(), "2周前");
            Assert.AreEqual(now.AddDays(-30).ToCnIntervalString(), "1个月前");
            Assert.AreEqual(now.AddDays(-365).ToCnIntervalString(), "1年前");
            Assert.IsTrue(now.AddSeconds(59).ToCnIntervalString().EndsWith("秒后"));
            Assert.IsTrue(now.AddMinutes(3).ToCnIntervalString().EndsWith("分钟后"));
            Assert.IsTrue(now.AddHours(3).ToCnIntervalString().EndsWith("小时后"));
            Assert.AreEqual(now.AddDays(1).ToCnIntervalString(), "明天");
            Assert.IsTrue(now.AddDays(3).ToCnIntervalString().EndsWith("天后"));
            Assert.IsTrue(now.AddDays(8).ToCnIntervalString().EndsWith("周后"));
            Assert.IsTrue(now.AddDays(32).ToCnIntervalString().EndsWith("月后"));
            Assert.IsTrue(now.AddDays(368).ToCnIntervalString().EndsWith("年后"));

            var datetime = new DateTime(2018, 1, 1, 0, 0, 0);
            Assert.AreEqual(datetime.GetYearStart(), new DateTime(2018, 1, 1));
            Assert.AreEqual(datetime.GetNextYearStart(), new DateTime(2019, 1, 1));
            Assert.AreEqual(datetime.GetMonthStart(), new DateTime(2018, 1, 1));
            Assert.AreEqual(datetime.GetNextMonthStart(), new DateTime(2018, 2, 1));
            Assert.AreEqual(datetime.GetWeekStart(), new DateTime(2017, 12, 31));
            Assert.AreEqual(datetime.GetNextWeekStart(), new DateTime(2018, 1, 7));
            Assert.AreEqual(datetime.GetWeekStartFromMonday(), new DateTime(2018, 1, 1));
            Assert.AreEqual(datetime.GetNextWeekStartFromMonday(), new DateTime(2018, 1, 8));

            var dtYSE = DateTimeExtensions.GetYearStartAndEndDate(datetime);
            var dtWWT = DateTimeExtensions.GetWeekWorkTime(datetime);
            var dtTD = DateTimeExtensions.GetTimeDelay(datetime, datetime.AddHours(-1).AddMinutes(10).AddSeconds(10));

            Assert.AreEqual(datetime.GetDayStart(), new DateTime(2018, 1, 1));
            Assert.AreEqual(datetime.GetNextDayStart(), new DateTime(2018, 1, 2));
            Assert.AreEqual(datetime.GetHourStart(), new DateTime(2018, 1, 1));
            Assert.AreEqual(datetime.GetNextHourStart(), new DateTime(2018, 1, 1, 1, 0, 0));
            Assert.AreEqual(datetime.GetMinuteStart(), new DateTime(2018, 1, 1));
            Assert.AreEqual(datetime.GetNextMinuteStart(), new DateTime(2018, 1, 1, 0, 1, 0));

            Assert.IsTrue(now.IsToday());
        }
    }
}
