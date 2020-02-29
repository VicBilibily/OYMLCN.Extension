using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// Newtonsoft.Json操作简化扩展
    /// </summary>
    public static partial class NewtonsoftJsonExtension
    {
        static string DecodeUnicode(Match match)
        {
            if (!match.Success)
                return null;
            char outStr = (char)int.Parse(match.Value.Remove(0, 2), NumberStyles.HexNumber);
            return new string(outStr, 1);
        }

        static JsonSerializerSettings DefaultSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        /// <summary>
        /// 将对象转为JSON字符串
        /// </summary>
        /// <param name="data">任意对象</param>
        /// <param name="settings">序列化配置</param>
        /// <returns>JSON字符串</returns>
        public static string ToJsonString<T>(this T data, JsonSerializerSettings settings = null) where T : class
        {
            var jsonString = JsonConvert.SerializeObject(data, settings ?? DefaultSettings);
            MatchEvaluator evaluator = new MatchEvaluator(DecodeUnicode);
            return Regex.Replace(jsonString, @"\\u[0123456789abcdef]{4}", evaluator);//或：[\\u007f-\\uffff]，\对应为\u000a，但一般情况下会保持\
        }

        /// <summary>
        /// JSON字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="JsonReaderException"> <paramref name="json"/> 不是有效的 JSON 字符串 </exception>
        public static T DeserializeJsonToObject<T>(this string json) => JsonConvert.DeserializeObject<T>(json);
        /// <summary>
        /// 转换JSON字符串为可供查询的Array数组
        /// </summary>
        /// <returns></returns>
        public static JToken ParseToJToken(this string str) => JToken.Parse(str.IsNullOrWhiteSpace() ? "{}" : str);
        /// <summary>
        /// 转换JToken对象为数组
        /// </summary>
        /// <returns></returns>
        public static T[] ToArray<T>(this JToken jt)
        {
            if (jt == null || jt.Count() == 0)
                return new T[0];
            int length = jt.Count();
            T[] array = new T[length];
            for (int i = 0; i < length; i++)
                array[i] = jt[i].Value<T>();
            return array;
        }
    }
}
