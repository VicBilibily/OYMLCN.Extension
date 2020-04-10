using OYMLCN.ArgumentChecker;
using System;
using System.IO;
using System.IO.Compression;
using System.Security;
using System.Text;
#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 数据压缩扩展
    /// </summary>
    public static partial class CompressExtensions
    {
        #region GZipCompress
        #region public static byte[] GZipCompress(this byte[] buffer, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        /// <summary>
        /// 使用 GZip 压缩字节序列
        /// </summary>
        /// <param name="buffer"> 要进行压缩操作的字节序列 </param>
        /// <param name="compressionLevel"> 压缩流时，指示是否要强调速度或压缩效率的枚举值之一。 </param>
        /// <returns> 压缩后的字节序列 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="buffer"/> 不能为 null </exception>
        public static byte[] GZipCompress(this byte[] buffer, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            buffer.ThrowIfNull(nameof(buffer));
            if (buffer.IsEmpty()) return new byte[0];
            using MemoryStream ms = new MemoryStream();
            using (GZipStream compressedzipStream = new GZipStream(ms, compressionLevel, true))
                compressedzipStream.Write(buffer, 0, buffer.Length);
            return ms.ToArray();
        }
        #endregion
        #region public static byte[] GZipCompress(this Stream stream, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        /// <summary>
        /// 使用 GZip 压缩流数据
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="compressionLevel"> 压缩流时，指示是否要强调速度或压缩效率的枚举值之一。 </param>
        /// <returns> 压缩后的字节序列 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> 不能为 null </exception>
        /// <exception cref="IOException"> 出现 I/O 错误 </exception>
        /// <exception cref="NotSupportedException"> 流不支持读取 </exception>
        /// <exception cref="ObjectDisposedException"> <paramref name="stream"/> 流已被释放 </exception>
        public static byte[] GZipCompress(this Stream stream, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            stream.ThrowIfNull(nameof(stream));
            return stream.ToBytes().GZipCompress(compressionLevel);
        }
        #endregion
        #region public static string GZipCompress(this string rawString, CompressionLevel compressionLevel = CompressionLevel.Optimal, bool removeEmpty = true)
        /// <summary>
        /// 将字符串采用 UTF-8 编码后获取字节序列后以 GZip 算法压缩，将结果序列采用 Base64 编码后返回字符串
        /// </summary>
        /// <param name="rawString"> 需要压缩的字符串 </param>
        /// <param name="compressionLevel"> 压缩流时，指示是否要强调速度或压缩效率的枚举值之一。 </param>
        /// <param name="removeEmpty"> 移除填充的 = 空符号 </param>
        /// <returns> 压缩结果字节序列采用 Base64 编码的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="rawString"/> 不能为 null </exception>
        public static string GZipCompress(this string rawString, CompressionLevel compressionLevel = CompressionLevel.Optimal, bool removeEmpty = true)
        {
            rawString.ThrowIfNull(nameof(rawString));
            if (rawString.IsEmpty()) return string.Empty;
            byte[] rawData = rawString.GetUTF8Bytes();
            byte[] zippedData = GZipCompress(rawData, compressionLevel);
            string result = zippedData.ConvertToBase64String();
            return removeEmpty ? result.TrimEnd('=') : result;
        }
        #endregion

        #region public static void GZipCompress(this byte[] buffer, string path, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        /// <summary>
        /// 将字节序列采用 GZip 算法压缩后保存文件到指定位置
        /// </summary>
        /// <param name="buffer"> 要压缩的字节序列 </param>
        /// <param name="path"> 文件的相对路径或绝对路径 </param>
        /// <param name="compressionLevel"> 压缩流时，指示是否要强调速度或压缩效率的枚举值之一。 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="buffer"/> 不能为 null </exception>
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
        public static void GZipCompress(this byte[] buffer, string path, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            buffer.ThrowIfNull(nameof(buffer));
            path.ThrowIfNullOrEmpty(nameof(path));
            buffer.GZipCompress(compressionLevel).WriteToFile(path);
        }
        #endregion
        #region public static void GZipCompress(this FileInfo file, string path, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        /// <summary>
        /// 将文件采用 GZip 算法压缩后保存文件到指定位置
        /// </summary>
        /// <param name="file"> 文件信息实例 </param>
        /// <param name="path"> 文件的相对路径或绝对路径 </param>
        /// <param name="compressionLevel"> 压缩流时，指示是否要强调速度或压缩效率的枚举值之一。 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="file"/> 不能为 null </exception>
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
        public static void GZipCompress(this FileInfo file, string path, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            file.ThrowIfNull(nameof(file));
            path.ThrowIfNullOrEmpty(nameof(path));
            using (var stream = file.ReadToStream())
                stream.GZipCompress(compressionLevel).WriteToFile(path);
        }
        #endregion

#if Xunit
        [Fact]
        public static void GZipCompressTest()
        {
            byte[] array = null;
            Assert.Throws<ArgumentNullException>(() => array.GZipCompress());

            Stream stream = null;
            Assert.Throws<ArgumentNullException>(() => stream.GZipCompress());
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.GZipCompress());

            array = new byte[0];
            Assert.Empty(array.GZipCompress());
            str = string.Empty;
            Assert.Empty(str.GZipCompress());

            string testFile = "test.gz";
            array = null;
            Assert.Throws<ArgumentNullException>(() => array.GZipCompress(testFile));
            FileInfo file = null;
            Assert.Throws<ArgumentNullException>(() => file.GZipCompress(testFile));

            file = testFile.GetFileInfo();
            testFile = null;
            array = new byte[0];
            Assert.Throws<ArgumentNullException>(() => array.GZipCompress(testFile));
            Assert.Throws<ArgumentNullException>(() => file.GZipCompress(testFile));
            testFile = string.Empty;
            Assert.Throws<ArgumentException>(() => array.GZipCompress(testFile));
            Assert.Throws<ArgumentException>(() => file.GZipCompress(testFile));
        }
