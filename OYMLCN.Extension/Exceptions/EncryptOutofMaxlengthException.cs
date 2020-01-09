using System;
using System.Security.Cryptography;

namespace OYMLCN.Exceptions
{
    /// <summary>
    /// 异常：加密字符串超出最大长度
    /// </summary>
    [Serializable]
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

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public EncryptOutofMaxlengthException() { }
        public EncryptOutofMaxlengthException(string message) : base(message) { }
        public EncryptOutofMaxlengthException(string message, Exception innerException) : base(message, innerException) { }
        public EncryptOutofMaxlengthException(int maxLength, int keySize, RSAEncryptionPadding rsaEncryptionPadding)
        {
            MaxLength = maxLength;
            KeySize = keySize;
            RSAEncryptionPadding = rsaEncryptionPadding;
        }
        public EncryptOutofMaxlengthException(string message, int maxLength, int keySize, RSAEncryptionPadding rsaEncryptionPadding) : this(maxLength, keySize, rsaEncryptionPadding)
        {
            ErrorMessage = message;
        }

        protected EncryptOutofMaxlengthException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释


    }
}
