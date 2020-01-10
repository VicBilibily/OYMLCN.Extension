using System;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Management;
using OYMLCN.Extensions;
using System.Collections.Generic;

namespace OYMLCN.Helpers.Hardware
{
    /// <summary>
    /// 定义CPU的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CPU_INFO
    {
#pragma warning disable 1591
        public uint dwOemId;
        public uint dwPageSize;
        public uint lpMinimumApplicationAddress;
        public uint lpMaximumApplicationAddress;
        public uint dwActiveProcessorMask;
        public uint dwNumberOfProcessors;
        public uint dwProcessorType;
        public uint dwAllocationGranularity;
        public uint dwProcessorLevel;
        public uint dwProcessorRevision;
#pragma warning restore 1591
    }
    /// <summary>
    /// 定义内存的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryInfo
    {
#pragma warning disable 1591
        public uint dwLength;
        public uint dwMemoryLoad;
        public uint dwTotalPhys;
        public uint dwAvailPhys;
        public uint dwTotalPageFile;
        public uint dwAvailPageFile;
        public uint dwTotalVirtual;
        public uint dwAvailVirtual;
#pragma warning restore 1591
    }
    /// <summary>
    /// 定义系统时间的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemtimeInfo
    {
#pragma warning disable 1591
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;
#pragma warning restore 1591
    }

    /// <summary>
    /// CPU模型
    /// </summary>
    public class CpuInfo
    {
        /// <summary>
        /// 设备ID端口
        /// </summary>
        public string DeviceID { get; set; }
        /// <summary>
        /// CPU型号 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// CPU厂商
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// CPU最大睿频
        /// </summary>
        public string MaxClockSpeed { get; set; }
        /// <summary>
        /// CPU的时钟频率
        /// </summary>
        public string CurrentClockSpeed { get; set; }
        /// <summary>
        /// CPU核心数
        /// </summary>
        public int NumberOfCores { get; set; }
        /// <summary>
        /// 逻辑处理器核心数
        /// </summary>
        public int NumberOfLogicalProcessors { get; set; }
        /// <summary>
        /// CPU使用率
        /// </summary>
        public double CpuLoad { get; set; }
        /// <summary>
        /// CPU位宽
        /// </summary>
        public string DataWidth { get; set; }
        /// <summary>
        /// 核心温度
        /// </summary>
        public double Temperature { get; set; }
    }
    /// <summary>
    /// 内存条模型
    /// </summary>
    public class RamInfo
    {
#pragma warning disable 1591
        public double MemoryAvailable { get; set; }
        public double PhysicalMemory { get; set; }
        public double TotalPageFile { get; set; }
        public double AvailablePageFile { get; set; }
        public double TotalVirtual { get; set; }
        public double AvailableVirtual { get; set; }

        public double MemoryUsage => (1 - MemoryAvailable / PhysicalMemory) * 100;
#pragma warning restore 1591
    }

    /// <summary>
    /// 磁盘数据
    /// </summary>
    public enum DiskData
    {
        /// <summary>
        /// 读写
        /// </summary>
        ReadAndWrite,
        /// <summary>
        /// 读
        /// </summary>
        Read,
        /// <summary>
        /// 写
        /// </summary>
        Write
    }
    /// <summary>
    /// 网络数据
    /// </summary>
    public enum NetData
    {
        /// <summary>
        /// 收发
        /// </summary>
        ReceivedAndSent,

        /// <summary>
        /// 收
        /// </summary>
        Received,

