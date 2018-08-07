using OYMLCN.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// FormatExtension
    /// </summary>
    public static class FormatExtensions
    {
        /// <summary>
        /// 将字符串作为HTML格式文本处理
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HtmlFormatHandler AsHtmlFormat(this string html) => html.IsNotNullOrWhiteSpace() ? new HtmlFormatHandler(html) : null;
        /// <summary>
        /// 将字符串作为URL格式文本处理
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static UrlFormatHandler AsUrlFormat(this string url) => url.IsNotNullOrWhiteSpace() ? new UrlFormatHandler(url) : null;


        /// <summary>
        /// 将枚举值转换为字符串值（替换 _ 标头）
        /// 用于部分不能使用数字作为枚举值 用 _ 作为开头（某些较旧的源码里有这种习惯……）
        /// </summary>
        /// <param name="enumClass"></param>
        /// <returns></returns>
        public static string EnumToString(this Enum enumClass) => enumClass.ToString().TrimStart('_');

        /// <summary>
        /// 获取Uri的协议域名地址
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetHost(this Uri uri) => uri.IsNull() ? null : $"{uri.Scheme}://{uri.Host}/";

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
                sb.AppendFormat("{0}={1}", kv.Key, kv.Value.AsUrlFormat().EncodeAsUrlData);
                if (i < formData.Count)
                    sb.Append("&");
            }
            return sb.ToString();
        }

  


    }
}
