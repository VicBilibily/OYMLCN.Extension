using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    public static class StringHtmlExtension
    {
        /// <summary>
        /// 清理Word文档转html后的冗余标签属性
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlClearWordTags(this string html)
        {
            string s = Regex.Match(Regex.Replace(html, @"background-color:#?\w{3,7}|font-family:'?[\w|\(|\)]*'?;?", string.Empty), @"<body[^>]*>([\s\S]*)<\/body>").Groups[1].Value.Replace("&#xa0;", string.Empty);
            s = Regex.Replace(s, @"\w+-?\w+:0\w+;?", string.Empty); //去除多余的零值属性
            s = Regex.Replace(s, "alt=\"(.+?)\"", string.Empty); //除去alt属性
            s = Regex.Replace(s, @"-aw.+?\s", string.Empty); //去除Word产生的-aw属性
            return s;
        }
        /// <summary>
        /// 使用正则表达式删除Script标签
        /// </summary>
        public static string HtmlRemoveScript(this string html)
            => Regex.Replace(html, @"(\<script(.+?)\</script\>)|(\<style(.+?)\</style\>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        /// <summary>
        /// 替换html的img路径为绝对路径
        /// </summary>
        /// <param name="html"></param>
        /// <param name="imgDest"></param>
        /// <returns></returns>
        public static string HtmlReplaceImgSource(this string html, string imgDest) => html.Replace("<img src=\"", "<img src=\"" + imgDest + "/");
        /// <summary>
        /// 将src的绝对路径换成相对路径
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlConvertImgSrcToRelativePath(this string html)
            => Regex.Replace(html, @"<img src=""(http:\/\/.+?)/", @"<img src=""/");
        /// <summary>
        /// 替换回车换行符为html换行符
        /// </summary>
        /// <param name="html">html</param>
        public static string HtmlStrFormat(this string html)
            => html.Replace("\r\n", "<br />").Replace("\n", "<br />");
        /// <summary>
        /// 替换html字符
        /// </summary>
        /// <param name="html">html</param>
        public static string HtmlWebEncode(this string html)
        {
            if (html != "")
            {
                html = html.Replace(",", "&def");
                html = html.Replace("'", "&dot");
                html = html.Replace(";", "&dec");
                return html;
            }
            return "";
        }
        /// <summary>
        /// 被转义HTML的字符串还原
        /// </summary>
        public static string HtmlDecode(this string html)
            => WebUtility.HtmlDecode(html);
        /// <summary>
        /// HTML转义为数据库合法模式
        /// </summary>
        public static string HtmlEncode(this string html)
            => WebUtility.HtmlEncode(html);

        /// <summary>
        /// 字符串去除 &amp;nbsp;/&amp;ensp;/&amp;emsp;/&amp;thinsp;/&amp;zwnj;/&amp;zwj; 空格符 制表符 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="keepOneSpace">保留一个空格</param>
        public static string HtmlRemoveSpace(this string html, bool keepOneSpace = true)
            => html.SplitByMultiSign("　", " ", "&nbsp;", "&ensp;", "&emsp;", "&thinsp;", "&zwnj;", "&zwj;", "\t")
                   .Where(d => !d.Trim().IsNullOrEmpty())
                   .Join(keepOneSpace ? " " : string.Empty);
    }

}
