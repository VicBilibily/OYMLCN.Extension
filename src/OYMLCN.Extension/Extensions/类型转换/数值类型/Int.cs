﻿using System;
using System.ComponentModel;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        public static int ConvertToInt(this string value)
            => Convert.ToInt32(value);



    }
}
