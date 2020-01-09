using OYMLCN.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// EncryptExtension
    /// </summary>
    public static class EncryptExtension
    {
        /// <summary>
        /// AES加密
        /// </summary>
        public static string AESEncrypt(this string str, string encodingAesKey)
        {
            byte[] toEncryptArray = str.GetUTF8Bytes();
            var aes = Aes.Create();
            aes.Key = encodingAesKey.HashToMD5().GetUTF8Bytes();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = aes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// AES解密
        /// </summary>
        public static string AESDecrypt(this string str, string encodingAesKey)
        {
            byte[] toEncryptArray = str.ConvertFromBase64String();
            var aes = Aes.Create();
            aes.Key = encodingAesKey.HashToMD5().GetUTF8Bytes();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = aes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return resultArray.GetUTF8String();
        }

        /// <summary> 
        /// DES加密
        /// </summary> 
        public static string DESEncrypt(this string str, string key)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var des = TripleDES.Create();
                var inputByteArray = str.GetUTF8Bytes();
                var bKey = Encoding.ASCII.GetBytes(key.HashToMD5().SubString(0, 24));
                des.Key = bKey;
                des.IV = bKey.Take(8).ToArray();
                var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                var ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                    ret.AppendFormat("{0:X2}", b);
                return ret.ToString();
            }
        }
        /// <summary> 
        /// DES解密 
        /// </summary> 
        public static string DESDecrypt(this string str, string key)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var des = TripleDES.Create();
                var len = str.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x;
                for (x = 0; x < len; x++)
                {
                    var i = Convert.ToInt32(str.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                var bKey = Encoding.ASCII.GetBytes(key.HashToMD5().SubString(0, 24));
                des.Key = bKey;
                des.IV = bKey.Take(8).ToArray();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }


        #region RSA
        /// <summary>
        /// 计算 RSA 签名
        /// </summary>
        public static string SignAsRSA(this string conent, string privateKey)
            => EncryptHelper.SignAsRSA(conent, privateKey, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1, Encoding.UTF8);
        /// <summary>
        /// 检查 RSA 签名的有效性
        /// </summary>
        public static bool VerifyRSASign(this string content, string signStr, string publickKey)
            => EncryptHelper.VerifyRSASign(content, signStr, publickKey, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1, Encoding.UTF8);

        /// <summary>
        /// RSA 加密
        /// </summary>
        public static string RSAEncrypt(this string srcString, string publicKey)
            => EncryptHelper.RSAEncrypt(publicKey, srcString, RSAEncryptionPadding.OaepSHA512);
        /// <summary>
        /// RSA 加密（PEM密钥）
        /// </summary>
        public static string RSAEncryptWithPem(this string srcString, string publicKey)
            => EncryptHelper.RSAEncrypt(publicKey, srcString, RSAEncryptionPadding.Pkcs1, true);

        /// <summary>
        /// RSA 解密
        /// </summary>
        public static string RSADecrypt(this string srcString, string privateKey)
            => EncryptHelper.RSADecrypt(privateKey, srcString, RSAEncryptionPadding.OaepSHA512);
        /// <summary>
        /// RSA 解密（PEM密钥）
        /// </summary>
        public static string RSADecryptWithPem(this string srcString, string privateKey)
            => EncryptHelper.RSADecrypt(privateKey, srcString, RSAEncryptionPadding.Pkcs1, true);
        #endregion
    }
}