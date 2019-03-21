using System;

namespace OYMLCN.Handlers
{
    /// <summary>
    /// 时间相关值以及判断
    /// </summary>
    public class DateTimeFormatHandler
    {
        private DateTime dt;
        internal DateTimeFormatHandler(DateTime dateTime)
            => dt = dateTime;

        #region DateTime
        /// <summary>
        /// 年 开始时间
        /// </summary>
        public DateTime YearStart
            => new DateTime(dt.Year, 1, 1);
        /// <summary>
        /// 年 结束时间
        /// </summary>
        /// <returns></returns>
        public DateTime YearEnd
            => YearStart.AddYears(1);

        /// <summary>
        /// 月 开始时间
        /// </summary>
        public DateTime MonthStart
            => new DateTime(dt.Year, dt.Month, 1);
        /// <summary>
        /// 月 结束时间
        /// </summary>
        public DateTime MonthEnd
            => MonthStart.AddMonths(1);

        /// <summary>
        /// 周(日)开始时间
        /// </summary>
        public DateTime WeekStart
            => dt.Date.AddDays(-(int)dt.DayOfWeek);
        /// <summary>
        /// 周(日)结束时间
        /// </summary>
        public DateTime WeekEnd
            => WeekStart.AddDays(7);
        /// <summary>
        /// 周(一)开始时间
        /// </summary>
        public DateTime WeekStartByMonday
            => dt.Date.AddDays(-(int)dt.DayOfWeek + 1);
        /// <summary>
        /// 周(一)结束时间
        /// </summary>
        public DateTime WeekEndByMonday
            => WeekStartByMonday.AddDays(7);

        /// <summary>
        /// 天 开始时间
        /// </summary>
        public DateTime DayStart
            => new DateTime(dt.Year, dt.Month, dt.Day);
        /// <summary>
        /// 天 结束时间
        /// </summary>
        public DateTime DayEnd
            => DayStart.AddDays(1);

        /// <summary>
        /// 时 开始时间
        /// </summary>
        public DateTime HourStart
            => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
        /// <summary>
        /// 时 结束时间
        /// </summary>
        public DateTime HourEnd
            => HourStart.AddHours(1);

        /// <summary>
        /// 分 开始时间
        /// </summary>
        public DateTime MinuteStart
            => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
        /// <summary>
        /// 分 结束时间
        /// </summary>
        public DateTime GetMinuteEnd
            => MinuteStart.AddMinutes(1);

        #endregion

        /// <summary>
        /// 判断是否是当天
        /// </summary>
        public bool IsToday
            => dt.Date == DateTime.Now.Date;

        #region DateTimeToString
        /// <summary>
        /// 日期转换为中文 年月
        /// </summary>
        public string CnMonthString
            => dt.ToString("yyyy年MM月");
        /// <summary>
        /// 日期转换为中文 年月日
        /// </summary>
        public string CnDateString
            => dt.ToString("yyyy年MM月dd日");
        /// <summary>
        /// 时间转换为中文 年月日时分
        /// </summary>
        public string CnDatetimeString
            => dt.ToString($"yyyy年MM月dd日 HH:mm");
        /// <summary>
        /// 时间转换为中文 年月日 时:分:秒
        /// </summary>
        public string CnDatetimeStringWithSecond
            => dt.ToString($"yyyy年MM月dd日 HH:mm:ss");


        /// <summary>
        /// 时间转字符 年-月
        /// </summary>
        public string MonthString
            => dt.ToString("yyyy-MM");
        /// <summary>
        /// 时间转字符 年-月-日
        /// </summary>
        public string DateString
            => dt.ToString("yyyy-MM-dd");
        /// <summary>
        /// 时间转字符 年-月-日 时:分
        /// </summary>
        public string DatetimeString
            => dt.ToString($"yyyy-MM-dd HH:mm");
        /// <summary>
        /// 时间转字符 年-月-日 时:分:秒
        /// </summary>
        public string DatetimeStringWithSecond
            => dt.ToString($"yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// 时间转换 时:分
        /// </summary>
        public string TimeString
            => dt.ToString($"HH:mm");
        /// <summary>
        /// 时间转换 时:分:秒
        /// </summary>
        public string TimeStringWithSecond
            => dt.ToString($"HH:mm:ss");
        /// <summary>
        /// 时间转换 日 时:分
        /// </summary>
        public string DayTimeString
            => dt.ToString($"dd HH:mm");
        /// <summary>
        /// 时间转换 日 时:分:秒
        /// </summary>
        public string DayTimeStringWithSecond
            => dt.ToString($"dd HH:mm:ss");

        /// <summary>
        /// 与现在时间的间隔（中文） --前/后
        /// </summary>
        public string CnIntervalString
        {
            get
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
        }
        #endregion

    }

}
