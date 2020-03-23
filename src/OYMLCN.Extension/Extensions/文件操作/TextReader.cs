using OYMLCN.ArgumentChecker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// TextReaderExtension
    /// </summary>
    public static class TextReaderExtension
    {
        #region public static IEnumerable<string> ToLines(this TextReader reader)
        /// <summary>
        /// 获取文本全部行
        /// </summary>
        /// <param name="reader"> 可读取有序字符系列的读取器 </param>
        /// <returns> 所有字符序列 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="reader"/> 不能为 null </exception>
        public static IEnumerable<string> ToLines(this TextReader reader)
        {
            reader.ThrowIfNull(nameof(reader));
            string line;
            while ((line = reader.ReadLine()) != null)
                yield return line;
        } 
        #endregion
        #region public static IEnumerable<string> NonEmptyLines(this TextReader reader)
        /// <summary>
        /// 获取文本全部非空行
        /// </summary>
        /// <param name="reader"> 可读取有序字符系列的读取器 </param>
        /// <returns> 所有非空字符序列 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="reader"/> 不能为 null </exception>
        public static IEnumerable<string> NonEmptyLines(this TextReader reader)
        {
            reader.ThrowIfNull(nameof(reader));
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line == "") continue;
                yield return line;
            }
        } 
        #endregion
        #region public static IEnumerable<string> NonWhiteSpaceLines(this TextReader reader)
        /// <summary>
        /// 获取文本全部非空格行
        /// </summary>
        /// <param name="reader"> 可读取有序字符系列的读取器 </param>
        /// <returns> 所有非空白字符序列 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="reader"/> 不能为 null </exception>
        public static IEnumerable<string> NonWhiteSpaceLines(this TextReader reader)
        {
            reader.ThrowIfNull(nameof(reader));
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.IsNullOrWhiteSpace()) continue;
                yield return line;
            }
        } 
        #endregion
    }
}
