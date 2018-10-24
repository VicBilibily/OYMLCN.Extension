using OYMLCN.Extensions;
using System;
using System.Net;
using System.Web;

namespace OYMLCN.Handlers
{
    /// <summary>
    /// Url字符串格式处理
    /// </summary>
    public class UrlFormatHandler
    {
        private string Url;
        internal UrlFormatHandler(string url)
            => Url = url;

        /// <summary>
        /// 将URL转义为合法参数地址
        /// </summary>
        public string UrlEncode
        {
            get
            {
#if NET35
                return HttpUtility.UrlEncode(Url);
#else
                return WebUtility.UrlEncode(Url);
#endif
            }
        }
        /// <summary>
        /// 被转义的URL字符串还原
        /// </summary>
        public string UrlDecode
        {
            get
            {
#if NET35
                return HttpUtility.UrlDecode(Url);
#else
                return WebUtility.UrlDecode(Url);
#endif
            }
        }
        /// <summary>
        /// 将 URL 中的参数名称/值编码为合法的格式。
        /// </summary>
        public string EncodeAsUrlData
            => Uri.EscapeDataString(Url);

        /// <summary>
        /// 获取url字符串的的协议域名地址
        /// </summary>
        public string UrlHost
            => Url.AsType().Uri.GetHost();

    }
}
