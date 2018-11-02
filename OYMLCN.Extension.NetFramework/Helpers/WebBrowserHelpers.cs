using Microsoft.Win32;
using OYMLCN.Extensions;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// WebBrowserHelper
    /// </summary>
    public static class WebBrowserHelpers
    {
        static string appName = Process.GetCurrentProcess().ProcessName + ".exe";
        #region SetIEKeyforWebBrowserControl
        static void SetIEKeyforWebBrowserControl(string verKey)
        {
            // 64位
            var keyPath = @"SOFTWARE\\Wow6432Node\\Microsoft\\Internet Explorer\\MAIN\\FeatureControl\\FEATURE_BROWSER_EMULATION";
            using (var Regkey = Registry.LocalMachine.OpenSubKey(keyPath))
            {
                if (Regkey != null && Convert.ToString(Regkey.GetValue(appName)) != verKey)
                {
                    if (!ProcessHelpers.IsAdministrator)
                        throw new Exception("需要管理员权限");
                    using (var SetRegkey = Registry.LocalMachine.OpenSubKey(keyPath, true))
                        SetRegkey.SetValue(appName, verKey, RegistryValueKind.DWord);
                }
            }
            // 32位
            keyPath = @"SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION";
            using (var Regkey = Registry.LocalMachine.OpenSubKey(keyPath))
            {
                if (Regkey != null && Convert.ToString(Regkey.GetValue(appName)) != verKey)
                {
                    if (!ProcessHelpers.IsAdministrator)
                        throw new Exception("需要管理员权限");
                    using (var SetRegkey = Registry.LocalMachine.OpenSubKey(keyPath, true))
                        SetRegkey.SetValue(appName, verKey, RegistryValueKind.DWord);
                }
            }
        }
        /// <summary>
        /// 设置WebBrowser调用 IE11 默认模式
        /// <para>需要使用管理员权限</para>
        /// </summary>
        public static void UseIE11WebBrowser() => SetIEKeyforWebBrowserControl("11000");
        /// <summary>
        /// 设置WebBrowser调用 IE11 Edge模式
        /// <para>需要使用管理员权限</para>
        /// </summary>
        public static void UseIE11EdgeWebBrowser() => SetIEKeyforWebBrowserControl("11001");
        /// <summary>
        /// 设置WebBrowser调用 IE10 默认模式
        /// <para>需要使用管理员权限</para>
        /// </summary>
        public static void UseIE10WebBrowser() => SetIEKeyforWebBrowserControl("10000");
        /// <summary>
        /// 设置WebBrowser调用 IE10 Standards模式
        /// <para>需要使用管理员权限</para>
        /// </summary>
        public static void UseIE10StandardsWebBrowser() => SetIEKeyforWebBrowserControl("10001");
        /// <summary>
        /// 设置WebBrowser调用 IE9 默认模式
        /// <para>需要使用管理员权限</para>
        /// </summary>
        public static void UseIE9WebBrowser() => SetIEKeyforWebBrowserControl("9000");
        /// <summary>
        /// 设置WebBrowser调用 IE9 Standards模式
        /// <para>需要使用管理员权限</para>
        /// </summary>
        public static void UseIE9StandardsWebBrowser() => SetIEKeyforWebBrowserControl("9999");
        /// <summary>
        /// 设置WebBrowser调用 IE8 默认模式
        /// <para>需要使用管理员权限</para>
        /// </summary>
        public static void UseIE8WebBrowser() => SetIEKeyforWebBrowserControl("8000");
        /// <summary>
        /// 设置WebBrowser调用 IE8 Standards模式
        /// <para>需要使用管理员权限</para>
        /// </summary>
        public static void UseIE8StandardsWebBrowser() => SetIEKeyforWebBrowserControl("8888");
        /// <summary>
        /// 设置WebBrowser调用 IE7 模式
        /// <para>需要使用管理员权限</para>
        /// </summary>
        public static void UseIE7WebBrowser() => SetIEKeyforWebBrowserControl("7000");
        #endregion

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);
        /// <summary>
        /// 获取指定地址的Cookies
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetCookies(string url)
        {
            uint datasize = 1024;
            StringBuilder cookieData = new StringBuilder((int)datasize);
            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;

                cookieData = new StringBuilder((int)datasize);
                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                    return null;
            }
            return cookieData.ToString();
        }

        #region IE代理
        //设置代理选项                
        const int INTERNET_OPTION_PROXY = 38;
        //设置代理类型                
        const int INTERNET_OPEN_TYPE_PROXY = 3;
        //设置代理类型，直接访问，不需要通过代理服务器
        const int INTERNET_OPEN_TYPE_DIRECT = 1;

        //You can change the proxy with InternetSetOption method from the wininet.dll                
        //这个就是设置一个Internet 选项。设置代理是Internet 选项其中的一个功能
        [DllImport("wininet.dll", SetLastError = true)]
        static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        //定义代理信息的结构体                  
        struct Struct_INTERNET_PROXY_INFO
        {
            public int dwAccessType;
            public IntPtr proxy;
            public IntPtr proxyBypass;
        }
        //设置代理的方法
        //strProxy为代理IP:端口        
        static bool InternetSetOption(string strProxy)
        {
            int bufferLength;
            IntPtr intptrStruct;
            Struct_INTERNET_PROXY_INFO struct_IPI;

            //Filling in structure 
            if ((strProxy =  strProxy.Trim()).IsNullOrEmpty())
                struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_DIRECT;
            else
                struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_PROXY;
            //把代理地址设置到非托管内存地址中
            struct_IPI.proxy = Marshal.StringToHGlobalAnsi(strProxy);
            //代理通过本地连接到代理服务器上
            struct_IPI.proxyBypass = Marshal.StringToHGlobalAnsi("local");
            bufferLength = Marshal.SizeOf(struct_IPI);

            //Allocating memory
            //关联到内存
            intptrStruct = Marshal.AllocCoTaskMem(bufferLength);

            //Converting structure to IntPtr 
            //把结构体转换到句柄
            Marshal.StructureToPtr(struct_IPI, intptrStruct, true);
            return InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY, intptrStruct, bufferLength);
        }
        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="strProxy">代理连接</param>
        /// <returns></returns>
        public static bool SetIEProxy(string strProxy) => InternetSetOption(strProxy);
        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="port">本地代理端口</param>
        /// <returns></returns>
        public static bool SetIEProxy(int port) => InternetSetOption("127.0.0.1:" + port.ToString());
        /// <summary>
        /// 取消代理
        /// </summary>
        /// <returns></returns>
        public static bool DisableIEProxy() => InternetSetOption(string.Empty);
        #endregion

        const int INTERNET_OPTION_SUPPRESS_BEHAVIOR = 81;
        const int INTERNET_SUPPRESS_COOKIE_PERSIST = 3;

        /// <summary>
        /// 禁用Cookie保持
        /// </summary>
        public static void SuppressCookiePersistence()
        {
            var lpBuffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)));
            Marshal.StructureToPtr(INTERNET_SUPPRESS_COOKIE_PERSIST, lpBuffer, true);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SUPPRESS_BEHAVIOR, lpBuffer, sizeof(int));
            Marshal.FreeCoTaskMem(lpBuffer);
        }

    }
}
