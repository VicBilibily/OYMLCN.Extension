using System;
using System.Text;
using System.Text.RegularExpressions;
using OYMLCN.ArgumentChecker;

#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 编码相关扩展
    /// </summary>
    public static class StringUnicodeExtension
    {
        #region public static string GetUnicodeString(this string value)
        /// <summary>
        /// 将可读字符串转换为 Unicode 编码表示的字符串
        /// </summary>
        /// <param name="value"> 要转换的字符串对象 </param>
        /// <returns> Unicode 编码表示的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        public static string GetUnicodeString(this string value)
        {
            value.ThrowIfNull(nameof(value));
            byte[] bytes = Encoding.Unicode.GetBytes(value);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            return stringBuilder.ToString();
        }
#if Xunit
        [Fact]
        public static void GetUnicodeStringTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.GetUnicodeString());

            str = string.Empty;
            Assert.Equal(string.Empty, str.GetUnicodeString());

            str = "Hello World!";
            Assert.Equal(@"\u0048\u0065\u006c\u006c\u006f\u0020\u0057\u006f\u0072\u006c\u0064\u0021", str.GetUnicodeString());
        }
#endif 
        #endregion

        #region public static string UnicodeToString(this string value)
        /// <summary>
        /// 将 Unicode 编码表示的字符串转换为可读字符串
        /// </summary>
        /// <param name="value"> 要转换的 Unicode 编码字符串对象或包含 Unicode 编码字符串对象 </param>
        /// <returns> 可读字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        public static string UnicodeToString(this string value)
        {
            value.ThrowIfNull(nameof(value));
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled)
                .Replace(value, x => Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)).ToString());
        }
#if Xunit
        [Fact]
        public static void UnicodeToStringTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.UnicodeToString());

            str = string.Empty;
            Assert.Equal(string.Empty, str.UnicodeToString());

            str = @"\u0048\u0065llo World\u0021";
            Assert.Equal("Hello World!", str.UnicodeToString());

            str = @"\u0048\u0065\u006c\u006c\u006f\u0020\u0057\u006f\u0072\u006c\u0064\u0021";
            Assert.Equal("Hello World!", str.UnicodeToString());
        }
#endif
        #endregion


        #region public static string GetBitString(this string value)
        /// <summary>
        /// 将可读字符串转成二进制数值表示的字符串
        /// </summary>
        /// <param name="value"> 要转换的可读字符串对象 </param>
        /// <returns> 二进制数值表示的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        public static string GetBitString(this string value)
        {
            value.ThrowIfNull(nameof(value));
            byte[] data = Encoding.Unicode.GetBytes(value);
            StringBuilder result = new StringBuilder(data.Length * 8);
            foreach (byte b in data)
                result.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            return result.ToString();
        }
#if Xunit
        [Fact]
        public static void GetBitStringTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.GetBitString());

            str = string.Empty;
            Assert.Equal(string.Empty, str.GetBitString());

            str = "Hello World!";
            string bit = "010010000000000001100101000000000110110000000000011011000000000001101111000000000010000000000000010101110000000001101111000000000111001000000000011011000000000001100100000000000010000100000000";
            Assert.Equal(bit, str.GetBitString());
        }
#endif
        #endregion

        #region public static string BitStringToString(this string value)
        /// <summary>
        /// 将二进制数值表示的字符串转成可读字符串
        /// </summary>
        /// <param name="value"> 要转换的二进制数值表示的字符串对象 </param>
        /// <returns> 可读字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        /// <exception cref="FormatException"> <paramref name="value"/> 不是由0/1组成的二进制字符串 </exception>
        /// <exception cref="FormatException"> <paramref name="value"/> 所表示的字符数量与转换结果不匹配 </exception>
        public static string BitStringToString(this string value)
        {
            value.ThrowIfNull(nameof(value));
            var cs = Regex.Match(value, @"([01]{8})+").Groups[1].Captures;
            if (value.Length != 0 && cs.Count == 0)
                throw new FormatException($"{nameof(value)} 不是由0/1组成的二进制字符串");
            if (Math.Ceiling(value.Length / 8M) != cs.Count)
                throw new FormatException($"{nameof(value)} 所表示的字符数量与转换结果不匹配");

            byte[] data = new byte[cs.Count];
            for (int i = 0; i < cs.Count; i++)
                data[i] = Convert.ToByte(cs[i].Value, 2);
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }
#if Xunit
        [Fact]
        public static void BitStringToStringTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.BitStringToString());

            str = string.Empty;
            Assert.Equal(string.Empty, str.BitStringToString());

            str = "Hello World!";
            Assert.Throws<FormatException>(() => str.BitStringToString());

            str = "0100100000";
            Assert.Throws<FormatException>(() => str.BitStringToString());

            str = "010010000000000001100101000000000110110000000000011011000000000001101111000000000010000000000000010101110000000001101111000000000111001000000000011011000000000001100100000000000010000100000000";
            string bit = "Hello World!";
            Assert.Equal(bit, str.BitStringToString());
        }
