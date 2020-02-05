using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OYMLCN.Extensions
{
    /// <summary>
    /// 格式相关扩展
    /// </summary>
    public static partial class StringFormatExtension
    {
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

    }
}
