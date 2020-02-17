using System;
using OYMLCN.ArgumentChecker;
#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 格式相关扩展
    /// </summary>
    public static partial class StringFormatExtension
    {
        static readonly char[] Puntuation = new char[] {
            '~', '～', '-', '—', '－', '–', '^', '*',
            ',', '，', '.', '。', '?', '？', '!','！',
            ':', '：', ';', '；',
            '[', '【', '{', ']', '】', '}',
            '|', '丨', '/', '\\',
            '(', '（', ')', '）', '<', '《', '>', '》',
            '·', '`', '\'', '"'
        };
        #region public static string TrimStartPuntuation(this string input)
        /// <summary>
        /// 去除文本开头的标点及标识符
        /// </summary>
        /// <param name="input"> 要处理的字符串 </param>
        /// <returns> 开头去除常见标点符号的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static string TrimStartPuntuation(this string input)
        {
            input.ThrowIfNull(nameof(input));
            return input.Trim().TrimStart(Puntuation).Trim();
        }
#if Xunit
        [Fact]
        public static void TrimStartPuntuationTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.TrimStartPuntuation());

            var punt = new string(Puntuation);
            str = $" {punt}Hello World! ";
            Assert.Equal("Hello World!", str.TrimStartPuntuation());
        }
#endif 
        #endregion

        #region public static string TrimEndPuntuation(this string input)
        /// <summary>
        /// 去除文本结尾的标点及标识符
        /// </summary>
        /// <param name="input"> 要处理的字符串 </param>
        /// <returns> 结尾去除常见标点符号的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static string TrimEndPuntuation(this string input)
        {
            input.ThrowIfNull(nameof(input));
            return input.Trim().TrimEnd(Puntuation).Trim();
        }
#if Xunit
        [Fact]
        public static void TrimEndPuntuationTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.TrimEndPuntuation());

            var punt = new string(Puntuation);
            str = $" Hello World!{punt} ";
            Assert.Equal("Hello World", str.TrimEndPuntuation());
        }
#endif 
        #endregion

        #region public static string TrimPuntuation(this string input)
        /// <summary>
        /// 去除文本两段的标点及标识符
        /// </summary>
        /// <param name="input"> 要处理的字符串 </param>
        /// <returns> <paramref name="input"/> 前后去除常见标点符号的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static string TrimPuntuation(this string input)
        {
            input.ThrowIfNull(nameof(input));
            return input.Trim().Trim(Puntuation).Trim();
        }
#if Xunit
        [Fact]
        public static void TrimPuntuationTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.TrimPuntuation());

            var punt = new string(Puntuation);
            str = $" {punt}Hello World!{punt} ";
            Assert.Equal("Hello World", str.TrimPuntuation());
        }
#endif 
        #endregion

    }
}
