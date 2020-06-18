using System.Text.Json;
using System.Text.Encodings.Web;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// JsonExtension
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// 将对象转为JSON字符串
        /// </summary>
        /// <param name="data">任意对象</param>
        /// <param name="options">序列化配置</param>
        /// <returns>JSON字符串</returns>
        public static string TextJsonSerialize<T>(this T data, JsonSerializerOptions options = null) where T : class
            => JsonSerializer.Serialize(data, options);

        internal static JsonSerializerOptions JsonOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            IgnoreNullValues = true,
        };
        /// <summary>
        /// 将对象转为JSON字符串（已修正 System.Text.Json 表现）
        /// </summary>
        /// <param name="data">任意对象</param>
        /// <returns>JSON字符串</returns>
        public static string TextJsonSerializeFix<T>(this T data) where T : class
            => JsonSerializer.Serialize(data, JsonOptions);

        /// <summary>
        /// JSON字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T TextJsonDeserialize<T>(this string str)
            => JsonSerializer.Deserialize<T>(str);
        /// <summary>
        /// 转换JSON字符串为可供查询的JsonDocument
        /// </summary>
        /// <returns></returns>
        public static JsonDocument ParseToJsonDocument(this string str, JsonDocumentOptions options = default)
            => JsonDocument.Parse(str.IsNullOrWhiteSpace() ? "{}" : str, options);
    }
}
