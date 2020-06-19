using OYMLCN.Extensions;
using System;
using TencentCloud.Common;
using TencentCloud.Sms.V20190711;
using TencentCloud.Sms.V20190711.Models;

namespace OYMLCN.TencentCloud
{
    /// <summary>
    /// SmsSender 基于 V3 版本接口 TencentCloud.Sms.V20190711
    /// </summary>
    public class SmsSender
    {
        private string SdkAppID;
        private SmsClient SmsClient;

        /// <summary>
        /// 腾讯云短信发送
        /// </summary>
        /// <param name="credential"> API 调用凭据 </param>
        public SmsSender(Credential credential)
        {
            this.SmsClient = new SmsClient(credential, "ap-guangzhou");
        }
        /// <summary>
        /// 腾讯云短信发送封装
        /// </summary>
        /// <param name="secretId"> API 调用者身份标识，可以简单类比为用户名 </param>
        /// <param name="secretKey"> API 调用者身份验证，可以简单类比为密码 </param>
        public SmsSender(string secretId, string secretKey) : this(new Credential() { SecretId = secretId, SecretKey = secretKey })
        {
        }
        /// <summary>
        /// 腾讯云短信发送
        /// </summary>
        /// <param name="sdkAppId"> SDK AppID是短信应用的唯一标识，调用短信API接口时需要提供该参数。 </param>
        /// <param name="secretId"> API 调用者身份标识，可以简单类比为用户名 </param>
        /// <param name="secretKey"> API 调用者身份验证，可以简单类比为密码 </param>
        public SmsSender(string sdkAppId, string secretId, string secretKey) : this(secretId, secretKey)
        {
            this.SdkAppID = sdkAppId;
        }

        /// <summary>
        /// （原生方式）短信发送接口，用户给用户发短信验证码、通知类短信或营销短信。
        /// </summary>
        /// <param name="templateID"> 模板 ID，必须填写已审核通过的模板 ID </param>
        /// <param name="sign"> 短信签名内容，必须填写已审核通过的签名，国内短信为必填参数 </param>
        /// <param name="phoneNumbers">
        ///   <para> 下发手机号码，采用 e.164 标准，格式为+[国家或地区码][手机号]，单次请求最多支持200个手机号且要求全为境内手机号或全为境外手机号。 </para> 
        ///   <para> 例如：+8613711112222， 其中前面有一个+号 ，86为国家码，13711112222为手机号。 </para>
        /// </param>
        /// <param name="templateParams"> 模板参数，若无模板参数，则设置为空 </param>
        /// <param name="extendCode"> 短信码号扩展号，默认未开通 </param>
        /// <param name="senderId"> 国际/港澳台短信 senderid，国内短信填空，默认未开通 </param>
        /// <param name="sessionContext"> 用户的 session 内容，可以携带用户侧 ID 等上下文信息，server 会原样返回 </param>
        /// <param name="sdkAppid"> （若已提供默认值，该参数可选）短信SdkAppid在 短信控制台 添加应用后生成的实际SdkAppid </param>
        public SendSmsResponse Send(string templateID, string sign, string[] phoneNumbers, string[] templateParams = null, string extendCode = null, string senderId = null, string sessionContext = null, string sdkAppid = null)
        {
            if (SdkAppID.IsNullOrEmpty() && sdkAppid.IsNullOrEmpty())
                throw new ArgumentException("在创建 SmsSender 实例时未提供默认的 SdkAppid，调用 Send 发送短信需要提供 sdkAppid 参数", nameof(sdkAppid));

            SendSmsRequest req = new SendSmsRequest();
            req.SmsSdkAppid = sdkAppid ?? this.SdkAppID;
            req.PhoneNumberSet = phoneNumbers;
            req.TemplateID = templateID;
            req.TemplateParamSet = templateParams ?? new string[0];

            req.Sign = sign;
            req.ExtendCode = extendCode;
            req.SenderId = senderId;
            req.SessionContext = sessionContext;

            return Send(req);
        }
        /// <summary>
        /// （原生方式）短信发送接口，用户给用户发短信验证码、通知类短信或营销短信。
        /// </summary>
        public SendSmsResponse Send(SendSmsRequest request)
            => SmsClient.SendSmsSync(request);

    }

    /// <summary>
    /// 腾讯云短信发送封装
    /// </summary>
    public class SmsSenderPackage : SmsSender
    {
        private TencentCloudOptions TencentCloudOptions;

