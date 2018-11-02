#if !NET35
using NETCore.Encrypt;
using NETCore.Encrypt.Extensions.Internal;
using NETCore.Encrypt.Internal;
#endif
using OYMLCN.Handlers;
using System;
using System.Security.Cryptography;
using System.Text;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// CryptographyExtensions
    /// </summary>
    public static class CryptographyExtensions
    {
        internal static string Encoder<T>(this T encryptor, string str) where T : HashAlgorithm
        {
            var sha1bytes = Encoding.UTF8.GetBytes(str);
            byte[] resultHash = encryptor.ComputeHash(sha1bytes);
            string sha1String = BitConverter.ToString(resultHash).ToLower();
            sha1String = sha1String.Replace("-", "");
            return sha1String;
        }
        internal static string Encoder<T>(this T encryptor, string key, string str) where T : HMAC
        {
            encryptor.Key = Encoding.UTF8.GetBytes(key);
            return encryptor.Encoder<T>(str);
        }
        internal static string Base64Encoder<T>(this T encryptor, string key, string str) where T : HMAC
        {
            encryptor.Key = Encoding.UTF8.GetBytes(key);
            byte[] dataBuffer = Encoding.UTF8.GetBytes(str);
            byte[] hashBytes = encryptor.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// 转向不可逆加密字符串处理
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key">HMAC加密需要提供密钥</param>
        /// <returns></returns>
        public static EncryptHandler AsEncrypt(this string str, string key = null)
            => new EncryptHandler(str, key);

        /// <summary>
        /// 转向可以加密字符串处理
        /// </summary>
        /// <param name="str"></param>
        /// <param name="keyOrPrivateKey">16/24/32长度密钥或RSA私/公钥</param>
        /// <param name="publicKey">RSA公钥</param>
        /// <returns></returns>
        public static CryptographyHandler AsCryptography(this string str, string keyOrPrivateKey, string publicKey = null)
            => new CryptographyHandler(str, keyOrPrivateKey, publicKey);

#if !NET35
        /// <summary>
        /// 生成 RSA 公钥和私钥
        /// </summary>
        /// <param name="rsaSize">密钥长度</param>
        public static RSAKey GenerateRSAKeys(RsaSize rsaSize = RsaSize.R2048)
        {
#if NET461
            using (var rsa = new RSACryptoServiceProvider())
                return new RSAKey()
                {
                    PublicKey = rsa.ToXmlString(false),
                    PrivateKey = rsa.ToXmlString(true),
                    Exponent = rsa.ExportParameters(false).Exponent.ToHexString(),
                    Modulus = rsa.ExportParameters(false).Modulus.ToHexString()
                };
#else
            return EncryptProvider.CreateRsaKey(rsaSize);
#endif
        }
#endif

    }
}
