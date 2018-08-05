#if NET461
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace OYMLCN.WPFExtensions
{
    /// <summary>
    /// SelectorExtensions
    /// </summary>
    public static class SelectorExtensions
    {
        /// <summary>
        /// 获取数据源IEnumerable&lt;T&gt;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetItemsSource<T>(this ItemsControl control) => control.ItemsSource as IEnumerable<T>;
        /// <summary>
        /// 获取选中项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static T GetSelectItem<T>(this Selector selector) => (T)selector.SelectedItem;
        /// <summary>
        /// 获取选中项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listBox"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetSelectItems<T>(this ListBox listBox)
        {
            foreach (var item in listBox.SelectedItems)
                yield return (T)item;
        }
        /// <summary>
        /// 获取选中项Value
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static string GetSelectItemValue(this Selector selector) => selector.SelectedValue?.ToString();


        /// <summary>
        /// 获取数据源IDictionary&lt;T&gt;
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> GetItemsSource<TKey, TValue>(this ItemsControl control) => control.ItemsSource as IDictionary<TKey, TValue>;
        /// <summary>
        /// 获取选中项
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static KeyValuePair<TKey, TValue> GetSelectItem<TKey, TValue>(this Selector selector) => (KeyValuePair<TKey, TValue>)selector.SelectedItem;
        /// <summary>
        /// 获取选中项Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static T GetSelectItemKey<T>(this Selector selector) => selector.GetSelectItem<T, string>().Key;
        /// <summary>
        /// 获取选中项Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static T GetSelectItemValue<T>(this Selector selector) => selector.GetSelectItem<string, T>().Value;
        /// <summary>
        /// 获取选中项
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="listBox"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> GetSelectItems<TKey, TValue>(this ListBox listBox)
        {
            var dic = new Dictionary<TKey, TValue>();
            foreach (var item in listBox.SelectedItems)
            {
                var keyValue = (KeyValuePair<TKey, TValue>)item;
                dic.Add(keyValue.Key, keyValue.Value);
            }
            return dic;
        }
        /// <summary>
        /// 获取选中项Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listBox"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetSelectItemsKey<T>(this ListBox listBox) => listBox.GetSelectItems<T, string>().Select(d => d.Key);
        /// <summary>
        /// 获取选中项Value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listBox"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetSelectItemsValue<T>(this ListBox listBox) => listBox.GetSelectItems<string, T>().Select(d => d.Value);
    }
}
#endif