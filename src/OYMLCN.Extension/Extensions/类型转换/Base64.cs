using System;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        #region public static byte[] ConvertFromBase64String(this string str)
        /// <summary>
        /// 将指定的字符串（它将二进制数据编码为 Base64 数字）转换为等效的 8 位无符号整数数组
        /// </summary>
        /// <param name="str"> 要转换的字符串 </param>
        /// <returns> 与 <paramref name="str"/> 等效的 8 位无符号整数数组 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str"/> 不能为 null </exception>
        /// <exception cref="FormatException"> <paramref name="str"/> 的长度（忽略空格）不是 0 或 4 的倍数 或 格式无效 </exception>
        /// <exception cref="FormatException"> <paramref name="str"/> 包含非 base 64 字符、两个以上的填充字符或者在填充字符中包含非空格字符 </exception>
        public static byte[] ConvertFromBase64String(this string str)
            => Convert.FromBase64String(str);
        #endregion

        #region public static string ConvertToBase64String(this byte[] inArray)
        /// <summary>
        /// 将 8 位无符号整数的数组转换为其用 Base64 数字编码的等效字符串表示形式
        /// </summary>
        /// <param name="inArray"> 8 位无符号整数数组 </param>
        /// <returns> <paramref name="inArray"/> 的内容以 Base64 表示的字符串表示式 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="inArray"/> 不能为 null </exception>
        public static string ConvertToBase64String(this byte[] inArray)
            => Convert.ToBase64String(inArray);
        #endregion
        #region public static string ConvertToBase64String(this byte[] inArray)
        /// <summary>
        /// 将 8 位无符号整数的数组转换为其用 Base64 数字编码的等效字符串表示形式，你可以指定是否在返回值中插入换行符。
        /// </summary>
        /// <param name="inArray"> 8 位无符号整数数组 </param>
        /// <param name="options"> 如果每 76 个字符插入一个分行符，则使用 <see cref="Base64FormattingOptions.InsertLineBreaks"/>，如果不插入分行符，则使用 <see cref="Base64FormattingOptions.None"/>。 </param>
        /// <returns> <paramref name="inArray"/> 的内容以 Base64 表示的字符串表示式 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="inArray"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="options"/> 不是有效的 <see cref="Base64FormattingOptions"/> 值 </exception>
        public static string ConvertToBase64String(this byte[] inArray, Base64FormattingOptions options)
            => Convert.ToBase64String(inArray, options);
        #endregion

    }
}
