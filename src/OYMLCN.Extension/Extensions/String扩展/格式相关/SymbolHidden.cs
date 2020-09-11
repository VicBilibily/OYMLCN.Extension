using System;
using System.Linq;
using OYMLCN.ArgumentChecker;

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
        private static string GetSymbolCharacter(int length, char symbol = '*')
            => new char[length].Select(c => symbol).ToArray().ConvertToString();


        #region public static string FormatAsSecretSymbolString(this string input, int start, int end, int symbolLength = 4, char symbol = '*')
        /// <summary>
        /// 按要求屏蔽字符串，保留指定长度的前后字符,其余屏蔽。若字符长度比保留量少，则忽略后字符，优先保留前字符，其余屏蔽字符填充。
        /// </summary>
        /// <param name="input"> 原始字符串 </param>
        /// <param name="start"> 开始保留位数 </param>
        /// <param name="end"> 后方保留位数 </param>
        /// <param name="symbolLength"> 占位字符补位长度 </param>
        /// <param name="symbol"> 占位字符，默认为 * 号 </param>
        /// <returns>
        /// 一个经过屏蔽处理的字符串
        /// <para> 按照提供的参数，保留字符串开头 <paramref name="start"/> 位内容， </para>
        /// <para> 然后输出指定 <paramref name="symbolLength"/> 个 <paramref name="symbol"/> 字符， </para>
        /// <para> 最后保留结尾处 <paramref name="end"/> 位内容，如果结尾长度不足则不输出结尾字符。 </para>
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="start"/> 不能小于等于0 </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="end"/> 不能小于等于0 </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="symbolLength"/> 不能小于0 </exception>
        public static string FormatAsSecretSymbolString(this string input, int start, int end, int symbolLength = 4, char symbol = '*')
        {
            input.ThrowIfNull(nameof(input));
            start.ThrowIfNegativeOrZero(nameof(start));
            end.ThrowIfNegativeOrZero(nameof(end));
            symbolLength.ThrowIfNegative(nameof(symbolLength));

            if (input.IsNullOrWhiteSpace()) return input;
            if (symbolLength == 0) return input;

            var chars = input.ToCharArray();
            var startC = input.Take(start).ToArray().ConvertToString();
            var endC = string.Empty;
            if (chars.Length >= start + end)
                endC = chars.Skip(chars.Length - end).ToArray().ConvertToString();
            return startC + GetSymbolCharacter(symbolLength, symbol) + endC;
        }
#if Xunit
        [Fact]
        public static void FormatAsSecretSymbolStringStartEndTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.FormatAsSecretSymbolString(1, 1));

            str = string.Empty;
            Assert.Equal(string.Empty, str.FormatAsSecretSymbolString(1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => str.FormatAsSecretSymbolString(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => str.FormatAsSecretSymbolString(1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => str.FormatAsSecretSymbolString(1, 1, symbolLength: -1));

            str = " ";
            Assert.Equal(str, str.FormatAsSecretSymbolString(1, 1));

            str = "Hello World";
            Assert.Equal(str, str.FormatAsSecretSymbolString(3, 3, symbolLength: 0));
            Assert.Equal("Hel**rld", str.FormatAsSecretSymbolString(3, 3, symbolLength: 2));
        }
#endif
        #endregion

        #region public static string FormatAsSecretSymbolString(this string input, int length, char symbol = '*')
        /// <summary>
        /// 按要求屏蔽字符串的指定长度的中间部分，保留前后字符，字符长度不足则全部屏蔽
        /// </summary>
        /// <param name="input"> 原始字符串 </param>
        /// <param name="length"> 占位字符补位长度，这里替代中间指定字符数量 </param>
        /// <param name="symbol"> 占位字符，默认为 * 号 </param>
        /// <returns></returns>
        public static string FormatAsSecretSymbolString(this string input, int length, char symbol = '*')
        {
            input.ThrowIfNull(nameof(input));
            length.ThrowIfNegative(nameof(length));

            if (input.IsNullOrWhiteSpace()) return input;
            if (length == 0) return input;

            int startIndex = (input.Length - length) / 2;
            startIndex = startIndex < 0 ? 0 : startIndex;
            var chars = input.ToCharArray();
            for (int i = 0; i < input.Length; i++)
                if (i >= startIndex && i < startIndex + length)
                    chars[i] = symbol;
            return chars.ConvertToString();
        }
#if Xunit
        [Fact]
        public static void FormatAsSecretSymbolStringLengthTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.FormatAsSecretSymbolString(1));

            str = string.Empty;
            Assert.Throws<ArgumentOutOfRangeException>(() => str.FormatAsSecretSymbolString(-1));
            Assert.Equal(string.Empty, str.FormatAsSecretSymbolString(0));

            str = " ";
            Assert.Equal(str, str.FormatAsSecretSymbolString(0));

            str = "Hello World";
            Assert.Equal(str, str.FormatAsSecretSymbolString(0));
            Assert.Equal("Hell**World", str.FormatAsSecretSymbolString(2));
            Assert.Equal("Hell***orld", str.FormatAsSecretSymbolString(3));
            Assert.Equal("Hel****orld", str.FormatAsSecretSymbolString(4));
            Assert.Equal("Hel*****rld", str.FormatAsSecretSymbolString(5));
        }
