using OYMLCN.ArgumentChecker;
using System;
using System.IO;
using System.Text;
#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class StringConvertExtension
    {
        #region public static Stream GetEncodingStream(this string str, Encoding encoding)
        /// <summary>
        /// 将指定字符串中的所有字符编码为一个字节序列后返回一个 <see cref="Stream"/> 对象
        /// </summary>
        /// <param name="str"> 字符串对象 </param>
        /// <param name="encoding"> 所要指定使用的字符编码方式 </param>
        /// <returns>  将指定字符串按指定编码后的 <see cref="Stream"/> 对象 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str"/> 的字符串对象不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="encoding"/> 所指定的字符编码方式不能为 null </exception>
        public static Stream GetEncodingStream(this string str, Encoding encoding)
        {
            str.ThrowIfNull(nameof(str));
            encoding.ThrowIfNull(nameof(encoding), $"{nameof(encoding)} 所指定的字符编码方式不能为 null");
            return new MemoryStream(encoding.GetBytes(str));
        }
#if Xunit
        [Fact]
        public static void GetEncodingStreamTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.GetEncodingStream(null));
            Assert.Throws<ArgumentNullException>(() => str.GetEncodingStream(Encoding.Default));

            str = string.Empty;
            Assert.Equal(str.Length, str.GetEncodingStream(Encoding.Default).Length);

            str = "Hello World!";
            Assert.Equal(str.Length, str.GetEncodingStream(Encoding.Default).Length);

            // 内置方法进行编码的各种方式，不属于本方法测试范围
        }
#endif 
        #endregion

        #region public static Stream GetUTF8Stream(this string str)
        /// <summary>
        /// 将指定字符串中的所有字符以 UTF8 的格式编码为一个字节序列后返回一个 <see cref="Stream"/> 对象
        /// </summary>
        /// <param name="str"> 字符串对象 </param>
        /// <returns>  将指定字符串按指定编码后的 <see cref="Stream"/> 对象 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str"/> 的字符串对象不能为 null </exception>
        public static Stream GetUTF8Stream(this string str)
        {
            str.ThrowIfNull(nameof(str));
            return new MemoryStream(Encoding.UTF8.GetBytes(str));
        }
#if Xunit
        [Fact]
        public static void GetUTF8StreamTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.GetUTF8Stream());

            str = string.Empty;
            Assert.Equal(str.Length, str.GetUTF8Stream().Length);

            str = "Hello World!";
            Assert.Equal(str.Length, str.GetUTF8Stream().Length);
        }
#endif 
        #endregion

    }
}
