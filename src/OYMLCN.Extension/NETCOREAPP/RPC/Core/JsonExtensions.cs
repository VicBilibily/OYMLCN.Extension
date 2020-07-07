using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text.RegularExpressions;

namespace OYMLCN.RPC.Core
{
    static class JsonExtensions
    {
        static JsonExtensions()
        {
            settings = new JsonSerializerSettings()
            {
                // 包含属性默认值
                DefaultValueHandling = DefaultValueHandling.Include,
                // 包含空值
                NullValueHandling = NullValueHandling.Include,
                // 格式化时间
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                // 输出内容有缩进格式
                Formatting = Formatting.Indented,
                // 最大序列化 10 层
                MaxDepth = 10,
                // 遇到循环引用时报错
                ReferenceLoopHandling = ReferenceLoopHandling.Error,

                // 采用驼峰式序列化
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

        }
        static readonly JsonSerializerSettings settings;

        public static string ToJson<T>(this T data) where T : class, new()
            => Regex.Replace(JsonConvert.SerializeObject(data, settings), @"\\u[0123456789abcdef]{4}", Extensions.NewtonsoftJsonExtension.DecodeUnicode);

        public static T FromJson<T>(this string json) where T : class, new()
            => JsonConvert.DeserializeObject<T>(json);
        public static object FromJson(this string json, Type type)
            => JsonConvert.DeserializeObject(json, type);
    }
}
