using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ConvertExtension
    {
        /// <summary>
        /// 将Byte[]转换为Base64字符串
        /// </summary>
        public static string ConvertToBase64String(this byte[] inArray)
            => Convert.ToBase64String(inArray);

        /// <summary>
        /// 将Base64转换为Byte[]
        /// </summary>
        public static byte[] ConvertFromBase64String(this string str)
            => Convert.FromBase64String(str);


    }
}
