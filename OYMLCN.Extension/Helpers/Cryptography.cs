using OYMLCN.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// CryptographyHelper
    /// </summary>
    public static class CryptographyHelper
    {
        /// <summary>
        /// 创建一个盐值
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string CreateSaltKey(int size)
        {
            using (var random = RandomNumberGenerator.Create())
            {
                var buff = new byte[size];
                random.GetBytes(buff);
                return buff.ConvertToBase64String();
            }
        }
        /// <summary>
        /// 创建解密密钥
        /// </summary>
        /// <param name="length">长度范围为16-48</param>
        /// <returns>DecryptionKey</returns>
        public static string CreateDecryptionKey(int length)
        {
            ArgumentChecker.IsNotOutOfRange(length, 16, 48, nameof(length));
            return CreateSaltKey(length);
        }
        /// <summary>
        /// 创建校验密钥
        /// </summary>
        /// <param name="length">长度范围为48-128</param>
        /// <returns>ValidationKey</returns>
        public static string CreateValidationKey(int length)
        {
            ArgumentChecker.IsNotOutOfRange(length, 48, 128, nameof(length));
            return CreateSaltKey(length);
        }

        /// <summary>
        /// 获取路径文件的MD5摘要值(文件不存在时返回空值)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileMD5Hash(string filePath)
            => filePath.GetFileInfo()?.GetMD5Hash() ?? string.Empty;
        /// <summary>
        /// 获取路径文件的SHA1摘要值(文件不存在时返回空值)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileSHA1Hash(string filePath)
            => filePath.GetFileInfo()?.GetSHA1Hash() ?? string.Empty;

    }
}
