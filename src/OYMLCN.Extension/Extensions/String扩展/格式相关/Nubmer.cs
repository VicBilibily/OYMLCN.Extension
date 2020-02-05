using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 格式相关扩展
    /// </summary>
    public static partial class StringFormatExtension
    {
        /// <summary>
        /// 获取文本中的数字（包括小数）
        /// </summary>
        public static string FormatAsNumeric(this string str)
            => str.IsNullOrEmpty() ? null : Regex.Match(str, @"[+-]?\d+(\.\d+)?", RegexOptions.Compiled).Value;
        
        /// <summary>
        /// 获取文本中的整数部分
        /// </summary>
        public static string FormatAsIntegerNumeric(this string str)
            //=> str.IsNullOrEmpty() ? null : Regex.Match(str, @"[+-]?\d+(\.\d+)?", RegexOptions.Compiled).Value;
            => str.FormatAsNumeric()?.SplitThenGetFirst(".");

    }
}
