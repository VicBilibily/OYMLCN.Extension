using System;
using System.Collections.Generic;
using System.IO;

namespace OYMLCN.ArgumentChecker
{
    /// <summary>
    /// 参数有效性验证
    /// </summary>
    public static class ArgumentChecker
    {
        #region 字符串判断
        /// <summary> 检查字符串是否为空或 null，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIfNullOrEmpty(this string argument, string argumentName)
        {
            ThrowIfNull(argument, argumentName);
            ThrowIfEmpty(argument, argumentName);
        }
        /// <summary> 检查字符串是否为空，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIfEmpty(this string argument, string argumentName)
        {
            if (string.IsNullOrEmpty((argument ?? string.Empty).Trim()))
                throw new ArgumentException(string.Format("\"{0}\" 是 System.String", argumentName), argumentName);
        }
        /// <summary> 检查字符串是否超出长度限制，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIfOutOfLength(this string argument, int length, string argumentName)
        {
            if (argument.Trim().Length > length)
                throw new ArgumentException(string.Format("\"{0}\" 不能超过 {1} 字符.", argumentName, length), argumentName);
        }
        #endregion

        #region 空值判断
        /// <summary> 检查 <seealso cref="Guid"/> 是否为空值，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIfEmpty(this Guid argument, string argumentName)
        {
            if (argument == Guid.Empty)
                throw new ArgumentException(string.Format("\"{0}\" 不能为空Guid.", argumentName), argumentName);
        }
        /// <summary> 检查对象是否为 null，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ThrowIfNull<T>(this T argument, string argumentName, string message = null)
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName, message ?? $"{argumentName} 为 null");
        }
        #endregion

        #region 数值判断
        /// <summary> 检查数值是否是负数，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfNegative(this int argument, string argumentName)
        {
            if (argument < 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }
        /// <summary> 检查数值是否非正数或非零，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfNegativeOrZero(this int argument, string argumentName)
        {
            if (argument <= 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary> 检查数值是否是负数，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfNegative(this long argument, string argumentName)
        {
            if (argument < 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }
        /// <summary> 检查数值是否非正数或非零，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfNegativeOrZero(this long argument, string argumentName)
        {
            if (argument <= 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary> 检查数值是否是负数，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfNegative(this float argument, string argumentName)
        {
            if (argument < 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }
        /// <summary> 检查数值是否非正数或非零，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfNegativeOrZero(this float argument, string argumentName)
        {
            if (argument <= 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary> 检查数值是否是负数，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfNegative(this decimal argument, string argumentName)
        {
            if (argument < 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }
        /// <summary> 检查数值是否非正数或非零，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfNegativeOrZero(this decimal argument, string argumentName)
        {
            if (argument <= 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }
        #endregion


        #region 时间判断
        /// <summary> 检查时间是否有效（1900年后），如果无效则抛出异常 </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfInvalidDateTime(this DateTime argument, string argumentName)
        {
            DateTime MinDate = new DateTime(1900, 1, 1);
            DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);
            if (!((argument >= MinDate) && (argument <= MaxDate)))
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary> 检查时间是否是过去的时间，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfPastTime(this DateTime argument, string argumentName)
        {
            if (argument < DateTime.Now)
                throw new ArgumentOutOfRangeException(argumentName);
        }
        /// <summary> 检查时间是否是未来的时间，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfFutureTime(this DateTime argument, string argumentName)
        {
            if (argument > DateTime.Now)
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary> 检查时间间隔是否小于0，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfNegative(this TimeSpan argument, string argumentName)
        {
            if (argument < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(argumentName);
        }
        /// <summary> 检查时间间隔是否小于0或等于0，如果是则抛出异常 </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfNegativeOrZero(this TimeSpan argument, string argumentName)
        {
            if (argument <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(argumentName);
        }
        #endregion

        /// <summary> 检查集合是否为空，如果是则报错 </summary>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIfEmpty<T>(this ICollection<T> argument, string argumentName)
        {
            ThrowIfNull(argument, argumentName, "集合不能为Null");
            if (argument.Count == 0)
                throw new ArgumentException("集合不能为空.", argumentName);
        }

        /// <summary> 检查取值是否在范围内，如果不在范围内则报错 </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void ThrowIfOutOfRange(this int argument, int min, int max, string argumentName)
        {
            if ((argument < min) || (argument > max))
                throw new ArgumentOutOfRangeException(argumentName, string.Format("{0} 必须在此区间 \"{1}\"-\"{2}\".", argumentName, min, max));
        }

        #region 文件(夹)判断
        /// <summary> 检查文件是否存在，如果文件不存在则报错 </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIfNotExistsFile(this string argument, string argumentName)
        {
            ThrowIfNull(argument, argumentName);
            if (!File.Exists(argument))
                throw new ArgumentException(string.Format("\"{0}\" 文件不存在", argumentName), argumentName);
        }
        /// <summary> 检查目录是否存在，如果目录不存在则报错 </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIfNotExistsDirectory(this string argument, string argumentName)
        {
            ThrowIfNull(argument, argumentName);
            if (!Directory.Exists(argument))
                throw new ArgumentException(string.Format("\"{0}\" 目录不存在", argumentName), argumentName);
        }
        #endregion
    }
}
