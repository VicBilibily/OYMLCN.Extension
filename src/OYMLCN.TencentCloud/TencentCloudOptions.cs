using System.Collections.Generic;

namespace OYMLCN.TencentCloud
{
    /// <summary>
    /// 腾讯云API基础配置信息
    /// </summary>
    public class TencentCloudOptions
    {
        /// <summary>
        /// 腾讯云帐号唯一的 APPID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// API 调用者身份标识，可以简单类比为用户名 
        /// </summary>
        public string SecretId { get; set; }
        /// <summary>
        /// API 调用者身份验证，可以简单类比为密码
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 腾讯云短信基础配置信息
        /// </summary>
        public TencentCloudSmsOptions Sms { get; set; }
    }

    /// <summary>
    /// 腾讯云短信基础配置信息
    /// </summary>
    public class TencentCloudSmsOptions
    {
        /// <summary>
        /// 短信应用的唯一标识 SDK AppID
        /// </summary>
        public string SdkAppId { get; set; }
        /// <summary>
        /// 用来校验短信发送合法性的密码 AppKey (V3接口中已弃用)
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 短信签名，必须填写已审核通过的签名，国内短信为必填参数
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 模板字典，Key:Value，Key为读取标识，Value为认证后的模板Id
        /// </summary>
        public Dictionary<string, string> Templates { get; set; }
    }

}
