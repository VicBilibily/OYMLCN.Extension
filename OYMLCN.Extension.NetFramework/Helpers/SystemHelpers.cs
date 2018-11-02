using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// SystemHelper
    /// </summary>
    public static class SystemHelpers
    {
#if NET35
        internal static class RegistryExtensions
        {

            public enum RegistryHiveType
            {
                X86,
                X64
            }

            static readonly Dictionary<RegistryHive, UIntPtr> _hiveKeys =
                new Dictionary<RegistryHive, UIntPtr> {
                    { RegistryHive.ClassesRoot, new UIntPtr(0x80000000u) },
                    { RegistryHive.CurrentConfig, new UIntPtr(0x80000005u) },
                    { RegistryHive.CurrentUser, new UIntPtr(0x80000001u) },
                    { RegistryHive.DynData, new UIntPtr(0x80000006u) },
                    { RegistryHive.LocalMachine, new UIntPtr(0x80000002u) },
                    { RegistryHive.PerformanceData, new UIntPtr(0x80000004u) },
                    { RegistryHive.Users, new UIntPtr(0x80000003u) }
                };

            static readonly Dictionary<RegistryHiveType, RegistryAccessMask> _accessMasks =
                new Dictionary<RegistryHiveType, RegistryAccessMask> {
                    { RegistryHiveType.X64, RegistryAccessMask.Wow6464 },
                    { RegistryHiveType.X86, RegistryAccessMask.WoW6432 }
                };

            [Flags]
            public enum RegistryAccessMask
            {
                QueryValue = 0x0001,
                SetValue = 0x0002,
                CreateSubKey = 0x0004,
                EnumerateSubKeys = 0x0008,
                Notify = 0x0010,
                CreateLink = 0x0020,
                WoW6432 = 0x0200,
                Wow6464 = 0x0100,
                Write = 0x20006,
                Read = 0x20019,
                Execute = 0x20019,
                AllAccess = 0xF003F
            }

            [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
            public static extern int RegOpenKeyEx(UIntPtr hKey, string subKey, uint ulOptions, uint samDesired, out IntPtr hkResult);

            public static RegistryKey OpenBaseKey(RegistryHive registryHive, RegistryHiveType registryType)
            {
                UIntPtr hiveKey = _hiveKeys[registryHive];
                if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major > 5)
                {
                    RegistryAccessMask flags = RegistryAccessMask.QueryValue | RegistryAccessMask.EnumerateSubKeys | RegistryAccessMask.SetValue | RegistryAccessMask.CreateSubKey | _accessMasks[registryType];
                    IntPtr keyHandlePointer = IntPtr.Zero;
                    int result = RegOpenKeyEx(hiveKey, string.Empty, 0, (uint)flags, out keyHandlePointer);
                    if (result == 0)
                    {
                        var safeRegistryHandleType = typeof(SafeHandleZeroOrMinusOneIsInvalid).Assembly.GetType("Microsoft.Win32.SafeHandles.SafeRegistryHandle");
                        var safeRegistryHandleConstructor = safeRegistryHandleType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(IntPtr), typeof(bool) }, null); // .NET < 4
                        if (safeRegistryHandleConstructor == null)
                            safeRegistryHandleConstructor = safeRegistryHandleType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new[] { typeof(IntPtr), typeof(bool) }, null); // .NET >= 4
                        var keyHandle = safeRegistryHandleConstructor.Invoke(new object[] { keyHandlePointer, true });
                        var net3Constructor = typeof(RegistryKey).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { safeRegistryHandleType, typeof(bool) }, null);
                        var net4Constructor = typeof(RegistryKey).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(IntPtr), typeof(bool), typeof(bool), typeof(bool), typeof(bool) }, null);
                        object key;
                        if (net4Constructor != null)
                            key = net4Constructor.Invoke(new object[] { keyHandlePointer, true, false, false, hiveKey == _hiveKeys[RegistryHive.PerformanceData] });
                        else if (net3Constructor != null)
                            key = net3Constructor.Invoke(new object[] { keyHandle, true });
                        else
                        {
                            var keyFromHandleMethod = typeof(RegistryKey).GetMethod("FromHandle", BindingFlags.Static | BindingFlags.Public, null, new[] { safeRegistryHandleType }, null);
                            key = keyFromHandleMethod.Invoke(null, new object[] { keyHandle });
                        }
                        var field = typeof(RegistryKey).GetField("keyName", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (field != null)
                            field.SetValue(key, string.Empty);
                        return (RegistryKey)key;
                    }
                    else if (result == 2) // The key does not exist.
                        return null;
                    throw new Win32Exception(result);
                }
                throw new PlatformNotSupportedException("The platform or operating system must be Windows XP or later.");
            }
        }

        #region 判断是否为64位系统
        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr LoadLibrary(string libraryName);

        [DllImport("kernel32", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        internal static extern IntPtr GetProcAddress(IntPtr hwnd, string procedureName);

        private delegate bool IsWow64ProcessDelegate([In] IntPtr handle, [Out] out bool isWow64Process);

        internal static bool IsOS64Bit
        {
            get
            {
                if (IntPtr.Size == 8 || (IntPtr.Size == 4 && Is32BitProcessOn64BitProcessor))
                    return true;
                else
                    return false;
            }
        }

        private static IsWow64ProcessDelegate GetIsWow64ProcessDelegate()
        {
            IntPtr handle = LoadLibrary("kernel32");

            if (handle != IntPtr.Zero)
            {
                IntPtr fnPtr = GetProcAddress(handle, "IsWow64Process");

                if (fnPtr != IntPtr.Zero)
                    return (IsWow64ProcessDelegate)Marshal.GetDelegateForFunctionPointer(fnPtr, typeof(IsWow64ProcessDelegate));
            }

            return null;
        }

        private static bool Is32BitProcessOn64BitProcessor
        {
            get
            {
                IsWow64ProcessDelegate fnDelegate = GetIsWow64ProcessDelegate();

                if (fnDelegate == null)
                    return false;

                bool retVal = fnDelegate.Invoke(Process.GetCurrentProcess().Handle, out bool isWow64);

                if (retVal == false)
                    return false;

                return isWow64;
            }
        }
        #endregion
#endif

        /// <summary>
        /// 打开注册表
        /// </summary>
        /// <param name="name">注册表路径</param>
        /// <param name="writable">是否以可写方式打开</param>
        /// <param name="hive">顶级节点（默认为CurrentUser）</param>
        /// <returns></returns>
        public static RegistryKey OpenRegKey(string name, bool writable = false, RegistryHive hive = RegistryHive.CurrentUser)
        {
            try
            {
#if NET35
                return RegistryExtensions.OpenBaseKey(
                    hive,
                    IsOS64Bit ? RegistryExtensions.RegistryHiveType.X64 : RegistryExtensions.RegistryHiveType.X86
                    ).OpenSubKey(name, writable);
#else
                return RegistryKey.OpenBaseKey(
                    hive,
                    Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32
                    ).OpenSubKey(name, writable);
#endif
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 可执行程序路径
        /// </summary>
        // Don't use Application.ExecutablePath
        // see https://stackoverflow.com/questions/12945805/odd-c-sharp-path-issue
        public static string ExecutablePath = Assembly.GetEntryAssembly().Location;

        /// <summary>
        /// 系统代理
        /// </summary>
        public class SystemProxy
        {
            [DllImport(@"wininet", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "InternetSetOption", CallingConvention = CallingConvention.StdCall)]
            static extern bool InternetSetOption(int hInternet, int dmOption, IntPtr lpBuffer, int dwBufferLength);

            private readonly string _host;
            private int _port;
            private readonly string _autoConfigPath;

            /// <summary>
            /// 系统代理
            /// </summary>
            /// <param name="ipOrHost">代理服务地址</param>
            /// <param name="port">代理服务端口</param>
            /// <param name="autoConfigPath">自动代理配置路径(仅包含域名后的路径)</param>
            public SystemProxy(string ipOrHost = "127.0.0.1", int port = 1080, string autoConfigPath = "pac")
            {
                _host = ipOrHost;
                _port = port;
                _autoConfigPath = autoConfigPath;
            }

            /// <summary>
            /// 设置代理
            /// </summary>
            /// <param name="enable"></param>
            /// <param name="global"></param>
            protected void SetProxy(bool enable, bool global)
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
                            setting.SetValue("AutoConfigURL", $"http://{_host}:{_port.ToString()}/{_autoConfigPath}?t={DateTime.Now.ToString("yyyyMMddHHmmssfff")}");
                        else
                        {
                            setting.SetValue("ProxyOverride", "<local>;localhost;127.*;10.*;172.16.*;172.17.*;172.18.*;172.19.*;172.20.*;172.21.*;172.22.*;172.23.*;172.24.*;172.25.*;172.26.*;172.27.*;172.28.*;172.29.*;172.30.*;172.31.*;192.168.*");
                            setting.SetValue("ProxyServer", $"{_host}:{_port.ToString()}");
                            setting.SetValue("ProxyEnable", 1);
                        }
                    }
                }

                //激活代理设置
                InternetSetOption(0, 39, IntPtr.Zero, 0);
                InternetSetOption(0, 37, IntPtr.Zero, 0);
            }

            /// <summary>
            /// 使用自动代理设置
            /// </summary>
            public void SetWithAutoConfig()
                => SetProxy(true, false);
            /// <summary>
            /// 使用全局代理设置
            /// </summary>
            public void SetGlobalProxy()
                => SetProxy(true, true);
            /// <summary>
            /// 禁用系统代理设置
            /// </summary>
            public void DisableProxy()
                => SetProxy(false, false);

        }

        /// <summary>
        /// Url协议操作
        /// </summary>
        public static class URLProcotol
        {
            /// <summary>
            /// 注册启动项到注册表
            /// </summary>
            /// <param name="procotol"></param>
            public static void Reg(string procotol)
                => Reg(procotol, ExecutablePath);
            /// <summary>
            /// 注册启动项到注册表
            /// </summary>
            /// <param name="procotol"></param>
            /// <param name="exeFullPath">指定执行程序</param>
            public static void Reg(string procotol, string exeFullPath)
            {
                //注册的协议头，即在地址栏中的路径 如QQ的：tencent://xxxxx/xxx
                var surekamKey = Registry.ClassesRoot.CreateSubKey(procotol);
                //以下这些参数都是固定的，不需要更改，直接复制过去 
                var shellKey = surekamKey.CreateSubKey("shell");
                var openKey = shellKey.CreateSubKey("open");
                var commandKey = openKey.CreateSubKey("command");
                surekamKey.SetValue("URL Protocol", "");
                //注册可执行文件取当前程序全路径
                commandKey.SetValue("", "\"" + exeFullPath + "\"" + " \"%1\"");
            }
            /// <summary>
            /// 取消注册
            /// </summary>
            /// <param name="procotol"></param>
            public static void UnReg(string procotol)
                //直接删除节点
                => Registry.ClassesRoot.DeleteSubKeyTree(procotol);
        }

        /// <summary>
        /// AutoStartup开机启动项操作
        /// </summary>
        public class AutoStartup
        {
            private readonly string Key;
            /// <summary>
            /// 将启动程序的主程序设置为开机启动项目（仅当前用户）
            /// </summary>
            /// <param name="programKey">程序集唯一标识</param>
            public AutoStartup(string programKey)
                => Key = programKey;

            /// <summary>
            /// 注册开机启动
            /// </summary>
            public void Enable()
            {
                using (RegistryKey runKey = OpenRegKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                    runKey.SetValue(Key, ExecutablePath);
            }
            /// <summary>
            /// 取消开机启动
            /// </summary>
            public void Disable()
            {
                using (RegistryKey runKey = OpenRegKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                    runKey.DeleteValue(Key);
            }

            /// <summary>
            /// 是否已经存在
            /// </summary>
            public bool IsEnabled
            {
                get
                {
                    try
                    {
                        using (RegistryKey runKey = OpenRegKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
                        {
                            if (runKey == null)
                                return false;
                            string[] runList = runKey.GetValueNames();
                            foreach (string item in runList)
                                if (item.Equals(Key, StringComparison.OrdinalIgnoreCase))
                                {
                                    // 修正路径变更后的记录
                                    if (!runKey.GetValue(Key).Equals(ExecutablePath))
                                        runKey.SetValue(Key, ExecutablePath);
                                    return true;
                                }
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }

                }
            }
        }

    }
}
