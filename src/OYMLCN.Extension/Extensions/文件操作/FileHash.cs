using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 文件操作相关扩展
    /// </summary>
    public static partial class FileInfoExtension
    {
        /// <summary>
        /// 获取路径文件的MD5摘要值(文件不存在时返回空值)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Obsolete("后期要处理文件Hash相关扩展")]
        public static string GetMD5Hash(this FileInfo file)
        {
            if (file.Exists)
                using (FileStream temp = new FileStream(file.FullName, FileMode.Open))
                    return temp.GetMD5Hash();
            else
                return string.Empty;
        }
        /// <summary>
        /// 计算文件流的MD5摘要值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetMD5Hash(this Stream stream)
        {
            MD5 md5 = MD5.Create();
            byte[] retVal = md5.ComputeHash(stream);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
                sb.Append(retVal[i].ToString("x2"));
            return sb.ToString();
        }
        /// <summary>
        /// 获取路径文件的SHA1摘要值(文件不存在时返回空值)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetSHA1Hash(this FileInfo file)
        {
            if (file.Exists)
                using (FileStream temp = new FileStream(file.FullName, FileMode.Open))
                    return temp.GetSHA1Hash();
            else
                return string.Empty;
        }
        /// <summary>
        /// 计算文件流的SHA1摘要值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetSHA1Hash(this Stream stream)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] retVal = sha1.ComputeHash(stream);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
                sb.Append(retVal[i].ToString("x2"));
            return sb.ToString();
        }

    }
}
