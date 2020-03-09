using OYMLCN.ArgumentChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 格式相关扩展
    /// </summary>
    public static partial class StringFormatExtension
    {
        #region public static bool FormatIsChineseIDCard(this string value)
        /// <summary>
        /// 验证居民身份证号码
        /// <para> 验证支持标准：GB11643-1989、GB11643-1999 </para>
        /// </summary>
        /// <param name="value"> 要验证的字符串 </param>
        /// <returns> 验证结果，如果符合标准则为 true，否则为 false </returns>
        public static bool FormatIsChineseIDCard(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;

            string address = "11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,51,52,53,54,50,61,62,63,64,65,71,81,82";
            long n;

            if (value.Length == 15)
            {
                if (long.TryParse(value, out n) == false || n < Math.Pow(10, 14))
                    return false; // 数字验证
                if (address.IndexOf(value.Remove(2), StringComparison.Ordinal) == -1)
                    return false; // 省份验证
                if (value.Substring(6, 6).Insert(4, "-").Insert(2, "-").ConvertToNullableDatetime().IsNull())
                    return false; // 生日验证  
                return true; // 符合GB11643-1989标准
            }
            else if (value.Length == 18)
            {
                if (long.TryParse(value.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(value.Replace('x', '0').Replace('X', '0'), out n) == false)
                    return false; // 数字验证  
                if (address.IndexOf(value.Remove(2), StringComparison.Ordinal) == -1)
                    return false; // 省份验证  
                if (value.Substring(6, 8).Insert(6, "-").Insert(4, "-").ConvertToNullableDatetime().IsNull())
                    return false; // 生日验证  
                string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
                string[] wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
                char[] ai = value.Remove(17).ToCharArray();
                int sum = 0;
                for (int i = 0; i < 17; i++)
                    sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
                if (arrVarifyCode[sum % 11] != value.Substring(17, 1).ToLower())
                    return false; // 校验码验证
                return true; // 符合GB11643-1999标准
            }
            return false;
            //throw new FormatException("身份证长度不正确或暂未支持该身份证验证");
        }
#if Xunit
        [Fact]
        public static void FormatIsChineseIDCardTest()
        {
            string str = null;
            Assert.False(str.FormatIsChineseIDCard());

            str = "320311770706001";
            Assert.True(str.FormatIsChineseIDCard());
            str = "3203117707060001";
            Assert.False(str.FormatIsChineseIDCard());

            // 测试值均为模拟值
            str = "440102198001021230";
            Assert.True(str.FormatIsChineseIDCard());
            str = "440102198001011230";
            Assert.False(str.FormatIsChineseIDCard());
        }
#endif
        #endregion

        #region public static string ChineseIDCard15UpTo18(this string input)
        /// <summary>
        /// 身份证号码从15位升位到18位
        /// </summary>
        /// <param name="input"> 要升位的 15 位身份证号码 </param>
        /// <returns> 从 15 位有效身份证升位的 18 位身份证号码 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="input"/> 不是长度为 15 位的字符串 </exception>
        /// <exception cref="ArgumentException"> <paramref name="input"/> 不是有效的 15 位身份证号码 </exception>
        public static string ChineseIDCard15UpTo18(this string input)
        {
            input.ThrowIfNull(nameof(input));
            if (input.Length != 15)
                throw new ArgumentException(nameof(input), $"{nameof(input)} 不是长度为 15 位的字符串");
            if (!input.FormatIsChineseIDCard())
                throw new ArgumentException($"{nameof(input)} 不是有效的 15 位身份证号码", nameof(input));

            int iS = 0;
            //加权因子常数
            int[] iW = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            //校验码常数
            string LastCode = "10X98765432";
            //新身份证号
            string perIDNew;
            perIDNew = input.Substring(0, 6);
            //填在第6位及第7位上填上‘1’，‘9’两个数字
            perIDNew += "19";
            perIDNew += input.Substring(6, 9);
            //进行加权求和
            for (int i = 0; i < 17; i++)
                iS += int.Parse(perIDNew.Substring(i, 1)) * iW[i];
            //取模运算，得到模值
            int iY = iS % 11;
            //从LastCode中取得以模为索引号的值，加到身份证的最后一位，即为新身份证号。
            perIDNew += LastCode.Substring(iY, 1);
            return perIDNew;
        }
#if Xunit
        [Fact]
        public static void ChineseIDCard15UpTo18Test()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.ChineseIDCard15UpTo18());

            // 测试值均为模拟值
            str = "3203117707060001";
            Assert.Throws<ArgumentException>(() => str.ChineseIDCard15UpTo18());

            str = "380311770706001";
            Assert.Throws<ArgumentException>(() => str.ChineseIDCard15UpTo18());

            str = "320311770706001";
            Assert.Equal(18, str.ChineseIDCard15UpTo18().Length);
        }
