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
        /// 将数字的指定字符串表示形式转换为等效的 8 位带符号整数 <see cref="sbyte"/>
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <returns> 与 <paramref name="value"/> 中数字等效的 8 位带符号整数，如果值为 null，则为 0（零）。 </returns>
        /// <exception cref="FormatException"> <paramref name="value"/> 不由一个可选符号后跟一系列数字 (0-9) 组成 </exception>
        /// <exception cref="OverflowException"> <paramref name="value"/> 是一个小于 <see cref="sbyte.MinValue"/> 或大于 <see cref="sbyte.MaxValue"/> 的数字 </exception>
        public static sbyte ConvertToSByte(this string value)
            => Convert.ToSByte(value);



    }
}
