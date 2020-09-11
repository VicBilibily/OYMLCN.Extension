using System;
using System.IO;
using System.Text;
using OYMLCN.ArgumentChecker;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// StreamExtension
    /// </summary>
    public static partial class StreamExtensions
    {
        #region public static string ReadToEnd(this Stream stream, Encoding encoding)
        /// <summary>
        /// 从流的当前位置到末尾读取所有字符
        /// </summary>
        /// <param name="stream"> 要读取的流对象 </param>
        /// <param name="encoding"> 数据流的编码方式 </param>
        /// <returns> 流对象从当前位置到末尾读取出的字节序列所表示的形式，如果当前位置位于流的末尾，则返回空字符串。 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="encoding"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="stream"/> 不支持读取 </exception>
        /// <exception cref="OutOfMemoryException"> 没有足够的内存来为返回的字符串分配缓冲区 </exception>
        /// <exception cref="IOException"> 出现 I/O 错误 </exception>
        public static string ReadToEnd(this Stream stream, Encoding encoding)
        {
            stream.ThrowIfNull(nameof(stream));
            stream.ThrowIfNull(nameof(stream));
            return new StreamReader(stream, encoding).ReadToEnd().Replace("\0", "");
        }
        #endregion
        #region public static string ReadToEnd(this Stream stream)
        /// <summary>
        /// 使用 UTF-8 编码，从流的当前位置到末尾读取所有字符
        /// </summary>
        /// <param name="stream"> 要读取的流对象 </param>
        /// <returns> 流对象从当前位置到末尾读取出的字节序列所表示的形式，如果当前位置位于流的末尾，则返回空字符串。 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="stream"/> 不支持读取 </exception>
        /// <exception cref="OutOfMemoryException"> 没有足够的内存来为返回的字符串分配缓冲区 </exception>
        /// <exception cref="IOException"> 出现 I/O 错误 </exception>
        public static string ReadToEnd(this Stream stream)
            => stream.ReadToEnd(Encoding.UTF8);
        #endregion


        #region public static byte[] ToBytes(this Stream stream)
        /// <summary>
        /// 从流的当前位置到末尾读取所有字节序列
        /// </summary>
        /// <param name="stream"> 要读取的流对象 </param>
        /// <returns> 流对象从当前位置到末尾读取出的字节序列 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> 不能为 null </exception>
        /// <exception cref="IOException"> 出现 I/O 错误 </exception>
        /// <exception cref="NotSupportedException"> 流不支持读取 </exception>
        /// <exception cref="ObjectDisposedException"> <paramref name="stream"/> 流已被释放 </exception>
        public static byte[] ToBytes(this Stream stream)
        {
            stream.ThrowIfNull(nameof(stream));
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        #endregion

        #region public static Stream ToStream(this byte[] buffer)
        /// <summary>
        /// 从当前字节数组创建为一个 <see cref="MemoryStream"/> 新实例
        /// </summary>
        /// <param name="buffer"> 无符号字节数组 </param>
        /// <returns> 一个 <see cref="MemoryStream"/> 新实例 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="buffer"/> 不能为 null </exception>
        public static Stream ToStream(this byte[] buffer)
            => new MemoryStream(buffer);
        #endregion


        #region public static void WriteBytes(this Stream stream, byte[] buffer)
        /// <summary>
        /// 向当前流中写入字节序列，将 <paramref name="buffer"/> 从头写入到 <paramref name="stream"/>
        /// </summary>
        /// <param name="stream"> 要写入的流对象 </param>
        /// <param name="buffer"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="buffer"/> 不能为 null </exception>
        /// <exception cref="IOException"> 发生 I/O 错误，例如找不到指定文件。 </exception>
        /// <exception cref="NotSupportedException"> <paramref name="stream"/> 流不支持写入 </exception>
        /// <exception cref="ObjectDisposedException"> <paramref name="stream"/> 流已被释放 </exception>
        public static void WriteBytes(this Stream stream, byte[] buffer)
        {
            stream.ThrowIfNull(nameof(stream));
            buffer.ThrowIfNull(nameof(buffer));
            stream.Write(buffer, 0, buffer.Length);
        }
        #endregion

    }
}