#endif
        #endregion


        #region public static string ToLowerASCII(this string value)
        /// <summary>
        /// 采用 ASCII 转换方式转换为小写形式的字符串
        /// </summary>
        /// <param name="value"> 要转换的字符串对象 </param>
        /// <returns> ASCII 转换后的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        public static string ToLowerASCII(this string value)
        {
            value.ThrowIfNull(nameof(value));
            if (value.IsEmpty()) return string.Empty;

            var sb = new StringBuilder(value.Length);
            foreach (var c in value)
                if (c < 'A' || c > 'Z')
                    sb.Append(c);
                else
                    sb.Append((char)(c + 0x20));
            return sb.ToString();
        }
#if Xunit
        [Fact]
        public static void ToLowerASCIITest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.ToLowerASCII());

            str = string.Empty;
            Assert.Equal(string.Empty, str.ToLowerASCII());

            str = "Hello World!";
            Assert.Equal("hello world!", str.ToLower());
        }
#endif
        #endregion

        #region public static string ToUpperASCII(this string value)
        /// <summary>
        /// 采用 ASCII 转换方式转换为大写形式的字符串
        /// </summary>
        /// <param name="value"> 要转换的字符串对象 </param>
        /// <returns> ASCII 转换后的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        public static string ToUpperASCII(this string value)
        {
            value.ThrowIfNull(nameof(value));
            if (value.IsEmpty()) return string.Empty;

            var sb = new StringBuilder(value.Length);
            foreach (var c in value)
                if (c < 'a' || c > 'z')
                    sb.Append(c);
                else
                    sb.Append((char)(c - 0x20));
            return sb.ToString();
        }
#if Xunit
        [Fact]
        public static void ToUpperASCIITest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.ToUpperASCII());

            str = string.Empty;
            Assert.Equal(string.Empty, str.ToUpperASCII());

            str = "Hello World!";
            Assert.Equal("HELLO WORLD!", str.ToUpperASCII());
        }
#endif
        #endregion


        #region public static string ToDBC(this string value)
        /// <summary>
        /// 将全角字符串转换为半角字符串
        /// </summary>
        /// <param name="value"> 包含全角字符的字符串 </param>
        /// <returns> 半角字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        public static string ToDBC(this string value)
        {
            value.ThrowIfNull(nameof(value));
            if (value.IsNullOrEmpty()) return string.Empty;

            var c = value.ToCharArray();
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
#if Xunit
        [Fact]
        public static void ToDBCTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.ToDBC());

            str = string.Empty;
            Assert.Equal(string.Empty, str.ToDBC());

            str = "　";
            Assert.Equal(" ", str.ToDBC());

            str = "Ｈｅｌｌｏ　Ｗｏｒｌｄ！";
            Assert.Equal("Hello World!", str.ToDBC());
        }
#endif
        #endregion

        #region public static string ToSBC(this string value)
        /// <summary>
        /// 将半角字符串转换为全角字符串
        /// </summary>
        /// <param name="value"> 包含半角字符的字符串 </param>
        /// <returns> 全角字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        public static string ToSBC(this string value)
        {
            value.ThrowIfNull(nameof(value));
            if (value.IsNullOrEmpty()) return string.Empty;

            var c = value.ToCharArray();
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
#if Xunit
        [Fact]
        public static void ToSBCTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.ToSBC());

            str = string.Empty;
            Assert.Equal(string.Empty, str.ToSBC());

            str = " ";
            Assert.Equal("　", str.ToSBC());

            str = "Hello World!";
            Assert.Equal("Ｈｅｌｌｏ　Ｗｏｒｌｄ！", str.ToSBC());
        }
#endif 
        #endregion


    }
}
