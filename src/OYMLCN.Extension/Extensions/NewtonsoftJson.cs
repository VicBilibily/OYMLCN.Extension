using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OYMLCN.ArgumentChecker;

#if Xunit
using Xunit;
using Xunit.Sdk;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// Newtonsoft.Json 扩展
    /// </summary>
    public static class NewtonsoftJsonExtension
    {
        /// <summary>
        /// 扩展默认序列化配置
        /// </summary>
        public readonly static JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        internal static string DecodeUnicode(Match match)
        {
            if (!match.Success)
                return null;
            char outStr = (char)int.Parse(match.Value.Remove(0, 2), NumberStyles.HexNumber);
            return new string(outStr, 1);
        }

        #region public static string JsonSerialize<T>(this T value, JsonSerializerSettings settings = null) where T : class
        /// <summary>
        /// 将当前对象实例序列化为 JSON 字符串
        /// </summary>
        /// <param name="value"> 要序列化的对象实例 </param>
        /// <param name="settings"> 用于序列化对象的参数配置 <see cref="JsonSerializerSettings"/> （不传入则使用默认配置） </param>
        /// <returns> 对象的 JSON 字符串表示 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        /// <exception cref="JsonReaderException"> <paramref name="value"/> 包含无效的 JavaScript 属性标识符字符或格式错误 </exception>
        public static string JsonSerialize<T>(this T value, JsonSerializerSettings settings = null) where T : class
        {
            value.ThrowIfNull(nameof(value));
            var jsonString = JsonConvert.SerializeObject(value, settings ?? DefaultSettings);
            return Regex.Replace(jsonString, @"\\u[0123456789abcdef]{4}", DecodeUnicode);//或：[\\u007f-\\uffff]，\对应为\u000a，但一般情况下会保持\
        }
        #endregion

        #region public static T JsonDeserialize<T>(this string value)
        /// <summary>
        /// 将 JSON 字符串反序列化为指定的 .NET 类型
        /// </summary>
        /// <typeparam name="T"> 要反序列化到的对象的类型 </typeparam>
        /// <param name="value"> 要反序列化的JSON 字符串 </param>
        /// <returns> 从JSON字符串反序列化的对象 </returns>
        /// <exception cref="JsonReaderException"> <paramref name="value"/> 不是有效的 JSON 字符串 </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        public static T JsonDeserialize<T>(this string value)
            => JsonConvert.DeserializeObject<T>(value);
        #endregion

#if Xunit
        [Serializable]
        public class TestClass
        {
            public string demo { get; set; }
        }
        [Fact]
        public static void JsonSerializerTest()
        {
            string json = null;
            Assert.Throws<ArgumentNullException>(() => json.JsonDeserialize<TestClass>());

            json = string.Empty;
            Assert.Null(json.JsonDeserialize<TestClass>());

            json = "{}";
            TestClass testClass = json.JsonDeserialize<TestClass>();
            Assert.NotNull(testClass);
            Assert.Null(testClass.demo);

            json = "{demo}";
            Assert.Throws<JsonReaderException>(() => json.JsonDeserialize<TestClass>());

            json = "{demo:null}";
            testClass = json.JsonDeserialize<TestClass>();
            Assert.NotNull(testClass);
            Assert.Null(testClass.demo);

            json = "{demo:''}";
            testClass = json.JsonDeserialize<TestClass>();
            Assert.NotNull(testClass);
            Assert.Empty(testClass.demo);

            json = "{demo:123}";
            testClass = json.JsonDeserialize<TestClass>();
            Assert.NotNull(testClass);
            Assert.Equal("123", testClass.demo);

            Assert.Equal($"\"{json}\"", json.JsonSerialize());
            Assert.Equal("{\"demo\":\"123\"}", testClass.JsonSerialize());

            testClass = null;
            Assert.Throws<ArgumentNullException>(() => testClass.JsonSerialize());
        }
#endif



        #region public static JToken JTokenParse(this string json)
        /// <summary>
        /// 将当前 JSON 字符串加载为抽象 <see cref="JToken"/> 类
        /// </summary>
        /// <param name="json"> 要转换的JSON 字符串 </param>
        /// <returns> 一个抽象的 <see cref="JToken"/> 对象 </returns>
        public static JToken JTokenParse(this string json)
            => JToken.Parse(json.IsNullOrWhiteSpace() ? "{}" : json);
#if Xunit
        [Fact]
        public static void JTokenParseTest()
        {
            string json = null;
            JToken jToken = json.JTokenParse();
            Assert.NotNull(jToken);
            Assert.Empty(jToken);

            json = "{demo:123}";
            jToken = json.JTokenParse();
            Assert.Single(jToken);
            Assert.Equal("123", jToken.Value<string>("demo"));
        }
#endif
        #endregion



        #region 为了避免冲突和规范命名，将在以后的版本移除以下方法
        /// <summary>
        /// 将当前对象实例序列化为 JSON 字符串
        /// </summary>
        /// <param name="value"> 要序列化的对象实例 </param>
        /// <param name="settings"> 用于序列化对象的参数配置 <see cref="JsonSerializerSettings"/> （不传入则使用默认配置） </param>
        /// <returns> 对象的 JSON 字符串表示 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        /// <exception cref="JsonReaderException"> <paramref name="value"/> 包含无效的 JavaScript 属性标识符字符或格式错误 </exception>
        [Obsolete("请改用 JsonSerialize<T>()，将在后续次要版本移除")]
        public static string ToJsonString<T>(this T value, JsonSerializerSettings settings = null) where T : class
            => JsonSerialize(value, settings);

        /// <summary>
        /// 将 JSON 字符串反序列化为指定的 .NET 类型
        /// </summary>
        /// <typeparam name="T"> 要反序列化到的对象的类型 </typeparam>
        /// <param name="value"> 要反序列化的JSON 字符串 </param>
        /// <returns> 从JSON字符串反序列化的对象 </returns>
        /// <exception cref="JsonReaderException"> <paramref name="value"/> 不是有效的 JSON 字符串 </exception>
        [Obsolete("请改用 JsonDeserialize<T>()，将在后续次要版本移除")]
        public static T DeserializeJsonToObject<T>(this string value)
            => JsonDeserialize<T>(value);
        #endregion

    }
}
