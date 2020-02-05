using System;
using System.ComponentModel;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        public static decimal ConvertToDecimal(this string value)
            => Convert.ToDecimal(value);



    }
}