#endif
        #endregion


        #region public static bool FormatIsEmailAddress(this string value)
        /// <summary>
        /// 验证字符串是否是电子邮箱地址格式
        /// </summary>
        /// <param name="value"> 要验证的字符串 </param>
        /// <returns> 如果 <paramref name="value"/> 是有效的电子邮箱地址，则为 true，否则为 false </returns>
        public static bool FormatIsEmailAddress(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return new Regex(@"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?").IsMatch(value);
        }

#if Xunit
        [Fact]
        public static void FormatIsEmailAddressTest()
        {
            string str = null;
            Assert.False(str.FormatIsEmailAddress());
            str = " ";
            Assert.False(str.FormatIsEmailAddress());
            str = "qq.com";
            Assert.False(str.FormatIsEmailAddress());
            str = "10000@qq.com";
            Assert.True(str.FormatIsEmailAddress());
            str = "www.测试@域名.中国";
            Assert.True(str.FormatIsEmailAddress());
        }
#endif
        #endregion


        #region public static bool FormatIsUrl(this string value)
        /// <summary>
        /// 判断字符串是否是网址
        /// <para> 通过尝试用字符串构造一个 URI 来指示字符串是否为格式良好的，并确保字符串不需要进一步转义 </para>
        /// </summary>
        /// <param name="value"> 要验证的字符串 </param>
        /// <returns> 如果 <paramref name="value"/> 是有效网址或相对地址，则为 true，否则为 false </returns>
        public static bool FormatIsUrl(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            if (Uri.TryCreate(value, UriKind.Absolute, out Uri uri))
                return uri.Scheme.Equals(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
            return false;
        }
#if Xunit
        [Fact]
        public static void FormatIsUrlTest()
        {
            string str = null;
            Assert.False(str.FormatIsUrl());
            str = " ";
            Assert.False(str.FormatIsUrl());
            str = "Test.aspx";
            Assert.False(str.FormatIsUrl());
            str = "/MyResource/Test.aspx";
            Assert.False(str.FormatIsUrl());

            str = "http://www.qq.com/MyResource/Test.aspx";
            Assert.True(str.FormatIsUrl());
            str = "https://www.qq.com/Test.aspx";
            Assert.True(str.FormatIsUrl());
        }
#endif
        #endregion


        #region public static string[] GetIPAddresses(this string value)
        /// <summary>
        /// 获取字符串表示的对应合法的 IPv4 或 IPv6 地址
        /// </summary>
        /// <param name="value"> 要获取信息的 IP 地址或访问地址 </param>
        /// <returns> 如果能获得 IP 地址，则为对应的所有 IP，否则为空序列 </returns>
        public static string[] GetIPAddresses(this string value)
        {
            value.ThrowIfNull(nameof(value));

            string[] ipArray = new string[0];
            if (value.IsNullOrWhiteSpace()) return ipArray;

            string ipAddress = null;
            UriHostNameType hostType = Uri.CheckHostName(value);
            if (hostType == UriHostNameType.Unknown)
            {
                if (Uri.TryCreate(value, UriKind.Absolute, out Uri url) && url.Host.IsNotNullOrEmpty())
                    ipAddress = IPAddressTryParse(url.Host);
                else if (Uri.TryCreate(string.Format("http://{0}", value), UriKind.Absolute, out url))
                    ipAddress = IPAddressTryParse(url.Host);
                if (ipAddress.IsNullOrEmpty() && url.Host.IsNotNullOrEmpty())
                    ipArray = Dns.GetHostAddresses(url.Host).Select(ip => ip.ToString()).WhereIsNotNullOrEmpty().ToArray();
            }
            else if (hostType == UriHostNameType.IPv4 || hostType == UriHostNameType.IPv6)
                ipAddress = IPAddressTryParse(value);

            if (ipArray.IsEmpty()) return new[] { ipAddress };
            else return ipArray;
        }
        private static string IPAddressTryParse(string value)
        {
            string ipAddress = string.Empty;
            IPAddress _ipAdr;
            if (IPAddress.TryParse(value, out _ipAdr))
                ipAddress = _ipAdr.ToString();
            return ipAddress;
        }
#if Xunit
        [Fact]
        public static void GetIPAddressesTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.GetIPAddresses());

            str = string.Empty;
            Assert.Empty(str.GetIPAddresses());

            str = "192.168.1.1:8060";
            Assert.Equal(new[] { "192.168.1.1" }, str.GetIPAddresses());
            str = "http://192.168.1.1:8060/test.aspx";
            Assert.Equal(new[] { "192.168.1.1" }, str.GetIPAddresses());

            str = "2001:0DB8:02de::0e13";
            Assert.Equal(new[] { "2001:db8:2de::e13" }, str.GetIPAddresses());
            str = "[2001:0DB8:02de::0e13]:9010";
            Assert.Equal(new[] { "2001:db8:2de::e13" }, str.GetIPAddresses());

            str = "http://localhost/test.aspx";
            Assert.Equal(new[] { "::1", "127.0.0.1" }, str.GetIPAddresses());
        }
