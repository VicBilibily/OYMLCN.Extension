using System;
using System.Collections.Generic;
using System.Linq;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// IEnumerableExtensions
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 匿名方法遍历列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            var forEach = source as T[] ?? source.ToArray();
            foreach (T item in forEach)
                action(item);
            return forEach;
        }
        /// <summary>
        /// 匿名方法遍历列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            var forEach = source.ToList();
            var list = forEach.ToList();
            foreach (T item in list)
                action(item, list.IndexOf(item));
            return forEach;
        }

    }
}
