using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{

    /// <summary>
    /// HashExtensions
    /// </summary>
    public static class HashExtensions
    {
        #region Encoder/Base64Encoder
        internal static string Encoder<T>(this T encryptor, string str) where T : HashAlgorithm
        {
            var sha1bytes = str.GetUTF8Bytes();
            byte[] resultHash = encryptor.ComputeHash(sha1bytes);
            string sha1String = BitConverter.ToString(resultHash).ToLower();
            sha1String = sha1String.Replace("-", "");
            return sha1String;
        }
        internal static string Base64Encoder<T>(this T encryptor, string str) where T : HMAC
        {
            byte[] dataBuffer = str.GetUTF8Bytes();
            byte[] hashBytes = encryptor.ComputeHash(dataBuffer);
            return hashBytes.ConvertToBase64String();
        }
        #endregion

        #region Hash
        /// <summary>
        /// SHA1摘要结果
        /// </summary>
        public static string HashToSHA1(this string str)
            => SHA1.Create().Encoder(str);
        /// <summary>
        /// HMACSHA1摘要结果
        /// </summary>
        public static string HashToHMACSHA1(this string str, string key)
            => new HMACSHA1(key.GetUTF8Bytes()).Encoder(str);
        /// <summary>
        /// HMACSHA1摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACSHA1Base64(this string str, string key)
            => new HMACSHA1(key.GetUTF8Bytes()).Base64Encoder(str);

        /// <summary>
        /// SHA256摘要结果
        /// </summary>
        public static string HashToSHA256(this string str)
            => SHA256.Create().Encoder(str);
        /// <summary>
        /// HMACSHA256摘要结果
        /// </summary>
        public static string HashToHMACSHA256(this string str, string key)
            => new HMACSHA256(key.GetUTF8Bytes()).Encoder(str);
        /// <summary>
        /// HMACSHA256摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACSHA256Base64(this string str, string key)
            => new HMACSHA256(key.GetUTF8Bytes()).Base64Encoder(str);

        /// <summary>
        /// SHA384摘要结果
        /// </summary>
        public static string HashToSHA384(this string str)
            => SHA384.Create().Encoder(str);
        /// <summary>
        /// HMACSHA384摘要结果
        /// </summary>
        public static string HashToHMACSHA384(this string str, string key)
            => new HMACSHA384(key.GetUTF8Bytes()).Encoder(str);
        /// <summary>
        /// HMACSHA384摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACSHA384Base64(this string str, string key)
            => new HMACSHA384(key.GetUTF8Bytes()).Base64Encoder(str);

        /// <summary>
        /// SHA512摘要结果
        /// </summary>
        public static string HashToSHA512(this string str)
            => SHA512.Create().Encoder(str);
        /// <summary>
        /// HMACSHA512摘要结果
        /// </summary>
        public static string HashToHMACSHA512(this string str, string key)
            => new HMACSHA512(key.GetUTF8Bytes()).Encoder(str);
        /// <summary>
        /// HMACSHA512摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACSHA512Base64(this string str, string key)
            => new HMACSHA512(key.GetUTF8Bytes()).Base64Encoder(str);

        /// <summary>
        /// MD5摘要结果
        /// </summary>
        public static string HashToMD5(this string str)
            => MD5.Create().Encoder(str);
        /// <summary>
        /// HMACMD5摘要结果
        /// </summary>
        public static string HashToHMACMD5(this string str, string key)
            => new HMACMD5(key.GetUTF8Bytes()).Encoder(str);
        /// <summary>
        /// HMACMD5摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACMD5Base64(this string str, string key)
            => new HMACMD5(key.GetUTF8Bytes()).Base64Encoder(str);
        #endregion
    }
}
