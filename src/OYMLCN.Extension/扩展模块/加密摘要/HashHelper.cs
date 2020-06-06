using OYMLCN.ArgumentChecker;
using OYMLCN.Encrypt;
using OYMLCN.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// EncryptHashHelper (base on Lvcc NETCore.Encrypt 2.0.8)
    /// </summary>
    public static class EncryptHashHelper
    {
        ///// <summary>
        ///// 获取路径文件的MD5摘要值(文件不存在时返回空值)
        ///// </summary>
        ///// <param name="filePath"></param>
        ///// <returns></returns>
        //public static string GetFileMD5Hash(string filePath)
        //    => filePath.GetFileInfo()?.GetMD5Hash() ?? string.Empty;
        ///// <summary>
        ///// 获取路径文件的SHA1摘要值(文件不存在时返回空值)
        ///// </summary>
        ///// <param name="filePath"></param>
        ///// <returns></returns>
        //public static string GetFileSHA1Hash(string filePath)
        //    => filePath.GetFileInfo()?.GetSHA1Hash() ?? string.Empty;


        ///// <summary>
        ///// MD5 Hash
        ///// </summary>
        //public static string Md5(string srcString, MD5Length length = MD5Length.L32)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));

        //    string str_md5_out = string.Empty;
        //    using (MD5 md5 = MD5.Create())
        //    {
        //        byte[] bytes_md5_in = Encoding.UTF8.GetBytes(srcString);
        //        byte[] bytes_md5_out = md5.ComputeHash(bytes_md5_in);

        //        str_md5_out = length == MD5Length.L32
        //            ? BitConverter.ToString(bytes_md5_out)
        //            : BitConverter.ToString(bytes_md5_out, 4, 8);

        //        str_md5_out = str_md5_out.Replace("-", "");
        //        return str_md5_out;
        //    }
        //}
        ///// <summary>
        ///// HMACMD5 Hash
        ///// </summary>
        //public static string HMACMD5(string srcString, string key)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));
        //    key.ThrowIfEmpty(nameof(key));

        //    byte[] secrectKey = Encoding.UTF8.GetBytes(key);
        //    using (HMACMD5 md5 = new HMACMD5(secrectKey))
        //        return md5.ComputeHash(key);
        //}
        ///// <summary>
        ///// HMACMD5 Hash Base64
        ///// </summary>
        //public static string HMACMD5Base64(string srcString, string key)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));
        //    key.ThrowIfEmpty(nameof(key));

        //    byte[] secrectKey = Encoding.UTF8.GetBytes(key);
        //    using (HMACMD5 md5 = new HMACMD5(secrectKey))
        //        return md5.ComputeHashBase64(key);
        //}

        ///// <summary>
        ///// SHA1 Hash
        ///// </summary>
        //public static string Sha1(string srcString)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));

        //    using (SHA1 sha1 = SHA1.Create())
        //        return sha1.ComputeHash(srcString);
        //}
        ///// <summary>
        ///// HMAC_SHA1 Hash
        ///// </summary>
        //public static string HMACSHA1(string srcString, string key)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));
        //    key.ThrowIfEmpty(nameof(key));

        //    byte[] secrectKey = Encoding.UTF8.GetBytes(key);
        //    using (HMACSHA1 hmac = new HMACSHA1(secrectKey))
        //    {
        //        hmac.Initialize();
        //        return hmac.ComputeHash(srcString);
        //    }
        //}
        ///// <summary>
        ///// HMAC_SHA1 Hash Base64
        ///// </summary>
        //public static string HMACSHA1Base64(string srcString, string key)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));
        //    key.ThrowIfEmpty(nameof(key));

        //    byte[] secrectKey = Encoding.UTF8.GetBytes(key);
        //    using (HMACSHA1 hmac = new HMACSHA1(secrectKey))
        //    {
        //        hmac.Initialize();
        //        return hmac.ComputeHashBase64(srcString);
        //    }
        //}

        ///// <summary>
        ///// SHA256 Hash
        ///// </summary>
        //public static string Sha256(string srcString)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));

        //    using (SHA256 sha256 = SHA256.Create())
        //        return sha256.ComputeHash(srcString);
        //}
        ///// <summary>
        ///// HMAC_SHA256 Hash
        ///// </summary>
        //public static string HMACSHA256(string srcString, string key)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));
        //    key.ThrowIfEmpty(nameof(key));

        //    byte[] secrectKey = Encoding.UTF8.GetBytes(key);
        //    using (HMACSHA256 hmac = new HMACSHA256(secrectKey))
        //    {
        //        hmac.Initialize();
        //        return hmac.ComputeHash(srcString);
        //    }
        //}
        ///// <summary>
        ///// HMAC_SHA256 Hash Base64
        ///// </summary>
        //public static string HMACSHA256Base64(string srcString, string key)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));
        //    key.ThrowIfEmpty(nameof(key));

        //    byte[] secrectKey = Encoding.UTF8.GetBytes(key);
        //    using (HMACSHA256 hmac = new HMACSHA256(secrectKey))
        //    {
        //        hmac.Initialize();
        //        return hmac.ComputeHashBase64(srcString);
        //    }
        //}

        ///// <summary>
        ///// SHA384 Hash
        ///// </summary>
        //public static string Sha384(string srcString)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));

        //    using (SHA384 sha384 = SHA384.Create())
        //        return sha384.ComputeHash(srcString);
        //}
        ///// <summary>
        ///// HMAC_SHA384 Hash
        ///// </summary>
        //public static string HMACSHA384(string srcString, string key)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));
        //    key.ThrowIfEmpty(nameof(key));

        //    byte[] secrectKey = Encoding.UTF8.GetBytes(key);
        //    using (HMACSHA384 hmac = new HMACSHA384(secrectKey))
        //    {
        //        hmac.Initialize();
        //        return hmac.ComputeHash(srcString);
        //    }
        //}
        ///// <summary>
        ///// HMAC_SHA384 Hash Base64
        ///// </summary>
        //public static string HMACSHA384Base64(string srcString, string key)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));
        //    key.ThrowIfEmpty(nameof(key));

        //    byte[] secrectKey = Encoding.UTF8.GetBytes(key);
        //    using (HMACSHA384 hmac = new HMACSHA384(secrectKey))
        //    {
        //        hmac.Initialize();
        //        return hmac.ComputeHashBase64(srcString);
        //    }
        //}

        ///// <summary>
        ///// SHA512 Hash
        ///// </summary>
        //public static string Sha512(string srcString)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));

        //    using (SHA512 sha512 = SHA512.Create())
        //        return sha512.ComputeHash(srcString);
        //}
        ///// <summary>
        ///// HMAC_SHA512 Hash
        ///// </summary>
        //public static string HMACSHA512(string srcString, string key)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));
        //    key.ThrowIfEmpty(nameof(key));

        //    byte[] secrectKey = Encoding.UTF8.GetBytes(key);
        //    using (HMACSHA512 hmac = new HMACSHA512(secrectKey))
        //    {
        //        hmac.Initialize();
        //        return hmac.ComputeHash(srcString);
        //    }
        //}
        ///// <summary>
        ///// HMAC_SHA512 Hash Base64
        ///// </summary>
        //public static string HMACSHA512Base64(string srcString, string key)
        //{
        //    srcString.ThrowIfEmpty(nameof(srcString));
        //    key.ThrowIfEmpty(nameof(key));

        //    byte[] secrectKey = Encoding.UTF8.GetBytes(key);
        //    using (HMACSHA512 hmac = new HMACSHA512(secrectKey))
        //    {
        //        hmac.Initialize();
        //        return hmac.ComputeHashBase64(srcString);
        //    }
        //}

    }
}
