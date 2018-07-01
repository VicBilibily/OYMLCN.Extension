#if NET461
using System;
using System.Management;
using System.Runtime.InteropServices;

namespace OYMLCN
{

    /// <summary>
    /// 系统信息辅助方法
    /// </summary>
    public static class SystemInfoHelpers
    {
        /// <summary>
        /// 内存信息
        /// </summary>
        public sealed class Memory
        {
            //定义内存的信息结构
            [StructLayout(LayoutKind.Sequential)]
            private struct MEMORY_INFO
            {
                public uint dwLength;
                public uint dwMemoryLoad;
                public uint dwTotalPhys;
                public uint dwAvailPhys;
                public uint dwTotalPageFile;
                public uint dwAvailPageFile;
                public uint dwTotalVirtual;
                public uint dwAvailVirtual;
            }
            [DllImport("kernel32")]
            private static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);

            private MEMORY_INFO MemInfo = new MEMORY_INFO();
            private Memory() => GlobalMemoryStatus(ref MemInfo);


            static uint PhysicalMemorySize = 0, FreePhysicalMemory = 0,
                TotalSwapSpaceSize = 0, FreeSpaceInPagingFiles = 0,
                VirtualMemorySize = 0, FreeVirtualMemory = 0;


            /// <summary>
            /// 内存配置及占用信息
            /// </summary>
            /// <returns></returns>
            public static Memory Initialize()
            {
                var memory = new Memory();
                ManagementClass osClass = new ManagementClass("Win32_OperatingSystem");
                foreach (ManagementObject obj in osClass.GetInstances())
                {
                    if (obj["TotalVisibleMemorySize"] != null)
                        PhysicalMemorySize = (uint)(ulong)obj["TotalVisibleMemorySize"];
                    if (obj["FreePhysicalMemory"] != null)
                        FreePhysicalMemory = (uint)(ulong)obj["FreePhysicalMemory"];

                    if (obj["TotalSwapSpaceSize"] != null)
                        TotalSwapSpaceSize = (uint)(ulong)obj["TotalSwapSpaceSize"];
                    if (obj["FreeSpaceInPagingFiles"] != null)
                        FreeSpaceInPagingFiles = (uint)(ulong)obj["FreeSpaceInPagingFiles"];

                    if (obj["TotalVirtualMemorySize"] != null)
                        VirtualMemorySize = (uint)(ulong)obj["TotalVirtualMemorySize"];
                    if (obj["FreeVirtualMemory"] != null)
                        FreeVirtualMemory = (uint)(ulong)obj["FreeVirtualMemory"];

                    break;
                }
                return memory;
            }




            /// <summary>
            /// 正在使用的内存百分比
            /// </summary>
            public uint UsedPercent => MemInfo.dwMemoryLoad;
            /// <summary>
            /// 安装的物理内存总量
            /// </summary>
            public uint Total => PhysicalMemorySize > 0 ? PhysicalMemorySize : MemInfo.dwTotalPhys / 1024;
            /// <summary>
            /// 可使用的物理内存
            /// </summary>
            public uint Availale => FreePhysicalMemory > 0 ? FreePhysicalMemory : MemInfo.dwAvailPhys / 1024;
            /// <summary>
            /// 交换文件总大小
            /// </summary>
            public uint TotalSwap => TotalSwapSpaceSize > 0 ? TotalSwapSpaceSize : MemInfo.dwTotalPageFile / 1024;
            /// <summary>
            /// 尚可交换文件大小为
            /// </summary>
            public uint AvailableSwap => FreeSpaceInPagingFiles > 0 ? FreeSpaceInPagingFiles : MemInfo.dwAvailPageFile / 1024;
            /// <summary>
            /// 总虚拟内存
            /// </summary>
            public uint TotalVirtual => VirtualMemorySize > 0 ? VirtualMemorySize : MemInfo.dwTotalVirtual / 1024;
            /// <summary>
            /// 可用虚拟内存
            /// </summary>
            public uint AvailableVirtual => FreeVirtualMemory > 0 ? FreeVirtualMemory : MemInfo.dwAvailVirtual / 1024;
        }
        
        /// <summary>
        /// 获取系统版本号
        /// 要正确获取Win10及Win8.1的版本号
        /// 请参考http://www.cnblogs.com/chihirosan/p/5139078.html
        /// </summary>
        public static string Version => Environment.OSVersion.Version.Major + "." + Environment.OSVersion.Version.Minor;
        /// <summary>
        /// 系统版本
        /// </summary>
        public enum WindowsVersion
        {
            /// <summary>
            /// Windows7
            /// </summary>
            Windows7,
            /// <summary>
            /// Windows8/8.1
            /// </summary>
            Windows8,
            /// <summary>
            /// Windows10
            /// </summary>
            Windows10,
            /// <summary>
            /// Windows2008
            /// </summary>
            Windows2008
        }

        /// <summary>
        /// 判断系统是否为Windows Server2008
        /// </summary>
        public static bool IsWindows2008 => Version == "6.0";
        /// <summary>
        /// 判断系统是否为Windows7/Server2012 R2
        /// </summary>
        public static bool IsWindows7 => Version == "6.1";
        /// <summary>
        /// 判断系统是否为Windows8/Server2012
        /// </summary>
        public static bool IsWindows8 => Version == "6.2";
        /// <summary>
        /// 判断系统是否为Windows8.1/Server2012 R2
        /// </summary>
        public static bool IsWindows8_1 => Version == "6.3";
        /// <summary>
        /// 判断系统是否为Windows10/Server2016
        /// </summary>
        public static bool IsWindows10 => Environment.OSVersion.Version.Major == 10;

        /// <summary>
        /// 判断系统版本是否高于或是Windows Server2008
        /// </summary>
        public static bool IsWindows2008OrHigher => Environment.OSVersion.Version.Major >= 6;
        /// <summary>
        /// 判断系统版本是否高于或是Windows7/Server2012 R2
        /// </summary>
        public static bool IsWindows7OrHigher => Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 1 || Environment.OSVersion.Version.Major > 6;
        /// <summary>
        /// 判断系统版本是否高于或是Windows7/Server2012 R2
        /// </summary>
        public static bool IsWindows8OrHigher => Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 2 || Environment.OSVersion.Version.Major > 6;

    }
}
#endif
