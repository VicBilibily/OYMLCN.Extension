using OYMLCN.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace OYMLCN.Handlers
{
    /// <summary>
    /// 加密处理
    /// </summary>
    public class EncryptHandler
    {
        private string Str;
        private string Key;
        internal EncryptHandler(string str, string key)
        {
            Str = str;
            Key = key;
        }

        /// <summary>
        /// SHA1加密值
        /// </summary>
        public string SHA1 
            => System.Security.Cryptography.SHA1.Create().Encoder(Str);
        /// <summary>
        /// HMACSHA1加密值
        /// </summary>
        public string HMACSHA1 
            => new HMACSHA1().Encoder(Key, Str);
        /// <summary>
        /// HMACSHA1加密值(base64结果)
        /// </summary>
        public string HMACSHA1Base64 
            => new HMACSHA1().Base64Encoder(Key, Str);


        /// <summary>
        /// SHA256加密值
        /// </summary>
        public string SHA256 
            => System.Security.Cryptography.SHA256.Create().Encoder(Str);
        /// <summary>
        /// HMACSHA256加密值
        /// </summary>
        public string HMACSHA256 
            => new HMACSHA256().Encoder(Key, Str);
        /// <summary>
        /// HMACSHA256加密值(base64结果)
        /// </summary>
        public string HMACSHA256Base64
            => new HMACSHA256().Base64Encoder(Key, Str);


        /// <summary>
        /// SHA384加密值
        /// </summary>
        public string SHA384 
            => System.Security.Cryptography.SHA384.Create().Encoder(Str);
        /// <summary>
        /// HMACSHA384加密值
        /// </summary>
        public string HMACSHA384 
            => new HMACSHA384().Encoder(Key, Str);
        /// <summary>
        /// HMACSHA384加密值(base64结果)
        /// </summary>
        public string HMACSHA384Base64 
            => new HMACSHA384().Base64Encoder(Key, Str);

        /// <summary>
        /// SHA512加密值
        /// </summary>
        public string SHA512 
            => System.Security.Cryptography.SHA512.Create().Encoder(Str);
        /// <summary>
        /// HMACSHA512加密值
        /// </summary>
        public string HMACSHA512 
            => new HMACSHA512().Encoder(Key, Str);
        /// <summary>
        /// HMACSHA512加密值(base64结果)
        /// </summary>
        public string HMACSHA512Base64
            => new HMACSHA512().Base64Encoder(Key, Str);

        /// <summary>
        /// MD5加密值
        /// </summary>
        public string MD5 
            => System.Security.Cryptography.MD5.Create().Encoder(Str);
        /// <summary>
        /// HMACMD5加密值
        /// </summary>
        public string HMACMD5
            => new HMACMD5().Encoder(Key, Str);
        /// <summary>
        /// HMACMD5加密值(base64结果)
        /// </summary>
        public string HMACMD5Base64
            => new HMACMD5().Base64Encoder(Key, Str);

    }

    /// <summary>
    /// 密码学加解密处理
    /// </summary>
    public class CryptographyHandler
    {
        private string Str;
        private string Key;
        private string PublicKey;
        internal CryptographyHandler(string str, string keyOrPrivateKey, string publicKey = null)
        {
            Str = str;
            Key = keyOrPrivateKey;
            PublicKey = publicKey;
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        public string EncodeToBase64 
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(Str));
        /// <summary>
        /// Base64解密
        /// </summary>
        public string DecodeFromBase64
            => Encoding.UTF8.GetString(Convert.FromBase64String(Str));

        /// <summary>
        /// 原明文字符串转成二进制字符串
        /// </summary>
        public string StringToBitString
        {
            get
            {
                byte[] data = Encoding.Unicode.GetBytes(Str);
                StringBuilder result = new StringBuilder(data.Length * 8);
                foreach (byte b in data)
                    result.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
                return result.ToString();
            }
        }
        /// <summary>
        /// 原二进制字符串转成明文字符串
        /// </summary>
        public string BitStringToString
        {
            get
            {
                CaptureCollection cs = Regex.Match(Str, @"([01]{8})+").Groups[1].Captures;
                byte[] data = new byte[cs.Count];
                for (int i = 0; i < cs.Count; i++)
                    data[i] = Convert.ToByte(cs[i].Value, 2);
                return Encoding.Unicode.GetString(data, 0, data.Length);
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        public string AESEncrypt//(this string str, string encodingAesKey)
        {
            get
            {
                byte[] toEncryptArray = Encoding.UTF8.GetBytes(Str);
                var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(Key/*.EncodeToMD5()*/);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = aes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }
        /// <summary>
        /// AES解密
        /// </summary>
        public string AESDecrypt//(this string str, string encodingAesKey)
        {
            get
            {
                byte[] toEncryptArray = Convert.FromBase64String(Str);
                var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(Key/*.EncodeToMD5()*/);
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = aes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Encoding.UTF8.GetString(resultArray);
            }
        }

        /// <summary> 
        /// DES加密
        /// </summary> 
        public string DESEncrypt//(this string str, string key = "12345678")
        {
            get
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    var des = TripleDES.Create();
                    var inputByteArray = Encoding.UTF8.GetBytes(Str);
                    var bKey = Encoding.ASCII.GetBytes(Key/*.EncodeToMD5().Substring(0, 24)*/);
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
        }
        /// <summary> 
        /// DES解密 
        /// </summary> 
        public string DESDecrypt//(this string str, string key = "12345678")
        {
            get
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    var des = TripleDES.Create();
                    var len = Str.Length / 2;
                    byte[] inputByteArray = new byte[len];
                    int x;
                    for (x = 0; x < len; x++)
                    {
                        var i = Convert.ToInt32(Str.Substring(x * 2, 2), 16);
                        inputByteArray[x] = (byte)i;
                    }
                    var bKey = Encoding.ASCII.GetBytes(Key/*.EncodeToMD5().Substring(0, 24)*/);
                    des.Key = bKey;
                    des.IV = bKey.Take(8).ToArray();
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// RSA 加密
        /// </summary>
        public string RSAEncrypt//(this string content, string publickey)
        {
            get
            {
#if NET461
                var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(PublicKey ?? Key);
                var cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(Str), false);

                return Convert.ToBase64String(cipherbytes);
#else
                return NETCore.Encrypt.EncryptProvider.RSAEncrypt(PublicKey ?? Key, Str);
#endif
            }
        }

        /// <summary>
        /// RSA 解密
        /// </summary>
        public string RSADecrypt//(this string content, string privatekey)
        {
            get
            {
#if NET461
                var rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(Key);
                var cipherbytes = rsa.Decrypt(Convert.FromBase64String(Str), false);

                return Encoding.UTF8.GetString(cipherbytes);
#else
                return NETCore.Encrypt.EncryptProvider.RSADecrypt(Key, Str);
#endif
            }
        }


    }

}
