using OYMLCN.ArgumentChecker;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// WebClientExtension
    /// </summary>
    public static partial class WebClientExtensions
    {
        #region public static string UploadUTF8FormString(this WebClient webClient, string address, string data)
        /// <summary>
        /// 使用 POST 方法将指定的 <paramref name="data"/> 表单数据字符串上载到 <paramref name="address"/> 指定的地址
        /// </summary>
        /// <param name="webClient"> <see cref="WebClient"/> 的实例 </param>
        /// <param name="address"> 要发送数据字符串资源到达的 URI，此 URI 必须可以接受使用 POST 方法发送的请求。 </param>
        /// <param name="data"> 要上载的表单字符串 </param>
        /// <returns> 服务器返回的响应 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="webClient"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="address"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <see cref="WebClient.BaseAddress"/> 为空时 <paramref name="address"/> 不能为空 </exception>
        /// <exception cref="WebException"> 通过组合 <see cref="WebClient.BaseAddress"/> 和 <paramref name="address"/> 所构成的 URI 无效 </exception>
        /// <exception cref="WebException"> 承载资源的服务器没有响应 </exception>
        public static string UploadUTF8FormString(this WebClient webClient, string address, string data)
        {
            webClient.ThrowIfNull(nameof(webClient));
            address.ThrowIfNull(nameof(address));
            data.ThrowIfNull(nameof(data));
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=utf-8");
            return webClient.UploadString(address, HttpMethod.Post.Method, data);
        }
#if Xunit
        [Fact]
        public static void UploadUTF8FormStringTest()
        {
            WebClient webClient = null;
            Assert.Throws<ArgumentNullException>(() => webClient.UploadUTF8FormString("Url", "Data"));

            //// 会发起请求，注释部分已测试通过

            webClient = new WebClient();
            Assert.Throws<ArgumentNullException>(() => webClient.UploadUTF8FormString(null, "Data"));
            Assert.Throws<ArgumentException>(() => webClient.UploadUTF8FormString(string.Empty, "Data"));

            webClient.BaseAddress = "https://www.baidu.com/";
            Assert.Throws<ArgumentNullException>(() => webClient.UploadUTF8FormString(string.Empty, null));

            webClient.BaseAddress = "ssh://www.baidu.com";
            Assert.Throws<WebException>(() => webClient.UploadUTF8FormString("", ""));

            //webClient.BaseAddress = "http://nothing.unknow.com/";
            //Assert.Throws<WebException>(() => webClient.UploadUTF8FormString("NotFound", ""));

            webClient.Dispose();
        }
#endif
        #endregion

        #region public static void UploadUTF8FormStringAsync(this WebClient webClient, Uri address, string data)
#pragma warning disable xUnit1013 // Public method should be marked as test
        /// <summary>
        /// 使用 POST 方法将指定的 <paramref name="data"/> 表单数据字符串上载到 <paramref name="address"/> 指定的地址
        /// <para> 此方法不会阻止调用线程。 </para>
        /// <para> 异步回调同 <see cref="WebClient.UploadStringAsync(Uri, string, string)"/>， </para>
        /// <para> 将会调用 <see cref="WebClient.OnUploadStringCompleted(UploadStringCompletedEventArgs)"/>。 </para>
        /// </summary>
        /// <param name="webClient"> <see cref="WebClient"/> 的实例 </param>
        /// <param name="address"> 要发送数据字符串资源到达的 URI，此 URI 必须可以接受使用 POST 方法发送的请求。 </param>
        /// <param name="data"> 要上载的表单字符串 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="webClient"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="address"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> 不能为 null </exception>
        public static void UploadUTF8FormStringAsync(this WebClient webClient, Uri address, string data)
        {
            webClient.ThrowIfNull(nameof(webClient));
            address.ThrowIfNull(nameof(address));
            data.ThrowIfNull(nameof(data));
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=utf-8");
            webClient.UploadStringAsync(address, HttpMethod.Post.Method, data);
        }
#pragma warning restore xUnit1013 // Public method should be marked as test
#if Xunit
        [Fact]
        public static void UploadUTF8FormStringAsyncTest()
        {
            WebClient webClient = null;
            Assert.Throws<ArgumentNullException>(() => webClient.UploadUTF8FormStringAsync(new Uri("http://www.baidu.com"), "Data"));

            webClient = new WebClient();
            Assert.Throws<ArgumentNullException>(() => webClient.UploadUTF8FormStringAsync(null, "Data"));
            Assert.Throws<ArgumentNullException>(() => webClient.UploadUTF8FormStringAsync(new Uri("http://www.baidu.com"), null));

            webClient.Dispose();
        }
#endif 
        #endregion

        #region public static Task<string> UploadUTF8FormStringAsync(this WebClient webClient, string address, string data)
        /// <summary>
        /// 使用异步方式及 POST 方法将指定的 <paramref name="data"/> 表单数据字符串上载到 <paramref name="address"/> 指定的地址
        /// </summary>
        /// <param name="webClient"> <see cref="WebClient"/> 的实例 </param>
        /// <param name="address"> 要发送数据字符串资源到达的 URI，此 URI 必须可以接受使用 POST 方法发送的请求。 </param>
        /// <param name="data"> 要上载的表单字符串 </param>
        /// <returns> 服务器异步返回的响应 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="webClient"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="address"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <see cref="WebClient.BaseAddress"/> 为空时 <paramref name="address"/> 不能为空 </exception>
        /// <exception cref="WebException"> 通过组合 <see cref="WebClient.BaseAddress"/> 和 <paramref name="address"/> 所构成的 URI 无效 </exception>
        /// <exception cref="WebException"> 承载资源的服务器没有响应 </exception>
        public static Task<string> UploadUTF8FormStringTaskAsync(this WebClient webClient, string address, string data)
        {
            webClient.ThrowIfNull(nameof(webClient));
            address.ThrowIfNull(nameof(address));
            data.ThrowIfNull(nameof(data));
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded;charset=utf-8");
            return webClient.UploadStringTaskAsync(address.ConvertToUri(), HttpMethod.Post.Method, data);
        }
#if Xunit
        [Fact]
        public static void UploadUTF8FormStringTaskAsyncTest()
        {
            WebClient webClient = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => webClient.UploadUTF8FormStringTaskAsync("Url", "Data"));

            //// 会发起请求，注释部分已测试通过

            webClient = new WebClient();
            Assert.ThrowsAsync<ArgumentNullException>(() => webClient.UploadUTF8FormStringTaskAsync(null, "Data"));
            Assert.ThrowsAsync<ArgumentException>(() => webClient.UploadUTF8FormStringTaskAsync(string.Empty, "Data"));

            webClient.BaseAddress = "https://www.baidu.com/";
            Assert.ThrowsAsync<ArgumentNullException>(() => webClient.UploadUTF8FormStringTaskAsync(string.Empty, null));

            webClient.BaseAddress = "ssh://www.baidu.com";
            Assert.ThrowsAsync<WebException>(() => webClient.UploadUTF8FormStringTaskAsync("", ""));

            //webClient.BaseAddress = "http://nothing.unknow.com/";
            //Assert.ThrowsAsync<WebException>(() => webClient.UploadUTF8FormStringTaskAsync("NotFound", ""));

            webClient.Dispose();
        }
