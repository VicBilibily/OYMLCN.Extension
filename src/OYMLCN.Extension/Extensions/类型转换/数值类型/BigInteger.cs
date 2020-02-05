using System;
using System.ComponentModel;
using System.Numerics;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        public static BigInteger ConvertToBigInteger(this string value)
            => BigInteger.Parse(value);



    }
}
