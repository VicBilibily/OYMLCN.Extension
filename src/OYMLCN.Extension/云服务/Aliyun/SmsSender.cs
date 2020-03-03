using OYMLCN.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace OYMLCN.Aliyun
{
    /// <summary>
    /// SmsSender 基于 2017-12-05 源码
    /// </summary>
    public class SmsSender
    {
        //const string product = "Dysmsapi";//短信API产品名称（短信产品名固定，无需修改）
        const string domain = "dysmsapi.aliyuncs.com";//短信API产品域名（接口地址固定，无需修改）
        private string accessKeyId;//你的accessKeyId，参考本文档步骤2
        private string accessKeySecret;//你的accessKeySecret，参考本文档步骤2

        /// <summary>
        /// SmsSender
        /// </summary>
        /// <param name="accessKeyId">AccessKeyId</param>
        /// <param name="accessKeySecret">AccessKeySecret</param>
        public SmsSender(string accessKeyId, string accessKeySecret)
        {
            this.accessKeyId = accessKeyId;
            this.accessKeySecret = accessKeySecret;
        }

        static string SpecialUrlEncoder(string str) =>
            str.UrlEncode().Replace("+", "%20").Replace("*", "%2A").Replace("%7E", "~");
        string GetAlidayuQueryString(IDictionary<string, string> parameters)
        {
            if (parameters.ContainsKey("Signature"))
                parameters.Remove("Signature");
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            StringBuilder query = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in sortedParams)
                if (!string.IsNullOrEmpty(kv.Key) && !string.IsNullOrEmpty(kv.Value))
                    query.Append("&").Append(SpecialUrlEncoder(kv.Key)).Append("=").Append(SpecialUrlEncoder(kv.Value));
            return query.ToString().Substring(1);
        }
        string GetAlidayuSign(string sortedQueryString, string secret)
        {
            StringBuilder stringToSign = new StringBuilder();
            stringToSign.Append("GET").Append("&");
            stringToSign.Append(SpecialUrlEncoder("/")).Append("&");
            stringToSign.Append(SpecialUrlEncoder(sortedQueryString));

            string secretString = accessKeySecret + "&";
            string signString = stringToSign.ToString();
            return signString;//.HashToHMACSHA1Base64(secretString);
        }

        /// <summary>
        /// SmsSenderResponse
        /// </summary>
        public class SmsSenderResponse
        {
            /// <summary>
            /// 请求ID
            /// </summary>
            public string RequestId { get; set; }
            /// <summary>
            /// 状态码-返回OK代表请求成功,其他错误码详见错误码列表
            /// </summary>
            public string Code { get; set; }
            /// <summary>
            /// 状态码的描述
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 发送回执ID,可根据该ID查询具体的发送状态
            /// </summary>
            public string BizId { get; set; }

            /// <summary>
            /// 是否发送成功
            /// </summary>
            public bool IsOK => Code?.Equals("OK") ?? false;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="signName">短信签名</param>
        /// <param name="templateCode">短信模板ID</param>
        /// <param name="templateParam">(可选)短信模板对象变量(不是string的话内部会转为Json)</param>
        /// <param name="outId">(可选)外部流水扩展字段</param>
        /// <param name="phoneNumbers">短信接收号码,支持批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式</param>
        /// <returns></returns>
        public SmsSenderResponse Send(string signName, string templateCode, object templateParam = null, string outId = null, params string[] phoneNumbers)
        {
            var txtParams = new Dictionary<string, string>
            {
                { "AccessKeyId", accessKeyId },
                { "Timestamp", DateTime.UtcNow.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") },
                { "Format", "JSON" },
                { "SignatureMethod", "HMAC-SHA1" },
                { "SignatureVersion", "1.0" },
                { "SignatureNonce", Guid.NewGuid().ToString() },
                //{ "Signature", "Signature" },

                { "Action", "SendSms" },
                { "Version", "2017-05-25" },
                { "RegionId", "cn-hangzhou" },
                { "PhoneNumbers", phoneNumbers.Join(",") },
                { "SignName", signName },
                { "TemplateCode", templateCode },
                { "TemplateParam", (templateParam is string) ? templateParam.ToString() : templateParam.JsonSerialize() },
                { "OutId", outId },
            };

            var query = GetAlidayuQueryString(txtParams);
            var sign = GetAlidayuSign(query, accessKeySecret);
            var requestUrl = $"http://{domain}/?Signature={SpecialUrlEncoder(sign)}&{query}";

            using (var webclient = new WebClient())
                return webclient.DownloadJson<SmsSenderResponse>(requestUrl);
        }

    }
}
