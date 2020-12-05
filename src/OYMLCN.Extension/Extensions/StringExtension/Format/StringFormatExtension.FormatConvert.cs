/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringFormatExtension.FormatConvert.cs
Author: VicBilibily
Description: 
    本代码所在文件夹的文件主要定义一些常用的字符串格式相关方法扩展，以提升开发效率为目的。
    本文件主要定义一些字符串格式转换的扩展方法。
*****************************************************************************/

namespace OYMLCN.Extensions
{
    public static partial class StringFormatExtension
    {
        /// <summary>
        ///   将 Boolean 转换为 Yes/是 或 No/否。
        /// </summary>
        /// <param name="boolean">Boolean 值</param>
        /// <param name="useCN">是否使用中文是/否，默认为 <see langword="false"/>。</param>
        /// <returns>
        ///   <para>根据 <paramref name="boolean"/> 的值返回 Yes/No。</para>
        ///   <para>若 <paramref name="useCN"/> 为
        ///   <see langword="true"/>，则返回 是/否。 </para>
        /// </returns>
        public static string ToYesOrNo(this bool boolean, bool useCN = false)
            => useCN ? (boolean ? "是" : "否") : (boolean ? "Yes" : "No");





    }
}
