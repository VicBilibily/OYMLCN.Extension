using Ganss.XSS;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace OYMLCN.Extensions
{
    public static class HtmlExtensionPlus
    {
        private static readonly HtmlSanitizer Sanitizer = new HtmlSanitizer();
        static HtmlExtensionPlus()
        {
            Sanitizer.AllowedAttributes.Remove("id");
            Sanitizer.AllowedAttributes.Remove("alt");
            Sanitizer.AllowedCssProperties.Remove("font-family");
            Sanitizer.AllowedCssProperties.Remove("background-color");
            Sanitizer.KeepChildNodes = true;
            Sanitizer.AllowedTags.Remove("input");
            Sanitizer.AllowedTags.Remove("button");
            Sanitizer.AllowedTags.Remove("iframe");
            Sanitizer.AllowedTags.Remove("frame");
            Sanitizer.AllowedTags.Remove("textarea");
            Sanitizer.AllowedTags.Remove("select");
            Sanitizer.AllowedTags.Remove("form");
            Sanitizer.AllowedAttributes.Add("src");
            Sanitizer.AllowedAttributes.Add("class");
            Sanitizer.AllowedAttributes.Add("style");
        }

        /// <summary>
        /// 标准的防止html的xss净化器
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlSantinizerStandard(this string html)
            => Sanitizer.Sanitize(html);
        /// <summary>
        /// 自定义的防止html的xss净化器
        /// </summary>
        /// <param name="html">源html</param>
        /// <param name="labels">需要移除的标签集合</param>
        /// <param name="attributes">需要移除的属性集合</param>
        /// <param name="styles">需要移除的样式集合</param>
        /// <returns></returns>
        public static string HtmlSantinizerCustom(this string html, string[] labels = null, string[] attributes = null, string[] styles = null)
        {
            if (labels != null)
                foreach (string label in labels)
                    Sanitizer.AllowedTags.Remove(label);
            if (attributes != null)
                foreach (string attr in attributes)
                    Sanitizer.AllowedAttributes.Remove(attr);
            if (styles != null)
                foreach (string p in styles)
                    Sanitizer.AllowedCssProperties.Remove(p);
            Sanitizer.KeepChildNodes = true;
            return Sanitizer.Sanitize(html);
        }
        /// <summary>
        /// 去除html标签后并截取字符串
        /// </summary>
        /// <param name="html">源html</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static string HtmlRemoveTag(this string html, int length = 0)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var strText = doc.DocumentNode.InnerText;
            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);
            return strText;
        }
      
        /// <summary>
        /// 匹配html的所有img标签集合
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> HtmlMatchImgTags(this string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var nodes = doc.DocumentNode.Descendants("img");
            return nodes;
        }
        /// <summary>
        /// 匹配html的所有img标签的src集合
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static IEnumerable<string> HtmlMatchImgSrcs(this string html)
            => HtmlMatchImgTags(html).Where(n => n.Attributes.Contains("src")).Select(n => n.Attributes["src"].Value);
        /// <summary>
        /// 获取html中第一个img标签的src
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlMatchFirstImgSrc(this string html)
            => HtmlMatchImgSrcs(html).FirstOrDefault();
        /// <summary>
        /// 随机获取html代码中的img标签的src属性
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlMatchRandomImgSrc(this string html)
        {
            int count = HtmlMatchImgSrcs(html).Count();
            var rnd = new Random();
            return HtmlMatchImgSrcs(html).ElementAtOrDefault(rnd.Next(count));
        }       
    }
}
