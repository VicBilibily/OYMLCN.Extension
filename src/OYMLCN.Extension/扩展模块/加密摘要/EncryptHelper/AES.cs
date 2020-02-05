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
        #region AES
        /// <summary>
        /// 生成 AES 密钥
        /// </summary>
        /// <returns></returns>
        public static AESKey GenerateAESKey()
            => new AESKey()
            {
                Key = StringHelper.RandCode(32),
                IV = StringHelper.RandCode(16)
            };

        /// <summary>  
        /// AES 加密
        /// </summary>  
        /// <param name="data">源数据</param>  
        /// <param name="key">密钥, 32位</param>  
        /// <param name="vector">IV, 16位</param>  
        public static string AESEncrypt(string data, string key, string vector)
        {
            data.ThrowIfEmpty(nameof(data));

            ArgumentChecker.IsNotEmpty(key, nameof(key));
            ArgumentChecker.IsNotOutOfRange(key.Length, 32, 32, nameof(key));

            ArgumentChecker.IsNotEmpty(vector, nameof(vector));
            ArgumentChecker.IsNotOutOfRange(vector.Length, 16, 16, nameof(vector));

            byte[] plainBytes = data.GetUTF8Bytes();
            var encryptBytes = AESEncrypt(plainBytes, key, vector);
            if (encryptBytes == null)
                return null;
            return encryptBytes.GetUTF8String();
        }
        /// <summary>  
        /// AES 加密
        /// </summary>  
        /// <param name="data">源数据</param>  
        /// <param name="key">密钥, 32位</param>  
        /// <param name="vector">IV, 16位</param>  
        public static byte[] AESEncrypt(byte[] data, string key, string vector)
        {
            ArgumentChecker.IsNotEmpty(data, nameof(data));

            ArgumentChecker.IsNotEmpty(key, nameof(key));
            ArgumentChecker.IsNotOutOfRange(key.Length, 32, 32, nameof(key));

            ArgumentChecker.IsNotEmpty(vector, nameof(vector));
            ArgumentChecker.IsNotOutOfRange(vector.Length, 16, 16, nameof(vector));

            byte[] plainBytes = data;
            byte[] bKey = new byte[32];
            Array.Copy(key.PadRight(bKey.Length).GetUTF8Bytes(), bKey, bKey.Length);
            byte[] bVector = new byte[16];
            Array.Copy(vector.PadRight(bVector.Length).GetUTF8Bytes(), bVector, bVector.Length);

            byte[] encryptData = null; // encrypted data
            using (Aes Aes = Aes.Create())
                try
                {
                    using (MemoryStream Memory = new MemoryStream())
                    using (CryptoStream Encryptor = new CryptoStream(Memory, Aes.CreateEncryptor(bKey, bVector), CryptoStreamMode.Write))
                    {
                        Encryptor.Write(plainBytes, 0, plainBytes.Length);
                        Encryptor.FlushFinalBlock();
                        encryptData = Memory.ToArray();
                    }
                }
                catch
                {
                    encryptData = null;
                }
            return encryptData;
        }

        /// <summary>  
        /// AES 解密
        /// </summary>  
        /// <param name="data">已加密数据</param>  
        /// <param name="key">密钥, 32位</param>  
        /// <param name="vector">IV, 16位</param>  
        public static string AESDecrypt(string data, string key, string vector)
        {
            ArgumentChecker.IsNotEmpty(data, nameof(data));

            ArgumentChecker.IsNotEmpty(key, nameof(key));
            ArgumentChecker.IsNotOutOfRange(key.Length, 32, 32, nameof(key));

            ArgumentChecker.IsNotEmpty(vector, nameof(vector));
            ArgumentChecker.IsNotOutOfRange(vector.Length, 16, 16, nameof(vector));

            byte[] encryptedBytes = data.GetUTF8Bytes();
            byte[] decryptBytes = AESDecrypt(encryptedBytes, key, vector);
            if (decryptBytes == null)
                return null;
            return decryptBytes.GetUTF8String();
        }
        /// <summary>  
        /// AES 解密
        /// </summary>  
        /// <param name="data">已加密数据</param>  
        /// <param name="key">密钥, 32位</param>  
        /// <param name="vector">IV, 16位</param>  
        public static byte[] AESDecrypt(byte[] data, string key, string vector)
        {
            ArgumentChecker.IsNotEmpty(data, nameof(data));

            ArgumentChecker.IsNotEmpty(key, nameof(key));
            ArgumentChecker.IsNotOutOfRange(key.Length, 32, 32, nameof(key));

            ArgumentChecker.IsNotEmpty(vector, nameof(vector));
            ArgumentChecker.IsNotOutOfRange(vector.Length, 16, 16, nameof(vector));

            byte[] encryptedBytes = data;
            byte[] bKey = new byte[32];
            Array.Copy(key.PadRight(bKey.Length).GetUTF8Bytes(), bKey, bKey.Length);
            byte[] bVector = new byte[16];
            Array.Copy(vector.PadRight(bVector.Length).GetUTF8Bytes(), bVector, bVector.Length);

            byte[] decryptedData = null; // decrypted data
            using (Aes Aes = Aes.Create())
                try
                {
                    using (MemoryStream Memory = new MemoryStream(encryptedBytes))
                    using (CryptoStream Decryptor = new CryptoStream(Memory, Aes.CreateDecryptor(bKey, bVector), CryptoStreamMode.Read))
                    using (MemoryStream tempMemory = new MemoryStream())
                    {
                        byte[] Buffer = new byte[1024];
                        int readBytes = 0;
                        while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                            tempMemory.Write(Buffer, 0, readBytes);
                        decryptedData = tempMemory.ToArray();
                    }
                }
                catch
                {
                    decryptedData = null;
                }
            return decryptedData;
        }

        /// <summary>  
        /// AES 加密（无IV向量） 
        /// </summary>  
        /// <param name="data">源数据</param>  
        /// <param name="key">密钥, 32位</param>  
        public static string AESEncrypt(string data, string key)
        {
            ArgumentChecker.IsNotEmpty(data, nameof(data));
            ArgumentChecker.IsNotEmpty(key, nameof(key));
            ArgumentChecker.IsNotOutOfRange(key.Length, 32, 32, nameof(key));

            using (MemoryStream Memory = new MemoryStream())
            using (Aes aes = Aes.Create())
            {
                byte[] plainBytes = data.GetUTF8Bytes();
                byte[] bKey = new byte[32];
                Array.Copy(key.PadRight(bKey.Length).GetUTF8Bytes(), bKey, bKey.Length);

                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 128;
                aes.Key = bKey;

                using (CryptoStream cryptoStream = new CryptoStream(Memory, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    try
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Memory.ToArray().ConvertToBase64String();
                    }
                    catch //(Exception ex)
                    {
                        return null;
                    }
            }
        }
        /// <summary>  
        /// AES 解密（无IV向量）
        /// </summary>  
        /// <param name="data">已加密数据</param>  
        /// <param name="key">密钥, 32位</param>  
        public static string AESDecrypt(string data, string key)
        {
            ArgumentChecker.IsNotEmpty(data, nameof(data));
            ArgumentChecker.IsNotEmpty(key, nameof(key));
            ArgumentChecker.IsNotOutOfRange(key.Length, 32, 32, nameof(key));

            byte[] encryptedBytes = data.ConvertFromBase64String();
            byte[] bKey = new byte[32];
            Array.Copy(key.PadRight(bKey.Length).GetUTF8Bytes(), bKey, bKey.Length);

            using (MemoryStream Memory = new MemoryStream(encryptedBytes))
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 128;
                aes.Key = bKey;

                using (CryptoStream cryptoStream = new CryptoStream(Memory, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    try
                    {
                        byte[] tmp = new byte[encryptedBytes.Length];
                        int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length);
                        byte[] ret = new byte[len];
                        Array.Copy(tmp, 0, ret, 0, len);
                        return Encoding.UTF8.GetString(ret, 0, len);
                    }
                    catch //(Exception ex)
                    {
                        return null;
                    }
            }
        }
        #endregion

    }
}
