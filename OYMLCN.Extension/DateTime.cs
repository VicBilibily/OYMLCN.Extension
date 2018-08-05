using System;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// DateTimeExtension
    /// </summary>
    public static partial class DateTimeExtensions
    {
        #region Timestamp
        /// <summary>
        /// 将Datetime转换成时间戳（1970-1-1 00:00:00至target的总秒数）
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns>时间戳（1970-1-1 00:00:00至target的总秒数）</returns>
        public static long ToTimestamp(this DateTime dateTime) => (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        /// <summary>
        /// 将Datetime转换成时间戳（1970-1-1 00:00:00至target的总秒数）
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns>时间戳（1970-1-1 00:00:00至target的总秒数）</returns>
        public static int ToTimestampInt32(this DateTime dateTime) => (int)dateTime.ToTimestamp();
        /// <summary>
        /// 将时间戳（1970-1-1 00:00:00至target的总秒数）转换成Datetime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime TimestampToDateTime(this long timestamp) => new DateTime(1970, 1, 1).AddTicks((timestamp + 8 * 60 * 60) * 10000000);
        /// <summary>
        /// 将时间戳（1970-1-1 00:00:00至target的总秒数）转换成Datetime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime TimestampToDateTime(this int timestamp) => TimestampToDateTime((long)timestamp);
        #endregion

        /// <summary>
        /// 获取年 开始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetYearStart(this DateTime dt) => new DateTime(dt.Year, 1, 1);
        /// <summary>
        /// 获取年 结束时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetYearEnd(this DateTime dt) => dt.GetYearStart().AddYears(1);

        /// <summary>
        /// 获取月 开始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMonthStart(this DateTime dt) => new DateTime(dt.Year, dt.Month, 1);
        /// <summary>
        /// 获取月 结束时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMonthEnd(this DateTime dt) => dt.GetMonthStart().AddMonths(1);

        /// <summary>
        /// 获取周(日)开始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetWeekStart(this DateTime dt) => dt.Date.AddDays(-(int)dt.DayOfWeek);
        /// <summary>
        /// 获取周(日)结束时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetWeekEnd(this DateTime dt) => dt.GetWeekStart().AddDays(7);
        /// <summary>
        /// 获取周(一)开始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetWeekStartByMon(this DateTime dt) => dt.Date.AddDays(-(int)dt.DayOfWeek + 1);
        /// <summary>
        /// 获取周(一)结束时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetWeekEndByMon(this DateTime dt) => dt.GetWeekStartByMon().AddDays(7);

        /// <summary>
        /// 获取天 开始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetDayStart(this DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day);
        /// <summary>
        /// 获取天 结束时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetDayEnd(this DateTime dt) => dt.GetDayStart().AddDays(1);

        /// <summary>
        /// 获取时 开始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetHourStart(this DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
        /// <summary>
        /// 获取时 结束时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetHourEnd(this DateTime dt) => dt.GetHourStart().AddHours(1);

        /// <summary>
        /// 获取分 开始时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMinuteStart(this DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        /// <summary>
        /// 获取分 结束时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMinuteEnd(this DateTime dt) => dt.GetMinuteStart().AddMinutes(1);

        /// <summary>
        /// 判断是否是当天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsToday(this DateTime dt) => dt.Date == DateTime.Now.Date;

    }
}
