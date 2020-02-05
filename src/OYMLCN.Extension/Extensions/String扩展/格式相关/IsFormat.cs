using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 格式相关扩展
    /// </summary>
    public static partial class StringFormatExtension
    {
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
        /// 身份证号码从15位升位到18位
        /// </summary>
        /// <param name="perIDSrc"></param>
        /// <returns></returns>
        public static string ChineseIDCard15UpTo18(this string perIDSrc)
        {
            int iS = 0;

            //加权因子常数
            int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //校验码常数
            string LastCode = "10X98765432";
            //新身份证号
            string perIDNew;
            perIDNew = perIDSrc.Substring(0, 6);
            //填在第6位及第7位上填上‘1’，‘9’两个数字
            perIDNew += "19";
            perIDNew += perIDSrc.Substring(6, 9);
            //进行加权求和
            for (int i = 0; i < 17; i++)
                iS += int.Parse(perIDNew.Substring(i, 1)) * iW[i];
            //取模运算，得到模值
            int iY = iS % 11;
            //从LastCode中取得以模为索引号的值，加到身份证的最后一位，即为新身份证号。
            perIDNew += LastCode.Substring(iY, 1);
            return perIDNew;
        }
        /// <summary>
        /// 判断字符串是否是邮箱地址
        /// </summary>
        public static bool FormatIsEmailAddress(this string str)
            => str.IsNullOrEmpty() ? false : new Regex(@"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?").IsMatch(str.Trim());
        /// <summary>
        /// 判断字符串是否是网址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool FormatIsUrl(this string str)
            => str.IsNullOrEmpty() ? false : Uri.TryCreate(str, UriKind.RelativeOrAbsolute, out _);

        private static string IPAddressTryParse(string ip)
        {
            string _ipAddress = string.Empty;
            IPAddress _ipAdr;
            if (IPAddress.TryParse(ip, out _ipAdr))
                _ipAddress = _ipAdr.ToString();
            return _ipAddress;
        }
        /// <summary>
        /// 判断是否是合法的IP4,IP6地址
        /// </summary>
        /// <param name="ip">需要判断的字符串</param>
        /// <returns>合法则返回host部分，若不合法则返回空</returns>
        public static string GetIPAddress(this string ip)
        {
            string _ipAddress = string.Empty;
            if (!string.IsNullOrEmpty(ip))
            {
                UriHostNameType _hostType = Uri.CheckHostName(ip);
                if (_hostType == UriHostNameType.Unknown)//譬如 "192.168.1.1:8060"或者[2001:0DB8:02de::0e13]:9010
                {
                    if (Uri.TryCreate(string.Format("http://{0}", ip), UriKind.Absolute, out Uri _url))
                        _ipAddress = IPAddressTryParse(_url.Host);
                }
                else if (_hostType == UriHostNameType.IPv4 || _hostType == UriHostNameType.IPv6)
                    _ipAddress = IPAddressTryParse(ip);
            }
            return _ipAddress;
        }

        /// <summary>
        /// 判断字符串是否是IP地址
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool FormatIsIPAddress(this string str)
            => IPAddress.TryParse(str, out IPAddress address);
        /// <summary>
        /// 判断字符串是否是IP地址(IPv4)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool FormatIsIPv4(this string str)
            => Uri.CheckHostName(str) == UriHostNameType.IPv4;
        /// <summary>
        /// 判断字符串是否是IP地址(IPv4)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool FormatIsIPv6(this string str)
            => Uri.CheckHostName(str) == UriHostNameType.IPv6;

        /// <summary>
        /// 判断字符串是否是电话号码格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool FormatIsTelephone(this string str)
            => str.IsNullOrEmpty() ? false : Regex.IsMatch(str, @"^(\d{3,4}-)?\d{6,8}$");
        /// <summary>
        /// 判断字符串是否是邮政编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool FormatIsPostalCode(this string str)
            => str.IsNullOrEmpty() ? false : Regex.IsMatch(str, @"^\d{6}$");
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

    }
}
