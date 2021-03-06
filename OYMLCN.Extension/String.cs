using OYMLCN.Handlers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        /// 将字符串作为HTML格式文本处理
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HtmlFormatHandler AsHtmlFormat(this string html)
            => html.IsNotNullOrWhiteSpace() ? new HtmlFormatHandler(html) : null;

        #region Is对比判断
        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
            => string.IsNullOrEmpty(str);
        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string str)
            => !IsNullOrEmpty(str);

        /// <summary>
        /// 判断字符串是否为空/空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
            => string.IsNullOrWhiteSpace(str);
        /// <summary>
        /// 判断字符串是否为空/空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNullOrWhiteSpace(this string str)
            => !IsNullOrWhiteSpace(str);

        /// <summary>
        /// 对比两个字符串是否相等
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static bool IsEqual(this string str, string value, StringComparison comparison = StringComparison.Ordinal)
            => (str.IsNull() || value.IsNull()) ? false : str.Equals(value, comparison);
        /// <summary>
        /// 对比字符串是否与列出的相等
        /// </summary>
        /// <param name="str"></param>
        /// <param name="values"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static bool IsEqual(this string str, string[] values, StringComparison comparison = StringComparison.Ordinal)
        {
            foreach (var value in values)
                if (str.Equals(value, comparison))
                    return true;
            return false;
        }
        /// <summary>
        /// 对比两个字符串是否相等（忽略大小写）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEqualIgnoreCase(this string str, string value)
            => str.IsEqual(value, StringComparison.OrdinalIgnoreCase);
        /// <summary>
        /// 对比字符串是否与列出的相等（忽略大小写）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool IsEqualIgnoreCase(this string str, string[] values)
            => str.IsEqual(values, StringComparison.OrdinalIgnoreCase);
        #endregion

        #region 截取字符串
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public static string Sub(this string str, int startIndex, int endIndex)
            => str.Substring(startIndex, endIndex - startIndex);
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="skipLength"></param>
        /// <param name="subLength"></param>
        /// <returns></returns>
        public static string SubString(this string str, int skipLength, int subLength = int.MaxValue)
            => new string(str.Skip(skipLength).Take(subLength).ToArray());
        #endregion

        /// <summary>
        /// 拼接字符串（有分隔符）
        /// 常用于 class 样式属性的拼接
        /// </summary>
        /// <param name="source">原属性</param>
        /// <param name="value">新增属性</param>
        /// <param name="splitKey">分隔符（默认为空格）</param>
        /// <param name="onEnd">新增属性位置</param>
        /// <returns></returns>
        public static string AppendWith(this string source, string value, string splitKey = " ", bool onEnd = true)
        {
            var val = value.SplitBySign(splitKey);
            var vals = source.ToString().SplitBySign(splitKey);
            var attrs = new List<string>();
            if (onEnd)
            {
                attrs.AddRange(val);
                attrs.AddRange(vals);
            }
            else
            {
                attrs.AddRange(vals);
                attrs.AddRange(val);
            }
            return attrs.Distinct().Join(splitKey);
        }
       

        #region 字符串分割
        /// <summary>
        /// 根据标志分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sign">分割标识</param>
        /// <param name="option">分割结果去重方式</param>
        /// <returns></returns>
        public static string[] SplitBySign(this string str, string sign, StringSplitOptions option = StringSplitOptions.None)
            => str?.Split(new string[] { sign }, option) ?? new string[0];
        /// <summary>
        /// 根据标志分割字符串(不包含空字符串)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static string[] SplitBySignWithoutEmpty(this string str, string sign)
            => str.SplitBySign(sign, StringSplitOptions.RemoveEmptyEntries);
        /// <summary>
        /// 根据标志分割字符串后获得第一个字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sign"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static string SplitThenGetFirst(this string str, string sign, StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries)
            => str?.SplitBySign(sign, option).FirstOrDefault();
        /// <summary>
        /// 根据标志分割字符串后获得最后一个字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sign">分割标志</param>
        /// <param name="option">分割结果去重方式</param>
        /// <returns></returns>
        public static string SplitThenGetLast(this string str, string sign, StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries)
            => str?.SplitBySign(sign, option).LastOrDefault();

        /// <summary>
        /// 根据标志分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="signs">分割标识，多重标识</param>
        /// <returns></returns>
        public static string[] SplitByMultiSign(this string str, params string[] signs)
            => str?.Split(signs, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
        /// <summary>
        /// 根据 | \ / 、 ， , 空格 中文空格 制表符空格换行 分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="option">分割结果去重方式</param>
        /// <returns></returns>
        public static string[] SplitAuto(this string str, StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries)
            => str.SplitByMultiSign("|", "\\", "/", "、", ":", "：", "，", ",", "　", " ", "\t");
        /// <summary>
        /// 根据换行符拆分字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] SplitByLine(this string str)
            => str.SplitByMultiSign("\r\n", "\r", "\n");
        #endregion

        /// <summary>
        /// 去除过多的换行符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="keep">文本中保留的换行符最大数量（文本末尾的换行将不会保留）</param>
        /// <returns></returns>
        public static string RemoveWrap(this string text, byte keep = 0)
        {
            string[] param = new string[] { "\r\n", "\r", "\n" };
            if (keep == 0)
                return text.ReplaceNormalWithRegex(string.Empty, param);

            var stop = "._RWHS_.";
            var word = text.ReplaceNormalWithRegex(stop, param);
            string wrap = string.Empty;
            for (var i = 0; i < keep; i++)
                wrap += "\r\n";
            return word.SplitBySign(stop, StringSplitOptions.RemoveEmptyEntries).Join(wrap);
        }

       
        /// <summary>
        /// 正则匹配所有结果
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string[] RegexMatches(this string str, string pattern, RegexOptions options = RegexOptions.None)
        {
            var result = new List<string>();
            if (!str.IsNullOrEmpty())
            {
                var data = Regex.Matches(str, pattern, options);
                foreach (var item in data)
                    result.Add(item.ToString());
            }
            return result.ToArray();
        }

        /// <summary>
        /// 替换字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="newValue">新值</param>
        /// <param name="oldValue">旧值</param>
        /// <returns></returns>
        public static string ReplaceNormal(this string str, string newValue, params string[] oldValue)
        {
            if (newValue.IsNull())
                newValue = string.Empty;

            foreach (var item in oldValue)
            {
                if (str.IsNullOrEmpty())
                    return str;
                if (!item.IsNullOrEmpty())
                    str = str.Replace(item, newValue);
            }
            return str;
        }
        /// <summary>
        /// 使用正则匹配替换字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="newValue">新值</param>
        /// <param name="oldValue">旧值(不能包含正则占位符)</param>
        /// <returns></returns>
        public static string ReplaceNormalWithRegex(this string str, string newValue, params string[] oldValue)
            => oldValue.Length == 0 ? str : Regex.Replace(str, $"({oldValue.Join("|")})", newValue);
        /// <summary>
        /// 使用正则匹配替换字符串（忽略大小写）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="newValue">新值</param>
        /// <param name="oldValue">旧值(不能包含正则占位符)</param>
        public static string ReplaceIgnoreCaseWithRegex(this string str, string newValue, params string[] oldValue)
            => oldValue.Length == 0 ? str : Regex.Replace(str, $"({oldValue.Join("|")})", newValue, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        /// <summary>
        /// 移除字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="word">需要移除的字符</param>
        /// <returns></returns>
        public static string RemoveNormal(this string str, params string[] word)
            => ReplaceNormal(str, string.Empty, word);

        /// <summary>
        /// 使用正则匹配移除字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="word">需要移除的字符(不能包含正则占位符)</param>
        /// <returns></returns>
        public static string RemoveNormalWithRegex(this string str, params string[] word)
            => word.Length == 0 ? str : str.RegexMatches($"[^({word.Join("|")})]").Join();
        /// <summary>
        /// 使用正则匹配移除字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="word">需要移除的字符(不能包含正则占位符)</param>
        /// <returns></returns>
        public static string RemoveIgnoreCaseWithRegex(this string str, params string[] word)
            => word.Length == 0 ? str : str.RegexMatches($"[^({word.Join("|")})]", RegexOptions.Compiled | RegexOptions.IgnoreCase).Join();

        /// <summary>
        /// 将所有换行及其前后多余的空格替换掉合并为一行
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string AllInOneLine(this string str)
            => str.RemoveNormalWithRegex("\r\n", "\r", "\n");

        #region IsBegin/FinishWith Char
        /// <summary>
        /// 判断字符串是否以指定字符开头
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsBeginWith(this string s, char c)
            => s.IsNullOrEmpty() ? false : s[0] == c;
        /// <summary>
        /// 判断字符串是否以指定字符开头
        /// </summary>
        /// <param name="s"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static bool IsBeginWithAny(this string s, IEnumerable<char> chars)
            => s.IsNullOrEmpty() ? false : chars.Contains(s[0]);
        /// <summary>
        /// 判断字符串是否以指定字符开头
        /// </summary>
        /// <param name="s"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static bool IsBeginWithAny(this string s, params char[] chars)
            => s.IsBeginWithAny(chars.AsEnumerable());
        /// <summary>
        /// 判断字符串是否以指定字符开头
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static bool IsBeginWith(this string a, string b, StringComparison comparisonType = StringComparison.Ordinal)
            => (a.IsNull() || b.IsNull()) ? false : a.StartsWith(b, comparisonType);
        /// <summary>
        /// 判断字符串是否以指定字符开头
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static bool IsBeginWith(this string a, string b, bool ignoreCase, CultureInfo culture)
            => (a.IsNull() || b.IsNull()) ? false : a.StartsWith(b, ignoreCase, culture);
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
        #endregion

        /// <summary>
        /// 返回一个值，该值指示指定的子串是否出现在此字符串‘,中。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public static bool Contains(this string str, params string[] words)
        {
            var contain = false;
            foreach (var word in words)
                if (contain = str.Contains(word))
                    break;
            return contain;
        }

        #region TrimPuntuation
        static char[] Puntuation = new char[] {
            '~', '～', '-', '—', '－', '–', '^', '*',
            ',', '，', '.', '。', '?', '？', ':', '：', ';', '；',
            '[', '【', '{', ']', '】', '}', '|', '丨', '/', '\\',
            '(', '（', ')', '）', '<', '《', '>', '》',
            '·', '`', '\'', '"'
        };
        /// <summary>
        /// 去除文本开头的标点及标识符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimStartPuntuation(this string str)
            => str?.TrimStart(Puntuation).Trim();
        /// <summary>
        /// 去除文本结尾的标点及标识符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimEndPuntuation(this string str)
            => str?.TrimEnd(Puntuation).Trim();
        /// <summary>
        /// 去除文本两段的标点及标识符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimPuntuation(this string str)
            => str?.Trim(Puntuation).Trim();
        #endregion

        #region 简繁体转换
#if NET461
        /// <summary>
        /// 中文字符工具类
        /// </summary>
        private const int LOCALE_SYSTEM_DEFAULT = 0x0800;
        private const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
        private const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;
        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);

        /// <summary>
        /// 将字符转换成简体中文
        /// </summary>
        /// <param name="source">输入要转换的字符串</param>
        /// <returns>转换完成后的字符串</returns>
        public static string ToSimplifiedChinese(this string source)
        {
            string target = new string(' ', source.Length);
            int ret = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_SIMPLIFIED_CHINESE, source, source.Length, target, source.Length);
            return target;
        }

        /// <summary>
        /// 将字符转换为繁体中文
        /// </summary>
        /// <param name="source">输入要转换的字符串</param>
        /// <returns>转换完成后的字符串</returns>
        public static string ToTraditionalChinese(this string source)
        {
            string target = new string(' ', source.Length);
            int ret = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_TRADITIONAL_CHINESE, source, source.Length, target, source.Length);
            return target;
        }
#endif
        #endregion

    }
}
