using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OYMLCN.Exceptions
{
    /// <summary>
    /// 异常：加密字符串超出最大长度
    /// </summary>
    public class EncryptOutofMaxlengthException : Exception
    {
        /// <summary>
        /// 允许的最大长度
        /// </summary>
        public int MaxLength { get; private set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; private set; }
        /// <summary>
        /// Rsa key size
        /// </summary>
        public int KeySize { get; private set; }
        /// <summary>
        /// Rsa Padding
        /// </summary>
        public RSAEncryptionPadding RSAEncryptionPadding { get; private set; }

        internal EncryptOutofMaxlengthException(int maxLength, int keySize, RSAEncryptionPadding rsaEncryptionPadding)
        {
            MaxLength = maxLength;
            KeySize = keySize;
            RSAEncryptionPadding = rsaEncryptionPadding;
        }
        internal EncryptOutofMaxlengthException(string message, int maxLength, int keySize, RSAEncryptionPadding rsaEncryptionPadding) : this(maxLength, keySize, rsaEncryptionPadding)
        {
            ErrorMessage = message;
        }
    }
}
