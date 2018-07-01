using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN
{
    /// <summary>
    /// FormatExtension
    /// </summary>
    public static class FormatExtensions
    {
        #region 字节转更大单位
        struct CapacityInfo
        {
            /// <summary>
            /// 值
            /// </summary>
            public float value;
            /// <summary>
            /// 单位
            /// </summary>
            public string unitName;
            /// <summary>
            /// 原始计数
            /// </summary>
            public long unit;

            /// <summary>
            /// 容量信息
            /// </summary>
            /// <param name="value"></param>
            /// <param name="unitName"></param>
            /// <param name="unit"></param>
            public CapacityInfo(float value, string unitName, long unit)
            {
                this.value = value;
                this.unitName = unitName;
                this.unit = unit;
            }
        }
        private static CapacityInfo GetCapacity(this ulong n)
        {
            long scale = 1;
            float f = n;
            string unit = "B";
            if (f > 1024)
            {
                f = f / 1024;
                scale <<= 10;
                unit = "KiB";
            }
            if (f > 1024)
            {
                f = f / 1024;
                scale <<= 10;
                unit = "MiB";
            }
            if (f > 1024)
            {
                f = f / 1024;
                scale <<= 10;
                unit = "GiB";
            }
            if (f > 1024)
            {
                f = f / 1024;
                scale <<= 10;
                unit = "TiB";
            }
            return new CapacityInfo(f, unit, scale);
        }

        /// <summary>
        /// 格式化字节数到更大单位
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static string FormatCapacity(this ulong n)
        {
            var result = GetCapacity(n);
            return $"{result.value:0.##}{result.unitName}";
        }
        /// <summary>
        /// 格式化字节数到更大单位
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatCapacityBytes(this ulong bytes)
        {
            const long K = 1024L;
            const long M = K * 1024L;
            const long G = M * 1024L;
            const long T = G * 1024L;
            const long P = T * 1024L;
            const long E = P * 1024L;

            if (bytes >= P * 990)
                return (bytes / (double)E).ToString("F5") + "EiB";
            if (bytes >= T * 990)
                return (bytes / (double)P).ToString("F5") + "PiB";
            if (bytes >= G * 990)
                return (bytes / (double)T).ToString("F5") + "TiB";
            if (bytes >= M * 990)
                return (bytes / (double)G).ToString("F4") + "GiB";
            if (bytes >= M * 100)
                return (bytes / (double)M).ToString("F1") + "MiB";
            if (bytes >= M * 10)
                return (bytes / (double)M).ToString("F2") + "MiB";
            if (bytes >= K * 990)
                return (bytes / (double)M).ToString("F3") + "MiB";
            if (bytes > K * 2)
                return (bytes / (double)K).ToString("F1") + "KiB";
            return bytes.ToString() + "B";
        }

        /// <summary>
        /// 字节总量转换为MB标识
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal BytesLengthToMB(this ulong length) => Math.Round(length / Convert.ToDecimal(1024 * 1024), 2, MidpointRounding.AwayFromZero);
        /// <summary>
        /// 字节总量转换为GB标识
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal BytesLengthToGB(this ulong length) => Math.Round(length / Convert.ToDecimal(1024 * 1024 * 1024), 2, MidpointRounding.AwayFromZero);
        #endregion

        #region DateTimeToString
        /// <summary>
        /// 日期转换为中文 年月
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToCnMonthString(this DateTime dt) => dt.ToString("yyyy年MM月");
        /// <summary>
        /// 日期转换为中文 年月日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToCnDateString(this DateTime dt) => dt.ToString("yyyy年MM月dd日");
        /// <summary>
        /// 时间转换为中文 年月日时分|秒
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="second">是否包含秒</param>
        /// <returns></returns>
        public static string ToCnDatetimeString(this DateTime dt, bool second = false) =>
            dt.ToString($"yyyy年MM月dd日 HH:mm{(second ? ":ss" : "")}");


        /// <summary>
        /// 时间转字符 年-月
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToMonthString(this DateTime dt) => dt.ToString("yyyy-MM");
        /// <summary>
        /// 时间转字符 年-月-日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime dt) => dt.ToString("yyyy-MM-dd");
        /// <summary>
        /// 时间转字符 年-月-日 时:分|秒
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="second">是否包含秒</param>
        /// <returns></returns> 
        public static string ToDatetimeString(this DateTime dt, bool second = false) =>
            dt.ToString($"yyyy-MM-dd HH:mm{(second ? ":ss" : "")}");

        /// <summary>
        /// 时间转换 时分|秒
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="second">是否包含秒</param>
        /// <returns></returns>
        public static string ToTimeString(this DateTime dt, bool second = false) =>
            dt.ToString($"HH:mm{(second ? ":ss" : "")}");
        /// <summary>
        /// 时间转换 日时分|秒
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="second">是否包含秒</param>
        /// <returns></returns>
        public static string ToDayTimeString(this DateTime dt, bool second = false) =>
            dt.ToString($"dd HH:mm{(second ? ":ss" : "")}");

        /// <summary>
        /// 与现在时间的间隔（中文） --前/后
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
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


    }
}
