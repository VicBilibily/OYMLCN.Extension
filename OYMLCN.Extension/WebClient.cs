using System.Net;
using System.Text;

namespace OYMLCN
{
    /// <summary>
    /// WebClientExtension
    /// </summary>
    public static partial class WebClientExtensions
    {
        /// <summary>
        /// 向指定的URL POST数据，并返回数据
        /// </summary>
        /// <param name="wc"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="postEncoding"></param>
        /// <param name="dataEncoding"></param>
        /// <returns></returns>
        public static string PostData(this WebClient wc, string url, string data, string postEncoding = "utf-8", string dataEncoding = "utf-8")
        {
            byte[] postData = Encoding.GetEncoding(postEncoding).GetBytes(data);
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            byte[] responseData = wc.UploadData(url, "POST", postData);
            string srcString = Encoding.GetEncoding(dataEncoding).GetString(responseData);
            //srcString = srcString.Replace("\t", "");
            //srcString = srcString.Replace("\r", "");
            //srcString = srcString.Replace("\n", "");
            return srcString;
        }

        /// <summary>
        /// 提交Json数据
        /// </summary>
        /// <param name="wc"></param>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="postEncoding"></param>
        /// <param name="dataEncoding"></param>
        /// <returns></returns>
        public static string PostJson(this WebClient wc, string url, string json, string postEncoding = "utf-8", string dataEncoding = "utf-8")
        {
            byte[] postData = Encoding.GetEncoding(postEncoding).GetBytes(json);
            wc.Headers.Add("Content-Type", "application/json");
            byte[] responseData = wc.UploadData(url, "POST", postData);
            string srcString = Encoding.GetEncoding(dataEncoding).GetString(responseData);
            return srcString;
        }

        /// <summary>
        /// 获得指定 URL 的源内容
        /// </summary>
        /// <param name="wc"></param>
        /// <param name="uriString"></param>
        /// <param name="dataEncoding"></param>
        /// <returns></returns>
        public static string GetSourceString(this WebClient wc, string uriString, string dataEncoding = "utf-8")
        {
            byte[] responseData = wc.DownloadData(uriString);
            string srcString = Encoding.GetEncoding(dataEncoding).GetString(responseData);
            return srcString;
        }
    }
}