#endif
        #endregion


        #region public static bool FormatIsIPAddress(this string value)
        /// <summary>
        /// 验证字符串是否是有效的 IP 地址
        /// <para> 如果是 IPv6 地址带有端口号，默认为有效 IP 并返回 true，但 IPv4 地址带端口会返回 false </para>
        /// </summary>
        /// <param name="value"> 要验证的 IP 地址 </param>
        /// <returns> 如果 <paramref name="value"/> 是否是有效的 IP 地址则为true，否则为false </returns>
        public static bool FormatIsIPAddress(this string value)
            => IPAddress.TryParse(value, out _);
#if Xunit
        [Fact]
        public static void FormatIsIPAddressTest()
        {
            string str = null;
            Assert.False(str.FormatIsIPAddress());

            str = "localhost";
            Assert.False(str.FormatIsIPAddress());

            str = "192.168.1.1";
            Assert.True(str.FormatIsIPAddress());
            str = "192.168.1.1:8060";
            Assert.False(str.FormatIsIPAddress());

            str = "2001:0DB8:02de::0e13";
            Assert.True(str.FormatIsIPAddress());
            str = "[2001:0DB8:02de::0e13]:9010";
            Assert.True(str.FormatIsIPAddress());
        }
#endif
        #endregion

        #region public static bool FormatIsIPv4(this string value)
        /// <summary>
        /// 验证字符串是否是有效的 IPv4 地址
        /// </summary>
        /// <param name="value"> 要验证的主机名或 IP 地址 </param>
        /// <returns> 如果 <paramref name="value"/> 是有效的 IPv4 地址则为true，否则为false </returns>
        public static bool FormatIsIPv4(this string value)
            => Uri.CheckHostName(value) == UriHostNameType.IPv4;
#if Xunit
        [Fact]
        public static void FormatIsIPv4Test()
        {
            string str = null;
            Assert.False(str.FormatIsIPv4());

            str = "localhost";
            Assert.False(str.FormatIsIPv4());

            str = "192.168.1.1";
            Assert.True(str.FormatIsIPv4());
            str = "192.168.1.1:8060";
            Assert.False(str.FormatIsIPv4());

            str = "2001:0DB8:02de::0e13";
            Assert.False(str.FormatIsIPv4());
            str = "[2001:0DB8:02de::0e13]:9010";
            Assert.False(str.FormatIsIPv4());
        }
#endif
        #endregion

        #region public static bool FormatIsIPv6(this string value)
        /// <summary>
        /// 验证字符串是否是有效的 IPv6 地址
        /// </summary>
        /// <param name="value"> 要验证的主机名或 IP 地址 </param>
        /// <returns> 如果 <paramref name="value"/> 是有效的 IPv6 地址则为true，否则为false </returns>
        public static bool FormatIsIPv6(this string value)
            => Uri.CheckHostName(value) == UriHostNameType.IPv6;
#if Xunit
        [Fact]
        public static void FormatIsIPv6Test()
        {
            string str = null;
            Assert.False(str.FormatIsIPv6());

            str = "localhost";
            Assert.False(str.FormatIsIPv6());

            str = "192.168.1.1";
            Assert.False(str.FormatIsIPv6());
            str = "192.168.1.1:8060";
            Assert.False(str.FormatIsIPv6());

            str = "2001:0DB8:02de::0e13";
            Assert.True(str.FormatIsIPv6());
            str = "[2001:0DB8:02de::0e13]:9010";
            Assert.False(str.FormatIsIPv6());
        }
#endif
        #endregion


        #region public static bool FormatIsTelephone(this string value)
        /// <summary>
        /// 验证字符串是否是电话号码格式
        /// </summary>
        /// <param name="value"> 要验证的字符串 </param>
        /// <returns> 如果 <paramref name="value"/> 是否是电话号码格式，则返回 true，否则为 false </returns>
        public static bool FormatIsTelephone(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, @"^(\d{3,4}-)?\d{6,8}$");
        }
#if Xunit
        [Fact]
        public static void FormatIsTelephoneTest()
        {
            string str = null;
            Assert.False(str.FormatIsTelephone());
            str = "13800138000";
            Assert.False(str.FormatIsTelephone());
            str = "010-10086";
            Assert.False(str.FormatIsTelephone());
            str = "020-88668866";
            Assert.True(str.FormatIsTelephone());
        }
