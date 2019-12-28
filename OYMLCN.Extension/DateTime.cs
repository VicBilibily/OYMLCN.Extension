using OYMLCN.Handlers;
using System;
using System.Globalization;
using System.Linq;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// DateTimeExtension
    /// </summary>
    public static partial class DateTimeExtensions
    {
        /// <summary>
        /// 中国农历信息
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static ChineseCalendar AsChineseCalendar(this DateTime dt) => new ChineseCalendar(dt);

        #region DateTime
        /// <summary>
        /// 本年有多少天
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year">年份</param>
        /// <returns>本年的天数</returns>
        public static int GetDaysOfYear(this DateTime dt, int year = 0)
            => IsLeapYear(dt, year) ? 366 : 365;
        private static readonly int[] m31d = new[] { 1, 3, 5, 7, 8, 10, 12 };
        /// <summary>
        /// 本月有多少天
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>天数</returns>
        public static int GetDaysOfMonth(this DateTime dt, int year = 0, int month = 0)
        {
            year = year > 0 ? year : dt.Year;
            month = month > 0 ? month : dt.Month;
            int days = 30;
            if (month == 2)
                days = IsLeapYear(dt, year) ? 29 : 28;
            else if (m31d.Contains(month))
                days = 31;
            return days;
        }
        /// <summary>
        /// 判断当前年份是否是闰年
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year">年份</param>
        /// <returns>是闰年：true ，不是闰年：false</returns>
        public static bool IsLeapYear(this DateTime dt, int year = 0)
        {
            year = year > 0 ? year : dt.Year;
            return year % 400 == 0 || year % 4 == 0 && year % 100 != 0;
        }

        #region GetYearXXX
        /// <summary>
        /// 年 开始时间
        /// </summary>
        public static DateTime GetYearStart(this DateTime dt)
            => new DateTime(dt.Year, 1, 1);
        /// <summary>
        /// 次年 开始时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetNextYearStart(this DateTime dt)
            => dt.GetYearStart().AddYears(1);
        /// <summary>
        /// 年 最后一天日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetYearLastDate(this DateTime dt)
            => dt.GetNextYearStart().AddSeconds(-1).Date;

        /// <summary>
        /// 年 第一天日期/最后一天日期
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        public static void GetYearStartAndEndDate(this DateTime dt, out DateTime dtStart, out DateTime dtEnd)
        {
            dtStart = dt.GetYearStart().Date;
            dtEnd = dt.GetYearLastDate();
        }
#if NETSTANDARD2_1
        /// <summary>
        /// 年 第一天日期/最后一天日期
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        public static (DateTime dtStart, DateTime dtEnd) GetYearStartAndEndDate(this DateTime dt)
        {
            DateTime dtStart, dtEnd;
            GetYearStartAndEndDate(dt, out dtStart, out dtEnd);
            return (dtStart, dtEnd);
        }
#endif
        #endregion

        #region GetMonthXXX
        /// <summary>
        /// 月 开始时间
        /// </summary>
        public static DateTime GetMonthStart(this DateTime dt)
            => new DateTime(dt.Year, dt.Month, 1);
        /// <summary>
        /// 次月 开始时间
        /// </summary>
        public static DateTime GetNextMonthStart(this DateTime dt)
            => dt.GetMonthStart().AddMonths(1);
        /// <summary>
        /// 月 最后一天日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMonthLastDate(this DateTime dt)
            => dt.GetNextMonthStart().AddSeconds(-1).Date;
        /// <summary>
        /// 月 第一天日期/最后一天日期
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        public static void GetMonthStartAndEndDate(this DateTime dt, out DateTime dtStart, out DateTime dtEnd)
        {
            dtStart = dt.GetMonthStart().Date;
            dtEnd = dt.GetMonthLastDate();
        }
#if NETSTANDARD2_1
        /// <summary>
        /// 月 第一天日期/最后一天日期
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        public static (DateTime dtStart, DateTime dtEnd) GetMonthStartAndEndDate(this DateTime dt)
        {
            DateTime dtStart, dtEnd;
            GetMonthStartAndEndDate(dt, out dtStart, out dtEnd);
            return (dtStart, dtEnd);
        }
#endif
        #endregion
        #endregion

        #region GetWeekXXX
        /// <summary>
        /// 周 开始时间（周日为第一天）
        /// </summary>
        public static DateTime GetWeekStart(this DateTime dt)
            => dt.Date.AddDays(-(int)dt.DayOfWeek);
        /// <summary>
        /// 下周 开始时间（周日为第一天）
        /// </summary>
        public static DateTime GetNextWeekStart(this DateTime dt)
            => dt.GetWeekStart().AddDays(7);
        /// <summary>
        /// 周 开始时间（周一为第一天）
        /// </summary>
        public static DateTime GetWeekStartFromMonday(this DateTime dt)
            => dt.Date.AddDays(-(int)dt.DayOfWeek + 1);
        /// <summary>
        /// 下周 开始时间（周一为第一天）
        /// </summary>
        public static DateTime GetNextWeekStartFromMonday(this DateTime dt)
            => dt.GetWeekStartFromMonday().AddDays(7);

        private static readonly GregorianCalendar gc = new GregorianCalendar();
        /// <summary>
        /// 获取当年或指定年份有多少周（默认星期日是第一天）
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year">年份</param>
        /// <param name="firstDayOfWeek">表示一周的第一天的枚举值（默认星期日）</param>
        /// <returns>该年周数</returns>
        public static int GetWeekAmount(this DateTime dt, int year = 0, DayOfWeek firstDayOfWeek = DayOfWeek.Sunday)
        {
            var end = new DateTime(year > 0 ? year : dt.Year, 12, 31); //该年最后一天
            return gc.GetWeekOfYear(end, CalendarWeekRule.FirstDay, firstDayOfWeek);
        }
        /// <summary>
        /// 获取当年或指定年份有多少周
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year">年份</param>
        /// <returns>该年周数</returns>
        public static int GetWeekAmountFromMonday(this DateTime dt, int year = 0)
            => dt.GetWeekAmount(year, DayOfWeek.Monday);

        /// <summary>
        /// 年度第几个星期（星期日是第一天）
        /// </summary>
        /// <param name="date"></param>
        /// <returns>该日周数</returns>
        public static int GetWeekOfYear(this DateTime date)
            => gc.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        /// <summary>
        /// 年度第几个星期（星期一是第一天）
        /// </summary>
        /// <param name="date"></param>
        /// <returns>该日周数</returns>
        public static int GetWeekOfYearFromMonday(this DateTime date)
            => gc.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

        #region GetWeekTime/GetWeekTimeFromMonday
        /// <summary>
        /// 得到一年中的某周的起始日和截止日（星期日是第一天）
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year">年份</param>
        /// <param name="week">第几周（为0时从dt获取）</param>
        /// <param name="dtStart">开始日期</param>
        /// <param name="dtEnd">结束日期</param>
        public static void GetWeekTime(this DateTime dt, int year, int week, out DateTime dtStart, out DateTime dtEnd)
        {
            var date = new DateTime(year > 0 ? year : dt.Year, 1, 1);
            date += new TimeSpan((week - 1) * 7, 0, 0, 0);
            dtStart = date.AddDays(-(int)date.DayOfWeek + 1);
            dtEnd = date.AddDays(7 - (int)date.DayOfWeek);
        }
        /// <summary>
        /// 得到一年中的某周的起始日和截止日（星期日是第一天）
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtStart">开始日期</param>
        /// <param name="dtEnd">结束日期</param>
        public static void GetWeekTime(this DateTime dt, out DateTime dtStart, out DateTime dtEnd)
            => GetWeekTime(dt, dt.Year, dt.GetWeekOfYear(), out dtStart, out dtEnd);
#if NETSTANDARD2_1
        /// <summary>
        /// 得到一年中的某周的起始日和截止日（星期日是第一天）
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year">年份</param>
        /// <param name="week">第几周</param>
        /// <returns>开始日期/结束日期</returns>
        public static (DateTime dtStart, DateTime dtEnd) GetWeekTime(this DateTime dt, int year, int week)
        {
            DateTime dtStart, dtEnd;
            GetWeekTime(dt, year, week, out dtStart, out dtEnd);
            return (dtStart, dtEnd);
        }
        /// <summary>
        /// 得到一年中的某周的起始日和截止日（星期日是第一天）
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>开始日期/结束日期</returns>
        public static (DateTime dtStart, DateTime dtEnd) GetWeekTime(this DateTime dt)
            => GetWeekTime(dt, dt.Year, dt.GetWeekOfYear());
#endif

        /// <summary>
        /// 得到一年中的某周的起始日和截止日（星期一是第一天）
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year">年份</param>
        /// <param name="week">第几周（为0时从dt获取）</param>
        /// <param name="dtStart">开始日期</param>
        /// <param name="dtEnd">结束日期</param>
        public static void GetWeekTimeFromMonday(this DateTime dt, int year, int week, out DateTime dtStart, out DateTime dtEnd)
        {
            var date = new DateTime(year > 0 ? year : dt.Year, 1, 1);
            date += new TimeSpan((week - 1) * 7, 0, 0, 0);
            dtStart = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
            dtEnd = date.AddDays((int)DayOfWeek.Saturday - (int)date.DayOfWeek + 1);
        }
        /// <summary>
        /// 得到一年中的某周的起始日和截止日（星期一是第一天）
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtStart">开始日期</param>
        /// <param name="dtEnd">结束日期</param>
        public static void GetWeekTimeFromMonday(this DateTime dt, out DateTime dtStart, out DateTime dtEnd)
            => GetWeekTimeFromMonday(dt, dt.Year, dt.GetWeekOfYearFromMonday(), out dtStart, out dtEnd);
#if NETSTANDARD2_1
        /// <summary>
        /// 得到一年中的某周的起始日和截止日（星期一是第一天）
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year">年份</param>
        /// <param name="week">第几周</param>
        /// <returns>开始日期/结束日期</returns>
        public static (DateTime dtStart, DateTime dtEnd) GetWeekTimeFromMonday(this DateTime dt, int year, int week)
        {
            DateTime dtStart, dtEnd;
            GetWeekTimeFromMonday(dt, year, week, out dtStart, out dtEnd);
            return (dtStart, dtEnd);
        }
        /// <summary>
        /// 得到一年中的某周的起始日和截止日（星期一是第一天）
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>开始日期/结束日期</returns>
        public static (DateTime dtStart, DateTime dtEnd) GetWeekTimeFromMonday(this DateTime dt)
            => GetWeekTimeFromMonday(dt, dt.Year, dt.GetWeekOfYearFromMonday());
#endif
        #endregion

        #region GetWeekWorkTime
        /// <summary>
        /// 得到一年中的某周的起始日和截止日 周一到周五/工作日
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year">年份</param>
        /// <param name="week">第几周</param>
        /// <param name="dtStart">开始日期</param>
        /// <param name="dtEnd">结束日期</param>
        public static void GetWeekWorkTime(this DateTime dt, int year, int week, out DateTime dtStart, out DateTime dtEnd)
        {
            var date = new DateTime(year, 1, 1);
            date += new TimeSpan((week - 1) * 7, 0, 0, 0);
            dtStart = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Monday);
            dtEnd = date.AddDays((int)DayOfWeek.Saturday - (int)date.DayOfWeek + 1).AddDays(-2);
        }
