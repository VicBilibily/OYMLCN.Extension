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
        public static DateTimeFormatHandler AsFormat(this DateTime dt)
            => new DateTimeFormatHandler(dt);

        #region Timestamp
        /// <summary>
        /// 将Datetime转换成时间戳（1970-1-1 00:00:00至target的总秒数）
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns>时间戳（1970-1-1 00:00:00至target的总秒数）</returns>
        public static long ToTimestamp(this DateTime dateTime)
            => (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        /// <summary>
        /// 将Datetime转换成时间戳（1970-1-1 00:00:00至target的总秒数）
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns>时间戳（1970-1-1 00:00:00至target的总秒数）</returns>
        public static int ToTimestampInt32(this DateTime dateTime)
            => (int)dateTime.ToTimestamp();

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
