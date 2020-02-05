using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    public static class IDictionaryExtension
    {
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

    }
}