#endif
        #endregion


        #region public static string FormatAsSecretSymbolStartString(this string input, int length, int symbolLength = 4, char symbol = '*')
        /// <summary>
        /// 保留指定长度的字符，其余均屏蔽（如果字符串长度小于指定长度，则保留源字符串的一半长度）
        /// </summary>
        /// <param name="input"> 原始字符串 </param>
        /// <param name="length"> 保留开头字符长度 </param>
        /// <param name="symbolLength"> 屏蔽字符补位长度 </param>
        /// <param name="symbol"> 屏蔽区域字符 </param>
        /// <returns> 已处理的字符串，保留指定长度的字符，然后跟上指定长度的屏蔽符号 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length"/> 不能为小于 0 </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="symbolLength"/> 不能为小于 0 </exception>
        public static string FormatAsSecretSymbolStartString(this string input, int length, int symbolLength = 4, char symbol = '*')
        {
            input.ThrowIfNull(nameof(input));
            length.ThrowIfNegative(nameof(length));
            symbolLength.ThrowIfNegative(nameof(symbolLength));
            if (input.IsNullOrWhiteSpace()) return input;

            return input.Take(input.Length > length ? length : input.Length / 2).ToArray().ConvertToString() +
                   GetSymbolCharacter(symbolLength, symbol);
        }
#if Xunit
        [Fact]
        public static void FormatAsSecretSymbolStartStringTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.FormatAsSecretSymbolStartString(1));

            str = string.Empty;
            Assert.Throws<ArgumentOutOfRangeException>(() => str.FormatAsSecretSymbolStartString(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => str.FormatAsSecretSymbolStartString(0, -1));
            Assert.Equal(string.Empty, str.FormatAsSecretSymbolStartString(0));

            str = " ";
            Assert.Equal(str, str.FormatAsSecretSymbolStartString(0));

            str = "Hello World";
            Assert.Equal("****", str.FormatAsSecretSymbolStartString(0));
            Assert.Equal("*****", str.FormatAsSecretSymbolStartString(0, 5));
            Assert.Equal("Hel****", str.FormatAsSecretSymbolStartString(3, 4));
            Assert.Equal("Hell*****", str.FormatAsSecretSymbolStartString(4, 5));
        }
#endif
        #endregion

        #region public static string FormatAsSecretSymbolEndString(this string input, int length, int symbolLength = 4, char symbol = '*')
        /// <summary>
        /// 保留在末尾指定长度的字符，前面的字符均屏蔽
        /// </summary>
        /// <param name="input"> 原始字符串 </param>
        /// <param name="length"> 结束字符长度 </param>
        /// <param name="symbolLength"> 屏蔽字符补位长度 </param>
        /// <param name="symbol"> 屏蔽区域字符 </param>
        /// <returns> 已处理的字符串，保留在末尾指定长度的字符，字符前面填充指定长度的屏蔽符 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length"/> 不能为小于 0 </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="symbolLength"/> 不能为小于 0 </exception>
        public static string FormatAsSecretSymbolEndString(this string input, int length, int symbolLength = 4, char symbol = '*')
        {
            input.ThrowIfNull(nameof(input));
            length.ThrowIfNegative(nameof(length));
            symbolLength.ThrowIfNegative(nameof(symbolLength));
            if (input.IsNullOrWhiteSpace()) return input;

            var symbolStr = GetSymbolCharacter(symbolLength, symbol);
            var chars = input.ToCharArray();
            if (chars.Length > length)
                return symbolStr + input.Skip(chars.Length - length).ToArray().ConvertToString();
            return symbolStr + input;
        }
