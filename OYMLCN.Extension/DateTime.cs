using OYMLCN.Handlers;
using System;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// DateTimeExtension
    /// </summary>
    public static partial class DateTimeExtensions
    {
        /// <summary>
        /// 时间相关值以及判断操作
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [Obsolete("此扩展即将弃用，请使用相关扩展")]
        public static DateTimeFormatHandler AsFormat(this DateTime dt)
     => new DateTimeFormatHandler(dt);


        #region DateTime
        /// <summary>
        /// 年 开始时间
        /// </summary>
        public static DateTime GetGetYearStart(this DateTime dt)
            => new DateTime(dt.Year, 1, 1);
        /// <summary>
        /// 年 结束时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetYearEnd(this DateTime dt)
            => dt.GetGetYearStart().AddYears(1);

        /// <summary>
        /// 月 开始时间
        /// </summary>
        public static DateTime GetMonthStart(this DateTime dt)
            => new DateTime(dt.Year, dt.Month, 1);
        /// <summary>
        /// 月 结束时间
        /// </summary>
        public static DateTime GetMonthEnd(this DateTime dt)
            => dt.GetMonthStart().AddMonths(1);

        /// <summary>
        /// 周(日)开始时间
        /// </summary>
        public static DateTime GetWeekStart(this DateTime dt)
            => dt.Date.AddDays(-(int)dt.DayOfWeek);
        /// <summary>
        /// 周(日)结束时间
        /// </summary>
        public static DateTime GetWeekEnd(this DateTime dt)
            => dt.GetWeekStart().AddDays(7);
        /// <summary>
        /// 周(一)开始时间
        /// </summary>
        public static DateTime GetWeekStartByMonday(this DateTime dt)
            => dt.Date.AddDays(-(int)dt.DayOfWeek + 1);
        /// <summary>
        /// 周(一)结束时间
        /// </summary>
        public static DateTime GetWeekEndByMonday(this DateTime dt)
            => dt.GetWeekStartByMonday().AddDays(7);

        /// <summary>
        /// 天 开始时间
        /// </summary>
        public static DateTime GetDayStart(this DateTime dt)
            => new DateTime(dt.Year, dt.Month, dt.Day);
        /// <summary>
        /// 天 结束时间
        /// </summary>
        public static DateTime GetDayEnd(this DateTime dt)
            => dt.GetDayStart().AddDays(1);

        /// <summary>
        /// 时 开始时间
        /// </summary>
        public static DateTime GetHourStart(this DateTime dt)
            => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
        /// <summary>
        /// 时 结束时间
        /// </summary>
        public static DateTime GetHourEnd(this DateTime dt)
            => dt.GetHourStart().AddHours(1);

        /// <summary>
        /// 分 开始时间
        /// </summary>
        public static DateTime GetMinuteStart(this DateTime dt)
            => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        /// <summary>
        /// 分 结束时间
        /// </summary>
        public static DateTime GetGetMinuteEnd(this DateTime dt)
            => dt.GetMinuteStart().AddMinutes(1);

        #endregion

        /// <summary>
        /// 判断是否是当天
        /// </summary>
        public static bool IsToday(this DateTime dt)
            => dt.Date == DateTime.Now.Date;

        #region DateTimeToString
        /// <summary>
        /// 日期转换为中文 年月
        /// </summary>
        public static string GetCnMonthString(this DateTime dt)
            => dt.ToString("yyyy年MM月");
        /// <summary>
        /// 时间转换为中文 年月日时分
        /// </summary>
        public static string GetCnDatetimeString(this DateTime dt, bool second = false)
            => second ? dt.ToString($"yyyy年MM月dd日 HH:mm:ss") : dt.ToString($"yyyy年MM月dd日 HH:mm");

        /// <summary>
        /// 时间转字符 年-月
        /// </summary>
        public static string GetMonthString(this DateTime dt)
            => dt.ToString("yyyy-MM");
        /// <summary>
        /// 时间转字符 年-月-日
        /// </summary>
        public static string GetDateString(this DateTime dt)
            => dt.ToString("yyyy-MM-dd");
        /// <summary>
        /// 时间转字符 年-月-日 时:分:秒
        /// </summary>
        public static string GetDatetimeString(this DateTime dt, bool second = false)
            => second ? dt.ToString($"yyyy-MM-dd HH:mm:ss") : dt.ToString($"yyyy-MM-dd HH:mm");

        /// <summary>
        /// 时间转换 时:分
        /// </summary>
        public static string GetTimeString(this DateTime dt, bool second = false)
            => second ? dt.ToString($"HH:mm:ss") : dt.ToString($"HH:mm");
        /// <summary>
        /// 时间转换 日 时:分
        /// </summary>
        public static string GetDayTimeString(this DateTime dt, bool second = false)
            => second ? dt.ToString($"dd HH:mm:ss") : dt.ToString($"dd HH:mm");

        /// <summary>
        /// 与现在时间的间隔（中文） --前/后
        /// </summary>
        public static string GetCnIntervalString(this DateTime dt)
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

        /// <summary>
        /// 将时间戳（1970-1-1 00:00:00至target的总秒数）转换成Datetime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime TimestampToDateTime(this long timestamp)
            => new DateTime(1970, 1, 1).AddTicks((timestamp + 8 * 60 * 60) * 10000000);
        /// <summary>
        /// 将时间戳（1970-1-1 00:00:00至target的总秒数）转换成Datetime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime TimestampToDateTime(this int timestamp)
            => TimestampToDateTime((long)timestamp);
        #endregion
    }
}
