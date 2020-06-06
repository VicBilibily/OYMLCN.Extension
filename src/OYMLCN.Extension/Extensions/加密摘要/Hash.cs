using OYMLCN.Encrypt;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// HashExtensions
    /// </summary>
    public static class HashExtensions
    {
        /// <summary>
        /// SHA1摘要结果
        /// </summary>
        public static string HashToSHA1(this string str)
            => EncryptProvider.Sha1(str);
        /// <summary>
        /// HMACSHA1摘要结果
        /// </summary>
        public static string HashToHMACSHA1(this string str, string key)
            => EncryptProvider.HMACSHA1(str, key);
        /// <summary>
        /// HMACSHA1摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACSHA1Base64(this string str, string key)
            => EncryptProvider.HMACSHA1(str, key, base64: true);

        /// <summary>
        /// SHA256摘要结果
        /// </summary>
        public static string HashToSHA256(this string str)
            => EncryptProvider.Sha256(str);
        /// <summary>
        /// HMACSHA256摘要结果
        /// </summary>
        public static string HashToHMACSHA256(this string str, string key)
            => EncryptProvider.HMACSHA256(str, key);
        /// <summary>
        /// HMACSHA256摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACSHA256Base64(this string str, string key)
            => EncryptProvider.HMACSHA256(str, key, base64: true);

        /// <summary>
        /// SHA384摘要结果
        /// </summary>
        public static string HashToSHA384(this string str)
            => EncryptProvider.Sha384(str);
        /// <summary>
        /// HMACSHA384摘要结果
        /// </summary>
        public static string HashToHMACSHA384(this string str, string key)
            => EncryptProvider.HMACSHA384(str, key);
        /// <summary>
        /// HMACSHA384摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACSHA384Base64(this string str, string key)
            => EncryptProvider.HMACSHA384(str, key, base64: true);

        /// <summary>
        /// SHA512摘要结果
        /// </summary>
        public static string HashToSHA512(this string str)
            => EncryptProvider.Sha512(str);
        /// <summary>
        /// HMACSHA512摘要结果
        /// </summary>
        public static string HashToHMACSHA512(this string str, string key)
            => EncryptProvider.HMACSHA512(str, key);
        /// <summary>
        /// HMACSHA512摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACSHA512Base64(this string str, string key)
            => EncryptProvider.HMACSHA512(str, key, base64: true);

        /// <summary>
        /// MD5摘要结果
        /// </summary>
        public static string HashToMD5(this string str)
            => EncryptProvider.Md5(str);
        /// <summary>
        /// HMACMD5摘要结果
        /// </summary>
        public static string HashToHMACMD5(this string str, string key)
            => EncryptProvider.HMACMD5(str, key);
        /// <summary>
        /// HMACMD5摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACMD5Base64(this string str, string key)
            => EncryptProvider.HMACMD5(str, key, base64: true);
    }
}
