using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 关闭窗口的辅助方法
    /// </summary>
    public static class CloseWindowHelpers
    {
        static int GetWindowFromPoint(int xPoint, int yPoint)
            =>
                Environment.Is64BitProcess ?
                    WindowFromPoint(new POINT() { x = xPoint, y = yPoint }) :
                    WindowFromPoint(xPoint, yPoint);

        [DllImport("user32", EntryPoint = "WindowFromPoint")]//指定坐标处窗体句柄
        static extern int WindowFromPoint(int xPoint, int yPoint);
        struct POINT
        {
            public int x { get; set; }
            public int y { get; set; }
        }
        [DllImport("user32", EntryPoint = "WindowFromPoint")]//指定坐标处窗体句柄
        static extern int WindowFromPoint(POINT point);
        const int WM_CLOSE = 0x10;
        [DllImport("user32", EntryPoint = "SendMessageA")]
        static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);
        //[DllImport("user32", EntryPoint = "GetWindowText")]
        //static extern int GetWindowText(int hwnd, StringBuilder lpString, int cch);
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(int hWnd, ref RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int Left;//最左坐标
            public int Top;//最上坐标
            public int Right;//最右坐标
            public int Bottom;//最下坐标
        }

        /// <summary>
        /// 关闭指定坐标位置的窗口
        /// </summary>
        /// <param name="xPoint"></param>
        /// <param name="yPoint"></param>
        /// <returns></returns>
        public static bool CloseWindowFormPoint(int xPoint, int yPoint)
        {
            var point = GetWindowFromPoint(xPoint, yPoint);
            if (point > 0)
                return SendMessage(point, WM_CLOSE, 0, 0) > 0;
            return false;
        }
        /// <summary>
        /// 关闭指定坐标位置并且窗口大小匹配的窗口
        /// </summary>
        /// <param name="xPoint"></param>
        /// <param name="yPoint"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static bool CloseWindowFormPointWidthSize(int xPoint, int yPoint, int width, int height)
        {
            var point = GetWindowFromPoint(xPoint, yPoint);
            if (point > 0)
            {
                RECT rc = new RECT();
                GetWindowRect(point, ref rc);
                if (rc.Right - rc.Left == width && rc.Bottom - rc.Top == height)
                {
                    SendMessage(point, WM_CLOSE, 0, 0);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 关闭屏幕右下角的弹出窗口
        /// </summary>
        /// <returns></returns>
        public static bool CloseWindowFormOnRightBottom()
        {
            var area = SystemInformation.WorkingArea;
            int iAreaWidth = area.Width, iAreaHeight = area.Height;
            var point = GetWindowFromPoint(iAreaWidth - 10, iAreaHeight - 10);
            if (point > 0)
            {
                RECT rc = new RECT();
                GetWindowRect(point, ref rc);
                if (rc.Left != 0 && rc.Top != 0 && rc.Right == iAreaWidth && rc.Bottom == iAreaHeight)
                {
                    SendMessage(point, WM_CLOSE, 0, 0);
                    return true;
                }
            }
            return false;
        }
    }

}