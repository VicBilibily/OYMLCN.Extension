using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace OYMLCN
{
    /// <summary>
    /// Extension
    /// </summary>
    public static class Extensions
    {
        #region ReleaseMemory释放内存
        [DllImport("kernel32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetProcessWorkingSetSize(IntPtr process, UIntPtr minimumWorkingSetSize, UIntPtr maximumWorkingSetSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        /// <param name="removePages">强迫症选项，将内存页大小设置为0</param>
        public static void ReleaseMemory(bool removePages = false)
        {
            // release any unused pages
            // making the numbers look good in task manager
            // this is totally nonsense in programming
            // but good for those users who care
            // making them happier with their everyday life
            // which is part of user experience
            GC.Collect(GC.MaxGeneration);
            GC.WaitForPendingFinalizers();
            if (removePages)
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr)0xFFFFFFFF, (UIntPtr)0xFFFFFFFF);
        }
        static bool ReleasingMemoryTimingStarted = false;
        static int ReleasingMemoryTimingSeconds = 60;
        static Thread _ramThread;
        /// <summary>
        /// 开启定时释放内存
        /// </summary>
        public static void StartReleasingMemoryTiming(int seconds = 60)
        {
            ReleasingMemoryTimingSeconds = seconds;
            if (ReleasingMemoryTimingStarted)
                return;

            _ramThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    ReleaseMemory(false);
                    Thread.Sleep(ReleasingMemoryTimingSeconds * 1000);
                }
            }))
            {
                IsBackground = true
            };
            _ramThread.Start();
            ReleasingMemoryTimingStarted = true;
        }
        /// <summary>
        /// 停止定时释放内存
        /// </summary>
        public static void StopReleasingMemoryTiming() => _ramThread?.Abort();
        #endregion

        #region Enum
        /// <summary>
        /// 将枚举值转换为字符串值（替换 _ 标头）
        /// </summary>
        /// <param name="enumClass"></param>
        /// <returns></returns>
        public static string EnumToString(this Enum enumClass) => enumClass.ToString().TrimStart('_');
        /// <summary>
        /// 将枚举类型转换为Key/Value数组
        /// 必须为enum枚举的任意值，其他类型将返回Null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumClass"></param>
        /// <returns></returns>
        public static Dictionary<string, T> EnumToKeyValues<T>(this T enumClass)
        {
            var reuslt = new Dictionary<string, T>();
            if (enumClass.GetType().BaseType != typeof(Enum))
                throw new ArgumentException("参数基类型并非类型System.Enum");
            foreach (T value in Enum.GetValues(enumClass.GetType()))
                reuslt[value.ToString()] = value;
            return reuslt;
        }
        #endregion

        /// <summary>
        /// 验证居民身份证号码
        /// 验证支持：GB11643-1989、GB11643-1999
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsChineseIDCard(this string str)
        {
            string address = "11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,51,52,53,54,50,61,62,63,64,65,71,81,82";
            long n = 0;

            if (str.Length == 15)
            {
                if (long.TryParse(str, out n) == false || n < Math.Pow(10, 14))
                    return false;//数字验证
                if (address.IndexOf(str.Remove(2)) == -1)
                    return false;//省份验证
                if (str.Substring(6, 6).Insert(4, "-").Insert(2, "-").ConvertToNullableDatetime().IsNull())
                    return false;//生日验证  
                return true;//符合GB11643-1989标准
            }
            else if (str.Length == 18)
            {
                if (long.TryParse(str.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(str.Replace('x', '0').Replace('X', '0'), out n) == false)
                    return false;//数字验证  
                if (address.IndexOf(str.Remove(2)) == -1)
                    return false;//省份验证  
                if (str.Substring(6, 8).Insert(6, "-").Insert(4, "-").ConvertToNullableDatetime().IsNull())
                    return false;//生日验证  
                string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
                string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
                char[] Ai = str.Remove(17).ToCharArray();
                int sum = 0;
                for (int i = 0; i < 17; i++)
                    sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
                if (arrVarifyCode[sum % 11] != str.Substring(17, 1).ToLower())
                    return false;//校验码验证
                return true;//符合GB11643-1999标准
            }
            throw new FormatException("身份证长度不正确或暂未支持该身份证验证");
        }

        #region IsDefault/IsNull/IsEmpty
        /// <summary>
        /// 判断字典键值类型是否未赋值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="keyValuePair"></param>
        /// <returns></returns>
        public static bool IsDefault<TKey, TValue>(this KeyValuePair<TKey, TValue> keyValuePair) =>
            default(KeyValuePair<TKey, TValue>).Equals(keyValuePair);

        /// <summary>
        /// 判断是否为Null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj) => obj == null;
        /// <summary>
        /// 判断是否为非Null
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj) => obj != null;
        /// <summary>
        /// 判断集合是否为空或Null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable) =>
            enumerable.IsNull() || !enumerable.Any();
        /// <summary>
        /// 判断集合是否不为空或Null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable) =>
            enumerable.IsNotNull() && enumerable.Any();
        #endregion

        #region IDictionary
        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue SelectValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue)) =>
             dict.ContainsKey(key) ? dict[key] : defaultValue;

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
        public static IEnumerable<TValue> SelectKeyContainsValues<TValue>(this IDictionary<string, TValue> dict, params string[] keys) =>
            dict.Where(d => d.Key.Contains(keys)).Select(d => d.Value);

        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<TValue> SelectKeyStartWithValues<TValue>(this IDictionary<string, TValue> dict, string key) =>
            dict.Where(d => d.Key.StartsWith(key)).Select(d => d.Value);

        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IEnumerable<TValue> SelectKeyEndWithValues<TValue>(this IDictionary<string, TValue> dict, string key) =>
            dict.Where(d => d.Key.EndsWith(key)).Select(d => d.Value);

        /// <summary>
        /// 拼接HTML标签属性项目
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static string ToHtmlAttributesString<TValue>(this IDictionary<string, TValue> dict) =>
            dict.Select(d => d.Value.IsNull() ? d.Key : d.Value.ToString().IsEmpty() ? null : $"{d.Key}=\"{d.Value}\"")
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
        public static string Join(this IEnumerable<string> list, string separator = "") => string.Join(separator, list);
        /// <summary>
        /// 搜索字符串数组
        /// </summary>
        /// <param name="list"></param>
        /// <param name="words"></param>
        /// <returns></returns>
        public static IEnumerable<string> SelectContains(this IEnumerable<string> list, params string[] words) =>
            list?.Where(d => d.Contains(words)) ?? new string[0];
        /// <summary>
        /// 搜索字符串数组
        /// </summary>
        /// <param name="list"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static IEnumerable<string> SelectStartWith(this IEnumerable<string> list, string word) =>
            list?.Where(d => d.StartsWith(word)) ?? new string[0];
        /// <summary>
        /// 搜索字符串数组
        /// </summary>
        /// <param name="list"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public static IEnumerable<string> SelectEndWith(this IEnumerable<string> list, string word) =>
            list?.Where(d => d.EndsWith(word)) ?? new string[0];
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
        /// <returns></returns>
        public static char ToChar(this int i) => (char)i;

        /// <summary>
        /// 获取分页的最大页码
        /// </summary>
        /// <param name="total"></param>
        /// <param name="limit">页条目</param>
        /// <returns></returns>
        public static int GetMaxPagination(this int total, int limit = 10) => total / limit + (total % limit == 0 ? 0 : 1);
        /// <summary>
        /// 获取分页的最大页码
        /// </summary>
        /// <param name="total"></param>
        /// <param name="limit">页条目</param>
        /// <returns></returns>
        public static long GetMaxPagination(this long total, int limit = 10) => total / limit + (total % limit == 0 ? 0 : 1);
    }
}
