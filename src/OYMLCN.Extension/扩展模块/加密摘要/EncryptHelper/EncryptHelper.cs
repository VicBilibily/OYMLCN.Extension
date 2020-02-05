using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// EncryptHelper (base on Lvcc NETCore.Encrypt 2.0.8)
    /// </summary>
    public static partial class EncryptHelper
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
    }
}
