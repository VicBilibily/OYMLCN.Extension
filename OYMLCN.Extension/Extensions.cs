using System.Collections.Generic;
using System.Linq;

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
        {
#if NET35
            return string.Join(separator, list.ToArray());
#else
            return string.Join(separator, list);
#endif
        }
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

    }
}
