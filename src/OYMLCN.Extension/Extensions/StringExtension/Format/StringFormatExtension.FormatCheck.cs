/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringFormatExtension.FormatCheck.cs
Author: VicBilibily
Description: 
    本代码所在文件夹的文件主要定义一些常用的字符串格式相关方法扩展，以提升开发效率为目的。
    本文件主要定义一些字符串内容格式检测和转换的扩展方法。
*****************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OYMLCN.Extensions
{
    public static partial class StringFormatExtension
    {
        /// <summary>
        ///   验证居民身份证号码（支持：GB11643-1989、GB11643-1999）
        /// </summary>
        /// <param name="value">要验证的字符串</param>
        /// <returns>
        ///   <para>如果符合 GB11643-1989、GB11643-1999 任一标准则为
        ///   <see langword="true"/>，否则为 <see langword="false"/>。</para>
        ///   <para>如果 <paramref name="value"/> 为
        ///   <see langword="null"/>、空字符串 ("") 或是仅由空白字符组成，则始终返回
        ///   <see langword="false"/>。</para>
        /// </returns>
        public static bool FormatIsChineseIDCard(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            string address = "11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,51,52,53,54,50,61,62,63,64,65,71,81,82";
            long n;

            if (value.Length == 15)
            {
                if (long.TryParse(value, out n) == false || n < Math.Pow(10, 14))
                    return false; // 数字验证
                if (address.IndexOf(value.Remove(2), StringComparison.Ordinal) == -1)
                    return false; // 省份验证
                if (!DateTime.TryParse(value.Substring(6, 6).Insert(4, "-").Insert(2, "-"), out _))
                    return false; // 生日验证  
                return true; // 符合GB11643-1989标准
            }

            if (value.Length == 18)
            {
                if (long.TryParse(value.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(value.Replace('x', '0').Replace('X', '0'), out n) == false)
                    return false; // 数字验证  
                if (address.IndexOf(value.Remove(2), StringComparison.Ordinal) == -1)
                    return false; // 省份验证  
                if (!DateTime.TryParse(value.Substring(6, 8).Insert(6, "-").Insert(4, "-"), out _))
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
        /// <summary>
        ///   身份证号码从15位升位到18位。
        /// </summary>
        /// <param name="input">要升位的 15 位身份证号码</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="input"/> 不能为 <see langword="null"/>。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="input"/> 不是长度为 15 位的字符串。
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="input"/> 不是有效的 15 位身份证号码.
        /// </exception>
        /// <returns>
        ///   从 15 位有效身份证升位的 18 位身份证号码。
        /// </returns>
        public static string ChineseIDCard15UpTo18(this string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input), $"{nameof(input)} 不能为 null");
            if (input.Length != 15)
                throw new ArgumentException(nameof(input), $"{nameof(input)} 不是长度为 15 位的字符串");
            if (!FormatIsChineseIDCard(input))
                throw new ArgumentException($"{nameof(input)} 不是有效的 15 位身份证号码", nameof(input));

            int iS = 0;
            //加权因子常数
            int[] iW = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
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

        /// <summary>
        ///   验证字符串是否是电子邮箱地址格式。
        /// </summary>
        /// <param name="value">要验证的字符串</param>
        /// <returns> 
        ///   <para>如果 <paramref name="value"/> 是有效的电子邮箱地址，则为
        ///   <see langword="true"/>，否则为 <see langword="false"/>。</para>
        ///   <para>如果 <paramref name="value"/> 为
        ///   <see langword="null"/>、空字符串 ("") 或是仅由空白字符组成，则始终返回
        ///   <see langword="false"/>。</para>
        /// </returns>
        public static bool FormatIsEmailAddress(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            return new Regex(@"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?").IsMatch(value);
        }
        /// <summary>
        ///   使用当前字符串创建 <see cref="Uri"/> 实例以验证当前字符串是有效网址。
        /// </summary>
        /// <param name="value">要验证的字符串</param>
        /// <returns> 
        ///   <para>如果 <paramref name="value"/> 是有效网址，则为
        ///   <see langword="true"/>，否则为 <see langword="false"/>。</para>
        ///   <para>如果 <paramref name="value"/> 为
        ///   <see langword="null"/>、空字符串 ("") 或是仅由空白字符组成，则始终返回
        ///   <see langword="false"/>。</para>
        /// </returns>
        public static bool FormatIsUrl(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            if (Uri.TryCreate(value, UriKind.Absolute, out Uri uri))
                return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
            return false;
        }

        /// <summary>
        ///   验证字符串是否是电话号码格式。
        /// </summary>
        /// <param name="value">要验证的字符串</param>
        /// <returns> 
        ///   <para>如果 <paramref name="value"/> 是电话号码格式，则返回
        ///   <see langword="true"/>，否则为 <see langword="false"/>。</para>
        ///   <para>如果 <paramref name="value"/> 为
        ///   <see langword="null"/>、空字符串 ("") 或是仅由空白字符组成，则始终返回
        ///   <see langword="false"/>。</para>
        /// </returns>
        public static bool FormatIsTelephone(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            return Regex.IsMatch(value, @"^(\d{3,4}-)?\d{6,8}$");
        }

        /// <summary>
        ///   验证字符串是否是手机号码且与中国手机号段匹配（包括物联网号段）
        ///   <para>号段参照 https://baike.baidu.com/item/手机号码/1417348 界定（最近核对于2020-11-25）</para>
        /// </summary>
        /// <param name="value">要验证的字符串</param>
        /// <returns>
        ///   <para>如果 <paramref name="value"/> 是由 11 位数值组成，且与中国手机号段匹配，则返回
        ///   <see langword="true"/>，否则为 <see langword="false"/>。</para>
        ///   <para>如果 <paramref name="value"/> 为
        ///   <see langword="null"/>、空字符串 ("") 或是仅由空白字符组成，则始终返回
        ///   <see langword="false"/>。</para>
        /// </returns>
        public static bool FormatIsMobilePhone(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            if (new Regex(@"^1[0-9]{10}$").IsMatch(value.Trim()))
                return _mobilePhoneNumberPrefix.Value.Any(phone => value.StartsWith(phone));
            else return false;
        }
        private static Lazy<string[]> _mobilePhoneNumberPrefix = new Lazy<string[]>(() =>
        {
            // 参照 https://baike.baidu.com/item/手机号码/1417348
            string[] chinaTelecomPrefix = "133、149、153、173、177、180、181、189、190、191、193、199".Split('、');
            string[] chinaUnicomPrefix = "130、131、132、145、155、156、166、167、171、175、176、185、186、196".Split('、');
            string[] chinaMobilePrefix = "134(0-8)、135、136、137、138、139、1440、147、148、150、151、152、157、158、159、172、178、182、183、184、187、188、195、197、198".Split('、');
            string[] chinaBroadcstPrefix = { "192" };
            string[] chinaTelecomVirtualPrefix = "1700、1701、1702、162".Split('、');
            string[] chinaUnicomVirtualPrefix = "1704、1707、1708、1709、171、167".Split('、');
            string[] chinaMobileVirtualPrefix = "1703、1705、1706、165".Split('、');
            string[] satelliteCommunicationPrefix = { "1349", "174" };
            string[] iotPrefix = "140、141、144、146、148".Split('、');

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
            prefix.AddRange(iotPrefix); // 物联网号段
            return prefix.Distinct().ToArray();
        });
        /// <summary>
        ///   验证字符串是否是邮政编码
        /// </summary>
        /// <param name="value">要验证的字符串</param>
        /// <returns>
        ///   <para>如果 <paramref name="value"/> 是由 6 位数值组成，则返回
        ///   <see langword="true"/>，否则为 <see langword="false"/>。</para>
        ///   <para>如果 <paramref name="value"/> 为
        ///   <see langword="null"/>、空字符串 ("") 或是仅由空白字符组成，则始终返回
        ///   <see langword="false"/>。</para>
        /// </returns>  
        public static bool FormatIsPostalCode(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            return Regex.IsMatch(value, @"^\d{6}$");
        }
        /// <summary>
        ///   验证字符串是否包含汉字。
        /// </summary>
        /// <param name="value">要验证的字符串</param>
        /// <returns> 
        ///   <para>如果 <paramref name="value"/> 包含汉字，则返回
        ///   <see langword="true"/>，否则为 <see langword="false"/>。</para>
        ///   <para>如果 <paramref name="value"/> 为
        ///   <see langword="null"/>、空字符串 ("") 或是仅由空白字符组成，则始终返回
        ///   <see langword="false"/>。</para>
        /// </returns>  
        public static bool FormatHasChineseCharacters(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return false;
            return Regex.IsMatch(value, @"[\u4e00-\u9fa5]+");
        }

    }
}
