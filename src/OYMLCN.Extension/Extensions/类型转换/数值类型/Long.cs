using System;
using System.ComponentModel;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        public static long ConvertToLong(this string value)
            => Convert.ToInt64(value);



    }
}
