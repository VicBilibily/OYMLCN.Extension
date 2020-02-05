using System;
using System.ComponentModel;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        /// <summary>
        /// 将数字的指定字符串表示形式转换为等效的 8 位带无符号整数 <see cref="byte"/>
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <returns> 一个与 <paramref name="value"/> 等效的 8 位无符号整数，如果 <paramref name="value"/> 为 null，则为零。 </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 不由一个可选符号后跟一系列数字 (0-9) 组成 </exception>
        /// <exception cref="OverflowException"> <paramref name="value"/> 是一个小于 <see cref="byte.MinValue"/> 或大于 <see cref="byte.MaxValue"/> 的数字 </exception>
        public static byte ConvertToByte(this string value)
            => Convert.ToByte(value);



    }
}
