using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 格式相关扩展
    /// </summary>
    public static partial class StringFormatExtension
    {
        /// <summary>
        /// 将URL转义为合法参数地址
        /// </summary>
        public static string UrlEncode(this string url)
            => WebUtility.UrlEncode(url);
        /// <summary>
        /// 被转义的URL字符串还原
        /// </summary>
        public static string UrlDecode(this string url)
            => WebUtility.UrlDecode(url);
        /// <summary>
        /// 将 URL 中的参数名称/值编码为合法的格式。
        /// </summary>
        public static string UrlDataEncode(this string url)
            => Uri.EscapeDataString(url);

        /// <summary>
        /// 获取url字符串的的域名地址（eg：www.qq.com）
        /// </summary>
        public static string UrlHost(this string url)
            => url.ConvertToUri()?.Host;

        /// <summary>
        /// 获取url字符串的的协议域名地址（eg：https://www.qq.com）
        /// </summary>
        public static string UrlSchemeHost(this string url)
            => url.ConvertToUri()?.GetSchemeHost();


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
                sb.AppendFormat("{0}={1}", kv.Key, kv.Value?.ToString().UrlDataEncode());
                if (i < formData.Count)
                    sb.Append("&");
            }
            return sb.ToString();
        }
    }
}
