using System;
using System.Collections.Generic;
using System.Linq;
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
        /// 去除过多的换行符，仅保留一个换行
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveWrap(this string text)
        {
            string[] param = new string[] { "\r\n", "\r", "\n" };
            return text.ReplaceNormalWithRegex("\r\n", param);
        }
        /// <summary>
        /// 去掉字符串内的所有空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveAllBlank(this string str)
            => Regex.Replace(str, @"\s", "");


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

    }
}
