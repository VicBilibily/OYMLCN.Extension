using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// StringExtension
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// string类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T Convert2<T>(this string input)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                    return (T)converter.ConvertFromString(input);
                return default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
        /// <summary>
        /// string类型转换
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Convert2(this string input, Type type)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(type);
                if (converter != null)
                    return converter.ConvertFromString(input);
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region 数字的转换
        /// <summary>
        /// 转换数字字符串为可空SByte类型
        /// </summary>
        public static sbyte? ConvertToNullableSByte(this string str)
        {
            try { return System.Convert.ToSByte(str); }
            catch
            {
                str = str.FormatAsIntegerNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return System.Convert.ToSByte(str);
            }
        }

        /// <summary>
        /// 转换数字字符串为可空Byte类型
        /// </summary>
        public static byte? ConvertToNullableByte(this string str)
        {
            try { return System.Convert.ToByte(str); }
            catch
            {
                str = str.FormatAsIntegerNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return System.Convert.ToByte(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Int16类型
        /// </summary>
        public static short? ConvertToNullableShort(this string str)
        {
            try { return System.Convert.ToInt16(str); }
            catch
            {
                str = str.FormatAsIntegerNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return System.Convert.ToInt16(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Int32类型
        /// </summary>
        public static int? ConvertToNullableInt(this string str)
        {
            try { return System.Convert.ToInt32(str); }
            catch
            {
                str = str.FormatAsIntegerNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return System.Convert.ToInt32(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Int64类型
        /// </summary>
        public static long? ConvertToNullableLong(this string str)
        {
            try { return System.Convert.ToInt64(str); }
            catch
            {
                str = str.FormatAsIntegerNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return System.Convert.ToInt64(str);
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
            try { return System.Convert.ToSingle(str); }
            catch
            {
                str = str.FormatAsNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return System.Convert.ToSingle(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Double类型
        /// </summary>
        public static double? ConvertToNullableDouble(this string str)
        {
            try { return System.Convert.ToDouble(str); }
            catch
            {
                str = str.FormatAsNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return System.Convert.ToDouble(str);
            }
        }
        /// <summary>
        /// 转换字符串为可空Decimal类型
        /// </summary>
        public static decimal? ConvertToNullableDecimal(this string str)
        {
            try { return System.Convert.ToDecimal(str); }
            catch
            {
                str = str.FormatAsNumeric();
                if (str.IsNullOrWhiteSpace())
                    return null;
                return System.Convert.ToDecimal(str);
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
        /// 将字符数组转换拼接为字符串
        /// </summary>
        public static string ConvertToString(this char[] chars) => new string(chars);
        /// <summary>
        /// 获取指定字符串中的所有字符编码为UTF8的字节序列
        /// </summary>
        public static byte[] GetUTF8Bytes(this string str)
            => Encoding.UTF8.GetBytes(str);
        /// <summary>
        /// 将Byte[]转换为Base64字符串
        /// </summary>
        public static string ConvertToBase64String(this byte[] bytes)
            => System.Convert.ToBase64String(bytes);
        /// <summary>
        /// 将Base64转换为Byte[]
        /// </summary>
        public static byte[] ConvertFromBase64String(this string base64)
            => System.Convert.FromBase64String(base64);

        /// <summary>
        /// 将Byte[]转换为UTF8字符串
        /// </summary>
        public static string GetUTF8String(this byte[] bytes)
            => Encoding.UTF8.GetString(bytes);
        /// <summary>
        /// 将Base64转换为UTF8字符串
        /// </summary>
        public static string EncodingUTF8FromBase64String(this string base64)
            => base64.ConvertFromBase64String().GetUTF8String();

        /// <summary>
        /// 原明文字符串转成二进制字符串
        /// </summary>
        public static string StringToBitString(this string str)
        {
            byte[] data = Encoding.Unicode.GetBytes(str);
            StringBuilder result = new StringBuilder(data.Length * 8);
            foreach (byte b in data)
                result.Append(System.Convert.ToString(b, 2).PadLeft(8, '0'));
            return result.ToString();
        }
        /// <summary>
        /// 原二进制字符串转成明文字符串
        /// </summary>
        public static string BitStringToString(this string str)
        {
            CaptureCollection cs = Regex.Match(str, @"([01]{8})+").Groups[1].Captures;
            byte[] data = new byte[cs.Count];
            for (int i = 0; i < cs.Count; i++)
                data[i] = System.Convert.ToByte(cs[i].Value, 2);
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }


        /// <summary>
        /// 转换字符串为Datetime类型
        /// </summary>
        public static DateTime ConvertToDatetime(this string str)
            => System.Convert.ToDateTime(str);
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
                return System.Convert.ToDateTime(str);
            }
            catch
            {
                return null;
            }
        }

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

        /// <summary>
        /// 把以英文逗号(,)分割的ID字符串提取为ID数组
        /// </summary>
        public static T[] ConvertToIdArray<T>(this string idStr)
        {
            if (string.IsNullOrEmpty(idStr)) return new T[0];
            string[] array = idStr.Split(new char[] { ',' });
            List<T> list = new List<T>();
            Type typeFromHandle = typeof(T);
            foreach (string value in array)
            {
                try
                {
                    list.Add((T)System.Convert.ChangeType(value, typeFromHandle));
                }
                catch (InvalidCastException)
                {
                }
                catch (FormatException)
                {
                }
            }
            return list.ToArray();
        }
    }
}