#endif
        #endregion


        #region public static string UploadUTF8JsonString(this WebClient webClient, string address, string json)
        /// <summary>
        /// 使用 POST 方法将指定的 <paramref name="json"/> 数据字符串上载到 <paramref name="address"/> 指定的地址
        /// </summary>
        /// <param name="webClient"> <see cref="WebClient"/> 的实例 </param>
        /// <param name="address"> 要发送数据字符串资源到达的 URI，此 URI 必须可以接受使用 POST 方法发送的请求。 </param>
        /// <param name="json"> 要上载的 JSON 字符串 </param>
        /// <returns> 服务器返回的响应 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="webClient"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="address"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="json"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <see cref="WebClient.BaseAddress"/> 为空时 <paramref name="address"/> 不能为空 </exception>
        /// <exception cref="WebException"> 通过组合 <see cref="WebClient.BaseAddress"/> 和 <paramref name="address"/> 所构成的 URI 无效 </exception>
        /// <exception cref="WebException"> 承载资源的服务器没有响应 </exception>
        public static string UploadUTF8JsonString(this WebClient webClient, string address, string json)
        {
            webClient.ThrowIfNull(nameof(webClient));
            address.ThrowIfNull(nameof(address));
            json.ThrowIfNull(nameof(json));
            webClient.Headers.Add("Content-Type", "application/json;charset=utf-8");
            return webClient.UploadString(address, HttpMethod.Post.Method, json);
        }