#if Xunit
        [Fact]
        public static void FormatAsSecretSymbolEndStringTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.FormatAsSecretSymbolEndString(1));

            str = string.Empty;
            Assert.Throws<ArgumentOutOfRangeException>(() => str.FormatAsSecretSymbolEndString(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => str.FormatAsSecretSymbolEndString(0, -1));
            Assert.Equal(string.Empty, str.FormatAsSecretSymbolEndString(0));

            str = " ";
            Assert.Equal(str, str.FormatAsSecretSymbolEndString(0));

            str = "Hello World";
            Assert.Equal("****", str.FormatAsSecretSymbolEndString(0));
            Assert.Equal("*****", str.FormatAsSecretSymbolEndString(0, 5));
            Assert.Equal("****rld", str.FormatAsSecretSymbolEndString(3, 4));
            Assert.Equal("*****orld", str.FormatAsSecretSymbolEndString(4, 5));
        }
#endif
        #endregion


        #region public static string FormatAsSecretPhoneNumber(this string input, int length = 4, int symbolLength = 4)
        /// <summary>
        /// 按要求屏蔽手机号的中间部分，如果不是手机号则返回指定位数的有效字符，超出部分字符屏蔽处理
        /// </summary>
        /// <param name="input"> 手机号码 </param>
        /// <param name="length"> 中间屏蔽位数，只允许取值4/6/8，其他值无效，默认为4 </param>
        /// <param name="symbolLength"> 屏蔽区域*号支付数量 </param>
        /// <returns> 处理后的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length"/> 不能为小于 0 </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="symbolLength"/> 不能为小于等于 0 </exception>
        public static string FormatAsSecretPhoneNumber(this string input, int length = 4, int symbolLength = 4)
        {
            input.ThrowIfNull(nameof(input));
            length.ThrowIfNegative(nameof(length));
            symbolLength.ThrowIfNegativeOrZero(nameof(symbolLength));
            if (input.IsNullOrWhiteSpace()) return input;
            //const string length4Pattern = "(\\d{3})\\d{4}(\\d{4})";
            //const string length6Pattern = "(\\d{3})\\d{6}(\\d{2})";
            //const string length8Pattern = "(\\d{2})\\d{8}(\\d{1})";
            if (input.FormatIsMobilePhone())
            {
                switch (length)
                {
                    case 4: return input.FormatAsSecretSymbolString(3, 4, symbolLength);
                    case 6: return input.FormatAsSecretSymbolString(3, 2, symbolLength);
                    case 8: return input.FormatAsSecretSymbolString(2, 1, symbolLength);
                }
                //var regex = new Regex(pattern);
                //return regex.Replace(str, $"$1{GetSymbolCharacter(symbolLength)}$2");
            }
            //else
            return input.FormatAsSecretSymbolStartString(length, symbolLength);
        }
#if Xunit
        [Fact]
        public static void FormatAsSecretPhoneNumberTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.FormatAsSecretPhoneNumber(1));

            str = string.Empty;
            Assert.Throws<ArgumentOutOfRangeException>(() => str.FormatAsSecretPhoneNumber(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => str.FormatAsSecretPhoneNumber(symbolLength: 0));
            Assert.Equal(string.Empty, str.FormatAsSecretPhoneNumber(0));

            str = " ";
            Assert.Equal(str, str.FormatAsSecretPhoneNumber(0));

            str = "13912345678";
            Assert.Equal("139****", str.FormatAsSecretPhoneNumber(3));
            Assert.Equal("139****5678", str.FormatAsSecretPhoneNumber());
            Assert.Equal("13912****", str.FormatAsSecretPhoneNumber(5));
            Assert.Equal("139****78", str.FormatAsSecretPhoneNumber(6));
            Assert.Equal("1391234****", str.FormatAsSecretPhoneNumber(7));
            Assert.Equal("13****8", str.FormatAsSecretPhoneNumber(8));
            Assert.Equal("139123456****", str.FormatAsSecretPhoneNumber(9));

            str = "Hello World";
            Assert.Equal("****", str.FormatAsSecretPhoneNumber(0));
            Assert.Equal("*****", str.FormatAsSecretPhoneNumber(0, 5));
            Assert.Equal("Hel****", str.FormatAsSecretPhoneNumber(3, 4));
            Assert.Equal("Hell*****", str.FormatAsSecretPhoneNumber(4, 5));
        }
#endif 
        #endregion

    }
}