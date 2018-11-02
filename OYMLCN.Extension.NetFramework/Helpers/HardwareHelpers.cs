using System;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Management;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 硬件信息
    /// </summary>
    public static class HardwareHelpers
    {
        /// <summary>
        /// CPU 唯一标识
        /// </summary>
        public static string CPUID
        {
            get
            {
                string strCpuID = "";
                try
                {
                    ManagementClass mc = new ManagementClass("Win32_Processor");
                    ManagementObjectCollection moc = mc.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                        break;
                    }
                }
                catch { }
                return strCpuID ?? string.Empty;
            }
        }

        /// <summary>
        /// 电脑网卡的MAC地址
        /// </summary>
        public static string MacAddress
        {
            get
            {
                var mac = "";
                try
                {
                    var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                    var moc = mc.GetInstances();
                    foreach (var o in moc)
                    {
                        var mo = (ManagementObject)o;
                        if ((bool)mo["IPEnabled"] != true) continue;
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                catch { }
                return mac ?? string.Empty;
            }
        }

        /// <summary>
        /// 主硬盘 唯一标识
        /// </summary>
        public static string DiskId
        {
            get
            {
                var hDid = "";
                try
                {
                    var mc = new ManagementClass("Win32_DiskDrive");
                    var moc = mc.GetInstances();
                    foreach (var o in moc)
                    {
                        var mo = (ManagementObject)o;
                        hDid = (string)mo.Properties["Model"].Value;
                    }
                }
                catch { }
                return hDid ?? string.Empty;
            }
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string IpAddress
        {
            get
            {
                var st = "";
                try
                {
                    var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                    var moc = mc.GetInstances();
                    foreach (var o in moc)
                    {
                        var mo = (ManagementObject)o;
                        if ((bool)mo["IPEnabled"] != true) continue;
                        var ar = (Array)(mo.Properties["IpAddress"].Value);
                        st = ar.GetValue(0).ToString();
                        break;
                    }
                }
                catch { }
                return st;
            }
        }

        /// <summary>
        /// 操作系统的登录用户名
        /// </summary>
        /// <returns></returns>
        public static string UserName
        {
            get
            {
                try
                {
                    return Environment.UserName ?? string.Empty;
                }
                catch
                {
                    return string.Empty;
                }
            }

        }

        /// <summary>
        /// 获取计算机名
        /// </summary>
        /// <returns></returns>
        public static string ComputerName
        {
            get
            {
                try
                {
                    return Environment.MachineName ?? string.Empty; ;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

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