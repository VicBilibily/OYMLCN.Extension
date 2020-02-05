using System;
using System.ComponentModel;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        public static double ConvertToDouble(this string value)
            => Convert.ToDouble(value);



    }
}