#endif
        #endregion

        #region GZipDecompress
        #region public static byte[] GZipDecompress(this byte[] buffer)
        /// <summary>
        /// 使用 GZip 解压字节序列
        /// </summary>
        /// <param name="buffer"> 要进行解压操作的字节序列 </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"> <paramref name="buffer"/> 不能为 null </exception>
        public static byte[] GZipDecompress(this byte[] buffer)
        {
            buffer.ThrowIfNull(nameof(buffer));
            if (buffer.IsEmpty()) return new byte[0];
            using MemoryStream ms = new MemoryStream(buffer);
            using MemoryStream outBuffer = new MemoryStream();
            using (GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress))
            {
                byte[] block = new byte[1024];
                while (true)
                {
                    int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                    if (bytesRead <= 0)
                        break;
                    else
                        outBuffer.Write(block, 0, bytesRead);
                }
            }
            return outBuffer.ToArray();
        }
        #endregion
        #region public static byte[] GZipDecompress(this Stream stream)
        /// <summary>
        /// 使用 GZip 解压流数据
        /// </summary>
        /// <param name="stream"></param>
        /// <returns> 压缩后的字节序列 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="stream"/> 不能为 null </exception>
        /// <exception cref="IOException"> 出现 I/O 错误 </exception>
        /// <exception cref="NotSupportedException"> 流不支持读取 </exception>
        /// <exception cref="ObjectDisposedException"> <paramref name="stream"/> 流已被释放 </exception>
        public static byte[] GZipDecompress(this Stream stream)
        {
            stream.ThrowIfNull(nameof(stream));
            return stream.ToBytes().GZipDecompress();
        }
        #endregion
        #region public static string GZipDecompress(this string gzipString)
        /// <summary>
        /// 把采用 <see cref="GZipCompress(string, CompressionLevel, bool)"/> 压缩的字符串解压 
        /// </summary>
        /// <param name="gzipString"> 经 GZip 压缩后的字符串 </param>
        /// <returns> 原始未压缩字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="gzipString"/> 不能为 null </exception>
        public static string GZipDecompress(this string gzipString)
        {
            gzipString.ThrowIfNull(nameof(gzipString));
            if (gzipString.IsEmpty()) return string.Empty;

            // 补充已移除的 = 空白符
            var length = gzipString.Length % 4;
            if (length != 0)
                for (var i = length; i < 4; i++)
                    gzipString += "=";

            byte[] zippedData = gzipString.ConvertFromBase64String();
            return GZipDecompress(zippedData).GetUTF8String();
        }
        #endregion

        #region public static void GZipDecompress(this byte[] buffer, string path)
        /// <summary>
        /// 将字节序列采用 GZip 算法解压后保存文件到指定位置
        /// </summary>
        /// <param name="buffer"> 要解压的字节序列 </param>
        /// <param name="path"> 文件的相对路径或绝对路径 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="buffer"/> 不能为 null </exception>
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
        public static void GZipDecompress(this byte[] buffer, string path)
        {
            buffer.ThrowIfNull(nameof(buffer));
            path.ThrowIfNullOrEmpty(nameof(path));
            File.WriteAllBytes(path, buffer.GZipDecompress());
        }
        #endregion
        #region public static void GZipDecompress(this FileInfo file, string path)
        /// <summary>
        /// 将文件采用 GZip 算法解压后保存文件到指定位置
        /// </summary>
        /// <param name="file"> 文件信息实例 </param>
        /// <param name="path"> 文件的相对路径或绝对路径 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="file"/> 不能为 null </exception>
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
        public static void GZipDecompress(this FileInfo file, string path)
        {
            file.ThrowIfNull(nameof(file));
            path.ThrowIfNullOrEmpty(nameof(path));
            using (var stream = file.ReadToStream())
                stream.GZipDecompress().WriteToFile(path);
        }
        #endregion

