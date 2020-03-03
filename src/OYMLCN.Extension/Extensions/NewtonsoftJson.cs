using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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
    public static partial class NewtonsoftJsonExtension
    {
        /// <summary>
        /// 扩展默认序列化配置
        /// </summary>
        public readonly static JsonSerializerSettings DefaultSettings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        static string DecodeUnicode(Match match)
        {
            if (!match.Success)
                return null;
            char outStr = (char)int.Parse(match.Value.Remove(0, 2), NumberStyles.HexNumber);
            return new string(outStr, 1);
        }

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
            MatchEvaluator evaluator = new MatchEvaluator(DecodeUnicode);
            return Regex.Replace(jsonString, @"\\u[0123456789abcdef]{4}", evaluator);//或：[\\u007f-\\uffff]，\对应为\u000a，但一般情况下会保持\
        }
        
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



        /// <summary>
        /// 将当前 JSON 字符串加载为抽象 <see cref="JToken"/> 类
        /// </summary>
        /// <param name="json"> 要转换的JSON 字符串 </param>
        /// <returns> 一个抽象的 <see cref="JToken"/> 对象 </returns>
        public static JToken JTokenParse(this string json)
            => JToken.Parse(json.IsNullOrWhiteSpace() ? "{}" : json);

        /// <summary>
        /// 将当前的抽象 <see cref="JToken"/> 对象转换为指定的类型序列
        /// </summary>
        /// <typeparam name="T"> 要转换的目标类型 </typeparam>
        /// <param name="jToken"> 要转换的抽象 <see cref="JToken"/> 对象 </param>
        /// <returns> 指定 <typeparamref name="T"/> 类型的序列 </returns>
        public static T[] ToArray<T>(this JToken jToken)
        {
            if (jToken == null)
                return new T[0];

            int length = jToken.Count();
            if (length == 0)
                return new T[1] { jToken.Value<T>() };

            T[] array = new T[length];
            for (int i = 0; i < length; i++)
                array[i] = jToken[i].Value<T>();
            return array;
        }

        /// <summary>
        /// 将当前的抽象 <see cref="JToken"/> 对象转换为指定的类型序列
        /// </summary>
        /// <typeparam name="T"> 要转换的目标类型 </typeparam>
        /// <param name="jToken"> 要转换的抽象 <see cref="JToken"/> 对象 </param>
        /// <returns> 指定 <typeparamref name="T"/> 类型的序列 </returns>
        public static T[] ToArray<T>(this JToken jToken, object key)
        {
            if (jToken == null)
                return new T[0];
            jToken = jToken[key];
            return jToken.ToArray<T>();
        }

#if Xunit
        [Fact]
        public static void JTokenTest()
        {
            string json = null;
            JToken jToken = json.JTokenParse();
            Assert.NotNull(jToken);
            Assert.Empty(jToken);

            json = "{demo:123}";
            jToken = json.JTokenParse();
            Assert.Single(jToken);
            Assert.Equal("123", jToken.Value<string>("demo"));


            jToken = null;
            Assert.NotNull(jToken.ToArray<string>());
            Assert.Empty(jToken.ToArray<string>());

            json = "{demo:123}";
            jToken = json.JTokenParse();
            Assert.Single(jToken["demo"].ToArray<string>());
            Assert.Single(jToken.ToArray<string>("demo"));

            json = "{demo:[1,2,3,4,5]}";
            jToken = json.JTokenParse();
            Assert.Equal(new[] { "1", "2", "3", "4", "5" }, jToken["demo"].ToArray<string>());
            Assert.Equal(new[] { "1", "2", "3", "4", "5" }, jToken.ToArray<string>("demo"));
        }
#endif



        #region 为了避免冲突和规范命名，将在以后的版本移除以下方法
        /// <summary>
        /// 将当前对象实例序列化为 JSON 字符串
        /// </summary>
        /// <param name="value"> 要序列化的对象实例 </param>
        /// <param name="settings"> 用于序列化对象的参数配置 <see cref="JsonSerializerSettings"/> （不传入则使用默认配置） </param>
        /// <returns> 对象的 JSON 字符串表示 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        /// <exception cref="JsonReaderException"> <paramref name="value"/> 包含无效的 JavaScript 属性标识符字符或格式错误 </exception>
        [Obsolete("请改用 JsonSerialize<T>()")]
        public static string ToJsonString<T>(this T value, JsonSerializerSettings settings = null) where T : class
            => JsonSerialize(value, settings);

        /// <summary>
        /// 将 JSON 字符串反序列化为指定的 .NET 类型
        /// </summary>
        /// <typeparam name="T"> 要反序列化到的对象的类型 </typeparam>
        /// <param name="value"> 要反序列化的JSON 字符串 </param>
        /// <returns> 从JSON字符串反序列化的对象 </returns>
        /// <exception cref="JsonReaderException"> <paramref name="value"/> 不是有效的 JSON 字符串 </exception>
        [Obsolete("请改用 JsonDeserialize<T>()")]
        public static T DeserializeJsonToObject<T>(this string value)
            => JsonDeserialize<T>(value);
        #endregion

    }
}
