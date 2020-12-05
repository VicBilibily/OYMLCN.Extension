/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringFormatExtension.Obsolete.cs
Author: VicBilibily
Description: 
    本代码所在文件夹的文件主要定义一些常用的字符串格式相关方法扩展，以提升开发效率为目的。
    本代码主要定义一些旧版本中存在，但并非常用或习惯用法的扩展方法，将在下一版本(V6)中移除。
*****************************************************************************/

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

using System;

namespace OYMLCN.Extensions
{
    public static partial class StringFormatExtension
    {
        [Obsolete("请使用 OYMLCN.Tool.UrlHelper." + nameof(Tools.UrlHelper.GetIPAddresses) + "，将在下一主要版本(V6)中移除扩展。")]
        public static string[] GetIPAddresses(this string value)
            => Tools.UrlHelper.GetIPAddresses(value);
        [Obsolete("请使用 OYMLCN.Tool.UrlHelper." + nameof(Tools.UrlHelper.CheckIPAddress) + "，将在下一主要版本(V6)中移除扩展。")]
        public static bool FormatIsIPAddress(this string value)
            => Tools.UrlHelper.CheckIPAddress(value);
        [Obsolete("请使用 OYMLCN.Tool.UrlHelper." + nameof(Tools.UrlHelper.CheckIPv4) + "，将在下一主要版本(V6)中移除扩展。")]
        public static bool FormatIsIPv4(this string value)
            => Tools.UrlHelper.CheckIPv4(value);
        [Obsolete("请使用 OYMLCN.Tool.UrlHelper." + nameof(Tools.UrlHelper.CheckIPv6) + "，将在下一主要版本(V6)中移除扩展。")]
        public static bool FormatIsIPv6(this string value)
            => Tools.UrlHelper.CheckIPv6(value);


    }
}
