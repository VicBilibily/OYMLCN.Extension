using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// JsonExtension
    /// </summary>
    public static partial class NewtonsoftJsonExtensions
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
        public static string ToJsonString(this object data, JsonSerializerSettings settings = null)
        {
            var jsonString = JsonConvert.SerializeObject(data, settings ?? DefaultSettings);
            MatchEvaluator evaluator = new MatchEvaluator(DecodeUnicode);
            return Regex.Replace(jsonString, @"\\u[0123456789abcdef]{4}", evaluator);//或：[\\u007f-\\uffff]，\对应为\u000a，但一般情况下会保持\
        }

        /// <summary>
        /// NewtonsoftJsonHandler
        /// </summary>
        public class NewtonsoftJsonHandler
        {
            internal string Str;
            internal NewtonsoftJsonHandler(string str) => Str = str;

            /// <summary>
            /// JSON字符串转换为对象
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public T DeserializeToObject<T>() => JsonConvert.DeserializeObject<T>(Str);
            /// <summary>
            /// 转换JSON字符串为可供查询的Array数组
            /// </summary>
            /// <returns></returns>
            public JToken ParseToJToken() => JToken.Parse(Str.IsNullOrWhiteSpace() ? "{}" : Str);
        }

        /// <summary>
        /// 将字符串作为Json对象处理
        /// </summary>
        /// <param name="jsonstr"></param>
        /// <returns></returns>
        public static NewtonsoftJsonHandler AsJsonHandler(this string jsonstr) => new NewtonsoftJsonHandler(jsonstr);

        ///// <summary>
        ///// JSON字符串转换为对象
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="jsonstr"></param>
        ///// <returns></returns>
        //[Obsolete("为减少扩展方法污染，请使用AsJsonHandler().DeserializeToObject<T>()", true)]
        //public static T DeserializeJsonString<T>(this string jsonstr) => throw new NotSupportedException();

        /// <summary>
        /// 获取指定值
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(this JToken jt, string key) => jt[key]?.Value<string>();
        /// <summary>
        /// 获取指定值
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int? GetInt32(this JToken jt, string key) => jt[key]?.Value<int>();
        /// <summary>
        /// 获取指定值
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long? GetInt64(this JToken jt, string key) => jt[key]?.Value<long>();
        /// <summary>
        /// 获取指定值
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool? GetBoolean(this JToken jt, string key) => jt[key]?.Value<bool>();
        /// <summary>
        /// 获取指定值
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DateTime? GetDateTime(this JToken jt, string key) => jt[key]?.Value<DateTime>();


        /// <summary>
        /// 转换为整型数组
        /// </summary>
        /// <param name="jt"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this JToken jt)
        {
            if (jt == null || jt.Count() == 0)
                return new int[0];
            int length = jt.Count();
            int[] array = new int[length];
            for (int i = 0; i < length; i++)
                array[i] = jt[i].Value<int>();
            return array;
        }
        /// <summary>
        /// 获取整型数组
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int[] GetIntArray(this JToken jt, string key) => jt[key].ToIntArray();
        /// <summary>
        /// 转换为长整型数组
        /// </summary>
        /// <param name="jt"></param>
        /// <returns></returns>
        public static long[] ToLongArray(this JToken jt)
        {
            if (jt == null || jt.Count() == 0)
                return new long[0];
            int length = jt.Count();
            long[] array = new long[length];
            for (int i = 0; i < length; i++)
                array[i] = jt[i].Value<long>();
            return array;
        }
        /// <summary>
        /// 获取长整型数组
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long[] GetLongArray(this JToken jt, string key) => jt[key].ToLongArray();

        /// <summary>
        /// 转换为字符串数组
        /// </summary>
        /// <param name="jt"></param>
        /// <returns></returns>
        public static string[] ToStringArray(this JToken jt)
        {
            if (jt == null || jt.Count() == 0)
                return new string[0];
            int length = jt.Count();
            string[] array = new string[length];
            for (int i = 0; i < length; i++)
                array[i] = jt[i].Value<string>();
            return array;
        }
        /// <summary>
        /// 获取字符串数组
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string[] GetStringArray(this JToken jt, string key) => jt[key].ToStringArray();

        /// <summary>
        /// 转换为对象数组
        /// </summary>
        /// <param name="jt"></param>
        /// <returns></returns>
        public static object[] ToObjectArray(this JToken jt)
        {
            if (jt == null || jt.Count() == 0)
                return new object[0];
            int length = jt.Count();
            object[] array = new object[length];
            for (int i = 0; i < length; i++)
                array[i] = jt[i].Value<JValue>().Value;
            return array;
        }
        /// <summary>
        /// 获取对象数组
        /// </summary>
        /// <param name="jt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object[] GetObjectArray(this JToken jt, string key) => jt[key].ToObjectArray();

    }
}
