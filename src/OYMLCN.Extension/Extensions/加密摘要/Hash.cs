using OYMLCN.T3P.Encrypt;

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
        /// MD5摘要16位结果
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HashToMD5L16(this string str)
            => EncryptProvider.Md5(str, MD5Length.L16);
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


        /// <summary>
        /// 获取路径文件的MD5摘要值(文件不存在时返回空值)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileMD5Hash(string filePath)
            => filePath.GetFileInfo()?.GetMD5Hash() ?? string.Empty;
        /// <summary>
        /// 获取路径文件的SHA1摘要值(文件不存在时返回空值)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileSHA1Hash(string filePath)
            => filePath.GetFileInfo()?.GetSHA1Hash() ?? string.Empty;



    }
}
