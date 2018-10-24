// base on https://github.com/myloveCc/NETCore.Encrypt
// version 2.0.7

using System;
using System.Collections.Generic;
using System.IO;

namespace OYMLCN
{
    /// <summary>
    /// 参数有效性验证
    /// </summary>
    public static class ArgumentChecker
    {
        /// <summary>
        /// 非空判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotEmpty(Guid argument, string argumentName)
        {
            if (argument == Guid.Empty)
                throw new ArgumentException(string.Format("\"{0}\" 不能为空Guid.", argumentName), argumentName);
        }

        /// <summary>
        /// 非空判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotEmpty(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty((argument ?? string.Empty).Trim()))
                throw new ArgumentException(string.Format("\"{0}\" 不能为空.", argumentName), argumentName);
        }

        /// <summary>
        /// 字符串长度限制判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="length"></param>
        /// <param name="argumentName"></param>
        public static void IsNotOutOfLength(string argument, int length, string argumentName)
        {
            if (argument.Trim().Length > length)
                throw new ArgumentException(string.Format("\"{0}\" 不能超过 {1} 字符.", argumentName, length), argumentName);
        }

        /// <summary>
        /// 非空判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        /// <param name="message"></param>
        public static void IsNotNull(object argument, string argumentName, string message = "")
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName, message);
        }

        /// <summary>
        /// 非负判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNegative(int argument, string argumentName)
        {
            if (argument < 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }
        /// <summary>
        /// 正数非零判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNegativeOrZero(int argument, string argumentName)
        {
            if (argument <= 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary>
        /// 非负判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNegative(long argument, string argumentName)
        {
            if (argument < 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }
        /// <summary>
        /// 正数非零判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNegativeOrZero(long argument, string argumentName)
        {
            if (argument <= 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary>
        /// 非负判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNegative(float argument, string argumentName)
        {
            if (argument < 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary>
        /// 正数非零判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNegativeOrZero(float argument, string argumentName)
        {
            if (argument <= 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary>
        /// 非负判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNegative(decimal argument, string argumentName)
        {
            if (argument < 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary>
        /// 正数非零判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNegativeOrZero(decimal argument, string argumentName)
        {
            if (argument <= 0)
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary>
        /// 时间有效性判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotInvalidDate(DateTime argument, string argumentName)
        {
            DateTime MinDate = new DateTime(1900, 1, 1);
            DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

            if (!((argument >= MinDate) && (argument <= MaxDate)))
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary>
        /// 非过去的时间判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotInPast(DateTime argument, string argumentName)
        {
            if (argument < DateTime.Now)
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary>
        /// 过去的时间判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotInFuture(DateTime argument, string argumentName)
        {
            if (argument > DateTime.Now)
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary>
        /// 非负判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNegative(TimeSpan argument, string argumentName)
        {
            if (argument < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(argumentName);
        }
        /// <summary>
        /// 非负非零判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotNegativeOrZero(TimeSpan argument, string argumentName)
        {
            if (argument <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(argumentName);
        }

        /// <summary>
        /// 非空判断
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotEmpty<T>(ICollection<T> argument, string argumentName)
        {
            IsNotNull(argument, argumentName, "集合不能为Null");

            if (argument.Count == 0)
                throw new ArgumentException("集合不能为空.", argumentName);
        }
        /// <summary>
        /// 值范围判断
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="argumentName"></param>
        public static void IsNotOutOfRange(int argument, int min, int max, string argumentName)
        {
            if ((argument < min) || (argument > max))
                throw new ArgumentOutOfRangeException(argumentName, string.Format("{0} 必须在此区间 \"{1}\"-\"{2}\".", argumentName, min, max));
        }

        /// <summary>
        /// 文件不存在报错
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotExistsFile(string argument, string argumentName)
        {
            IsNotEmpty(argument, argumentName);

            if (!File.Exists(argument))
                throw new ArgumentException(string.Format("\"{0}\" 文件不存在", argumentName), argumentName);
        }
        /// <summary>
        /// 目录不存在报错
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void IsNotExistsDirectory(string argument, string argumentName)
        {
            IsNotEmpty(argument, argumentName);

            if (!Directory.Exists(argument))
                throw new ArgumentException(string.Format("\"{0}\" 目录不存在", argumentName), argumentName);
        }
    }
}
