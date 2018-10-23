#if NET461
using System.Collections;
using System.Windows.Forms;

namespace OYMLCN.WinForm.Extensions
{
    /// <summary>
    /// ComboBoxExtension
    /// </summary>
    public static class ComboBoxExtensions
    {
        /// <summary>
        /// 清空选项
        /// </summary>
        /// <param name="cb"></param>
        public static void ClearItems(this ComboBox cb)
        {
            cb.SelectedItem = null;
            cb.Items.Clear();
        }
        /// <summary>
        /// 选中指定选项
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SelectItem(this ComboBox cb, object key, object value) => cb.SelectedItem = new DictionaryEntry(key, value);

        /// <summary>
        /// 添加选项
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddItem(this ComboBox cb, object key, object value)=> cb.Items.Add(new DictionaryEntry(key, value));

        /// <summary>
        /// 获取选中项
        /// </summary>
        /// <param name="cb"></param>
        /// <returns></returns>
        public static DictionaryEntry GetSelectedItem(this ComboBox cb)
        {
            if (cb.SelectedItem == null)
                return new DictionaryEntry();
            return (DictionaryEntry)cb.SelectedItem;
        }

        /// <summary>
        /// 获取选中项的键
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cb"></param>
        /// <returns></returns>
        public static T GetSelectedItemKey<T>(this ComboBox cb)
        {
            if (cb.SelectedItem == null)
                return default(T);
            return (T)cb.GetSelectedItem().Key;
        }
        /// <summary>
        /// 获取选中项的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cb"></param>
        /// <returns></returns>
        public static T GetSelectedItemValue<T>(this ComboBox cb)
        {
            if (cb.SelectedItem == null)
                return default(T);
            return (T)cb.GetSelectedItem().Value;
        }
        /// <summary>
        /// 设置默认参数
        /// </summary>
        /// <param name="cb"></param>
        public static void Default(this ComboBox cb)
        {
            cb.DisplayMember = "Value";
            if (cb.Items.Count > 0)
                cb.SelectedIndex = 0;
        }
    }
}
#endif