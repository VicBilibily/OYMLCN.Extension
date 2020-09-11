using System.Linq;
using Newtonsoft.Json;

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
            Interface = @interface;
            Target = target;
            Action = action;

            Paramters = args;
            if (args?.Length == 1)
            {
                var arg = args.First();
                if (arg?.GetType().IsClass ?? false)
                    Paramters = arg;
            }
        }

        /// <summary>
        /// 调用上下文信息
        /// </summary>
        [JsonProperty(PropertyName = "sessions")]
        [RpcProperty(Description = "请求上下文必要信息")]
        public object Sessions { get; set; }
        /// <summary>
        /// Rpc Token
        /// </summary>
        [JsonProperty(PropertyName = "token")]
        [RpcProperty(Description = "用于身份认证的Token")]
        public string Token { get; set; }

        /// <summary>
        /// 调用接口名称
        /// </summary>
        [JsonProperty(PropertyName = "interface")]
        [RpcProperty(Description = "调用接口名称")]
        public string Interface { get; set; }
        /// <summary>
        /// 目标类型名称
        /// </summary>
        [JsonProperty(PropertyName = "target")]
        [RpcProperty(Description = "调用目标类型", Require = true)]
        public string Target { get; set; }
        /// <summary>
        /// 目标名称（调用的目标方法）
        /// </summary>
        [JsonProperty(PropertyName = "action")]
        [RpcProperty(Description = "调用目标名称", Require = true)]
        public string Action { get; set; }
        /// <summary>
        /// 调用参数（调用目标的参数）
        /// </summary>
        [JsonProperty(PropertyName = "params")]
        [RpcProperty(Description = "调用参数（如果调用方法有参数则必须提供）")]
        public object Paramters { get; set; }

        /// <summary>
        /// 获取远程调用目标方法的请求及响应结构信息
        /// </summary>
        [JsonProperty(PropertyName = "gsinfo")]
        [RpcProperty(Description = "获取远程调用目标方法的请求及响应结构信息")]
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
