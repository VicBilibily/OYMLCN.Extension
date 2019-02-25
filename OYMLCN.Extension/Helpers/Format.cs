using OYMLCN.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 格式处理辅助
    /// </summary>
    public static class FormatHelpers
    {
        /// <summary>
        /// 将枚举类型转换为Key/Value数组
        /// 必须为enum枚举的任意值，其他类型将返回Null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumClass"></param>
        /// <returns></returns>
        public static Dictionary<string, T> EnumToKeyValues<T>(T enumClass)
        {
            var reuslt = new Dictionary<string, T>();
            if (enumClass.GetType().BaseType != typeof(Enum))
                throw new ArgumentException("参数基类型并非类型System.Enum");
            foreach (T value in Enum.GetValues(enumClass.GetType()))
                reuslt[value.ToString()] = value;
            return reuslt;
        }

        /// <summary>
        /// QueryString拆解为字典
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public static Dictionary<string, string> QueryStringToDictionary(string queryString)
        {
            var dic = new Dictionary<string, string>();
            queryString = WebUtility.HtmlDecode(queryString);
            queryString = queryString.SplitThenGetLast("?");
            foreach (var item in queryString.SplitBySign("&"))
            {
                var key = item.SplitThenGetFirst("=");
                var value = item.SubString(key.Length + 1);
                value = WebUtility.UrlDecode(value);
                dic.Add(key, value);
            }
            return dic;
        }

        #region FormatCapacity 字节容量格式化
        /// <summary>
        /// 格式化字节数到更大单位
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytesCapacity(ulong bytes)
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
        /// 字节总量转换为KB标识
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal BytesLengthToKB(ulong length)
            => Math.Round(length / Convert.ToDecimal(1000), 2, MidpointRounding.AwayFromZero);
        /// <summary>
        /// 字节总量转换为MB标识
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal BytesLengthToMB(ulong length)
            => Math.Round(length / Convert.ToDecimal(1000 * 1000), 2, MidpointRounding.AwayFromZero);
        /// <summary>
        /// 字节总量转换为GB标识
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal BytesLengthToGB(ulong length)
            => Math.Round(length / Convert.ToDecimal(1000 * 1000 * 1000), 2, MidpointRounding.AwayFromZero);

        /// <summary>
        /// 字节总量转换为KiB标识
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal BytesLengthToKiB(ulong length)
            => Math.Round(length / Convert.ToDecimal(1024), 2, MidpointRounding.AwayFromZero);
        /// <summary>
        /// 字节总量转换为MiB标识
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal BytesLengthToMiB(ulong length)
            => Math.Round(length / Convert.ToDecimal(1024 * 1024), 2, MidpointRounding.AwayFromZero);
        /// <summary>
        /// 字节总量转换为GiB标识
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static decimal BytesLengthToGiB(ulong length)
            => Math.Round(length / Convert.ToDecimal(1024 * 1024 * 1024), 2, MidpointRounding.AwayFromZero);
        #endregion

    }
}
