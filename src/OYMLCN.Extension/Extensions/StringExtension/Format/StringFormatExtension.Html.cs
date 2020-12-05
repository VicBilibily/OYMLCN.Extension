/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringFormatExtension.Html.cs
Author: VicBilibily
Description: 
    本代码所在文件夹的文件主要定义一些常用的字符串格式相关方法扩展，以提升开发效率为目的。
    本文件主要定义一些前端网页相关字符串过滤扩展方法。
*****************************************************************************/

using System.Text.RegularExpressions;

namespace OYMLCN.Extensions
{
    public static partial class StringFormatExtension
    {
        /// <summary>
        ///   删除字符串内的 script 标签块。
        /// </summary>
        /// <param name="html">HTML字符串</param>
        /// <returns>
        ///   <para>已移除 &lt;script/&gt; 标签的字符串。</para>
        ///   <para>如果 <paramref name="html"/> 为
        ///   <see langword="null"/>、空字符串 ("") 或是仅由空白字符组成，则原样返回
        ///   <paramref name="html"/>。</para>
        /// </returns>
        public static string HtmlRemoveScript(this string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            var result = Regex.Replace(html, @"(\<script(.+?)\</script\>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (html == result)
                result = Regex.Replace(html, @"(\<script(.+?)>|\</script\>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return result;
        }
        /// <summary>
        ///   删除字符串内的 style 标签块。
        /// </summary>
        /// <param name="html">HTML字符串</param>
        /// <returns>
        ///   <para>已移除 &lt;style/&gt; 标签块的字符串。</para>
        ///   <para>如果 <paramref name="html"/> 为
        ///   <see langword="null"/>、空字符串 ("") 或是仅由空白字符组成，则原样返回
        ///   <paramref name="html"/>。</para>
        /// </returns>
        public static string HtmlRemoveStyleBlock(this string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            return Regex.Replace(html, @"(\<style(.+?)\</style\>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        /// <summary>
        ///   替换回车(\r)和换行符(\n)为 &lt;br/&gt; 换行符。
        /// </summary>
        /// <param name="html">HTML字符串</param>
        /// <returns>
        ///   <para>已将换行符(\n)或回车(\r)替换为 &lt;br/&gt; 换行符的字符串。</para>
        ///   <para>如果 <paramref name="html"/> 为
        ///   <see langword="null"/>、空字符串 ("") 或是仅由空白字符组成，则原样返回
        ///   <paramref name="html"/>。</para>
        /// </returns>
        public static string HtmlFormatNewLines(this string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            return html.Replace("\r\n", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>");
        }


        /// <summary>
        ///   将已经为 HTTP 传输进行过 HTML 编码的字符串转换为已解码的字符串。
        /// </summary>
        /// <param name="html">要解码的字符串</param>
        /// <returns>已解码的字符串</returns>
        public static string HtmlDecode(this string html)
            => System.Net.WebUtility.HtmlDecode(html);
        /// <summary>
        ///   将字符串转换为 HTML 编码字符串。
        /// </summary>
        /// <param name="html">要编码的字符串</param>
        /// <returns>已编码的字符串</returns>
        public static string HtmlEncode(this string html)
            => System.Net.WebUtility.HtmlEncode(html);


    }
}