#if Xunit
        [Fact]
        public static void GZipDecompressTest()
        {
            byte[] array = null;
            Assert.Throws<ArgumentNullException>(() => array.GZipDecompress());
            Stream stream = null;
            Assert.Throws<ArgumentNullException>(() => stream.GZipDecompress());
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.GZipDecompress());

            array = new byte[0];
            Assert.Empty(array.GZipDecompress());
            str = string.Empty;
            Assert.Empty(str.GZipDecompress());

            string testFile = "test.gz";
            array = null;
            Assert.Throws<ArgumentNullException>(() => array.GZipDecompress(testFile));
            FileInfo file = null;
            Assert.Throws<ArgumentNullException>(() => file.GZipDecompress(testFile));

            file = testFile.GetFileInfo();
            testFile = null;
            array = new byte[0];
            Assert.Throws<ArgumentNullException>(() => array.GZipDecompress(testFile));
            Assert.Throws<ArgumentNullException>(() => file.GZipDecompress(testFile));
            testFile = string.Empty;
            Assert.Throws<ArgumentException>(() => array.GZipDecompress(testFile));
            Assert.Throws<ArgumentException>(() => file.GZipDecompress(testFile));
        }
#endif
        #endregion

#if Xunit
        [Fact]
        public static void GZipTest()
        {
            string str = "Hello World!";
            Assert.Equal(str, str.GZipCompress().GZipDecompress());
            byte[] buffer = str.GetUTF8Bytes();
            Assert.Equal(buffer, buffer.GZipCompress().GZipDecompress());

            string testFile = "test.gz";
            var file = testFile.GetFileInfo();
            buffer.GZipCompress(testFile);
            using (var stram = file.ReadToStream())
                Assert.Equal(buffer, stram.GZipDecompress());
            file.Delete();
        }
#endif

    }
}
