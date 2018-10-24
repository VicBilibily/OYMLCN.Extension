using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// ZipExtension
    /// </summary>
    public static class ZipExtensions
    {
        /// <summary>
        /// 将传入字符串以GZip算法压缩后，返回Base64编码字符
        /// </summary>
        /// <param name="rawString">需要压缩的字符串</param>
        /// <param name="compressionLevel">压缩效率</param>
        /// <param name="removeEmpty">移除填充的 = 空符号</param>
        /// <returns>压缩后的Base64编码的字符串</returns>
        public static string GZipCompressString(this string rawString, bool removeEmpty = true)
        {
            if (string.IsNullOrEmpty(rawString) || rawString.Length == 0)
                return "";
            byte[] rawData = Encoding.UTF8.GetBytes(rawString.ToString());
            byte[] zippedData = GZipCompress(rawData);
            string result = Convert.ToBase64String(zippedData);
            return removeEmpty ? result.TrimEnd('=') : result;
        }

        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="compressionLevel">压缩效率</param>
        /// <returns></returns>
        public static byte[] GZipCompress(this byte[] rawData)
        {
            MemoryStream ms = new MemoryStream();
#if NET35
            using (GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true))
#else
            using (GZipStream compressedzipStream = new GZipStream(ms, CompressionLevel.Optimal, true))
#endif
                compressedzipStream.Write(rawData, 0, rawData.Length);
            return ms.ToArray();
        }
        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static byte[] GZipCompress(this Stream rawData)
            => rawData?.ToBytes().GZipCompress();

        /// <summary>
        /// GZip压缩文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        public static void GZipCompressToFile(this FileInfo file, string fileName)
            => file.ReadToStream().GZipCompress().WriteToFile(fileName);


        /// <summary>
        /// 将传入的二进制字符串资料以GZip算法解压缩
        /// </summary>
        /// <param name="zippedString">经GZip压缩后的二进制字符串</param>
        /// <returns>原始未压缩字符串</returns>
        public static string GZipDecompressString(this string zippedString)
        {
            if (string.IsNullOrEmpty(zippedString) || zippedString.Length == 0)
                return "";

            // 补充已移除的 = 空白符
            var length = zippedString.Length % 4;
            if (length != 0)
                for (var i = length; i < 4; i++)
                    zippedString += "=";

            byte[] zippedData = Convert.FromBase64String(zippedString.ToString());
            return Encoding.UTF8.GetString(GZipDecompress(zippedData));
        }

        /// <summary>
        /// GZip解压
        /// </summary>
        /// <param name="zippedData"></param>
        /// <returns></returns>
        public static byte[] GZipDecompress(this byte[] zippedData)
        {
            using (MemoryStream ms = new MemoryStream(zippedData))
            using (MemoryStream outBuffer = new MemoryStream())
            {
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
        }
        /// <summary>
        /// GZip解压
        /// </summary>
        /// <param name="zippedData"></param>
        /// <returns></returns>
        public static byte[] GZipDecompress(this Stream zippedData)
            => zippedData?.ToBytes().GZipDecompress();

        /// <summary>
        /// GZip解压文件
        /// </summary>
        /// <param name="zippedData"></param>
        /// <param name="fileName"></param>
        public static void GZipDecompressToFile(byte[] zippedData, string fileName)
            => File.WriteAllBytes(fileName, zippedData.GZipDecompress());
        /// <summary>
        /// GZip解压文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        public static void GZipDecompressToFile(this FileInfo file, string fileName)
            => file.ReadToStream().GZipDecompress().WriteToFile(fileName);

#if !NET35
        /// <summary>
        /// 使用指定的文件夹创建Zip压缩文件
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="fileName">压缩文件路径</param>
        public static void CreateZipFile(this DirectoryInfo directory, string fileName)
            => ZipFile.CreateFromDirectory(directory.FullName, fileName);
        /// <summary>
        /// 解压Zip压缩文件到指定文件夹
        /// </summary>
        /// <param name="file"></param>
        /// <param name="targetPath">文件夹路径</param>
        public static void ExtractZipFile(this FileInfo file, string targetPath)
            => ZipFile.ExtractToDirectory(file.FullName, targetPath);
#endif
    }
}
