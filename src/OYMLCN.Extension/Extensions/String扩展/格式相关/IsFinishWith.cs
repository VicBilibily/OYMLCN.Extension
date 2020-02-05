using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// 判断字符串是否以指定字符结尾
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsFinishWith(this string s, char c)
        {
            if (s.IsNullOrEmpty()) return false;
            return s.Last() == c;
        }
        /// <summary>
        /// 判断字符串是否以指定字符结尾
        /// </summary>
        /// <param name="s"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static bool IsFinishWithAny(this string s, IEnumerable<char> chars)
            => s.IsNullOrEmpty() ? false : chars.Contains(s.Last());
        /// <summary>
        /// 判断字符串是否以指定字符结尾
        /// </summary>
        /// <param name="s"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static bool IsFinishWithAny(this string s, params char[] chars)
            => s.IsFinishWithAny(chars.AsEnumerable());
        /// <summary>
        /// 判断字符串是否以指定字符结尾
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static bool IsFinishWith(this string a, string b, StringComparison comparisonType = StringComparison.Ordinal)
            => (a.IsNull() || b.IsNull()) ? false : a.EndsWith(b, comparisonType);
        /// <summary>
        /// 判断字符串是否以指定字符结尾
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static bool IsFinishWith(this string a, string b, bool ignoreCase, CultureInfo culture)
            => (a.IsNull() || b.IsNull()) ? false : a.EndsWith(b, ignoreCase, culture);

    }
}
