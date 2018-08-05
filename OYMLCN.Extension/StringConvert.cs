using System;
using System.IO;
using System.Net;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// StringExtension
    /// </summary>
    public static partial class StringExtensions
    {
        #region 数字的转换操作
        /// <summary>
        /// 判断文本是否为数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string value) =>
            Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$", RegexOptions.Compiled);
        /// <summary>
        /// 判断文本是否为整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInteger(this string value) =>
            Regex.IsMatch(value, @"^[+-]?\d*$", RegexOptions.Compiled);
        /// <summary>
        /// 判断文本是否为正数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUnsignNumeric(this string value) =>
            Regex.IsMatch(value, @"^\d*[.]?\d*$", RegexOptions.Compiled);
        /// <summary>
        /// 获取文本中的数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToNumeric(this string str) => str.IsNullOrEmpty() ? null : Regex.Match(str, @"[+-]?\d+(\.\d+)?", RegexOptions.Compiled).Value;
        /// <summary>
        /// 获取文本中的数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToIntegerNumeric(this string str) => str?.ToNumeric()?.SplitThenGetFirst(".");

        /// <summary>
        /// 转换数字字符串为可空SByte类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static sbyte? ConvertToNullableSByte(this string str)
        {
            str = str.ToIntegerNumeric();
            if (str.IsNullOrEmpty())
                return null;
            return Convert.ToSByte(str);
        }

        /// <summary>
        /// 转换数字字符串为可空Byte类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte? ConvertToNullableByte(this string str)
        {
            str = str.ToIntegerNumeric();
            if (str.IsNullOrEmpty())
                return null;
            return Convert.ToByte(str);
        }
        /// <summary>
        /// 转换字符串为可空Int16类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static short? ConvertToNullableShort(this string str)
        {
            str = str.ToIntegerNumeric();
            if (str.IsNullOrEmpty())
                return null;
            return Convert.ToInt16(str);
        }
        /// <summary>
        /// 转换字符串为可空Int32类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int? ConvertToNullableInt(this string str)
        {
            str = str.ToIntegerNumeric();
            if (str.IsNullOrEmpty())
                return null;
            return Convert.ToInt32(str);
        }
        /// <summary>
        /// 转换字符串为可空Int64类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long? ConvertToNullableLong(this string str)
        {
            str = str.ToIntegerNumeric();
            if (str.IsNullOrEmpty())
                return null;
            return Convert.ToInt64(str);
        }
        /// <summary>
        /// 转换字符串为可空BigInteger类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static BigInteger? ConvertToNullableBigInteger(this string str)
        {
            str = str.ToIntegerNumeric();
            if (str.IsNullOrEmpty())
                return null;
            return BigInteger.Parse(str);
        }

        /// <summary>
        /// 转换字符串为可空Single/float类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float? ConvertToNullableFloat(this string str)
        {
            str = str.ToNumeric();
            if (str.IsNullOrEmpty())
                return null;
            return Convert.ToSingle(str);
        }
        /// <summary>
        /// 转换字符串为可空Double类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double? ConvertToNullableDouble(this string str)
        {
            str = str.ToNumeric();
            if (str.IsNullOrEmpty())
                return null;
            return Convert.ToDouble(str);
        }
        /// <summary>
        /// 转换字符串为可空Decimal类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal? ConvertToNullableDecimal(this string str)
        {
            str = str.ToNumeric();
            if (str.IsNullOrEmpty())
                return null;
            return Convert.ToDecimal(str);
        }



        /// <summary>
        /// 转换数字字符串为SByte类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static sbyte ConvertToSByte(this string str) => str.ConvertToNullableSByte() ?? 0;
        /// <summary>
        /// 转换数字字符串为Byte类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte ConvertToByte(this string str) => str.ConvertToNullableByte() ?? 0;
        /// <summary>
        /// 转换字符串为Int16类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static short ConvertToShort(this string str) => str.ConvertToNullableShort() ?? 0;
        /// <summary>
        /// 转换字符串为Int32类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ConvertToInt(this string str) => str.ConvertToNullableInt() ?? 0;
        /// <summary>
        /// 转换字符串为Int64类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ConvertToLong(this string str) => str.ConvertToNullableLong() ?? 0;
        /// <summary>
        /// 转换字符串为可空BigInteger类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static BigInteger ConvertToBigInteger(this string str) => str.ConvertToNullableBigInteger() ?? 0;

        /// <summary>
        /// 转换字符串为Single/float类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ConvertToFloat(this string str) => str.ConvertToNullableFloat() ?? 0;
        /// <summary>
        /// 转换字符串为Double类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ConvertToDouble(this string str) => str.ConvertToNullableDouble() ?? 0;
        /// <summary>
        /// 转换字符串为Decimal类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ConvertToDecimal(this string str) => str.ConvertToNullableDecimal() ?? 0;
        #endregion

        /// <summary>
        /// 转换字符串为Datetime类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ConvertToDatetime(this string str) => Convert.ToDateTime(str);
        /// <summary>
        /// 转换字符串为可空Datetime类型，转换失败返回Null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 转换网页地址为Uri
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Uri ToUri(this string str) => new Uri(str);
        /// <summary>
        /// 转换网页地址为Uri(失败返回null)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Uri ToNullableUri(this string str)

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
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ConvertToBoolean(this string str)
        {
            string[] yesValues = new string[] { "1", "true", "yes", "checked", "是", "对" };
            return str.IsEqualIgnoreCase(yesValues);
        }
        /// <summary>
        /// 将字符串转换为可空Boolean类型（当字符串中包含“是/对”时返回true）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool? ConvertToNullableBoolean(this string str)
        {
            if (str.IsNullOrEmpty())
                return null;
            return str.ConvertToBoolean();
        }

        /// <summary>
        /// 将单个Cookie字符串转换为Cookie类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Cookie ToCookie(this string str)
        {
            if (str.IsNullOrEmpty())
                return null;
            var index = str.IndexOf('=');
            return new Cookie(str.SubString(0, index).Trim(), str.SubString(++index, int.MaxValue));
        }
        /// <summary>
        /// 将Cookies字符串转换为CookieCollection集合
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static CookieCollection ToCookieCollection(this string str)
        {
            var result = new CookieCollection();
            foreach (var cookie in str?.SplitBySign(";") ?? new string[0])
                result.Add(cookie.ToCookie());
            return result;
        }

        /// <summary>
        /// 将字符串填充到Steam中
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoder">默认使用UTF-8进行编码</param>
        /// <returns></returns>
        public static Stream StringToStream(this string str, Encoding encoder = null) => new MemoryStream(str.StringToBytes(encoder));

        /// <summary>
        /// 将字符串填充到byte[]字节流中
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoder">默认使用UTF-8进行编码</param>
        /// <returns></returns>
        public static byte[] StringToBytes(this string str, Encoding encoder = null) => encoder?.GetBytes(str) ?? Encoding.UTF8.GetBytes(str);


    }
}