#if Xunit
        [Fact]
        public static void UploadUTF8JsonStringTest()
        {
            WebClient webClient = null;
            Assert.Throws<ArgumentNullException>(() => webClient.UploadUTF8JsonString("Url", "Data"));

            //// 会发起请求，注释部分已测试通过

            webClient = new WebClient();
            Assert.Throws<ArgumentNullException>(() => webClient.UploadUTF8JsonString(null, "Data"));
            Assert.Throws<ArgumentException>(() => webClient.UploadUTF8JsonString(string.Empty, "Data"));

            webClient.BaseAddress = "https://www.baidu.com/";
            Assert.Throws<ArgumentNullException>(() => webClient.UploadUTF8JsonString(string.Empty, null));

            webClient.BaseAddress = "ssh://www.baidu.com";
            Assert.Throws<WebException>(() => webClient.UploadUTF8JsonString("", ""));

            //webClient.BaseAddress = "http://nothing.unknow.com/";
            //Assert.Throws<WebException>(() => webClient.UploadUTF8JsonString("NotFound", ""));

            webClient.Dispose();
        }
#endif
        #endregion

        #region public static void UploadUTF8JsonStringAsync(this WebClient webClient, Uri address, string json)
#pragma warning disable xUnit1013 // Public method should be marked as test
        /// <summary>
        /// 使用 POST 方法将指定的 <paramref name="json"/> 数据字符串上载到 <paramref name="address"/> 指定的地址
        /// <para> 此方法不会阻止调用线程。 </para>
        /// <para> 异步回调同 <see cref="WebClient.UploadStringAsync(Uri, string, string)"/>， </para>
        /// <para> 将会调用 <see cref="WebClient.OnUploadStringCompleted(UploadStringCompletedEventArgs)"/>。 </para>
        /// </summary>
        /// <param name="webClient"> <see cref="WebClient"/> 的实例 </param>
        /// <param name="address"> 要发送数据字符串资源到达的 URI，此 URI 必须可以接受使用 POST 方法发送的请求。 </param>
        /// <param name="json"> 要上载的 JSON 字符串 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="webClient"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="address"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="json"/> 不能为 null </exception>
        public static void UploadUTF8JsonStringAsync(this WebClient webClient, Uri address, string json)
        {
            webClient.ThrowIfNull(nameof(webClient));
            address.ThrowIfNull(nameof(address));
            json.ThrowIfNull(nameof(json));
            webClient.Headers.Add("Content-Type", "application/json;charset=utf-8");
            webClient.UploadStringTaskAsync(address, HttpMethod.Post.Method, json);
        }
#pragma warning restore xUnit1013 // Public method should be marked as test
#if Xunit
        [Fact]
        public static void UploadUTF8JsonStringAsyncTest()
        {
            WebClient webClient = null;
            Assert.Throws<ArgumentNullException>(() => webClient.UploadUTF8JsonStringAsync(new Uri("http://www.baidu.com"), "Data"));

            webClient = new WebClient();
            Assert.Throws<ArgumentNullException>(() => webClient.UploadUTF8JsonStringAsync(null, "Data"));
            Assert.Throws<ArgumentNullException>(() => webClient.UploadUTF8JsonStringAsync(new Uri("http://www.baidu.com"), null));

            webClient.Dispose();
        }
#endif
        #endregion

        #region public static Task<string> UploadUTF8JsonStringTaskAsync(this WebClient webClient, string address, string json)
        /// <summary>
        /// 使用异步方式及 POST 方法将指定的 <paramref name="json"/> 数据字符串上载到 <paramref name="address"/> 指定的地址
        /// </summary>
        /// <param name="webClient"> <see cref="WebClient"/> 的实例 </param>
        /// <param name="address"> 要发送数据字符串资源到达的 URI，此 URI 必须可以接受使用 POST 方法发送的请求。 </param>
        /// <param name="json"> 要上载的 JSON 字符串 </param>
        /// <returns> 服务器返回的响应 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="webClient"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="address"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="json"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <see cref="WebClient.BaseAddress"/> 为空时 <paramref name="address"/> 不能为空 </exception>
        /// <exception cref="WebException"> 通过组合 <see cref="WebClient.BaseAddress"/> 和 <paramref name="address"/> 所构成的 URI 无效 </exception>
        /// <exception cref="WebException"> 承载资源的服务器没有响应 </exception>
        public static Task<string> UploadUTF8JsonStringTaskAsync(this WebClient webClient, string address, string json)
        {
            webClient.ThrowIfNull(nameof(webClient));
            address.ThrowIfNull(nameof(address));
            json.ThrowIfNull(nameof(json));
            webClient.Headers.Add("Content-Type", "application/json;charset=utf-8");
            return webClient.UploadStringTaskAsync(address, HttpMethod.Post.Method, json);
        }
