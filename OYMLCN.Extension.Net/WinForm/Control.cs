#if NET461
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace OYMLCN.WinForm.Extensions
{
    /// <summary>
    /// ControlExtension
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// 获取全部子控件
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="control"></param>
        /// <returns></returns>
        public static IEnumerable<TControl> GetChildControls<TControl>(this Control control) where TControl : Control
        {
            if (control.Controls.Count == 0)
                return Enumerable.Empty<TControl>();

            var children = control.Controls.OfType<TControl>().ToList();
            return children.SelectMany(GetChildControls<TControl>).Concat(children);
        }

        // Workaround NotifyIcon's 63 chars limit
        // https://stackoverflow.com/questions/579665/how-can-i-show-a-systray-tooltip-longer-than-63-chars
        /// <summary>
        /// 设置任务栏图标提示文字
        /// </summary>
        /// <param name="ni"></param>
        /// <param name="text"></param>
        public static void SetNotifyIconText(this NotifyIcon ni, string text)
        {
            if (text.Length >= 128)
                throw new ArgumentOutOfRangeException("字符长度不能大于 127");
            Type t = typeof(NotifyIcon);
            BindingFlags hidden = BindingFlags.NonPublic | BindingFlags.Instance;
            t.GetField("text", hidden).SetValue(ni, text);
            if ((bool)t.GetField("added", hidden).GetValue(ni))
                t.GetMethod("UpdateIcon", hidden).Invoke(ni, new object[] { true });
        }
    }
}
#endif