#endif
        #endregion

        #region public static bool FormatIsPostalCode(this string value)
        /// <summary>
        /// 验证字符串是否是邮政编码
        /// </summary>
        /// <param name="value"> 要验证的字符串 </param>
        /// <returns> 如果 <paramref name="value"/> 是由 6 位数值组成，则返回 true，否则为 false </returns>
        public static bool FormatIsPostalCode(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, @"^\d{6}$");
        }
#if Xunit
        [Fact]
        public static void FormatIsPostalCodeTest()
        {
            string str = null;
            Assert.False(str.FormatIsPostalCode());
            Assert.False(" ".FormatIsPostalCode());
            Assert.False("abc123".FormatIsPostalCode());
            Assert.True("102200".FormatIsPostalCode());
        }
#endif
        #endregion

        #region public static bool FormatIsMobilePhone(this string value)
        /// <summary>
        /// 验证字符串是否是手机号码且与中国手机号段匹配
        /// <para> 号段参照 https://baike.baidu.com/item/手机号码/1417348 界定（最近核对于2020-02）  </para>
        /// </summary>
        /// <param name="value"> 要验证的字符串 </param>
        /// <returns> 如果 <paramref name="value"/> 是由 11 位数值组成且与中国手机号段匹配，则返回 true，否则为 false </returns>
        public static bool FormatIsMobilePhone(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            if (new Regex(@"^1[0-9]{10}$").IsMatch(value.Trim()))
            {
                if (_mobilePhoneNumberPrefix == null)
                {
                    string[] chinaTelecomPrefix = "133、149、153、173、177、180、181、189、191、193、199、190".Split('、');
                    string[] chinaUnicomPrefix = "130、131、132、145、155、156、166、171、175、176、185、186、196".Split('、');
                    string[] chinaMobilePrefix = "134(0-8)、135、136、137、138、139、147、150、151、152、157、158、159、172、178、182、183、184、187、188、195、198、197".Split('、');
                    string[] chinaBroadcstPrefix = new[] { "192" };
                    string[] chinaTelecomVirtualPrefix = "1700、1701、1702、162".Split('、');
                    string[] chinaUnicomVirtualPrefix = "1704、1707、1708、1709、171、167".Split('、');
                    string[] chinaMobileVirtualPrefix = "1703、1705、1706、165".Split('、');
                    string[] satelliteCommunicationPrefix = new[] { "1349" };

                    var prefix = new List<string>();
                    prefix.AddRange(chinaTelecomPrefix); // 中国电信号段
                    prefix.AddRange(chinaUnicomPrefix); // 中国联通号段
                    // 中国移动 134 号段特殊处理
                    for (var i = 0; i <= 8; i++) prefix.Add($"134{i.ToString()}");
                    prefix.AddRange(chinaMobilePrefix.Skip(1)); // 中国移动号段
                    prefix.AddRange(chinaBroadcstPrefix); // 中国广电号段
                    prefix.AddRange(chinaTelecomVirtualPrefix); // 中国电信虚拟运营商号段
                    prefix.AddRange(chinaUnicomVirtualPrefix); // 中国联通虚拟运营商号段
                    prefix.AddRange(chinaMobileVirtualPrefix); // 中国移动虚拟运营商号段
                    prefix.AddRange(satelliteCommunicationPrefix); // 卫星通信号段
                    _mobilePhoneNumberPrefix = prefix.ToArray();
                }
            }
            else return false;
            return value.StartsWith(_mobilePhoneNumberPrefix);
        }
        private static string[] _mobilePhoneNumberPrefix;
#if Xunit
        [Fact]
        public static void FormatIsMobilePhoneNumberTest()
        {
            string str = null;
            Assert.False(str.FormatIsMobilePhone());
            Assert.False("14123456789".FormatIsMobilePhone());
            Assert.True("13800138000".FormatIsMobilePhone());
        }
#endif
        #endregion

        #region public static bool FormatHasChineseCharacters(this string value)
        /// <summary>
        /// 验证字符是否包含汉字
        /// </summary>
        /// <param name="value"> 要验证的字符串 </param>
        /// <returns> 如果 <paramref name="value"/> 包含汉字，则返回 true，否则为 false </returns>
        public static bool FormatHasChineseCharacters(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, @"[\u4e00-\u9fa5]+");
        }
#if Xunit
        [Fact]
        public static void FormatHasChineseCharactersTest()
        {
            string str = null;
            Assert.False(str.FormatHasChineseCharacters());
            str = "Hello World！";
            Assert.False(str.FormatHasChineseCharacters());
            str = "你好，世界！";
            Assert.True(str.FormatHasChineseCharacters());
        }
#endif 
        #endregion

    }
}
