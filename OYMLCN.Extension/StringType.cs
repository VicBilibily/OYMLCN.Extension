using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// StringExtension
    /// </summary>
    public static partial class StringExtensions
    {
        #region 数字的转换
        /// <summary>
        /// 转换数字字符串为可空SByte类型
        /// </summary>
        public static sbyte? ConvertToNullableSByte(this string str)
        {
            try { return Convert.ToSByte(str); }
            catch
            {
                str = str.FormatAsIntegerNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return Convert.ToSByte(str);
            }
        }

        /// <summary>
        /// 转换数字字符串为可空Byte类型
        /// </summary>
        public static byte? ConvertToNullableByte(this string str)
        {
            try { return Convert.ToByte(str); }
            catch
            {
                str = str.FormatAsIntegerNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return Convert.ToByte(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Int16类型
        /// </summary>
        public static short? ConvertToNullableShort(this string str)
        {
            try { return Convert.ToInt16(str); }
            catch
            {
                str = str.FormatAsIntegerNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return Convert.ToInt16(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Int32类型
        /// </summary>
        public static int? ConvertToNullableInt(this string str)
        {
            try { return Convert.ToInt32(str); }
            catch
            {
                str = str.FormatAsIntegerNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return Convert.ToInt32(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Int64类型
        /// </summary>
        public static long? ConvertToNullableLong(this string str)
        {
            try { return Convert.ToInt64(str); }
            catch
            {
                str = str.FormatAsIntegerNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return Convert.ToInt64(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空BigInteger类型
        /// </summary>
        public static System.Numerics.BigInteger? ConvertToNullableBigInteger(this string str)
        {
            try { return System.Numerics.BigInteger.Parse(str); }
            catch
            {
                str = str.FormatAsIntegerNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return System.Numerics.BigInteger.Parse(str);
            }
        }

        /// <summary>
        /// 转换字符串为可空Single/float类型
        /// </summary>
        public static float? ConvertToNullableFloat(this string str)
        {
            try { return Convert.ToSingle(str); }
            catch
            {
                str = str.FormatAsNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return Convert.ToSingle(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Double类型
        /// </summary>
        public static double? ConvertToNullableDouble(this string str)
        {
            try { return Convert.ToDouble(str); }
            catch
            {
                str = str.FormatAsNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return Convert.ToDouble(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Decimal类型
        /// </summary>
        public static decimal? ConvertToNullableDecimal(this string str)
        {
            try { return Convert.ToDecimal(str); }
            catch
            {
                str = str.FormatAsNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return Convert.ToDecimal(str);
            }
        }


        /// <summary>
        /// 转换数字字符串为SByte类型
        /// </summary>
        public static sbyte ConvertToSByte(this string str)
            => str.ConvertToNullableSByte() ?? 0;
        /// <summary>
        /// 转换数字字符串为Byte类型
        /// </summary>
        public static byte ConvertToByte(this string str)
            => str.ConvertToNullableByte() ?? 0;
        /// <summary>
        /// 转换字符串为Int16类型
        /// </summary>
        public static short ConvertToShort(this string str)
            => str.ConvertToNullableShort() ?? 0;
        /// <summary>
        /// 转换字符串为Int32类型
        /// </summary>
        public static int ConvertToInt(this string str)
            => str.ConvertToNullableInt() ?? 0;
        /// <summary>
        /// 转换字符串为Int64类型
        /// </summary>
        public static long ConvertToLong(this string str)
            => str.ConvertToNullableLong() ?? 0;
        /// <summary>
        /// 转换字符串为可空BigInteger类型
        /// </summary>
        public static System.Numerics.BigInteger ConvertToBigInteger(this string str)
            => str.ConvertToNullableBigInteger() ?? 0;

        /// <summary>
        /// 转换字符串为Single/float类型
        /// </summary>
        public static float ConvertToFloat(this string str)
            => str.ConvertToNullableFloat() ?? 0;
        /// <summary>
        /// 转换字符串为Double类型
        /// </summary>
        public static double ConvertToDouble(this string str)
            => str.ConvertToNullableDouble() ?? 0;
        /// <summary>
        /// 转换字符串为Decimal类型
        /// </summary>
        public static decimal ConvertToDecimal(this string str)
            => str.ConvertToNullableDecimal() ?? 0;
        #endregion

        /// <summary>
        /// 转换字符串为Datetime类型
        /// </summary>
        public static DateTime ConvertToDatetime(this string str)
            => Convert.ToDateTime(str);
        /// <summary>
        /// 转换字符串为Datetime类型（失败时返回默认值）
        /// </summary>
        public static DateTime ConvertToDatetime(this string str, DateTime defaultValue)
            => str.ConvertToNullableDatetime() ?? defaultValue;
        /// <summary>
        /// 转换字符串为可空Datetime类型，转换失败返回Null
        /// </summary>
        public static DateTime? ConvertToNullableDatetime(this string str)
        {
            if (str.IsNullOrWhiteSpace())
                return null;
            try
            {
                return Convert.ToDateTime(str);
            }
            catch
            {
                return null;
            }
        }

        ///// <summary>
        ///// 转换网页地址为Uri
        ///// </summary>
        //public Uri Uri => new Uri(Str);
        /// <summary>
        /// 转换网页地址为Uri(失败返回null)
        /// </summary>
        public static Uri ConvertToUri(this string str)
        {
            try
            {
                return new Uri(str);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将字符串转换为Boolean类型（当字符串是“true/1/yes/checked/是/对”(不区分大小写)时返回true）
        /// </summary>
        public static bool ConvertToBoolean(this string str)
            => str.IsEqualIgnoreCase(new string[] { "1", "true", "yes", "checked", "是", "对" });
        /// <summary>
        /// 将字符串转换为可空Boolean类型（当字符串中包含“true/1/yes/checked/是/对”时返回true，为空时返回null）
        /// </summary>
        public static bool? ConvertToNullableBoolean(this string str)
        {
            if (str.IsNullOrWhiteSpace())
                return null;
            return str.ConvertToBoolean();
        }


        /// <summary>
        /// 将单个Cookie字符串转换为Cookie类型
        /// </summary>
        public static Cookie ConvertToCookie(this string str)
        {
            if (str.IsNullOrWhiteSpace())
                return null;
            var index = str.IndexOf('=');
            return new Cookie(str.SubString(0, index).Trim(), str.SubString(++index, int.MaxValue));
        }
        /// <summary>
        /// 将Cookies字符串转换为CookieCollection集合
        /// </summary>
        public static CookieCollection ConvertToCookieCollection(this string str)
        {
            var result = new CookieCollection();
            foreach (var cookie in str?.SplitBySign(";") ?? new string[0])
                result.Add(cookie.ConvertToCookie());
            return result;
        }
    }
}
