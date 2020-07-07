using Newtonsoft.Json;

namespace OYMLCN.RPC.Core
{
    /// <summary>
    /// 过程调用请求数据模型
    /// </summary>
    public sealed class RequestModel
    {
        /// <summary>
        /// 调用接口名称
        /// </summary>
        [JsonProperty(PropertyName = "interface")]
        public string Interface { get; set; }
        /// <summary>
        /// 目标类型名称
        /// </summary>
        [JsonProperty(PropertyName = "target")]
        public string Target { get; set; }
        /// <summary>
        /// 目标名称（调用的目标方法）
        /// </summary>
        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }
        /// <summary>
        /// 调用参数（调用所使用的必要参数）
        /// </summary>
        [JsonProperty(PropertyName = "params")]
        public object Paramters { get; set; }
    }

    /// <summary>
    /// 过程调用结果
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// 过程调用响应代码
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }
        /// <summary>
        /// 请求处理时间(ms)
        /// </summary>
        [JsonProperty(PropertyName = "time")]
        public double? Time { get; set; }
        /// <summary>
        /// 过程调用响应信息
        /// </summary>
        [JsonProperty(PropertyName = "msg")]
        public string Message { get; set; }
        /// <summary>
        /// 过程调用响应数据
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }
    }
}
