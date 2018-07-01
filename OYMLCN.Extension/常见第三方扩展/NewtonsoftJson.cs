using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OYMLCN
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
        /// JSON字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonstr"></param>
        /// <returns></returns>
        public static T DeserializeJsonString<T>(this string jsonstr) => JsonConvert.DeserializeObject<T>(jsonstr);


        /// <summary>
        /// 转换JSON字符串为可供查询的Array数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static JToken ParseToJToken(this string str) => JToken.Parse(str.IsNullOrWhiteSpace() ? "{}" : str);



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
