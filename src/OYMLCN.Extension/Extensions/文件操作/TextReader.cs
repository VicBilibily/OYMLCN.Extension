using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    public static class TextReaderExtension
    {
        /// <summary>
        /// 获取文本全部行
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToLines(this TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
                yield return line;
        }
        /// <summary>
        /// 获取文本全部非空行
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<string> NonEmptyLines(this TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line == "") continue;
                yield return line;
            }
        }
        /// <summary>
        /// 获取文本全部非空格行
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<string> NonWhiteSpaceLines(this TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.IsNullOrWhiteSpace()) continue;
                yield return line;
            }
        }
    }
}
