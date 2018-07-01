#if NET461
using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace OYMLCN
{
    /// <summary>
    /// SystemHelper
    /// </summary>
    public static class SystemHelpers
    {
        [DllImport(@"wininet", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "InternetSetOption", CallingConvention = CallingConvention.StdCall)]
        static extern bool InternetSetOption(int hInternet, int dmOption, IntPtr lpBuffer, int dwBufferLength);

        static void SetIEProxy(bool enable = false, bool global = false, string host = "127.0.0.1", int port = 1080, string autoConfigPath = "")
        {
            using (var setting = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true))
            {
                setting.SetValue("MigrateProxy", 1);
                setting.DeleteValue("ProxyOverride", false);
                setting.DeleteValue("ProxyServer", false);
                setting.DeleteValue("AutoConfigURL", false);
                setting.SetValue("ProxyEnable", 0);
                if (enable)
                {
                    if (!global)
                        setting.SetValue("AutoConfigURL", $"http://{host}:{port.ToString()}/{autoConfigPath}?t={DateTime.Now.ToString("yyyyMMddHHmmssfff")}");
                    else
                    {
                        setting.SetValue("ProxyOverride", "<local>;localhost;127.*;10.*;172.16.*;172.17.*;172.18.*;172.19.*;172.20.*;172.21.*;172.22.*;172.23.*;172.24.*;172.25.*;172.26.*;172.27.*;172.28.*;172.29.*;172.30.*;172.31.*;172.32.*;192.168.*");
                        setting.SetValue("ProxyServer", $"{host}:{port.ToString()}");
                        setting.SetValue("ProxyEnable", 1);
                    }
                }
            }

            //激活代理设置
            InternetSetOption(0, 39, IntPtr.Zero, 0);
            InternetSetOption(0, 37, IntPtr.Zero, 0);
        }

        /// <summary>
        /// 设置自动智能代理
        /// </summary>
        /// <param name="host">域名</param>
        /// <param name="port">端口</param>
        /// <param name="autoConfigPath">配置文件路径</param>
        public static void SetIEProxyWithAutoConfigUrl(string host = "127.0.0.1", int port = 1080, string autoConfigPath = "pac") =>
            SetIEProxy(true, false, host, port, autoConfigPath);
        /// <summary>
        /// 设置全局代理
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public static void SetIEProxyWithGlobal(string host = "127.0.0.1", int port = 1080) =>
            SetIEProxy(true, true, host, port);
        /// <summary>
        /// 设置禁用代理
        /// </summary>
        public static void SetIEProxyInDisable() => SetIEProxy();

        static int GetWindowFromPoint(int xPoint, int yPoint) =>
            Environment.Is64BitProcess ?
                WindowFromPoint(new POINT() { x = xPoint, y = yPoint })
                : WindowFromPoint(xPoint, yPoint);
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
#endif