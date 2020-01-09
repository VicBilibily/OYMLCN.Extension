﻿using OYMLCN.Helpers;
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
        /// <param name="str">源数据</param>  
        /// <param name="AesKey">密钥, 32位</param>
        public static string AESEncrypt(this string str, string AesKey)
            => EncryptHelper.AESEncrypt(str, AesKey);
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="str">已加密数据</param>  
        /// <param name="AesKey">密钥, 32位</param>  
        public static string AESDecrypt(this string str, string AesKey)
            => EncryptHelper.AESDecrypt(str, AesKey);
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str">源数据</param>  
        /// <param name="AesKey">密钥, 32位</param>
        /// <param name="AesIV">IV, 16位</param>  
        public static string AESEncrypt(this string str, string AesKey, string AesIV)
            => EncryptHelper.AESEncrypt(str, AesKey, AesIV);
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="str">已加密数据</param>  
        /// <param name="AesKey">密钥, 32位</param>  
        /// <param name="AesIV">IV, 16位</param>  
        public static string AESDecrypt(this string str, string AesKey, string AesIV)
            => EncryptHelper.AESDecrypt(str, AesKey, AesIV);

        /// <summary> 
        /// DES加密
        /// </summary>
        /// <param name="str">源数据</param>  
        /// <param name="DesKey">密钥, 24位</param>  
        public static string DESEncrypt(this string str, string DesKey)
            => EncryptHelper.DESEncrypt(str, DesKey);
        /// <summary> 
        /// DES解密 
        /// </summary> 
        /// <param name="str">已加密数据</param>  
        /// <param name="DesKey">密钥, 24位</param>  
        public static string DESDecrypt(this string str, string DesKey)
            => EncryptHelper.DESDecrypt(str, DesKey);

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