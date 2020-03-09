using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using OYMLCN.ArgumentChecker;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// StreamExtension
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// 指定编码读取Stream数据
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoder">编码格式 默认为UTF-8</param>
        /// <returns></returns>
        public static string ReadToEnd(this Stream stream, Encoding encoder = null)
            => new StreamReader(stream, encoder ?? Encoding.UTF8).ReadToEnd().Replace("\0", "");

        /// <summary>
        /// 填充表单信息
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="formData">表单信息字典</param>
        public static Stream FillFormDataToStream(this Stream stream, Dictionary<string, string> formData)
        {
            var formDataBytes = formData.IsEmpty() ? new byte[0] : Encoding.UTF8.GetBytes(formData.ToQueryString());
            stream.Write(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        /// <summary>
        /// 将Byte[]转换为字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="encoder">默认使用UTF-8进行编码</param>
        /// <returns></returns>
        public static string ConvertToString(this byte[] bytes, Encoding encoder = null)
            => (encoder ?? Encoding.UTF8).GetString(bytes);

        /// <summary>
        /// 将Stream转换为Byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 将Byte转换为Stream
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream ToStream(this byte[] bytes)
            => new MemoryStream(bytes);

        /// <summary>
        /// 将byte[]从头写入Stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="bytes"></param>
        public static void WriteAll(this Stream stream, byte[] bytes)
            => stream.Write(bytes, 0, bytes.Length);

        /// <summary>
        /// 将Stream保存到文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName">文件的相对路径或绝对路径</param>
        public static void WriteToFile(this Stream stream, string fileName)
        {
            stream.ThrowIfNull(nameof(stream));
            stream.Seek(0, SeekOrigin.Begin);
            ((MemoryStream)stream).ToArray().WriteToFile(fileName);
        }
        /// <summary>
        /// 将byte[]字节数组写入文件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        public static void WriteToFile(this byte[] content, string fileName)
        {
            using (var localFile = new FileStream(fileName, FileMode.OpenOrCreate))
                localFile.Write(content, 0, content.Length);
        }

        /// <summary>
        /// 将Stream保存到文件（使用缓冲区）
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileName">文件的相对路径或绝对路径</param>
        /// <param name="bufferSize">缓冲区大小，默认8MB</param>
        public static void WriteToFileWithBuffer(this Stream stream, string fileName, int bufferSize = 1024 * 8 * 1024)
        {
            using var fsWrite = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            byte[] buf = new byte[bufferSize];
            int len;
            while ((len = stream.Read(buf, 0, buf.Length)) != 0)
                fsWrite.Write(buf, 0, len);
        }

        /// <summary>
        /// 将Stream保存到文件（使用缓冲区）
        /// </summary>
        /// <param name="stream">源</param>
        /// <param name="dest">目标地址</param>
        /// <param name="bufferSize">缓冲区大小，默认8MB</param>
        public static async void WriteToFileWithBufferAsync(this Stream stream, string dest, int bufferSize = 1024 * 1024 * 8)
        {
            await Task.Run(() => WriteToFileWithBuffer(stream, dest, bufferSize)).ConfigureAwait(true);
        }

    }
}
