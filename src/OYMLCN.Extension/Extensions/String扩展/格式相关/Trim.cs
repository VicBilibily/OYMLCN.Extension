using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 格式相关扩展
    /// </summary>
    public static partial class StringFormatExtension
    {
        static readonly char[] Puntuation = new char[] {
            '~', '～', '-', '—', '－', '–', '^', '*',
            ',', '，', '.', '。', '?', '？', ':', '：', ';', '；',
            '[', '【', '{', ']', '】', '}', '|', '丨', '/', '\\',
            '(', '（', ')', '）', '<', '《', '>', '》',
            '·', '`', '\'', '"'
        };
        /// <summary>
        /// 去除文本开头的标点及标识符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimStartPuntuation(this string str)
            => str?.TrimStart(Puntuation).Trim();
        /// <summary>
        /// 去除文本结尾的标点及标识符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimEndPuntuation(this string str)
            => str?.TrimEnd(Puntuation).Trim();
        /// <summary>
        /// 去除文本两段的标点及标识符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimPuntuation(this string str)
            => str?.Trim(Puntuation).Trim();
    }
}