        /// <summary>
        /// 发
        /// </summary>
        Sent
    }
    /// <summary>
    /// 字节单位枚举
    /// </summary>
    enum Unit
    {
        B,
        KB,
        MB,
        GB,
        EB
    }
}

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 硬件信息
    /// </summary>
    public static class HardwareHelpers
    {
        /// <summary>
        /// CPU 标识
        /// </summary>
        public static IList<string> CPUID
        {
            get
            {
                IList<string> list = new List<string>();
                try
                {
                    ManagementClass mc = new ManagementClass("Win32_Processor");
                    ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        var strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                        if (strCpuID.IsNotNullOrEmpty())
                            list.Add(strCpuID);
                        break;
                    }
                }
                catch { }
                return list;
            }
        }

        /// <summary>
        /// 电脑网卡的MAC地址
        /// </summary>
        public static IList<string> MacAddress => SystemInfoHelpers.GetMacAddress();

        /// <summary>
        /// 主硬盘 标识
        /// </summary>
        public static IList<string> DiskID
        {
            get
            {
                IList<string> list = new List<string>();
                try
                {
                    var mc = new ManagementClass("Win32_DiskDrive");
                    var moc = mc.GetInstances();
                    foreach (var o in moc)
                    {
                        var mo = (ManagementObject)o;
                        var hDid = (string)mo.Properties["Model"].Value;
                        if (hDid.IsNotNullOrEmpty())
                            list.Add(hDid);
                    }
                }
                catch { }
                return list;
            }
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static IList<string> IpAddress => SystemInfoHelpers.GetIPAddress();

        /// <summary>
        /// 操作系统的登录用户名
        /// </summary>
        /// <returns></returns>
        public static string UserName
            => Environment.UserName ?? string.Empty;

        /// <summary>
        /// 获取计算机名
        /// </summary>
        /// <returns></returns>
        public static string ComputerName
            => Environment.MachineName ?? string.Empty;

        /// <summary>
        /// PC类型
        /// </summary>
        public static string SystemType
        {
            get
            {
                var st = "";
                try
                {
                    var mc = new ManagementClass("Win32_ComputerSystem");
                    var moc = mc.GetInstances();
                    foreach (var o in moc)
                    {
                        var mo = (ManagementObject)o;
                        st = mo["SystemType"].ToString();
                    }
                }
                catch { }
                return st ?? string.Empty;
            }
        }

        /// <summary>
        /// 物理内存总数
        /// </summary>
        /// <returns></returns>
        public static string TotalPhysicalMemory
        {
            get
            {
                var st = "";
                try
                {
                    var mc = new ManagementClass("Win32_ComputerSystem");
                    var moc = mc.GetInstances();
                    foreach (var o in moc)
                    {
                        var mo = (ManagementObject)o;
                        st = mo["TotalPhysicalMemory"].ToString();
                    }
                }
                catch { }
                return st;
            }
        }

        private const int INTERNET_CONNECTION_MODEM = 1;
        private const int INTERNET_CONNECTION_LAN = 2;
        [DllImport("winInet.dll")]
        private static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved);
        private static bool PingNetAddress(string strNetAdd)
        {
            bool Flage = false;
            Ping ping = new Ping();
            try
            {
                PingReply pr = ping.Send(strNetAdd, 3000);
                if (pr.Status == IPStatus.TimedOut)
                    Flage = false;
                if (pr.Status == IPStatus.Success)
                    Flage = true;
                else
                    Flage = false;
            }
            catch
            {
                Flage = false;
            }
            return Flage;
        }
        /// <summary>
        /// 判断网络连接到指定服务器是否成功
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool CheckConnection(string address)
        {
            int iNetStatus = 0;
            System.Int32 dwFlag = new int();
            if (!InternetGetConnectedState(ref dwFlag, 0))
            {
                //没有能连上互联网
                iNetStatus = 1;
            }
            else if ((dwFlag & INTERNET_CONNECTION_MODEM) != 0)
            {
                //采用调治解调器上网,需要进一步判断能否登录具体网站
                if (PingNetAddress(address))
                    //可以ping通给定的网址,网络OK
                    iNetStatus = 2;
                else
                    //不可以ping通给定的网址,网络不OK
                    iNetStatus = 4;
            }

            else if ((dwFlag & INTERNET_CONNECTION_LAN) != 0)
            {
                //采用网卡上网,需要进一步判断能否登录具体网站
                if (PingNetAddress(address))
                    //可以ping通给定的网址,网络OK
                    iNetStatus = 3;
                else
                    //不可以ping通给定的网址,网络不OK
                    iNetStatus = 5;
            }
            switch (iNetStatus)
            {
                case 2:
                case 3:
                    return true;
                default:
                    return false;
            }
        }
    }
}