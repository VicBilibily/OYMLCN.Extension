using System;
using System.IO;
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
        #region Unicode
        /// <summary>
        /// 普通字符串转Unicode字符串
        /// </summary>
        public static string ToUnicode(this string str)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            return stringBuilder.ToString();
        }
        /// <summary>
        /// Unicode字符串转普通字符串
        /// </summary>
        public static string UnicodeToString(this string str)
            => new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled)
                .Replace(str, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        #endregion

        #region 字符串格式判断
        /// <summary>
        /// 验证居民身份证号码
        /// 验证支持：GB11643-1989、GB11643-1999
        /// </summary>
        public static bool FormatIsChineseIDCard(this string str)
        {
            if (str.IsNullOrEmpty()) return false;

            string address = "11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,51,52,53,54,50,61,62,63,64,65,71,81,82";
            long n = 0;

            if (str.Length == 15)
            {
                if (long.TryParse(str, out n) == false || n < Math.Pow(10, 14))
                    return false;//数字验证
                if (address.IndexOf(str.Remove(2)) == -1)
                    return false;//省份验证
                if (str.Substring(6, 6).Insert(4, "-").Insert(2, "-").ConvertToNullableDatetime().IsNull())
                    return false;//生日验证  
                return true;//符合GB11643-1989标准
            }
            else if (str.Length == 18)
            {
                if (long.TryParse(str.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(str.Replace('x', '0').Replace('X', '0'), out n) == false)
                    return false;//数字验证  
                if (address.IndexOf(str.Remove(2)) == -1)
                    return false;//省份验证  
                if (str.Substring(6, 8).Insert(6, "-").Insert(4, "-").ConvertToNullableDatetime().IsNull())
                    return false;//生日验证  
                string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
                string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
                char[] Ai = str.Remove(17).ToCharArray();
                int sum = 0;
                for (int i = 0; i < 17; i++)
                    sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
                if (arrVarifyCode[sum % 11] != str.Substring(17, 1).ToLower())
                    return false;//校验码验证
                return true;//符合GB11643-1999标准
            }
            return false;
            //throw new FormatException("身份证长度不正确或暂未支持该身份证验证");
        }
        /// <summary>
        /// 判断字符串是否是邮箱地址
        /// </summary>
        public static bool FormatIsEmailAddress(this string str)
            => str.IsNullOrEmpty() ? false : new Regex(@"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?").IsMatch(str.Trim());
        /// <summary>
        /// 判断字符串是否是手机号码
        /// </summary>
        public static bool FormatIsMobilePhoneNumber(this string str)
            => str.IsNullOrEmpty() ? false : new Regex(@"^1[0-9]{10}$").IsMatch(str.Trim());
        /// <summary>
        /// 判断字符是不是汉字
        /// </summary>
        public static bool FormatIsChineseRegString(this string str)
            => Regex.IsMatch(str, @"[\u4e00-\u9fbb]+$");
        #endregion

        #region 字符串按格式屏蔽
        /// <summary>
        /// 获取匹配字符数组
        /// </summary>
        /// <param name="length"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private static string GetSymbolCharacter(int length, char symbol = '*')
        {
            var chars = new char[length];
            for (var i = 0; i < length; i++)
                chars[i] = symbol;
            return chars.ConvertToString();
        }

        /// <summary>
        /// 按要求屏蔽手机号的中间部分，如果不是手机号则返回指定位数的有效字符，超出部分字符屏蔽处理
        /// </summary>
        /// <param name="str">手机号码</param>
        /// <param name="length">中间屏蔽位数，只允许取值4/6/8，其他值无效，默认为4</param>
        /// <param name="symbolLength">屏蔽区域*号支付数量</param>
        /// <returns></returns>
        public static string FormatAsSecretPhoneNumber(this string str, int length = 4, int symbolLength = 4)
        {
            //const string length4Pattern = "(\\d{3})\\d{4}(\\d{4})";
            //const string length6Pattern = "(\\d{3})\\d{6}(\\d{2})";
            //const string length8Pattern = "(\\d{2})\\d{8}(\\d{1})";
            if (str.FormatIsMobilePhoneNumber())
            {
                switch (length)
                {
                    case 4: return str.FormatAsSecretSymbolString(3, 4, symbolLength);
                    case 6: return str.FormatAsSecretSymbolString(3, 2, symbolLength);
                    case 8: return str.FormatAsSecretSymbolString(2, 1, symbolLength);
                }
                //var regex = new Regex(pattern);
                //return regex.Replace(str, $"$1{GetSymbolCharacter(symbolLength)}$2");
            }
            //else
            return str.FormatAsSecretSymbolStartString(length, symbolLength);
        }

        /// <summary>
        /// 保留指定长度的字符，其余均屏蔽（如果字符串长度小于指定长度，则保留源字符串的一半长度）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length">开头字符长度</param>
        /// <param name="symbolLength">屏蔽字符补位长度</param>
        /// <param name="symbol">屏蔽区域字符</param>
        /// <returns></returns>
        public static string FormatAsSecretSymbolStartString(this string str, int length, int symbolLength = 4, char symbol = '*')
            => str.IsNullOrWhiteSpace() ? str : str.Take(str.Length > length ? length : str.Length / 2).ToArray().ConvertToString() + GetSymbolCharacter(symbolLength, symbol);
        /// <summary>
        /// 保留指定长度的字符，其余均屏蔽
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length">结束字符长度</param>
        /// <param name="symbolLength">屏蔽字符补位长度</param>
        /// <param name="symbol">屏蔽区域字符</param>
        /// <returns></returns>
        public static string FormatAsSecretSymbolEndString(this string str, int length, int symbolLength = 4, char symbol = '*')
        {
            if (str.IsNullOrWhiteSpace()) return str;

            var symbolStr = GetSymbolCharacter(symbolLength, symbol);
            var chars = str.ToCharArray();
            if (chars.Length > length)
                return symbolStr + str.Skip(chars.Length - length).ToArray().ConvertToString();
            else
                return symbolStr + str;
        }

        /// <summary>
        /// 按要求屏蔽字符串的指定长度的中间部分，保留前后字符，字符长度不足则全部屏蔽
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length">替换位数，这里替代中间指定位</param>
        /// <param name="symbol">替换字符，默认为*号</param>
        /// <returns></returns>
        public static string FormatAsSecretSymbolString(this string str, int length, char symbol = '*')
        {
            if (str.IsNullOrWhiteSpace()) return str;
            int startIndex = str.Length / 2 - length / 2;
            var chars = str.ToCharArray();
            for (int i = 0; i < str.Length; i++)
                if (i >= startIndex - 1 && i < startIndex + length)
                    chars[i] = symbol;
            return chars.ConvertToString();
        }
        /// <summary>
        /// 按要求屏蔽字符串，保留指定长度的前后字符,其余屏蔽。若字符长度比保留量少，则忽略后字符，优先保留前字符，其余屏蔽字符填充。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start">开始保留位数</param>
        /// <param name="end">后方保留位数</param>
        /// <param name="symbolLength">占位字符补位长度</param>
        /// <param name="symbol">占位字符，默认为*号</param>
        /// <returns></returns>
        public static string FormatAsSecretSymbolString(this string str, int start, int end, int symbolLength = 4, char symbol = '*')
        {
            if (str.IsNullOrWhiteSpace()) return str;
            var chars = str.ToCharArray();
            var startC = str.Take(start).ToArray().ConvertToString();
            var endC = string.Empty;
            if (chars.Length >= start + end)
                endC = chars.Skip(chars.Length - end).ToArray().ConvertToString();
            return startC + GetSymbolCharacter(symbolLength, symbol) + endC;
        }
        #endregion

        #region 字符串数字的判断
        /// <summary>
        /// 判断文本是否为数字
        /// </summary>
        public static bool FormatIsNumeric(this string str)
            => str.IsNullOrEmpty() ? false : Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$", RegexOptions.Compiled);
        /// <summary>
        /// 判断文本是否为整数
        /// </summary>
        public static bool FormatIsInteger(this string str)
            => str.IsNullOrEmpty() ? false : Regex.IsMatch(str, @"^[+-]?\d*$", RegexOptions.Compiled);
        /// <summary>
        /// 判断文本是否为正数
        /// </summary>
        public static bool FormatIsUnsignNumeric(this string str)
            => str.IsNullOrEmpty() ? false : Regex.IsMatch(str, @"^\d*[.]?\d*$", RegexOptions.Compiled);
        #endregion

        #region 字符串数字的转换
        /// <summary>
        /// 获取文本中的数字（包括小数）
        /// </summary>
        public static string FormatAsNumeric(this string str)
            => str.IsNullOrEmpty() ? null : Regex.Match(str, @"[+-]?\d+(\.\d+)?", RegexOptions.Compiled).Value;
        /// <summary>
        /// 获取文本中的整数部分
        /// </summary>
        public static string FormatAsIntegerNumeric(this string str)
            => str.FormatAsNumeric()?.SplitThenGetFirst(".");
        #endregion

        #region 编码转换
        /// <summary>
        /// 转化为半角字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToDBC(this string str)
        {
            var c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                // 全角空格为12288，半角空格为32
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                // 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// 转化为全角字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToSBC(this string str)
        {
            var c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] > 32 && c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// ASCII转小写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToLowerForASCII(this string value)
        {
            if (value.IsNullOrWhiteSpace())
                return value;
            var sb = new StringBuilder(value.Length);
            foreach (var c in value)
                if (c < 'A' || c > 'Z')
                    sb.Append(c);
                else
                    sb.Append((char)(c + 0x20));
            return sb.ToString();
        }
        /// <summary>
        /// ASCII转大写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns> 
        public static string ToUpperForASCII(this string value)
        {
            if (value.IsNullOrWhiteSpace())
                return value;
            var sb = new StringBuilder(value.Length);
            foreach (var c in value)
                if (c < 'a' || c > 'z')
                    sb.Append(c);
                else
                    sb.Append((char)(c - 0x20));
            return sb.ToString();
        }
        #endregion

        #region Url相关
        /// <summary>
        /// 将URL转义为合法参数地址
        /// </summary>
        public static string UrlEncode(this string url)
            => WebUtility.UrlEncode(url);
        /// <summary>
        /// 被转义的URL字符串还原
        /// </summary>
        public static string UrlDecode(this string url)
            => WebUtility.UrlDecode(url);
        /// <summary>
        /// 将 URL 中的参数名称/值编码为合法的格式。
        /// </summary>
        public static string UrlDataEncode(this string url)
            => Uri.EscapeDataString(url);
        /// <summary>
        /// 获取url字符串的的协议域名地址
        /// </summary>
        public static string UrlHost(this string url)
            => url.ConvertToUri().GetHost();
        #endregion


        /// <summary>
        /// 根据占位符紧接多个字符串 即string.Format
        /// </summary>
        /// <param name="str"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string StringFormat(this string str, params string[] param)
            => string.Format(str, param);
        /// <summary>
        /// 分割字符串未每一个单字
        /// </summary>
        /// <param name="str">待分割字符串</param>
        /// <returns></returns>
        public static string[] StringToArray(this string str)
            => str?.Select(x => x.ToString()).ToArray() ?? new string[0];

        /// <summary>
        /// 将Boolean转换为Yes是或No否
        /// </summary>
        /// <param name="boolean"></param>
        /// <param name="cnString">是否返回中文是/否</param>
        /// <returns></returns>
        public static string ToYesOrNo(this bool boolean, bool cnString = true)
            => cnString ? boolean ? "是" : "否" : boolean ? "Yes" : "No";


        /// <summary>
        /// 将字符串填充到Steam中
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoder">默认使用UTF-8进行编码</param>
        /// <returns></returns>
        public static Stream StringToStream(this string str, Encoding encoder = null)
            => new MemoryStream(str.StringToBytes(encoder));
        /// <summary>
        /// 将字符串填充到byte[]字节流中
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoder">默认使用UTF-8进行编码</param>
        /// <returns></returns>
        public static byte[] StringToBytes(this string str, Encoding encoder = null)
            => encoder?.GetBytes(str) ?? Encoding.UTF8.GetBytes(str);

        /// <summary>
        /// 16进制字符串转换
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] HexToBytes(this string hex)
        {
            if (hex.Length == 0)
                return new byte[] { 0 };
            if (hex.Length % 2 == 1)
                hex = "0" + hex;
            byte[] result = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length / 2; i++)
                result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            return result;
        }

        /// <summary>
        /// 16进制字符串转换
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                sb.Append(bytes[i].ToString("X2"));
            return sb.ToString();
        }



    }
}
