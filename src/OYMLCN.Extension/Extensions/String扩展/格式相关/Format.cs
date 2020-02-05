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
        /// <summary> 检查字符串 是 null 或者是 <see cref="string.Empty"/> 字符串 </summary>
        public static bool IsNullOrEmpty(this string str)
            => string.IsNullOrEmpty(str);
        /// <summary> 检查字符串 不是 null 且不是 <see cref="string.Empty"/> 字符串 </summary>
        public static bool IsNotNullOrEmpty(this string str)
            => !string.IsNullOrEmpty(str);

        /// <summary> 检查字符串 是 null、空 <see cref="string.Empty"/> 或是仅由空白字符组成 </summary>
        public static bool IsNullOrWhiteSpace(this string str)
            => string.IsNullOrWhiteSpace(str);
        /// <summary> 检查字符串 不是 null、空 <see cref="string.Empty"/> 或是仅由空白字符组成 </summary>
        public static bool IsNotNullOrWhiteSpace(this string str)
            => !string.IsNullOrWhiteSpace(str);

        /// <summary>
        /// 根据占位符紧接多个字符串 即string.Format
        /// </summary>
        /// <param name="str"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string StringFormat(this string str, params string[] param)
            => string.Format(str, param);




        /// <summary> 从此实例检索子字符串。子字符串忽略指定的长度字符，然后返回具有指定长度的后续字符。 </summary>
        /// <param name="str"> 字符串实例 </param>
        /// <param name="skipLength"> 要忽略的字符数量 </param>
        /// <param name="subLength"> 要取得的字符数量 </param>
        public static string TakeSubString(this string str, int skipLength, int subLength = int.MaxValue)
            => new string(str.Skip(skipLength).Take(subLength).ToArray());

    }
}
