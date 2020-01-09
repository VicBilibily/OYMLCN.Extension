using OYMLCN.Helpers;

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
            => EncryptHelper.Sha1(str);
        /// <summary>
        /// HMACSHA1摘要结果
        /// </summary>
        public static string HashToHMACSHA1(this string str, string key)
            => EncryptHelper.HMACSHA1(str, key);
        /// <summary>
        /// HMACSHA1摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACSHA1Base64(this string str, string key)
            => EncryptHelper.HMACSHA1Base64(str, key);

        /// <summary>
        /// SHA256摘要结果
        /// </summary>
        public static string HashToSHA256(this string str)
            => EncryptHelper.Sha256(str);
        /// <summary>
        /// HMACSHA256摘要结果
        /// </summary>
        public static string HashToHMACSHA256(this string str, string key)
            => EncryptHelper.HMACSHA256(str, key);
        /// <summary>
        /// HMACSHA256摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACSHA256Base64(this string str, string key)
            => EncryptHelper.HMACSHA256Base64(str, key);

        /// <summary>
        /// SHA384摘要结果
        /// </summary>
        public static string HashToSHA384(this string str)
            => EncryptHelper.Sha384(str);
        /// <summary>
        /// HMACSHA384摘要结果
        /// </summary>
        public static string HashToHMACSHA384(this string str, string key)
            => EncryptHelper.HMACSHA384(str, key);
        /// <summary>
        /// HMACSHA384摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACSHA384Base64(this string str, string key)
            => EncryptHelper.HMACSHA384Base64(str, key);

        /// <summary>
        /// SHA512摘要结果
        /// </summary>
        public static string HashToSHA512(this string str)
            => EncryptHelper.Sha512(str);
        /// <summary>
        /// HMACSHA512摘要结果
        /// </summary>
        public static string HashToHMACSHA512(this string str, string key)
            => EncryptHelper.HMACSHA512(str, key);
        /// <summary>
        /// HMACSHA512摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACSHA512Base64(this string str, string key)
            => EncryptHelper.HMACSHA512Base64(str, key);

        /// <summary>
        /// MD5摘要结果
        /// </summary>
        public static string HashToMD5(this string str)
            => EncryptHelper.Md5(str);
        /// <summary>
        /// HMACMD5摘要结果
        /// </summary>
        public static string HashToHMACMD5(this string str, string key)
            => EncryptHelper.HMACMD5(str, key);
        /// <summary>
        /// HMACMD5摘要结果(Base64结果)
        /// </summary>
        public static string HashToHMACMD5Base64(this string str, string key)
            => EncryptHelper.HMACMD5Base64(str, key);
    }
}
