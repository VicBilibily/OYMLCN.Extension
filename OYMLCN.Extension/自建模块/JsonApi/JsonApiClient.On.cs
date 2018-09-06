#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
using OYMLCN.Extensions;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.JsonApi
{
    public partial class JsonApiClient
    {
        /// <summary>
        /// 代码回应处理
        /// </summary>
        /// <param name="statuCode"></param>
        /// <param name="handler"></param>
        public void On(int statuCode, Action<ResponseResult<PartialResponse[]>> handler) => ResponseHandlers[statuCode] = handler;

        /// <summary>
        /// 回应处理
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="handler"></param>
        public void On(string methodName, Action<PartialResponse> handler) => PartialHandlers[methodName] = handler;

        /// <summary>
        /// 回应处理
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="handler"></param>
        public void On(string methodName, Action handler) => On(methodName, args => handler());
        public void On<T1>(string methodName, Action<T1> handler) => On(methodName, args =>
        {
            var arr = new PartialResponse<T1>(args);
            handler(arr.GetT1);
        });
        public void On<T1, T2>(string methodName, Action<T1, T2> handler) => On(methodName, args =>
        {
            var arr = new PartialResponse<T1, T2>(args);
            handler(arr.GetT1, arr.GetT2);
        });
        public void On<T1, T2, T3>(string methodName, Action<T1, T2, T3> handler) => On(methodName, args =>
        {
            var arr = new PartialResponse<T1, T2, T3>(args);
            handler(arr.GetT1, arr.GetT2, arr.GetT3);
        });
        public void On<T1, T2, T3, T4>(string methodName, Action<T1, T2, T3, T4> handler) => On(methodName, args =>
        {
            var arr = new PartialResponse<T1, T2, T3, T4>(args);
            handler(arr.GetT1, arr.GetT2, arr.GetT3, arr.GetT4);
        });
        public void On<T1, T2, T3, T4, T5>(string methodName, Action<T1, T2, T3, T4, T5> handler) => On(methodName, args =>
        {
            var arr = new PartialResponse<T1, T2, T3, T4, T5>(args);
            handler(arr.GetT1, arr.GetT2, arr.GetT3, arr.GetT4, arr.GetT5);
        });
        public void On<T1, T2, T3, T4, T5, T6>(string methodName, Action<T1, T2, T3, T4, T5, T6> handler) => On(methodName, args =>
        {
            var arr = new PartialResponse<T1, T2, T3, T4, T5, T6>(args);
            handler(arr.GetT1, arr.GetT2, arr.GetT3, arr.GetT4, arr.GetT5, arr.GetT6);
        });

        /// <summary>
        /// 调用接口方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void Invoke(string methodName, object obj) => InvokeAsync(methodName, obj).Wait();
        /// <summary>
        /// 调用接口方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<ResponseResult<PartialResponse[]>> InvokeAsync(string methodName, object obj)
        {
            return await await HttpClient
                .PostAsync($"{HubUrl}/{methodName}", new StringContent(obj.ToJsonString(), Encoding.UTF8, "application/json"))
                .ContinueWith((rsp) =>
                {
                    ResponseResult<PartialResponse[]> result;
                    try
                    {
                        return rsp.Result.EnsureSuccessStatusCode().Content.ReadAsStringAsync().ContinueWith((str) =>
                        {
                            result = str.Result.DeserializeJsonToObject<ResponseResult<PartialResponse[]>>();
                            if (result.code == 0)
                                foreach (var item in result.data)
                                    PartialHandlers.SelectValueOrDefault(item.name)?.Invoke(item);
                            else
                                ResponseHandlers.SelectValueOrDefault(result.code)?.Invoke(result);
                            return result;
                        });
                    }
                    catch (HttpRequestException ex)
                    {
                        result = new ResponseResult<PartialResponse[]>()
                        {
                            code = -1,
                            msg = ex.Message,
                            data = new PartialResponse[] {
                                new PartialResponse()
                                {
                                    code = (int)rsp.Result.StatusCode,
                                    msg = rsp.Result.StatusCode.ToString()
                                }
                            }
                        };
                    }
                    catch (Exception ex)
                    {
                        result = new ResponseResult<PartialResponse[]>()
                        {
                            code = -1,
                            msg = ex.Message
                        };
                    }
                    result.source = methodName;
                    ResponseHandlers.SelectValueOrDefault(result.code)?.Invoke(result);
                    return Task.FromResult(result);
                });
        }

    }
}