#if NETSTANDARD2_1
        /// <summary>
        /// 得到一年中的某周的起始日和截止日 周一到周五/工作日
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="year">年份</param>
        /// <param name="week">第几周</param>
        /// <returns>开始日期/结束日期</returns>
        public static (DateTime dtStart, DateTime dtEnd) GetWeekWorkTime(this DateTime dt, int year, int week)
        {
            DateTime dtStart, dtEnd;
            GetWeekWorkTime(dt, year, week, out dtStart, out dtEnd);
            return (dtStart, dtEnd);
        }
        /// <summary>
        /// 得到一年中的某周的起始日和截止日 周一到周五/工作日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>开始日期/结束日期</returns>
        public static (DateTime dtStart, DateTime dtEnd) GetWeekWorkTime(this DateTime dt)
            => GetWeekWorkTime(dt, dt.Year, dt.GetWeekOfYearFromMonday());
#endif
        #endregion
        #endregion

        #region GetDay/GetHour/GetMinute XXX
        /// <summary>
        /// 天 开始时间
        /// </summary>
        public static DateTime GetDayStart(this DateTime dt)
            => new DateTime(dt.Year, dt.Month, dt.Day);
        /// <summary>
        /// 明天 开始时间
        /// </summary>
        public static DateTime GetNextDayStart(this DateTime dt)
            => dt.GetDayStart().AddDays(1);

        /// <summary>
        /// 时 开始时间
        /// </summary>
        public static DateTime GetHourStart(this DateTime dt)
            => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
        /// <summary>
        /// 下一小时 开始时间
        /// </summary>
        public static DateTime GetNextHourStart(this DateTime dt)
            => dt.GetHourStart().AddHours(1);

        /// <summary>
        /// 分 开始时间
        /// </summary>
        public static DateTime GetMinuteStart(this DateTime dt)
            => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        /// <summary>
        /// 下一分钟 开始时间
        /// </summary>
        public static DateTime GetNextMinuteStart(this DateTime dt)
            => dt.GetMinuteStart().AddMinutes(1);
        #endregion

        /// <summary>
        /// 判断是否是昨天
        /// </summary>
        public static bool IsYesterday(this DateTime dt)
            => dt.Date == DateTime.Now.Date.AddDays(-1);
        /// <summary>
        /// 判断是否是当天
        /// </summary>
        public static bool IsToday(this DateTime dt)
            => dt.Date == DateTime.Now.Date;
        /// <summary>
        /// 判断是否是明天
        /// </summary>
        public static bool IsTomorrow(this DateTime dt)
            => dt.Date == DateTime.Now.Date.AddDays(1);

        #region DateTimeToString
        /// <summary>
        /// 日期转换为中文 年月
        /// </summary>
        public static string ToCnMonthString(this DateTime dt)
            => dt.ToString("yyyy年MM月");
        /// <summary>
        /// 日期转换为中文 年月日
        /// </summary>
        public static string ToCnDateString(this DateTime dt)
            => dt.ToString("yyyy年MM月dd日");
        /// <summary>
        /// 时间转字符 月日
        /// </summary>
        public static string ToCNMonthDayString(this DateTime dt)
            => dt.ToString("MM月dd日");
        /// <summary>
        /// 时间转换为中文 年月日 时:分:秒
        /// </summary>
        public static string ToCnDatetimeString(this DateTime dt, bool second = false)
            => second ? dt.ToString($"yyyy年MM月dd日 HH:mm:ss") : dt.ToString($"yyyy年MM月dd日 HH:mm");
        private static readonly string[] WeekdayFullCN = new[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
        private static readonly string[] WeekdayShortCN = new[] { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
        /// <summary>
        /// 时间转换 星期一至日（默认全称，可输出缩写）
        /// eg： 星期日 / 周日
        /// </summary>
        /// <returns></returns>
        public static string ToCNWeekdayString(this DateTime dt, bool @short = false)
            => (@short ? WeekdayShortCN : WeekdayFullCN)[(int)dt.DayOfWeek];


        /// <summary>
        /// 时间转字符 年-月
        /// </summary>
        public static string ToMonthString(this DateTime dt)
            => dt.ToString("yyyy-MM");
        /// <summary>
        /// 时间转字符 年-月-日
        /// </summary>
        public static string ToDateString(this DateTime dt)
            => dt.ToString("yyyy-MM-dd");
        /// <summary>
        /// 时间转字符 月-日
        /// </summary>
        public static string ToMonthDayString(this DateTime dt)
            => dt.ToString("MM-dd");
        /// <summary>
        /// 时间转字符 年-月-日 时:分:秒
        /// </summary>
        public static string ToDatetimeString(this DateTime dt, bool second = false)
            => second ? dt.ToString($"yyyy-MM-dd HH:mm:ss") : dt.ToString($"yyyy-MM-dd HH:mm");
        /// <summary>
        /// 标准时间格式 年-月-日 时:分:秒.毫秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDatetimeMsString(this DateTime dt)
            => dt.ToString($"yyyy-MM-dd HH:mm:ss.fff");
        /// <summary>
        /// 标准时间格式 年-月-日 时:分:秒.微秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDatetimeUsString(this DateTime dt)
            => dt.ToString($"yyyy-MM-dd HH:mm:ss.ffffff");

        /// <summary>
        /// 时间转换 时:分
        /// </summary>
        public static string ToTimeString(this DateTime dt, bool second = false)
            => second ? dt.ToString($"HH:mm:ss") : dt.ToString($"HH:mm");
        /// <summary>
        /// 时间转换 日 时:分
        /// </summary>
        public static string ToDayTimeString(this DateTime dt, bool second = false)
            => second ? dt.ToString($"dd HH:mm:ss") : dt.ToString($"dd HH:mm");

        private static readonly string[] WeekdayFullEN = new[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        private static readonly string[] WeekdayShortEN = new[] { "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT" };
        /// <summary>
        /// 时间转换 星期一至日（默认全称，可输出缩写）
        /// eg： Sunday / SUN
        /// </summary>
        /// <returns></returns>
        public static string ToWeekdayString(this DateTime dt, bool @short = false)
            => (@short ? WeekdayShortEN : WeekdayFullEN)[(int)dt.DayOfWeek];


        /// <summary>
        /// 与现在时间的间隔（中文） --前/后
        /// </summary>
        public static string ToCnIntervalString(this DateTime dt)
        {
            var interval = dt - DateTime.Now;
            var endStr = interval > TimeSpan.Parse("0") ? "后" : "前";
            var day = interval.Days;
            if (day != 0)
            {
                if (day == -1)
                    return "昨天";
                if (day == 1)
                    return "明天";
                day = day < 0 ? day * -1 : day;
                if (day >= 365)
                    return $"{day / 365}年{endStr}";
                else if (day >= 30)
                    return $"{day / 30}个月{endStr}";
                else if (day >= 7)
                    return $"{day / 7}周{endStr}";
                else
                    return $"{day}天{endStr}";
            }
            var hour = interval.Hours;
            if (hour != 0)
            {
                if (dt.Date == DateTime.Now.Date && hour < -6)
                    return "今天";
                if (hour > 0 && DateTime.Now.AddDays(1).Date == dt.Date)
                    return "明天";
                hour = hour < 0 ? hour * -1 : hour;
                return $"{hour}小时{endStr}";
            }
            var minute = interval.Minutes;
            if (minute != 0)
            {
                if (minute < 0 && minute > -1)
                    return "刚刚";
                minute = minute < 0 ? minute * -1 : minute;
                return $"{minute}分钟{endStr}";
            }
            var second = interval.Seconds;
            {
                if (second > 0)
                    return $"{second}秒后";
                return "刚刚";
            }
        }
        #endregion

        #region Timestamp
        /// <summary>
        /// 将Datetime转换成时间戳（1970-1-1 00:00:00至target的总秒数）
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns>时间戳（1970-1-1 00:00:00至target的总秒数）</returns>
        public static long ToTimestampInt64(this DateTime dateTime)
            => (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        /// <summary>
        /// 将Datetime转换成时间戳（1970-1-1 00:00:00至target的总秒数）
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns>时间戳（1970-1-1 00:00:00至target的总秒数）</returns>
        public static int ToTimestamp(this DateTime dateTime)
            => (int)dateTime.ToTimestampInt64();

        private static readonly DateTime Start1970 = new DateTime(1970, 1, 1);
        /// <summary>
        /// 将时间戳（1970-1-1 00:00:00至target的总秒数）转换成Datetime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime TimestampToDateTime(this long timestamp)
            => Start1970.AddTicks((timestamp + 8 * 60 * 60) * 10000000);
        /// <summary>
        /// 将时间戳（1970-1-1 00:00:00至target的总秒数）转换成Datetime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime TimestampToDateTime(this int timestamp)
            => TimestampToDateTime((long)timestamp);
        #endregion

        /// <summary>
        /// 获得一段时间内相隔时分秒
        /// </summary>
        /// <param name="dtStart">起始时间</param>
        /// <param name="dtEnd">终止时间</param>
        /// <param name="abs">绝对时间(True时不使用 - 表示之前)</param>
        /// <returns>小时差</returns>
        public static string GetTimeDelay(this DateTime dtStart, DateTime dtEnd, bool abs = true)
        {
            long lTicks = (dtEnd.Ticks - dtStart.Ticks) / 10000000;
            string sTemp = abs == false && lTicks < 0 ? "-" : string.Empty;
            lTicks = Math.Abs(lTicks);
            sTemp += (lTicks / 3600).ToString().PadLeft(2, '0') + ":";
            sTemp += (lTicks % 3600 / 60).ToString().PadLeft(2, '0') + ":";
            sTemp += (lTicks % 3600 % 60).ToString().PadLeft(2, '0');
            return sTemp;
        }
    }
}