#if Xunit
        [Fact]
        public static void UploadUTF8JsonStringTaskAsyncTest()
        {
            WebClient webClient = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => webClient.UploadUTF8JsonStringTaskAsync("Url", "Data"));

            //// 会发起请求，注释部分已测试通过

            webClient = new WebClient();
            Assert.ThrowsAsync<ArgumentNullException>(() => webClient.UploadUTF8JsonStringTaskAsync(null, "Data"));
            Assert.ThrowsAsync<ArgumentException>(() => webClient.UploadUTF8JsonStringTaskAsync(string.Empty, "Data"));

            webClient.BaseAddress = "https://www.baidu.com/";
            Assert.ThrowsAsync<ArgumentNullException>(() => webClient.UploadUTF8JsonStringTaskAsync(string.Empty, null));

            webClient.BaseAddress = "ssh://www.baidu.com";
            Assert.ThrowsAsync<WebException>(() => webClient.UploadUTF8JsonStringTaskAsync("", ""));

            //webClient.BaseAddress = "http://nothing.unknow.com/";
            //Assert.ThrowsAsync<WebException>(() => webClient.UploadUTF8JsonStringTaskAsync("NotFound", ""));

            webClient.Dispose();
        }
#endif
        #endregion

        #region public static T UploadUTF8JsonString<T>(this WebClient webClient, string address, string json)
        /// <summary>
        /// 使用 POST 方法将指定的 <paramref name="json"/> 数据字符串上载到 <paramref name="address"/> 指定的地址
        /// </summary>
        /// <typeparam name="T"> 泛型类 </typeparam>
        /// <param name="webClient"> <see cref="WebClient"/> 的实例 </param>
        /// <param name="address"> 要发送数据字符串资源到达的 URI，此 URI 必须可以接受使用 POST 方法发送的请求。 </param>
        /// <param name="json"> 要上载的 JSON 字符串 </param>
        /// <returns> 服务器返回的 JSON 响应 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="webClient"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="address"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="json"/> 不能为 null </exception>
        /// <exception cref="JsonReaderException"> 服务器返回的内容不是有效的 JSON 字符串 </exception>
        public static T UploadUTF8JsonString<T>(this WebClient webClient, string address, string json)
            => webClient.UploadUTF8JsonString(address, json).JsonDeserialize<T>();
        #endregion

        #region public static Task<T> UploadUTF8JsonStringTaskAsync<T>(this WebClient webClient, string address, string json) where T : class, new()
        /// <summary>
        /// 使用异步方式及 POST 方法将指定的 <paramref name="json"/> 数据字符串上载到 <paramref name="address"/> 指定的地址
        /// </summary>
        /// <typeparam name="T"> 泛型类 </typeparam>
        /// <param name="webClient"> <see cref="WebClient"/> 的实例 </param>
        /// <param name="address"> 要发送数据字符串资源到达的 URI，此 URI 必须可以接受使用 POST 方法发送的请求。 </param>
        /// <param name="json"> 要上载的 JSON 字符串 </param>
        /// <returns> 服务器返回的 JSON 响应 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="webClient"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="address"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="json"/> 不能为 null </exception>
        /// <exception cref="JsonReaderException"> 服务器返回的内容不是有效的 JSON 字符串 </exception>
        public static Task<T> UploadUTF8JsonStringTaskAsync<T>(this WebClient webClient, string address, string json) where T : class, new()
            => webClient.UploadUTF8JsonStringTaskAsync(address, json)
                .ContinueWith(task => task.IsCompleted ? task.Result.JsonDeserialize<T>() : default(T));
        #endregion


        #region public static T DownloadJson<T>(this WebClient webClient, string address) where T : class, new()
        /// <summary>
        /// 从 <paramref name="address"/> URI 下载的 JSON 形式的资源并反序列化为实体类
        /// </summary>
        /// <typeparam name="T"> 泛型类 </typeparam>
        /// <param name="webClient"> <see cref="WebClient"/> 的实例 </param>
        /// <param name="address"> 要发送数据字符串资源到达的 URI </param>
        /// <returns> 从服务器获取的 JSON 响应 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="webClient"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="address"/> 不能为 null </exception>
        /// <exception cref="NotSupportedException"> 该方法已在多个线程上同时调用 </exception>
        /// <exception cref="WebException"> 通过组合 <see cref="WebClient.BaseAddress"/> 和 <paramref name="address"/> 所构成的 URI 无效 </exception>
        /// <exception cref="WebException"> 承载资源的服务器没有响应 </exception>
        /// <exception cref="JsonReaderException"> 从服务器获取的响应内容不是有效的 JSON 字符串 </exception>
        public static T DownloadJson<T>(this WebClient webClient, string address) where T : class, new()
        {
            webClient.ThrowIfNull(nameof(webClient));
            address.ThrowIfNull(nameof(address));

            var result = webClient.DownloadString(address);
            return result.JsonDeserialize<T>();
        }