        /// <summary>
        /// 腾讯云短信发送封装
        /// </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="options"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="options"/> 对象的 <see cref="TencentCloudOptions.Sms"/> 引用不能为 null </exception>
        public SmsSenderPackage(TencentCloudOptions options) : base(options?.Sms?.SdkAppId, options?.SecretId, options?.SecretKey)
        {
            if (options == null) throw new ArgumentNullException($"初始化参数 {nameof(options)} 不能为 null", nameof(options));
            if (options.SecretId.IsNullOrEmpty()) throw new ArgumentException($"初始化对象参数 {nameof(options.SecretId)} 不能为 null 或 空", nameof(options.SecretId));
            if (options.SecretKey.IsNullOrEmpty()) throw new ArgumentException($"初始化对象参数 {nameof(options.SecretKey)} 不能为 null 或", nameof(options.SecretKey));
            if (options.Sms == null) throw new ArgumentException($"初始化对象参数 Sms 的引用不能为 null", nameof(options.Sms));
            if (options.Sms.SdkAppId.IsNullOrEmpty()) throw new ArgumentException($"初始化对象参数 {nameof(options.Sms.SdkAppId)} 不能为 null 或", nameof(options.Sms.SdkAppId));

            this.TencentCloudOptions = options;
        }

        /// <summary>
        /// 从配置中获取对应键值 模板 ID 的 TemplateID
        /// </summary>
        /// <param name="key"> 在配置中设定的模板 key 键 </param>
        /// <returns> 模板 ID </returns>
        public string GetTemplateID(string key)
            => TencentCloudOptions.Sms.Templates[key];

        /// <summary>
        /// （封装方法）发送无参数的国内模板短信
        /// </summary>
        /// <param name="templateID"> 模板 ID，必须填写已审核通过的模板 ID </param>
        /// <param name="phoneNumbers">
        ///   <para> 下发手机号码，采用 e.164 标准，格式为+[国家或地区码][手机号]，单次请求最多支持200个手机号且要求全为境内手机号或全为境外手机号。 </para> 
        ///   <para> 例如：+8613711112222， 其中前面有一个+号 ，86为国家码，13711112222为手机号。 </para>
        /// </param>
        public SendSmsResponse SendTemplate(string templateID, params string[] phoneNumbers)
            => SendTemplateWithSign(templateID, TencentCloudOptions.Sms.Sign, phoneNumbers);
        /// <summary>
        /// （封装方法）发送无参数的国内模板短信
        /// </summary>
        /// <param name="templateID"> 模板 ID，必须填写已审核通过的模板 ID </param>
        /// <param name="sign"> 短信签名内容，必须填写已审核通过的签名，国内短信为必填参数 </param>
        /// <param name="phoneNumbers">
        ///   <para> 下发手机号码，采用 e.164 标准，格式为+[国家或地区码][手机号]，单次请求最多支持200个手机号且要求全为境内手机号或全为境外手机号。 </para> 
        ///   <para> 例如：+8613711112222， 其中前面有一个+号 ，86为国家码，13711112222为手机号。 </para>
        /// </param>
        public SendSmsResponse SendTemplateWithSign(string templateID, string sign, params string[] phoneNumbers)
            => Send(templateID, sign, phoneNumbers);


        /// <summary>
        /// （封装方法）发送带参数的国内模板短信到手机号
        /// </summary>
        /// <param name="templateID"> 模板 ID，必须填写已审核通过的模板 ID </param>
        /// <param name="phoneNumber">
        ///   <para> 下发手机号码，如传入中国 11 位手机号码，会先行补充 +86 前缀以符合 e.164 标准。 </para> 
        ///   <para> 下发手机号码，采用 e.164 标准，格式为+[国家或地区码][手机号]，单次请求最多支持200个手机号且要求全为境内手机号或全为境外手机号。 </para> 
        ///   <para> 例如：+8613711112222， 其中前面有一个+号 ，86为国家码，13711112222为手机号。 </para>
        /// </param>
        /// <param name="templateParams"> 模板参数，若无模板参数，则设置为空 </param>
        public SendSmsResponse SendToPhone(string templateID, string phoneNumber, params string[] templateParams)
            => SendToPhoneWithSign(templateID, TencentCloudOptions.Sms.Sign, phoneNumber, templateParams);
        /// <summary>
        /// （封装方法）发送带参数的国内模板短信到手机号
        /// </summary>
        /// <param name="templateID"> 模板 ID，必须填写已审核通过的模板 ID </param>
        /// <param name="sign"> 短信签名内容，必须填写已审核通过的签名，国内短信为必填参数 </param>
        /// <param name="phoneNumber">
        ///   <para> 下发手机号码，如传入中国 11 位手机号码，会先行补充 +86 前缀以符合 e.164 标准。 </para> 
        ///   <para> 下发手机号码，采用 e.164 标准，格式为+[国家或地区码][手机号]，单次请求最多支持200个手机号且要求全为境内手机号或全为境外手机号。 </para> 
        ///   <para> 例如：+8613711112222， 其中前面有一个+号 ，86为国家码，13711112222为手机号。 </para>
        /// </param>
        /// <param name="templateParams"> 模板参数，若无模板参数，则设置为空 </param>
        public SendSmsResponse SendToPhoneWithSign(string templateID, string sign, string phoneNumber, params string[] templateParams)
        {
            if (phoneNumber.FormatIsMobilePhone())
                phoneNumber = $"+86{phoneNumber.Trim()}";
            return Send(templateID, sign, new[] { phoneNumber }, templateParams);
        }


    }


}
