/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: UrlHelper.cs
Author: VicBilibily
Description: 
    本代码主要定义一些 URL 地址相关的帮助方法，一般都是偶尔需要用到的低频方法。
*****************************************************************************/

using System;
using System.Linq;
using System.Net;

namespace OYMLCN.Tools
{
    /// <summary>
    /// Url地址相关的辅助方法
    /// </summary>
    public static class UrlHelper
    {
        /// <summary>
        ///   获取URL字符串表示的对应合法的 IPv4 或 IPv6 地址。
        /// </summary>
        /// <param name="value">要获取信息的 IP 地址或访问地址。</param>
        /// <returns>
        ///    根据 <paramref name="value"/> 解析获得的所有 IP 地址，或者是一个空序列。
        /// </returns>
        public static string[] GetIPAddresses(string value)
        {
            string[] ipArray = new string[0];
            if (string.IsNullOrWhiteSpace(value)) return ipArray;

            string ipAddress = null;
            UriHostNameType hostType = Uri.CheckHostName(value);
            if (hostType == UriHostNameType.Unknown)
            {
                if (Uri.TryCreate(value, UriKind.Absolute, out Uri url) && !string.IsNullOrEmpty(url.Host))
                    ipAddress = IPAddressTryParse(url.Host);
                else if (Uri.TryCreate(string.Format("http://{0}", value), UriKind.Absolute, out url))
                    ipAddress = IPAddressTryParse(url.Host);
                if (string.IsNullOrEmpty(ipAddress) && !string.IsNullOrEmpty(url.Host))
                    ipArray = Dns.GetHostAddresses(url.Host).Select(ip => ip.ToString()).SkipWhile(string.IsNullOrEmpty).ToArray();
            }
            else if (hostType == UriHostNameType.IPv4 || hostType == UriHostNameType.IPv6)
                ipAddress = IPAddressTryParse(value);

            if (ipArray.Any()) return ipArray;
            return new[] { ipAddress };
        }
        private static string IPAddressTryParse(string value)
        {
            string ipAddress = string.Empty;
            IPAddress _ipAdr;
            if (IPAddress.TryParse(value, out _ipAdr))
                ipAddress = _ipAdr.ToString();
            return ipAddress;
        }

        /// <summary>
        ///   验证字符串是否是有效的 IP 地址。
        /// </summary>
        /// <param name="value">要验证的 IP 地址</param>
        /// <returns> 
        ///   <para>如果 <paramref name="value"/> 是否是有效的 IP 地址则为
        ///   <see langword="true"/>，否则为 <see langword="false"/>。</para>
        ///   <para>如果是 IPv6 地址带有端口号，默认为有效 IP 并返回
        ///   <see langword="true"/>，但 IPv4 地址带端口会返回
        ///   <see langword="false"/>。</para>
        /// </returns>
        public static bool CheckIPAddress(string value)
            => IPAddress.TryParse(value, out _);
        /// <summary>
        ///   验证字符串是否是有效的 IPv4 地址。
        /// </summary>
        /// <param name="value">要验证的主机名或 IP 地址</param>
        /// <returns>
        ///   如果 <paramref name="value"/> 是有效的 IPv4 地址则为
        ///   <see langword="true"/>，否则为 <see langword="false"/>。
        /// </returns>
        public static bool CheckIPv4(string value)
            => Uri.CheckHostName(value) == UriHostNameType.IPv4;
        /// <summary>
        ///   验证字符串是否是有效的 IPv6 地址。
        /// </summary>
        /// <param name="value">要验证的主机名或 IP 地址</param>
        /// <returns>
        ///   如果 <paramref name="value"/> 是有效的 IPv6 地址则为
        ///   <see langword="true"/>，否则为 <see langword="false"/>。
        /// </returns>
        public static bool CheckIPv6(string value)
            => Uri.CheckHostName(value) == UriHostNameType.IPv6;


    }
}
