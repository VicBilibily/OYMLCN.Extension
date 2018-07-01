#if NET461
using Microsoft.Win32;
using System;

namespace OYMLCN
{
    /// <summary>
    /// 注册表操作
    /// </summary>
    public static class RegistryHelpers
    {
        /// <summary>
        /// Url协议操作
        /// </summary>
        public static class URLProcotol
        {
            /// <summary>
            /// 注册启动项到注册表
            /// </summary>
            /// <param name="procotol"></param>
            /// <param name="exeFullPath">System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName</param>
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
            public static void UnReg(string procotol) =>
                //直接删除节点
                Registry.ClassesRoot.DeleteSubKeyTree(procotol);
        }

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
                return RegistryKey.OpenBaseKey(
                    hive, 
                    Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32
                    ).OpenSubKey(name, writable);
            }
            catch
            {
                return null;
            }
        }
    }
}
#endif