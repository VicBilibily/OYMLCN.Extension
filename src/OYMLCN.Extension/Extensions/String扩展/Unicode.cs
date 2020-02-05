using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 编码相关扩展
    /// </summary>
    public static partial class StringUnicodeExtension
    {
        #region public static string ConvertToString(this char[] chars)
        /// <summary>
        /// 将 Unicode 字符数组拼接为字符串
        /// </summary>
        /// <param name="chars"> Unicode 字符数组 </param>
        public static string ConvertToString(this char[] chars)
            => new string(chars);
#if Xunit
        [Fact]
        public static void CharArrayConvertToStringTest()
        {
            char[] chars = null;
            Assert.Equal(string.Empty, chars.ConvertToString());

            chars = new char[0];
            Assert.Equal(string.Empty, chars.ConvertToString());

            chars = new[] { 'Y', 'e', 's' };
            Assert.Equal("Yes", chars.ConvertToString());
        }
#endif 
        #endregion

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

        /// <summary>
        /// 原明文字符串转成二进制字符串
        /// </summary>
        public static string StringToBitString(this string str)
        {
            byte[] data = Encoding.Unicode.GetBytes(str);
            StringBuilder result = new StringBuilder(data.Length * 8);
            foreach (byte b in data)
                result.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
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
                data[i] = Convert.ToByte(cs[i].Value, 2);
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }


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


    }
}
