using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 注册表操作
    /// </summary>
    public static class HotKeyHelpers
    {
        /// <summary>
        /// 定义了辅助键的名称 将数字转变为字符以便于识别
        /// </summary>
        [Flags]
        public enum KeyModifiers
        {
            /// <summary>
            /// None
            /// </summary>
            None = 0,
            /// <summary>
            /// Alt
            /// </summary>
            Alt = 1,
            /// <summary>
            /// Ctrl
            /// </summary>
            Ctrl = 2,
            /// <summary>
            /// Shift
            /// </summary>
            Shift = 4,
            /// <summary>
            /// WindowsKey
            /// </summary>
            WindowsKey = 8
        }
        /// <summary>
        /// 如果函数执行成功，返回值不为0。
        /// </summary>
        /// <param name="hWnd">要定义热键的窗口的句柄</param>
        /// <param name="id">定义热键ID（不能与其它ID重复）</param>
        /// <param name="fsModifiers">标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效</param>
        /// <param name="vk">定义热键的内容</param>
        /// <returns></returns>
        [DllImport("user32", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);
        /// <summary>
        /// 取消热键
        /// </summary>
        /// <param name="hWnd">要取消热键的窗口的句柄</param>
        /// <param name="id">要取消热键的ID</param>
        /// <returns></returns>
        [DllImport("user32", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}