#if Xunit
        public class TestClass { }
        [Fact]
        public static void DownloadJsonTest()
        {
            WebClient webClient = null;
            Assert.Throws<ArgumentNullException>(() => webClient.DownloadJson<TestClass>("Url"));

            //// 会发起请求，注释部分已测试通过

            webClient = new WebClient();
            Assert.Throws<ArgumentNullException>(() => webClient.DownloadJson<TestClass>(null));
            Assert.Throws<WebException>(() => webClient.DownloadJson<TestClass>("Url"));
            //webClient.BaseAddress = "http://nothing.unknow.com/";
            //Assert.Throws<WebException>(() => webClient.DownloadJson<TestClass>("NotFound"));

            //Assert.NotNull(webClient.DownloadJson<TestClass>("http://apis.juhe.cn/simpleWeather/query"));
            //Assert.Throws<JsonReaderException>(() => webClient.DownloadJson<TestClass>("http://www.baidu.com/"));

            webClient.Dispose();
        }
#endif 
        #endregion

        #region public static Task<T> DownloadJsonAsync<T>(this WebClient webClient, string address) where T : class, new()
        /// <summary>
        /// 使用异步方式从 <paramref name="address"/> URI 下载的 JSON 形式的资源并反序列化为实体类
        /// </summary>
        /// <typeparam name="T"> 泛型类 </typeparam>
        /// <param name="webClient"> <see cref="WebClient"/> 的实例 </param>
        /// <param name="address"> 要发送数据字符串资源到达的 URI </param>
        /// <returns> 从服务器获取的 JSON 响应 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="webClient"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="address"/> 不能为 null </exception>
        /// <exception cref="NotSupportedException"> 该方法已在多个线程上同时调用 </exception>
        /// <exception cref="WebException"> 通过组合 <see cref="WebClient.BaseAddress"/> 和 <paramref name="address"/> 所构成的 URI 无效 </exception>
        /// <exception cref="WebException"> 承载资源的服务器没有响应 </exception>
        /// <exception cref="JsonReaderException"> 从服务器获取的响应内容不是有效的 JSON 字符串 </exception>
        public static Task<T> DownloadJsonAsync<T>(this WebClient webClient, string address) where T : class, new()
        {
            webClient.ThrowIfNull(nameof(webClient));
            address.ThrowIfNull(nameof(address));

            return webClient.DownloadStringTaskAsync(address)
                .ContinueWith(task => task.IsCompleted ? task.Result.JsonDeserialize<T>() : default(T));
        }
#if Xunit
        [Fact]
        public static void DownloadJsonAsyncTest()
        {
            WebClient webClient = null;
            Assert.ThrowsAsync<ArgumentNullException>(() => webClient.DownloadJsonAsync<TestClass>("Url"));

            //// 会发起请求，注释部分已测试通过

            webClient = new WebClient();
            Assert.ThrowsAsync<ArgumentNullException>(() => webClient.DownloadJsonAsync<TestClass>(null));
            Assert.ThrowsAsync<WebException>(() => webClient.DownloadJsonAsync<TestClass>("Url"));
            //webClient.BaseAddress = "http://nothing.unknow.com/";
            //Assert.ThrowsAsync<WebException>(() => webClient.DownloadJsonAsync<TestClass>("NotFound"));
            //Assert.ThrowsAsync<NotSupportedException>(() => webClient.DownloadJsonAsync<TestClass>("http://apis.juhe.cn/simpleWeather/query"));
            //webClient.Dispose();

            //webClient = new WebClient();
            //var result = webClient.DownloadJsonAsync<TestClass>("http://apis.juhe.cn/simpleWeather/query");
            //result.Wait();
            //Assert.NotNull(result.Result);
            //Assert.ThrowsAsync<JsonReaderException>(() => webClient.DownloadJsonAsync<TestClass>("http://www.baidu.com/"));

            webClient.Dispose();
        }
#endif 
        #endregion

    }
}