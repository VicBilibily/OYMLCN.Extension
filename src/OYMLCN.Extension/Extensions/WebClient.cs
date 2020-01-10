using System.Net;
using System.Text;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// WebClientExtension
    /// </summary>
    public static partial class WebClientExtensions
    {
        private static string SendData(this WebClient wc, string method, string url, string data, string postEncoding, string dataEncoding)
        {
            byte[] postData = Encoding.GetEncoding(postEncoding).GetBytes(data);
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            byte[] responseData = wc.UploadData(url, method, postData);
            string srcString = Encoding.GetEncoding(dataEncoding).GetString(responseData);
            //srcString = srcString.Replace("\t", "");
            //srcString = srcString.Replace("\r", "");
            //srcString = srcString.Replace("\n", "");
            return srcString;
        }
        private static string SendJson(this WebClient wc, string method, string url, string json, string postEncoding, string dataEncoding)
        {
            byte[] postData = Encoding.GetEncoding(postEncoding).GetBytes(json);
            wc.Headers.Add("Content-Type", "application/json");
            byte[] responseData = wc.UploadData(url, method, postData);
            string srcString = Encoding.GetEncoding(dataEncoding).GetString(responseData);
            return srcString;
        }

        /// <summary>
        /// 向指定的URL POST数据，并返回数据
        /// </summary>
        /// <param name="wc"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="postEncoding"></param>
        /// <param name="dataEncoding"></param>
        /// <returns></returns>
        public static string PostData(this WebClient wc, string url, string data, string postEncoding = "utf-8", string dataEncoding = "utf-8") =>
            SendData(wc, "POST", url, data, postEncoding, dataEncoding);
        /// <summary>
        /// 提交Json数据
        /// </summary>
        /// <param name="wc"></param>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="postEncoding"></param>
        /// <param name="dataEncoding"></param>
        /// <returns></returns>
        public static string PostJson(this WebClient wc, string url, string json, string postEncoding = "utf-8", string dataEncoding = "utf-8") =>
            SendJson(wc, "POST", url, json, postEncoding, dataEncoding);
        /// <summary>
        /// 向指定的URL Put数据，并返回数据
        /// </summary>
        /// <param name="wc"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="postEncoding"></param>
        /// <param name="dataEncoding"></param>
        /// <returns></returns>
        public static string PutData(this WebClient wc, string url, string data, string postEncoding = "utf-8", string dataEncoding = "utf-8") =>
            SendData(wc, "Put", url, data, postEncoding, dataEncoding);
        /// <summary>
        /// 推送Json数据
        /// </summary>
        /// <param name="wc"></param>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="postEncoding"></param>
        /// <param name="dataEncoding"></param>
        /// <returns></returns>
        public static string PutJson(this WebClient wc, string url, string json, string postEncoding = "utf-8", string dataEncoding = "utf-8") =>
            SendJson(wc, "Put", url, json, postEncoding, dataEncoding);

        /// <summary>
        /// 发送Delete请求并接收数据
        /// </summary>
        /// <param name="wc"></param>
        /// <param name="url"></param>
        /// <param name="dataEncoding">返回数据编码</param>
        /// <returns></returns>
        public static string Delete(this WebClient wc, string url, string dataEncoding = "utf-8")
        {
            byte[] responseData = wc.UploadData(url, "DELETE", new byte[0]);
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