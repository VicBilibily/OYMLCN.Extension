using NETCore.Encrypt;
using NETCore.Encrypt.Internal;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// CryptographyExtension
    /// </summary>
    public static class CryptographyExtensions
    {
        private static string Encoder<T>(this T encryptor, string str) where T : HashAlgorithm
        {
            var sha1bytes = Encoding.UTF8.GetBytes(str);
            byte[] resultHash = encryptor.ComputeHash(sha1bytes);
            string sha1String = BitConverter.ToString(resultHash).ToLower();
            sha1String = sha1String.Replace("-", "");
            return sha1String;
        }
        private static string Base64Encoder<T>(this T encryptor, string key, string str) where T : HMAC
        {
            encryptor.Key = Encoding.UTF8.GetBytes(key);
            byte[] dataBuffer = Encoding.UTF8.GetBytes(str);
            byte[] hashBytes = encryptor.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// 转换字符串为SHA1加密值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeToSHA1(this string str) => SHA1.Create().Encoder(str);
        /// <summary>
        /// HMACSHA1加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncodeToHMACSHA1(this string str, string key) => new HMACSHA1(Encoding.UTF8.GetBytes(key)).Encoder(str);
        /// <summary>
        /// HMACSHA1加密(base64结果)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncodeToHMACSHA1Base64(this string str, string key) => new HMACSHA1().Base64Encoder(key, str);


        /// <summary>
        /// 转换字符串为SHA256加密值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeToSHA256(this string str) => SHA256.Create().Encoder(str);
        /// <summary>
        /// HMACSHA256加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncodeToHMACSHA256(this string str, string key) => new HMACSHA256(Encoding.UTF8.GetBytes(key)).Encoder(str);
        /// <summary>
        /// HMACSHA256加密(base64结果)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncodeToHMACSHA256Base64(this string str, string key) => new HMACSHA256().Base64Encoder(key, str);


        /// <summary>
        /// 转换字符串为SHA384加密值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeToSHA384(this string str) => SHA384.Create().Encoder(str);
        /// <summary>
        /// HMACSHA384加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncodeToHMACSHA384(this string str, string key) => new HMACSHA384(Encoding.UTF8.GetBytes(key)).Encoder(str);
        /// <summary>
        /// HMACSHA384加密(base64结果)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncodeToHMACSHA384Base64(this string str, string key) => new HMACSHA384().Base64Encoder(key, str);

        /// <summary>
        /// 转换字符串为SHA512加密值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeToSHA512(this string str) => SHA512.Create().Encoder(str);
        /// <summary>
        /// HMACSHA512加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncodeToHMACSHA512(this string str, string key) => new HMACSHA512(Encoding.UTF8.GetBytes(key)).Encoder(str);
        /// <summary>
        /// HMACSHA512加密(base64结果)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncodeToHMACSHA512Base64(this string str, string key) => new HMACSHA512().Base64Encoder(key, str);

        /// <summary>
        /// 转换字符串为MD5加密值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeToMD5(this string str) => MD5.Create().Encoder(str);
        /// <summary>
        /// HMACMD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncodeToHMACMD5(this string str, string key) => new HMACMD5(Encoding.UTF8.GetBytes(key)).Encoder(str);


        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoder">编码方式（默认为UTF8）</param>
        /// <returns></returns>
        public static string EncodeToBase64(this string str, Encoding encoder = null) =>
            Convert.ToBase64String((encoder ?? Encoding.UTF8).GetBytes(str));
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoder">编码方式（默认为UTF8）</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeFromBase64(this string str, Encoding encoder = null) =>
            (encoder ?? Encoding.UTF8).GetString(Convert.FromBase64String(str));


        /// <summary>
        /// 将明文字符串转成二进制字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringToBitString(this string str)
        {
            byte[] data = Encoding.Unicode.GetBytes(str);
            StringBuilder result = new StringBuilder(data.Length * 8);
            foreach (byte b in data)
                result.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            return result.ToString();
        }
        /// <summary>
        /// 将二进制字符串转成明文字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string BitStringToString(this string str)
        {
            CaptureCollection cs =
                Regex.Match(str, @"([01]{8})+").Groups[1].Captures;
            byte[] data = new byte[cs.Count];
            for (int i = 0; i < cs.Count; i++)
                data[i] = Convert.ToByte(cs[i].Value, 2);
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="str">明文</param>
        /// <param name="encodingAesKey">密钥</param>
        /// <returns></returns>
        public static string AESEncrypt(this string str, string encodingAesKey)
        {
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);
            var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(encodingAesKey.EncodeToMD5());
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = aes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="str">密文</param>
        /// <param name="encodingAesKey">密钥</param>
        /// <returns></returns>
        public static string AESDecrypt(this string str, string encodingAesKey)
        {
            byte[] toEncryptArray = Convert.FromBase64String(str);
            var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(encodingAesKey.EncodeToMD5());
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = aes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }



        /// <summary> 
        /// DES加密
        /// </summary> 
        /// <param name="str"></param> 
        /// <param name="key">密钥</param> 
        /// <returns></returns> 
        public static string DESEncrypt(this string str, string key = "12345678")
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var des = TripleDES.Create();
                var inputByteArray = Encoding.UTF8.GetBytes(str);
                var bKey = Encoding.ASCII.GetBytes(key.EncodeToMD5().Substring(0, 24));
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
        /// <param name="str"></param> 
        /// <param name="key">密钥</param> 
        /// <returns></returns> 
        public static string DESDecrypt(this string str, string key = "12345678")
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
                var bKey = Encoding.ASCII.GetBytes(key.EncodeToMD5().Substring(0, 24));
                des.Key = bKey;
                des.IV = bKey.Take(8).ToArray();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

#if NET461
        /// <summary>
        /// 生成 RSA 公钥和私钥
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="privateKey">私钥</param>
        public static void GenerateRSAKeys(out string publicKey, out string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                publicKey = rsa.ToXmlString(false);
                privateKey = rsa.ToXmlString(true);
            }
        }
#else
        /// <summary>
        /// 生成 RSA 公钥和私钥
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="privateKey">私钥</param>
        /// <param name="rsaSize">加密长度</param>
        public static void GenerateRSAKeys(out string publicKey, out string privateKey, RsaSize rsaSize = RsaSize.R2048)
        {
            var key = EncryptProvider.CreateRsaKey(rsaSize);
            publicKey = key.PublicKey;
            privateKey = key.PrivateKey;
        }
#endif

        /// <summary>
        /// RSA 加密
        /// </summary>
        /// <param name="content">待加密的内容</param>
        /// <param name="publickey">公钥</param>
        /// <returns>经过加密的字符串</returns>
        public static string RSAEncrypt(this string content, string publickey)
        {
#if NET461
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publickey);
            var cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

            return Convert.ToBase64String(cipherbytes);
#else
            return EncryptProvider.RSAEncrypt(publickey, content);
#endif
        }

        /// <summary>
        /// RSA 解密
        /// </summary>
        /// <param name="content">待解密的内容</param>
        /// <param name="privatekey">私钥</param>
        /// <returns>解密后的字符串</returns>
        public static string RSADecrypt(this string content, string privatekey)
        {
#if NET461
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privatekey);
            var cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);

            return Encoding.UTF8.GetString(cipherbytes);
#else
            return EncryptProvider.RSADecrypt(privatekey, content);
#endif
        }

    }
}
