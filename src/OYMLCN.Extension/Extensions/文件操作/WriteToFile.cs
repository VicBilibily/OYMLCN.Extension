using System;
using System.IO;
using System.Security;
using OYMLCN.ArgumentChecker;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// StreamExtension
    /// </summary>
    public static partial class StreamExtensions
    {
        #region public static void WriteToFile(this Stream stream, string path, int bufferSize = 1024 * 1024 * 1)
        /// <summary>
        /// 将当前字节流 <paramref name="stream"/> 写入到指定路径的文件 <paramref name="path"/>
        /// </summary>
        /// <param name="stream"> 要写入文件流的数据字节序列 </param>
        /// <param name="path"> 文件的相对路径或绝对路径 </param>
        /// <param name="bufferSize">缓冲区大小，默认1MB</param>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="path"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="path"/> 是一个空字符串 ("")，仅包含空格，或者包含一个或多个无效字符。 </exception>
        /// <exception cref="ArgumentException"> <paramref name="path"/> 指向非文件设备，如 NTFS 环境中的“con:”、“com1:”、“lpt1:”等 </exception>
        /// <exception cref="NotSupportedException"> <paramref name="path"/> 指向非文件设备，如非 NTFS 环境中的“con:”、“com1:”、“lpt1:”等 </exception>
        /// <exception cref="NotSupportedException"> <paramref name="stream"/> 流不支持查找，例如在流通过管道或控制台输出构造的情况下即为如此 </exception>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        /// <exception cref="IOException"> 出现 I/O 错误 </exception>
        /// <exception cref="IOException"> 另一个线程可能导致操作系统的文件句柄的位置发生意外更改 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="path"/> 指向一个只读文件 </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径无效，例如位于未映射的驱动器上 </exception>
        /// <exception cref="PathTooLongException"> 指定的路径和/或文件名超过了系统定义的最大长度 </exception>
        /// <exception cref="ObjectDisposedException"> <paramref name="stream"/> 流已关闭或已释放 </exception>
        public static void WriteToFile(this Stream stream, string path, int bufferSize = 1024 * 1024 * 1)
        {
            stream.ThrowIfNull(nameof(stream));
            path.ThrowIfNull(nameof(path));

            byte[] buffer = new byte[bufferSize];
            int length;
            using (var fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                while ((length = stream.Read(buffer, 0, buffer.Length)) != 0)
                    fsWrite.Write(buffer, 0, length);
        }
        #endregion

        #region public static async void WriteToFileAsync(this Stream stream, string path, int bufferSize = 1024 * 1024 * 1)
        /// <summary>
        /// 异步将当前字节流 <paramref name="stream"/> 写入到指定路径的文件 <paramref name="path"/>
        /// </summary>
        /// <param name="stream"> 要写入文件流的数据字节序列 </param>
        /// <param name="path"> 文件的相对路径或绝对路径 </param>
        /// <param name="bufferSize">缓冲区大小，默认1MB</param>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="path"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="path"/> 是一个空字符串 ("")，仅包含空格，或者包含一个或多个无效字符。 </exception>
        /// <exception cref="ArgumentException"> <paramref name="path"/> 指向非文件设备，如 NTFS 环境中的“con:”、“com1:”、“lpt1:”等 </exception>
        /// <exception cref="NotSupportedException"> <paramref name="path"/> 指向非文件设备，如非 NTFS 环境中的“con:”、“com1:”、“lpt1:”等 </exception>
        /// <exception cref="NotSupportedException"> <paramref name="stream"/> 流不支持查找，例如在流通过管道或控制台输出构造的情况下即为如此 </exception>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        /// <exception cref="IOException"> 出现 I/O 错误 </exception>
        /// <exception cref="IOException"> 另一个线程可能导致操作系统的文件句柄的位置发生意外更改 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="path"/> 指向一个只读文件 </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径无效，例如位于未映射的驱动器上 </exception>
        /// <exception cref="PathTooLongException"> 指定的路径和/或文件名超过了系统定义的最大长度 </exception>
        /// <exception cref="ObjectDisposedException"> <paramref name="stream"/> 流已关闭或已释放 </exception>
        /// <exception cref="InvalidOperationException"> 当前操作的流正在被使用 </exception>
        public static async void WriteToFileAsync(this Stream stream, string path, int bufferSize = 1024 * 1024 * 1)
        {
            stream.ThrowIfNull(nameof(stream));
            path.ThrowIfNull(nameof(path));

            var buffer = new byte[bufferSize];
            int read, totalRead = 0;
            var totalLength = stream.Length;
            using (var fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                while (totalRead < totalLength)
                {
                    read = await stream.ReadAsync(buffer, 0, buffer.Length);
                    totalRead += read;
                    await fsWrite.WriteAsync(buffer, 0, read);
                }
        }
        #endregion

        #region public static void WriteToFile(this byte[] array, string path)
        /// <summary>
        /// 将当前字节序列 <paramref name="array"/> 写入到指定路径的文件 <paramref name="path"/>
        /// </summary>
        /// <param name="array"> 要写入文件流的数据字节序列 </param>
        /// <param name="path"> 文件的相对路径或绝对路径 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="array"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="path"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="path"/> 是一个空字符串 ("")，仅包含空格，或者包含一个或多个无效字符。 </exception>
        /// <exception cref="ArgumentException"> <paramref name="path"/> 指向非文件设备，如 NTFS 环境中的“con:”、“com1:”、“lpt1:”等 </exception>
        /// <exception cref="NotSupportedException"> <paramref name="path"/> 指向非文件设备，如非 NTFS 环境中的“con:”、“com1:”、“lpt1:”等 </exception>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        /// <exception cref="IOException"> 出现 I/O 错误 </exception>
        /// <exception cref="IOException"> 另一个线程可能导致操作系统的文件句柄的位置发生意外更改 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="path"/> 指向一个只读文件 </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径无效，例如位于未映射的驱动器上 </exception>
        /// <exception cref="PathTooLongException"> 指定的路径和/或文件名超过了系统定义的最大长度 </exception>
        public static void WriteToFile(this byte[] array, string path)
        {
            array.ThrowIfNull(nameof(array));
            path.ThrowIfNull(nameof(path));
            using (var localFile = new FileStream(path, FileMode.OpenOrCreate))
                localFile.Write(array, 0, array.Length);
        }
        #endregion

    }
}
