using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace OYMLCN
{
    /// <summary>
    /// HTMLExtension
    /// </summary>
    public static class HTMLExtensions
    {
        /// <summary>
        /// 使用正则表达式删除Html标签
        /// </summary>
        /// <param name="html"></param>
        /// <param name="length">截取长度（默认0则返回完整结果）</param>
        /// <returns></returns>
        public static string RemoveHtml(this string html, int length = 0)
        {
            if (html.IsNull())
                return string.Empty;
            string strText = Regex.Replace(html, "<[^>]+>", "");
            strText = Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
        /// <summary>
        /// 使用正则表达式删除Script标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveScript(this string html)
        {
            if (html.IsNull())
                return string.Empty;
            return Regex.Replace(html, @"(\<script(.+?)\</script\>)|(\<style(.+?)\</style\>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }
        /// <summary>
        /// 替换换行Br为换行符
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ReplaceHtmlBr(this string html)
        {
            if (html.IsNull())
                return string.Empty;
            return html.ReplaceIgnoreCaseWithRegex("\r\n", "<br>", "<br/>", "<br />");
        }


        /// <summary>
        /// 字符串去除 &amp;nbsp;/&amp;ensp;/&amp;emsp;/&amp;thinsp;/&amp;zwnj;/&amp;zwj; 空格符 制表符 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keepOneSpace">保留一个空格</param>
        /// <returns></returns>
        public static string RemoveSpace(this string html, bool keepOneSpace = true)
        {
            if (html.IsNull())
                return string.Empty;
            return html.SplitByMultiSign("　", " ", "&nbsp;", "&ensp;", "&emsp;", "&thinsp;", "&zwnj;", "&zwj;", "\t")
                       .Where(d => !d.Trim().IsNullOrEmpty())
                       .Join(keepOneSpace ? " " : string.Empty);
        }

        /// <summary>
        /// HTML转义为数据库合法模式
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string html) => WebUtility.HtmlEncode(html);
        /// <summary>
        /// 被转义HTML的字符串还原
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string html) => WebUtility.HtmlDecode(html);
        /// <summary>
        /// 将URL转义为合法参数地址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(this string url) => WebUtility.UrlEncode(url);
        /// <summary>
        /// 被转义的URL字符串还原
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDecode(this string url) => WebUtility.UrlDecode(url);
        /// <summary>
        /// 将 URL 中的参数名称/值编码为合法的格式。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeAsUrlData(this string str) => Uri.EscapeDataString(str);


        /// <summary>
        /// 组装QueryString
        /// 参数之间用&amp;连接，首位没有符号，如：a=1&amp;b=2&amp;c=3
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        public static string ToQueryString(this Dictionary<string, string> formData)
        {
            if (formData.IsNull() || formData.Count == 0)
                return "";

            StringBuilder sb = new StringBuilder();
            var i = 0;
            foreach (var kv in formData)
            {
                i++;
                sb.AppendFormat("{0}={1}", kv.Key, kv.Value.EncodeAsUrlData());
                if (i < formData.Count)
                    sb.Append("&");
            }
            return sb.ToString();
        }

        /// <summary>
        /// QueryString拆解为字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Dictionary<string, string> QueryStringToDictionary(this string query)
        {
            var dic = new Dictionary<string, string>();
            query = query.HtmlDecode().SplitThenGetLast("?");
            foreach (var item in query.SplitBySign("&"))
            {
                var key = item.SplitThenGetFirst("=");
                var value = item.SubString(key.Length + 1).UrlDecode();
                dic.Add(key, value);
            }
            return dic;
        }

    }
}
