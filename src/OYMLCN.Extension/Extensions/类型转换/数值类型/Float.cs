using System;
using System.ComponentModel;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        public static float ConvertToFloat(this string value)
            => Convert.ToSingle(value);



    }
}
