using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 杂项扩展
    /// </summary>
    public static partial class Extensions
    {
        #region IsDefault/IsNull/IsEmpty
        /// <summary>
        /// 判断字典键值类型是否未赋值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="keyValuePair"></param>
        /// <returns></returns>
        public static bool IsDefault<TKey, TValue>(this KeyValuePair<TKey, TValue> keyValuePair)
            => default(KeyValuePair<TKey, TValue>).Equals(keyValuePair);

        /// <summary>
        /// 判断是否为Null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
            => obj == null;
        /// <summary>
        /// 判断是否为非Null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
            => obj != null;
        /// <summary>
        /// 判断集合是否为空或Null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
            => enumerable.IsNull() || !enumerable.Any();
        /// <summary>
        /// 判断集合是否不为空或Null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable)
            => enumerable.IsNotNull() && enumerable.Any();
        #endregion

        #region IDictionary
        /// <summary>
        /// 判断两个字典集合是否完全相等
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict1"></param>
        /// <param name="dict2"></param>
        /// <returns></returns>
        public static bool IsFullEqual<TKey, TValue>(this IDictionary<TKey, TValue> dict1, IDictionary<TKey, TValue> dict2)
        {
            if (dict1.Count != dict2.Count)
                return false;
            Dictionary<TKey, TValue>
                _d1 = dict1.OrderBy(v => v.Key).ToDictionary(v => v.Key, v => v.Value),
                _d2 = dict2.OrderBy(v => v.Key).ToDictionary(v => v.Key, v => v.Value);
            foreach (var item in dict1)
            {
                TValue v1 = item.Value, v2;
                if (_d2.TryGetValue(item.Key, out v2))
                {
                    if (v1.Equals(v2) == false)
                        return false;
                }
                else
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 增加或更新字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }
        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue SelectValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
            => dict.ContainsKey(key) ? dict[key] : defaultValue;
        /// <summary>
        /// 获取字典值，如果为空则设置为默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetValueOrSetDefaultFunc<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            TValue tvalue = defaultValue;
            if (!dict.TryGetValue(key, out tvalue))
                dict.Add(key, tvalue);
            return tvalue;
        }
        /// <summary>
        /// 获取字典值，如果为空则按照委托方法返回值设为默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="setValue"></param>
        /// <returns></returns>
        public static TValue GetValueOrSetDefaultFunc<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> setValue)
        {
            TValue tvalue = default(TValue);
            if (!dict.TryGetValue(key, out tvalue))
            {
                tvalue = setValue(key);
                dict.Add(key, tvalue);
            }
            return tvalue;
        }

        /// <summary>
        /// 追加合并字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Union<TKey, TValue>(this IDictionary<TKey, TValue> dict, IDictionary<TKey, TValue> dictionary)
        {
            foreach (var item in dictionary)
                dict[item.Key] = item.Value;
            return dict;
        }

        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static IEnumerable<TValue> SelectKeyContainsValues<TValue>(this IDictionary<string, TValue> dict, params string[] keys)
            => dict.Where(d => d.Key.Contains(keys)).Select(d => d.Value);

        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<TValue> SelectKeyStartWithValues<TValue>(this IDictionary<string, TValue> dict, string key)
            => dict.Where(d => d.Key.StartsWith(key)).Select(d => d.Value);

        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<TValue> SelectKeyEndWithValues<TValue>(this IDictionary<string, TValue> dict, string key)
            => dict.Where(d => d.Key.EndsWith(key)).Select(d => d.Value);

        /// <summary>
        /// 拼接HTML标签属性项目
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string ToHtmlAttributesString<TValue>(this IDictionary<string, TValue> dict)
            => dict.Select(d => d.Value.IsNull() ? d.Key : d.Value.ToString().IsEmpty() ? null : $"{d.Key}=\"{d.Value}\"")
                   .Where(d => d.IsNotNull())
                   .Join(" ");
        #endregion

        #region IEnumerable
        /// <summary>
        /// 将字符串数组拼接成字符串
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable<string> list, string separator = "")
            => string.Join(separator, list);
        /// <summary>
        /// 搜索字符串数组
        /// </summary>
        /// <param name="list"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public static IEnumerable<string> SelectContains(this IEnumerable<string> list, params string[] words)
            => list?.Where(d => d.Contains(words)) ?? new string[0];
        /// <summary>
        /// 搜索字符串数组
        /// </summary>
        /// <param name="list"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static IEnumerable<string> SelectStartWith(this IEnumerable<string> list, string word)
            => list?.Where(d => d.StartsWith(word)) ?? new string[0];
        /// <summary>
        /// 搜索字符串数组
        /// </summary>
        /// <param name="list"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static IEnumerable<string> SelectEndWith(this IEnumerable<string> list, string word)
            => list?.Where(d => d.EndsWith(word)) ?? new string[0];
        /// <summary>
        /// 匿名方法遍历列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T t in source)
            {
                T obj = t;
                action(obj);
            }
            return source;
        }
        #endregion

        /// <summary>
        /// CharToInt32
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static int ToInt32(this char ch) => ch;
        /// <summary>
        /// Int32ToChar
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>s
        public static char ToChar(this int i) => (char)i;

        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <param name="second">秒数</param>
        /// <returns>分钟数</returns>
        public static int SecondToMinute(this int second)
            => Convert.ToInt32(Math.Ceiling(second / (decimal)60));

        #region 人民币金额小写转大写
        /// <summary>
        /// 人民币金额小写转大写
        /// </summary>
        public static string ConvertToUppercaseRMB(this double money)
        {
            var valueString = money.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var temp = Regex.Replace(valueString, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var result = Regex.Replace(temp, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            return result;
        }
        /// <summary>
        /// 人民币金额小写转大写
        /// </summary>
        public static string ConvertToUppercaseRMB(this decimal money)
            => ConvertToUppercaseRMB((double)money);
        /// <summary>
        /// 人民币金额小写转大写
        /// </summary>
        public static string ConvertToUppercaseRMB(this int money)
            => ConvertToUppercaseRMB((double)money); 
        #endregion

        #region 对象拷贝Clone
        /// <summary>
        /// Json序列化的方式实现深拷贝
        /// </summary>
        public static T JsonDeepClone<T>(this T t) where T : class, new()
            => t.ToJsonString().DeserializeJsonToObject<T>();
        /// <summary>
        /// XML序列化的方式实现深拷贝
        /// </summary>
        public static T XmlDeepClone<T>(this T t) where T : class, new()
        {
            //创建Xml序列化对象
            XmlSerializer xml = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())//创建内存流
            {
                //将对象序列化到内存中
                xml.Serialize(ms, t);
                ms.Position = default;//将内存流的位置设为0
                return (T)xml.Deserialize(ms);//继续反序列化
            }
        }
        /// <summary>
        /// 二进制序列化的方式进行深拷贝
        /// <para>确保需要拷贝的类里的所有成员已经标记为 [Serializable] 如果没有加该特性特报错</para>
        /// </summary>
        public static T BinaryDeepCopy<T>(this T t) where T : class, new()
        {
            //创建二进制序列化对象
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())//创建内存流
            {
                //将对象序列化到内存中
                bf.Serialize(ms, t);
                ms.Position = default;//将内存流的位置设为0
                return (T)bf.Deserialize(ms);//继续反序列化
            }
        }
        /// <summary>
        /// Reflection方式进行深拷贝
        /// </summary>
        public static T DeepClone<T>(this T obj) where T : class, new()
        {
            Type type = obj.GetType();
            // 对于没有公共无参构造函数的类型此处会报错
            T returnObj = Activator.CreateInstance(type) as T;
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                var fieldValue = field.GetValue(obj);
                // 值类型，字符串，枚举类型直接把值复制，不存在浅拷贝
                if (fieldValue.GetType().IsValueType || fieldValue.GetType().Equals(typeof(System.String)) || fieldValue.GetType().IsEnum)
                    field.SetValue(returnObj, fieldValue);
                else
                    field.SetValue(returnObj, DeepClone(fieldValue));
            }
            // 属性
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                var propertyValue = property.GetValue(obj);
                if (propertyValue.GetType().IsValueType || propertyValue.GetType().Equals(typeof(String)) || propertyValue.GetType().IsEnum)
                    property.SetValue(returnObj, propertyValue);
                else
                    property.SetValue(returnObj, DeepClone(propertyValue));
            }
            return returnObj;
        }
        /// <summary>
        /// Reflection方式进行浅拷贝（仅克隆值类型）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Clone<T>(this T obj) where T : class, new()
        {
            Type type = obj.GetType();
            // 对于没有公共无参构造函数的类型此处会报错
            T returnObj = Activator.CreateInstance(type) as T;
            //字段
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                field.SetValue(returnObj, field.GetValue(obj));
            }
            //属性
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                property.SetValue(returnObj, property.GetValue(obj));
            }
            return returnObj;
        }
        #endregion

    }
}
