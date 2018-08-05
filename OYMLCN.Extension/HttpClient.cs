using System;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// HttpClient方法封装
    /// </summary>
    public static partial class HttpClientExtensions
    {
        /// <summary>
        /// 通过HttpGet获取数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="decoder">返回数据编码形式（默认为UTF-8）</param>
        /// <param name="timeout">请求超时时间（单位：秒）</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static string GetString(string url, Encoding decoder = null, int timeout = 30, CookieContainer cookieContainer = null) =>
            GetData(url, timeout, cookieContainer).ReadToEnd(decoder ?? Encoding.UTF8);
        /// <summary>
        /// 通过HttpGet获取数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="queryStr">请求参数字符串</param>
        /// <param name="decoder">返回数据编码形式（默认为UTF-8）</param>
        /// <param name="timeout">请求超时时间（单位：秒）</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static string GetString(string url, string queryStr, Encoding decoder = null, int timeout = 30, CookieContainer cookieContainer = null) =>
            GetData($"{url}?{queryStr.TrimStart('?')}", timeout, cookieContainer).ReadToEnd(decoder ?? Encoding.UTF8);
        /// <summary>
        /// 通过HttpGet获取数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="queryDir">请求参数字典集合</param>
        /// <param name="decoder">返回数据编码形式（默认为UTF-8）</param>
        /// <param name="timeout">请求超时时间（单位：秒）</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static string GetString(string url, Dictionary<string, string> queryDir, Encoding decoder = null, int timeout = 30, CookieContainer cookieContainer = null) =>
             GetData($"{url}?{queryDir.ToQueryString()}", timeout, cookieContainer).ReadToEnd(decoder ?? Encoding.UTF8);

        /// <summary>
        /// 通过HttpGet获取数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="timeout">请求超时时间（单位：秒）</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static Stream GetData(string url, int timeout = 30, CookieContainer cookieContainer = null)
        {
            if (url.IsNullOrEmpty())
                throw new ArgumentNullException("url", "请求URL地址不能为空");
            HttpClient client;
            if (cookieContainer != null)
                client = new HttpClient(new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    UseCookies = true,
                });
            else
                client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, timeout);
            var t = client.GetStreamAsync(url);
            t.Wait();
            var result = t.Result;
            client.Dispose();
            return result;
        }


        /// <summary>
        /// 通过HttpPost提交字符数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="data">提交字符数据</param>
        /// <param name="encoder">字符数据编码（默认为UTF-8）</param>
        /// <param name="decoder">返回数据编码（默认为UTF-8）</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static string PostString(string url, string data, Encoding encoder = null, Encoding decoder = null, CookieContainer cookieContainer = null) =>
            PostData(url, data, encoder, "text/text", cookieContainer: cookieContainer).ReadToEnd(decoder ?? Encoding.UTF8);
        /// <summary>
        /// 通过HttpPost提交Json数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="json">提交Json数据</param>
        /// <param name="encoder">Json数据编码（默认为UTF-8）</param>
        /// <param name="decoder">返回数据编码（默认为UTF-8）</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static string PostJsonString(string url, string json, Encoding encoder = null, Encoding decoder = null, CookieContainer cookieContainer = null) =>
            PostData(url, json, encoder, "application/json", cookieContainer: cookieContainer).ReadToEnd(decoder ?? Encoding.UTF8);
        /// <summary>
        /// 通过HttpPost提交Xml数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="xml">提交Xml数据</param>
        /// <param name="encoder">Xml数据编码（默认为UTF-8）</param>
        /// <param name="decoder">返回数据编码（默认为UTF-8）</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static string PostXmlString(string url, string xml, Encoding encoder = null, Encoding decoder = null, CookieContainer cookieContainer = null) =>
            PostData(url, xml, encoder, "text/xml", cookieContainer: cookieContainer).ReadToEnd(decoder ?? Encoding.UTF8);

        /// <summary>
        /// 通过HttpPost提交数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="data">提交字符数据</param>
        /// <param name="encoder">字符数据编码（默认为UTF-8）</param>
        /// <param name="mediaType">字符数据类型</param>
        /// <param name="queryDir">表单数据集合（Value可为有效的文件路径，否者均为文本text/text）</param>
        /// <param name="timeout">请求超时时间（单位：秒）</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static Stream PostData(string url, string data = null, Encoding encoder = null, string mediaType = "text/text", Dictionary<string, string> queryDir = null, int timeout = 30, CookieContainer cookieContainer = null)
        {
            if (url.IsNullOrEmpty())
                throw new ArgumentNullException("url", "请求URL地址不能为空");
            HttpClient client;
            if (cookieContainer != null)
                client = new HttpClient(new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    UseCookies = true,
                });
            else
                client = new HttpClient();
            client.Timeout = new TimeSpan(timeout * 1000 * 10000);

            HttpContent content = null;
            StringContent strContent = null;
            FormUrlEncodedContent formContent = null;
            MultipartFormDataContent multiContent = null;
            if (!data.IsNullOrEmpty())
                content = strContent = new StringContent(data, (encoder ?? Encoding.UTF8), mediaType);

            if (queryDir != null)
            {
                var queries = queryDir.Select(d => new { d.Key, FileInfo = d.Value.GetFileInfo(), d.Value }).GroupBy(d => d.FileInfo?.Exists ?? false);
                if (queries.Where(d => d.Key).Count() == 0 && data.IsNullOrEmpty())
                    content = formContent = new FormUrlEncodedContent(queryDir);
                else
                {
                    content = multiContent = new MultipartFormDataContent();
                    if (strContent != null)
                        multiContent.Add(strContent);
                    var texts = queries.Where(d => !d.Key).FirstOrDefault();
                    if (texts != null)
                        foreach (var item in texts)
                            multiContent.Add(new StringContent(item.Value, (encoder ?? Encoding.UTF8), "text/text"), item.Key);
                    var files = queries.Where(d => d.Key).FirstOrDefault();
                    if (files != null)
                        foreach (var file in files)
                            multiContent.Add(new StreamContent(file.FileInfo.ReadToStream()), file.Key, file.FileInfo.Name);
                }
            }

            if (content.IsNull())
                content = new StringContent(string.Empty);
            var stri = content.ReadAsStringAsync().Result;
            var t = client.PostAsync(url, content);
            t.Wait();
            var result = t.Result.Content.ReadAsStreamAsync();
            result.Wait();
            client.Dispose();
            return result.Result;
        }


        /// <summary>
        /// 通过HttpPut提交字符数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="data">提交字符数据</param>
        /// <param name="encoder">字符数据编码（默认为UTF-8）</param>
        /// <param name="decoder">返回数据编码（默认为UTF-8）</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static string PutString(string url, string data, Encoding encoder = null, Encoding decoder = null, CookieContainer cookieContainer = null) =>
            PutData(url, data, encoder, "text/text", cookieContainer: cookieContainer).ReadToEnd(decoder ?? Encoding.UTF8);
        /// <summary>
        /// 通过HttpPut提交Json数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="json">提交Json数据</param>
        /// <param name="encoder">Json数据编码（默认为UTF-8）</param>
        /// <param name="decoder">返回数据编码（默认为UTF-8）</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static string PutJsonString(string url, string json, Encoding encoder = null, Encoding decoder = null, CookieContainer cookieContainer = null) =>
            PutData(url, json, encoder, "application/json", cookieContainer: cookieContainer).ReadToEnd(decoder ?? Encoding.UTF8);
        /// <summary>
        /// 通过HttpPut提交Xml数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="xml">提交Xml数据</param>
        /// <param name="encoder">Xml数据编码（默认为UTF-8）</param>
        /// <param name="decoder">返回数据编码（默认为UTF-8）</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static string PutXmlString(string url, string xml, Encoding encoder = null, Encoding decoder = null, CookieContainer cookieContainer = null) =>
            PutData(url, xml, encoder, "text/xml", cookieContainer: cookieContainer).ReadToEnd(decoder ?? Encoding.UTF8);

        /// <summary>
        /// 通过HttpPut提交数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="data">提交字符数据</param>
        /// <param name="encoder">字符数据编码（默认为UTF-8）</param>
        /// <param name="mediaType">字符数据类型</param>
        /// <param name="timeout">请求超时时间（单位：秒）</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static Stream PutData(string url, string data, Encoding encoder = null, string mediaType = "text/text", int timeout = 30, CookieContainer cookieContainer = null)
        {
            if (url.IsNullOrEmpty())
                throw new ArgumentNullException("url", "请求URL地址不能为空");
            HttpClient client;
            if (cookieContainer != null)
                client = new HttpClient(new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    UseCookies = true,
                });
            else
                client = new HttpClient();
            client.Timeout = new TimeSpan(timeout * 1000 * 10000);

            HttpContent content = null;
            if (!data.IsNullOrEmpty())
                content = new StringContent(data, (encoder ?? Encoding.UTF8), mediaType);
            if (content.IsNull())
                content = new StringContent(string.Empty);
            var stri = content.ReadAsStringAsync().Result;
            var t = client.PutAsync(url, content);
            t.Wait();
            var result = t.Result.Content.ReadAsStreamAsync();
            result.Wait();
            client.Dispose();
            return result.Result;
        }

        /// <summary>
        /// 通过HttpDelete提交数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static string Delete(string url, CookieContainer cookieContainer = null) => Delete(url, cookieContainer);
        /// <summary>
        /// 通过HttpDelete提交数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="timeout">请求超时时间（单位：秒）</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static Stream Delete(string url, int timeout = 30, CookieContainer cookieContainer = null)
        {
            if (url.IsNullOrEmpty())
                throw new ArgumentNullException("url", "请求URL地址不能为空");
            HttpClient client;
            if (cookieContainer != null)
                client = new HttpClient(new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    UseCookies = true,
                });
            else
                client = new HttpClient();
            client.Timeout = new TimeSpan(timeout * 1000 * 10000);

            var t = client.DeleteAsync(url);
            t.Wait();
            var result = t.Result.Content.ReadAsStreamAsync();
            result.Wait();
            client.Dispose();
            return result.Result;
        }

        /// <summary>
        /// 通过模拟CurlPost提交数据
        /// </summary>
        /// <param name="url">请求Url</param>
        /// <param name="queryDir">表单数据集合（Value可为有效的文件路径，否者均为文本text/text）</param>
        /// <param name="timeout">请求超时时间（单位：秒）</param>
        /// <param name="cookieContainer">Cookie容器</param>
        /// <returns></returns>
        public static Stream CurlPost(string url, Dictionary<string, string> queryDir = null, int timeout = 30, CookieContainer cookieContainer = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Timeout = timeout * 1000;
            var postStream = new MemoryStream();
            string boundary = "----" + DateTime.Now.Ticks.ToString("x");
            string fileFormdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
            string dataFormdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (var file in queryDir)
            {
                var fileName = file.Value;
                using (var fileStream = fileName.GetFileInfo().ReadToStream())
                {
                    string formdata = null;
                    if (fileStream != null)
                        formdata = string.Format(fileFormdataTemplate, file.Key, Path.GetFileName(fileName));
                    else
                        formdata = string.Format(dataFormdataTemplate, file.Key, file.Value);
                    var formdataBytes = Encoding.UTF8.GetBytes(postStream.Length == 0 ? formdata.Substring(2, formdata.Length - 2) : formdata);//第一行不需要换行
                    postStream.Write(formdataBytes, 0, formdataBytes.Length);
                    if (fileStream != null)
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead = 0;
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                            postStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
            var footer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            postStream.Write(footer, 0, footer.Length);
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            if (cookieContainer != null)
                request.CookieContainer = cookieContainer;
            if (postStream != null)
            {
                postStream.Position = 0;
                Stream requestStream = request.GetRequestStreamAsync().Result;
                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                    requestStream.Write(buffer, 0, bytesRead);
                postStream.Flush();
            }
            return ((HttpWebResponse)request.GetResponseAsync().Result).GetResponseStream();
        }

    }
}
