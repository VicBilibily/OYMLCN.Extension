using System;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 字符串 <see cref="string"/> 类型扩展
    /// </summary>
    public static class StringExtension
    {
        /// <summary> 
        /// 检查当前字符串 是 <see langword="null" /> 或者是 空字符串 ("")
        /// </summary>
        /// <param name="value"> 要测试的字符串 </param>
        /// <returns> 如果 <paramref name="value"/> 为 <see langword="null" /> 或空字符串 ("") 则为 <see langword="true" />，否则为 <see langword="false" />。 </returns>
        public static bool IsNullOrEmpty(this string value)
            => string.IsNullOrEmpty(value);



        #region Obsolete
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        [Obsolete("建议改用标准用法" + nameof(IsNullOrEmpty) + "，将在下一版本移除。")]
        public static bool IsNotNullOrEmpty(this string value)
            => string.IsNullOrEmpty(value) == false;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        #endregion

    }
}