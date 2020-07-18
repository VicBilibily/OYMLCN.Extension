using OYMLCN.ArgumentChecker;
using OYMLCN.RPC.Core;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OYMLCN.RPC
{
    /// <summary>
    /// 远程调用客户端
    /// </summary>
    public class RpcClient
    {
        private HttpClient HttpClient;
        /// <summary>
        /// 远程调用客户端
        /// </summary>
        /// <param name="httpClient"> <see cref="HttpClient"/> 实例 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="httpClient"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="httpClient"/> 的 <see cref="HttpClient.BaseAddress"/> 不能为 null </exception>
        public RpcClient(HttpClient httpClient)
        {
            httpClient.ThrowIfNull(nameof(httpClient), $"{nameof(HttpClient)} 不能为 null");
            if (httpClient.BaseAddress == null)
                throw new ArgumentException($"{nameof(HttpClient)} 的 {nameof(HttpClient.BaseAddress)} 不能为 null，您需要为 {nameof(HttpClient)} 提供发送请求时使用的 Internet 资源的统一资源标识符 (URI) 的基址 {nameof(HttpClient.BaseAddress)}", nameof(httpClient));
            this.HttpClient = httpClient;
        }

        /// <summary>
        /// 远程目标调用
        /// </summary>
        /// <typeparam name="TResult"> 返回数据的类型 </typeparam>
        /// <param name="target"> 调用目标类型 </param>
        /// <param name="action"> 调用目标名称 </param>
        /// <param name="args"> 调用目标参数 </param>
        public async Task<TResult> InvokeAsync<TResult>(string target, string action, params object[] args) where TResult : class, new()
            => await InvokeAsync<TResult>(null, target, action, args);
        /// <summary>
        /// 远程目标调用
        /// </summary>
        /// <typeparam name="TResult"> 返回数据的类型 </typeparam>
        /// <param name="interface"> 调用目标接口 </param>
        /// <param name="target"> 调用目标类型 </param>
        /// <param name="action"> 调用目标名称 </param>
        /// <param name="args"> 调用目标参数 </param>
        public async Task<TResult> InvokeAsync<TResult>(string @interface, string target, string action, params object[] args) where TResult : class, new()
            => await InvokeAsync<TResult>(new RequestModel(@interface, target, action, args));
        /// <summary>
        /// 远程目标调用
        /// </summary>
        /// <typeparam name="TResult"> 返回数据的类型 </typeparam>
        /// <param name="requestModel"> 调用目标数据模型 </param>
        public async Task<TResult> InvokeAsync<TResult>(RequestModel requestModel) where TResult : class, new()
        {
            requestModel.ThrowIfNull(nameof(requestModel));

            HttpContent httpContent = new StringContent(requestModel.ToJson());
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var responseMessage = await HttpClient.PostAsync("/", httpContent);
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                string result = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (!string.IsNullOrEmpty(result))
                {
                    ResponseModel responseModel = result.FromJson<ResponseModel>();
                    if (responseModel.Code != 0)
                        throw new Exception($"请求出错,返回内容:{result}");
                    if (responseModel.Data != null)
                        return responseModel.Data.ToJson().FromJson<TResult>();
                    return default;
                }
            }
            throw new Exception($"请求异常,StatusCode:{responseMessage.StatusCode}");
        }
    }
}
