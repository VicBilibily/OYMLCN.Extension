using Newtonsoft.Json;
using System.Linq;

namespace OYMLCN.RPC.Core
{
    /// <summary>
    /// 过程调用请求数据模型
    /// </summary>
    public sealed class RequestModel
    {
        /// <summary>
        /// 过程调用请求数据模型
        /// </summary>
        public RequestModel() { }
        /// <summary>
        /// 过程调用请求数据模型
        /// </summary>
        /// <param name="target"> 目标类型（或全限定名称） </param>
        /// <param name="action"> 调用方法名称 </param>
        /// <param name="args"> 调用参数（若有则传入） </param>
        public RequestModel(string target, string action, object[] args = null) : this(null, target, action, args) { }
        /// <summary>
        /// 过程调用请求数据模型
        /// </summary>
        /// <param name="interface"> 接口名称（或全限定名称） </param>
        /// <param name="target"> 目标类型（或全限定名称） </param>
        /// <param name="action"> 调用方法名称 </param>
        /// <param name="args"> 调用参数（若有则传入） </param>
        public RequestModel(string @interface, string target, string action, object[] args = null)
        {
            this.Interface = @interface;
            this.Target = target;
            this.Action = action;

            this.Paramters = args;
            if (args.Length == 1)
            {
                var arg = args.First();
                if (arg?.GetType().IsClass ?? false)
                    this.Paramters = arg;
            }
        }

        /// <summary>
        /// 调用上下文信息
        /// </summary>
        [JsonProperty(PropertyName = "sessions")]
        public object Sessions { get; set; }

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

        /// <summary>
        /// 获取远程调用目标方法的请求及响应结构信息
        /// </summary>
        [JsonProperty(PropertyName = "gsinfo")]
        public bool GetStructInfoData { get; set; }
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
