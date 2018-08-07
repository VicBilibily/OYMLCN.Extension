using OYMLCN.Extensions;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace OYMLCN.Handlers
{
    /// <summary>
    /// HTML格式处理
    /// </summary>
    public class HtmlFormatHandler
    {
        internal HtmlFormatHandler(string html) => Result = html;

        /// <summary>
        /// 被转义HTML的字符串还原
        /// </summary>
        public HtmlFormatHandler HtmlDecode()
        {
            Result = WebUtility.HtmlDecode(Result);
            return this;
        }
        /// <summary>
        /// HTML转义为数据库合法模式
        /// </summary>
        public string HtmlEncode => WebUtility.HtmlEncode(Result);

        /// <summary>
        /// 使用正则表达式删除Html标签
        /// </summary>
        public HtmlFormatHandler RemoveHtml(int length = 0)
        {
            Result = Regex.Replace(Result, "<[^>]+>", "");
            Result = Regex.Replace(Result, "&[^;]+;", "");
            return this;
        }

        /// <summary>
        /// 使用正则表达式删除Script标签
        /// </summary>
        public HtmlFormatHandler RemoveScript()
        {
            Result = Regex.Replace(Result, @"(\<script(.+?)\</script\>)|(\<style(.+?)\</style\>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return this;
        }


        /// <summary>
        /// 替换换行Br为换行符
        /// </summary>
        public HtmlFormatHandler ReplaceHtmlBr()
        {
            Result = Result.ReplaceIgnoreCaseWithRegex("\r\n", "<br>", "<br/>", "<br />");
            return this;
        }


        /// <summary>
        /// 字符串去除 &amp;nbsp;/&amp;ensp;/&amp;emsp;/&amp;thinsp;/&amp;zwnj;/&amp;zwj; 空格符 制表符 
        /// </summary>
        /// <param name="keepOneSpace">保留一个空格</param>
        public HtmlFormatHandler RemoveSpace(bool keepOneSpace = true)
        {
            Result = Result.SplitByMultiSign("　", " ", "&nbsp;", "&ensp;", "&emsp;", "&thinsp;", "&zwnj;", "&zwj;", "\t")
                       .Where(d => !d.Trim().IsNullOrEmpty())
                       .Join(keepOneSpace ? " " : string.Empty);
            return this;
        }

        /// <summary>
        /// 处理结果
        /// </summary>
        public string Result { get; private set; }
    }
}
