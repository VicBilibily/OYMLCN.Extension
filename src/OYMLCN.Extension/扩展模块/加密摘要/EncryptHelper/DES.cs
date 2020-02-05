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
        #region DES
        /// <summary>
        /// 生成 DES 密钥
        /// </summary>
        /// <returns></returns>
        public static string GenerateDesKey()
            => StringHelper.RandCode(24);
        /// <summary>
        /// 生成 DES 密钥向量
        /// </summary>
        /// <returns></returns>
        public static string GenerateDesKeyIV()
            => StringHelper.RandCode(8);

        /// <summary>  
        /// DES 加密
        /// </summary>  
        /// <param name="data">源数据</param>  
        /// <param name="key">密钥, 24位</param>  
        public static string DESEncrypt(string data, string key)
        {
            ArgumentChecker.IsNotEmpty(data, nameof(data));
            ArgumentChecker.IsNotEmpty(key, nameof(key));
            ArgumentChecker.IsNotOutOfRange(key.Length, 24, 24, nameof(key));

            byte[] plainBytes = data.GetUTF8Bytes();
            var encryptBytes = DESEncrypt(plainBytes, key, CipherMode.ECB);
            if (encryptBytes == null)
                return null;
            return encryptBytes.ConvertToBase64String();
        }
        /// <summary>  
        /// DES 加密
        /// </summary>  
        /// <param name="data">源数据</param>  
        /// <param name="key">密钥, 24位</param> 
        public static byte[] DESEncrypt(byte[] data, string key)
        {
            ArgumentChecker.IsNotEmpty(data, nameof(data));
            ArgumentChecker.IsNotEmpty(key, nameof(key));
            ArgumentChecker.IsNotOutOfRange(key.Length, 24, 24, nameof(key));
            return DESEncrypt(data, key, CipherMode.ECB);
        }


        /// <summary>  
        /// DES 加密
        /// </summary>
        /// <param name="data">源数据</param>  
        /// <param name="key">密钥, 24位</param>  
        /// <param name="vector">IV, 8位</param>  
        public static byte[] DESEncrypt(byte[] data, string key, string vector)
        {
            ArgumentChecker.IsNotEmpty(data, nameof(data));
            ArgumentChecker.IsNotEmpty(key, nameof(key));
            ArgumentChecker.IsNotOutOfRange(key.Length, 24, 24, nameof(key));
            ArgumentChecker.IsNotEmpty(vector, nameof(vector));
            ArgumentChecker.IsNotOutOfRange(vector.Length, 8, 8, nameof(vector));
            return DESEncrypt(data, key, CipherMode.CBC, vector);
        }
        /// <summary>  
        /// DES 加密
        /// </summary>
        /// <param name="data">源数据</param>  
        /// <param name="key">密钥, 24位</param>  
        /// <param name="cipherMode"><see cref="CipherMode"/></param>  
        /// <param name="paddingMode"><see cref="PaddingMode"/> 默认为 PKCS7</param>  
        /// <param name="vector">IV, 8位</param>  
        private static byte[] DESEncrypt(byte[] data, string key, CipherMode cipherMode, string vector = "", PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            ArgumentChecker.IsNotEmpty(data, nameof(data));
            ArgumentChecker.IsNotEmpty(key, nameof(key));
            ArgumentChecker.IsNotOutOfRange(key.Length, 24, 24, nameof(key));

            using (MemoryStream Memory = new MemoryStream())
            using (TripleDES des = TripleDES.Create())
            {
                byte[] plainBytes = data;
                byte[] bKey = new byte[24];
                Array.Copy(key.PadRight(bKey.Length).GetUTF8Bytes(), bKey, bKey.Length);

                des.Mode = cipherMode;
                des.Padding = paddingMode;
                des.Key = bKey;

                if (cipherMode == CipherMode.CBC)
                {
                    byte[] bVector = new byte[8];
                    Array.Copy(vector.PadRight(bVector.Length).GetUTF8Bytes(), bVector, bVector.Length);
                    des.IV = bVector;
                }

                using (CryptoStream cryptoStream = new CryptoStream(Memory, des.CreateEncryptor(), CryptoStreamMode.Write))
                    try
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Memory.ToArray();
                    }
                    catch //(Exception ex)
                    {
                        return null;
                    }
            }
        }

        /// <summary>  
        /// DES 解密
        /// </summary>  
        /// <param name="data">已加密数据</param>  
        /// <param name="key">密钥, 24位</param>  
        public static string DESDecrypt(string data, string key)
        {
            ArgumentChecker.IsNotEmpty(data, nameof(data));
            ArgumentChecker.IsNotEmpty(key, nameof(key));
            ArgumentChecker.IsNotOutOfRange(key.Length, 24, 24, nameof(key));

            byte[] encryptedBytes = data.ConvertFromBase64String();
            byte[] bytes = DESDecrypt(encryptedBytes, key, CipherMode.ECB);
            if (bytes == null)
                return null;
            return bytes.GetUTF8String();
        }
        /// <summary>  
        /// DES 解密
        /// </summary>  
        /// <param name="data">已加密数据</param>  
        /// <param name="key">密钥, 24位</param>  
        public static byte[] DESDecrypt(byte[] data, string key)
        {
            ArgumentChecker.IsNotEmpty(data, nameof(data));
            ArgumentChecker.IsNotEmpty(key, nameof(key));
            ArgumentChecker.IsNotOutOfRange(key.Length, 24, 24, nameof(key));
            return DESDecrypt(data, key, CipherMode.ECB);
        }
        /// <summary>  
        /// DES 解密
        /// </summary>  
        /// <param name="data">已加密数据</param>  
        /// <param name="key">密钥, 24位</param>  
        /// <param name="vector">IV, 8位</param>  
        public static byte[] DESDecrypt(byte[] data, string key, string vector)
        {
            ArgumentChecker.IsNotEmpty(data, nameof(data));
            ArgumentChecker.IsNotEmpty(key, nameof(key));
            ArgumentChecker.IsNotOutOfRange(key.Length, 24, 24, nameof(key));
            ArgumentChecker.IsNotEmpty(vector, nameof(vector));
            ArgumentChecker.IsNotOutOfRange(vector.Length, 8, 8, nameof(vector));
            return DESDecrypt(data, key, CipherMode.CBC, vector);
        }
        /// <summary>  
        /// DES 解密
        /// </summary>  
        /// <param name="data">已加密数据</param>  
        /// <param name="key">密钥, 24位</param>  
        /// <param name="cipherMode"><see cref="CipherMode"/></param>  
        /// <param name="vector">IV, 8位</param>  
        /// <param name="paddingMode"><see cref="PaddingMode"/> 默认为 PKCS7</param>  
        /// <returns>Decrypted byte array</returns>  
        private static byte[] DESDecrypt(byte[] data, string key, CipherMode cipherMode, string vector = "", PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            ArgumentChecker.IsNotEmpty(data, nameof(data));
            ArgumentChecker.IsNotEmpty(key, nameof(key));
            ArgumentChecker.IsNotOutOfRange(key.Length, 24, 24, nameof(key));

            byte[] encryptedBytes = data;
            byte[] bKey = new byte[24];
            Array.Copy(key.PadRight(bKey.Length).GetUTF8Bytes(), bKey, bKey.Length);

            using (MemoryStream Memory = new MemoryStream(encryptedBytes))
            using (TripleDES des = TripleDES.Create())
            {
                des.Mode = cipherMode;
                des.Padding = paddingMode;
                des.Key = bKey;

                if (cipherMode == CipherMode.CBC)
                {
                    byte[] bVector = new byte[8];
                    Array.Copy(vector.PadRight(bVector.Length).GetUTF8Bytes(), bVector, bVector.Length);
                    des.IV = bVector;
                }

                using (CryptoStream cryptoStream = new CryptoStream(Memory, des.CreateDecryptor(), CryptoStreamMode.Read))
                    try
                    {
                        byte[] tmp = new byte[encryptedBytes.Length];
                        int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length);
                        byte[] ret = new byte[len];
                        Array.Copy(tmp, 0, ret, 0, len);
                        return ret;
                    }
                    catch
                    {
                        return null;
                    }
            }
        }
        #endregion

    }
}
