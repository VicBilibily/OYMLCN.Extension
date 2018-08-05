#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Xml;
using System.IO;
using OYMLCN.Extensions;

// base on https://github.com/myloveCc/NETCore.Encrypt v2.0.3
namespace OYMLCN.ThirdParty.Cryptography
{
    internal static class BytesAndStringExtensions
    {
        /// <summary>
        /// byte to hex string extension
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        internal static string ToHexString(this byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                sb.Append(bytes[i].ToString("X2"));
            return sb.ToString();
        }

        /// <summary>
        /// hex string to byte extension
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        internal static byte[] ToBytes(this string hex)
        {
            if (hex.Length == 0)
                return new byte[] { 0 };
            if (hex.Length % 2 == 1)
                hex = "0" + hex;
            byte[] result = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length / 2; i++)
                result[i] = byte.Parse(hex.Substring(2 * i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            return result;
        }
    }

    #region Model
    /// <summary>
    /// AESKey
    /// </summary>
    public class AESKey
    {
        /// <summary>
        /// ase key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// ase IV
        /// </summary>
        public string IV { get; set; }
    }
    public enum MD5Length
    {
        L16 = 16,
        L32 = 32
    }
    public class RSAKey
    {
        /// <summary>
        /// Rsa public key
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// Rsa private key
        /// </summary>
        public string PrivateKey { get; set; }

        /// <summary>
        /// Rsa public key Exponent
        /// </summary>
        public string Exponent { get; set; }

        /// <summary>
        /// Rsa public key Modulus
        /// </summary>
        public string Modulus { get; set; }
    }
    public enum RsaKeyType
    {
        XML,
        JSON
    }
    internal class RSAParametersJson
    {
        //Public key Modulus
        public string Modulus { get; set; }
        //Public key Exponent
        public string Exponent { get; set; }
        public string P { get; set; }
        public string Q { get; set; }
        public string DP { get; set; }
        public string DQ { get; set; }
        public string InverseQ { get; set; }
        public string D { get; set; }
    }
    public enum RsaSize
    {
        R2048 = 2048,
        R3072 = 3072,
        R4096 = 4096
    }
    #endregion

    /// <summary>
    /// RSA参数格式化扩展
    /// </summary>
    internal static class RSAKeyExtensions
    {
        internal static void ImportKey(this RSA rsa, string key, RsaKeyType rsaKeyType)
        {
            switch (rsaKeyType)
            {
                case RsaKeyType.JSON:
                    rsa.FromJsonString(key);
                    break;
                case RsaKeyType.XML:
#if NET461
                    rsa.FromXmlString(key);
#else
                    rsa.FromLvccXmlString(key);
#endif
                    break;
            }
        }

        #region JSON
        /// <summary>
        /// RSA导入key
        /// </summary>
        /// <param name="rsa">RSA实例<see cref="RSA"/></param>
        /// <param name="jsonString">RSA的Key序列化JSON字符串</param>
        internal static void FromJsonString(this RSA rsa, string jsonString)
        {
            RSAParameters parameters = new RSAParameters();
            try
            {
                var paramsJson = jsonString.AsJsonHandler().DeserializeToObject<RSAParametersJson>();

                parameters.Modulus = paramsJson.Modulus != null ? Convert.FromBase64String(paramsJson.Modulus) : null;
                parameters.Exponent = paramsJson.Exponent != null ? Convert.FromBase64String(paramsJson.Exponent) : null;
                parameters.P = paramsJson.P != null ? Convert.FromBase64String(paramsJson.P) : null;
                parameters.Q = paramsJson.Q != null ? Convert.FromBase64String(paramsJson.Q) : null;
                parameters.DP = paramsJson.DP != null ? Convert.FromBase64String(paramsJson.DP) : null;
                parameters.DQ = paramsJson.DQ != null ? Convert.FromBase64String(paramsJson.DQ) : null;
                parameters.InverseQ = paramsJson.InverseQ != null ? Convert.FromBase64String(paramsJson.InverseQ) : null;
                parameters.D = paramsJson.D != null ? Convert.FromBase64String(paramsJson.D) : null;
            }
            catch
            {
                throw new Exception("Invalid Json RSA key.");
            }
            rsa.ImportParameters(parameters);
        }

        /// <summary>
        /// 获取RSA Key序列化Json
        /// </summary>
        /// <param name="rsa">RSA实例<see cref="RSA"/></param>
        /// <param name="includePrivateParameters">是否包含私钥</param>
        /// <returns></returns>
        internal static string ToJsonString(this RSA rsa, bool includePrivateParameters)
        {
            RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

            var parasJson = new RSAParametersJson()
            {
                Modulus = parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
                Exponent = parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
                P = parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
                Q = parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
                DP = parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
                DQ = parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
                InverseQ = parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
                D = parameters.D != null ? Convert.ToBase64String(parameters.D) : null
            };

            return parasJson.ToJsonString();
        }
        #endregion

        #region XML

        /// <summary>
        /// RSA导入key
        /// </summary>
        /// <param name="rsa">RSA实例<see cref="RSA"/></param>
        /// <param name="xmlString">RSA的Key序列化XML字符串</param>
        public static void FromLvccXmlString(this RSA rsa, string xmlString)
        {
            RSAParameters parameters = new RSAParameters();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Exponent": parameters.Exponent = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "P": parameters.P = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Q": parameters.Q = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DP": parameters.DP = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DQ": parameters.DQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "InverseQ": parameters.InverseQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "D": parameters.D = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
        }

        /// <summary>
        /// 获取RSA Key序列化XML
        /// </summary>
        /// <param name="rsa">RSA实例<see cref="RSA"/></param>
        /// <param name="includePrivateParameters">是否包含私钥</param>
        /// <returns></returns>
        public static string ToLvccXmlString(this RSA rsa, bool includePrivateParameters)
        {
            RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                  parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
                  parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
                  parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
                  parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
                  parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
                  parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
                  parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
                  parameters.D != null ? Convert.ToBase64String(parameters.D) : null);
        }

        #endregion
    }


    public class EncryptProvider
    {
        #region Common
        /// <summary>
        /// Generate a random key
        /// </summary>
        /// <param name="length">key length，IV is 16，Key is 32</param>
        /// <returns>return random value</returns>
        private static string GetRandomStr(int length)
        {
            char[] arrChar = new char[]{
                'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
                '0','1','2','3','4','5','6','7','8','9',
                'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
            };

            StringBuilder num = new StringBuilder();

            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < length; i++)
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());

            return num.ToString();
        }
        #endregion

        #region AES
        /// <summary>
        /// Create ase key
        /// </summary>
        /// <returns></returns>
        public static AESKey CreateAesKey() =>
            new AESKey()
            {
                Key = GetRandomStr(32),
                IV = GetRandomStr(16)
            };

        /// <summary>  
        /// AES encrypt
        /// </summary>  
        /// <param name="data">Raw data</param>  
        /// <param name="key">Key, requires 32 bits</param>  
        /// <param name="vector">IV,requires 16 bits</param>  
        /// <returns>Encrypted string</returns>  
        public static string AESEncrypt(string data, string key, string vector)
        {
            Byte[] plainBytes = Encoding.UTF8.GetBytes(data);
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            Byte[] bVector = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);

            Byte[] Cryptograph = null; // encrypted data
            using (Aes Aes = Aes.Create())
            {
                try
                {
                    using (MemoryStream Memory = new MemoryStream())
                    using (CryptoStream Encryptor = new CryptoStream(Memory,
                        Aes.CreateEncryptor(bKey, bVector),
                        CryptoStreamMode.Write))
                    {
                        Encryptor.Write(plainBytes, 0, plainBytes.Length);
                        Encryptor.FlushFinalBlock();

                        Cryptograph = Memory.ToArray();
                    }
                }
                catch
                {
                    Cryptograph = null;
                }
                return Convert.ToBase64String(Cryptograph);
            }
        }

        /// <summary>  
        ///  AES decrypt
        /// </summary>  
        /// <param name="data">Encrypted data</param>  
        /// <param name="key">Key, requires 32 bits</param>  
        /// <param name="vector">IV,requires 16 bits</param>  
        /// <returns>Decrypted string</returns>  
        public static string AESDecrypt(string data, string key, string vector)
        {
            Byte[] encryptedBytes = Convert.FromBase64String(data);
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            Byte[] bVector = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);

            Byte[] original = null; // decrypted data

            using (Aes Aes = Aes.Create())
            {
                try
                {
                    using (MemoryStream Memory = new MemoryStream(encryptedBytes))
                    using (CryptoStream Decryptor = new CryptoStream(Memory, Aes.CreateDecryptor(bKey, bVector), CryptoStreamMode.Read))
                    using (MemoryStream originalMemory = new MemoryStream())
                    {
                        Byte[] Buffer = new Byte[1024];
                        Int32 readBytes = 0;
                        while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                            originalMemory.Write(Buffer, 0, readBytes);

                        original = originalMemory.ToArray();
                    }
                }
                catch
                {
                    original = null;
                }
                return Encoding.UTF8.GetString(original);
            }
        }

        /// <summary>  
        /// AES encrypt ( no IV)  
        /// </summary>  
        /// <param name="data">Raw data</param>  
        /// <param name="key">Key, requires 32 bits</param>  
        /// <returns>Encrypted string</returns>  
        public static string AESEncrypt(string data, string key)
        {
            using (MemoryStream mStream = new MemoryStream())
            using (Aes aes = Aes.Create())
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(data);
                Byte[] bKey = new Byte[32];
                Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 128;
                aes.Key = bKey;
                using (CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    try
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(mStream.ToArray());
                    }
                    catch //(Exception ex)
                    {
                        return null;
                    }
            }
        }

        /// <summary>  
        /// AES decrypt( no IV)  
        /// </summary>  
        /// <param name="data">Encrypted data</param>  
        /// <param name="key">Key, requires 32 bits</param>  
        /// <returns>Decrypted string</returns>  
        public static string AESDecrypt(string data, string key)
        {
            Byte[] encryptedBytes = Convert.FromBase64String(data);
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

            using (MemoryStream mStream = new MemoryStream(encryptedBytes))
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 128;
                aes.Key = bKey;
                //aes.IV = _iV;  
                using (CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    try
                    {
                        byte[] tmp = new byte[encryptedBytes.Length];
                        int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length);
                        byte[] ret = new byte[len];
                        Array.Copy(tmp, 0, ret, 0, len);
                        return Encoding.UTF8.GetString(ret);
                    }
                    catch //(Exception ex)
                    {
                        return null;
                    }
            }
        }
        #endregion

        #region DES

        /// <summary>
        /// Create des key
        /// </summary>
        /// <returns></returns>
        public static string CreateDesKey() => GetRandomStr(24);

        /// <summary>  
        /// DES encrypt
        /// </summary>  
        /// <param name="data">Raw data</param>  
        /// <param name="key">Key, requires 24 bits</param>  
        /// <returns>Encrypted string</returns>  
        public static string DESEncrypt(string data, string key)
        {
            using (MemoryStream mStream = new MemoryStream())
            using (TripleDES des = TripleDES.Create())
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(data);
                Byte[] bKey = new Byte[24];
                Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;
                des.Key = bKey;
                using (CryptoStream cryptoStream = new CryptoStream(mStream, des.CreateEncryptor(), CryptoStreamMode.Write))
                    try
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(mStream.ToArray());
                    }
                    catch //(Exception ex)
                    {
                        return null;
                    }
            }
        }

        /// <summary>  
        /// DES decrypt
        /// </summary>  
        /// <param name="data">Encrypted data</param>  
        /// <param name="key">Key, requires 24 bits</param>  
        /// <returns>Decrypted string</returns>  
        public static string DESDecrypt(string data, string key)
        {
            Byte[] encryptedBytes = Convert.FromBase64String(data);
            Byte[] bKey = new Byte[24];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

            using (MemoryStream mStream = new MemoryStream(encryptedBytes))
            using (TripleDES des = TripleDES.Create())
            {
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.PKCS7;
                des.Key = bKey;
                using (CryptoStream cryptoStream = new CryptoStream(mStream, des.CreateDecryptor(), CryptoStreamMode.Read))
                    try
                    {
                        byte[] tmp = new byte[encryptedBytes.Length];
                        int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length);
                        byte[] ret = new byte[len];
                        Array.Copy(tmp, 0, ret, 0, len);
                        return Encoding.UTF8.GetString(ret);
                    }
                    catch
                    {
                        return null;
                    }
            }
        }

        #endregion

        #region RSA


        /// <summary>
        /// RSA encrypt 
        /// </summary>
        /// <param name="publicKey">public key</param>
        /// <param name="srcString">src string</param>
        /// <param name="rsaKeyType">key serialization format</param>
        /// <param name="padding">default value is Pkcs1</param>
        /// <returns>encrypted string</returns>
        public static string RSAEncrypt(string publicKey, string srcString, RsaKeyType rsaKeyType = RsaKeyType.JSON, RSAEncryptionPadding padding = null)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportKey(publicKey, rsaKeyType);
                byte[] encryptBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(srcString), padding ?? RSAEncryptionPadding.Pkcs1);
                return encryptBytes.ToHexString();
            }
        }
        /// <summary>
        /// RSA decrypt
        /// </summary>
        /// <param name="privateKey">private key</param>
        /// <param name="srcString">encrypted string</param>
        /// <param name="rsaKeyType">key serialization format</param>
        /// <param name="padding">default value is Pkcs1</param>
        /// <returns>Decrypted string</returns>
        public static string RSADecrypt(string privateKey, string srcString, RsaKeyType rsaKeyType = RsaKeyType.JSON, RSAEncryptionPadding padding = null)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportKey(privateKey, rsaKeyType);
                byte[] srcBytes = srcString.ToBytes();
                byte[] decryptBytes = rsa.Decrypt(srcBytes, padding ?? RSAEncryptionPadding.Pkcs1);
                return Encoding.UTF8.GetString(decryptBytes);
            }
        }

        /// <summary>
        /// RSA from json string
        /// </summary>
        /// <param name="rsaKey">rsa json string</param>
        /// <param name="rsaKeyType">key serialization format</param>
        /// <returns></returns>
        public static RSA RSAFromString(string rsaKey, RsaKeyType rsaKeyType = RsaKeyType.JSON)
        {
            RSA rsa = RSA.Create();
            rsa.ImportKey(rsaKey, rsaKeyType);
            return rsa;
        }

        /// <summary>
        /// Create an RSA key 
        /// </summary>
        /// <param name="rsaSize">the default size is 2048</param>
        /// <param name="rsaKeyType">key serialization format</param>
        /// <returns></returns>
        public static RSAKey CreateRsaKey(RsaSize rsaSize = RsaSize.R2048, RsaKeyType rsaKeyType = RsaKeyType.JSON)
        {
            //#if NET461
            using (var rsa = new RSACryptoServiceProvider((int)rsaSize))
            //#else // only netcoreapp2.0
            //using (RSA rsa = RSA.Create((int)rsaSize))
            //#endif
            {
                //string publicKey = rsa.ToJsonString(false);
                //string privateKey = rsa.ToJsonString(true);
                string publicKey = string.Empty, privateKey = string.Empty;
                switch (rsaKeyType)
                {
                    case RsaKeyType.JSON:
                        publicKey = rsa.ToJsonString(false);
                        privateKey = rsa.ToJsonString(true);
                        break;
                    case RsaKeyType.XML:
#if NET461
                        publicKey = rsa.ToXmlString(false);
                        privateKey = rsa.ToXmlString(true);
#else
                        publicKey = rsa.ToLvccXmlString(false);
                        privateKey = rsa.ToLvccXmlString(true);
#endif
                        break;
                }

                return new RSAKey()
                {
                    PublicKey = publicKey,
                    PrivateKey = privateKey,
                    Exponent = rsa.ExportParameters(false).Exponent.ToHexString(),
                    Modulus = rsa.ExportParameters(false).Modulus.ToHexString()
                };
            }
        }
        #endregion

        #region MD5
        /// <summary>
        /// MD5 hash
        /// </summary>
        /// <param name="srcString">The string to be encrypted.</param>
        /// <param name="length">The length of hash result , default value is <see cref="MD5Length.L32"/>.</param>
        /// <returns></returns>
        public static string Md5(string srcString, MD5Length length = MD5Length.L32)
        {
            string str_md5_out = string.Empty;
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes_md5_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_md5_out = md5.ComputeHash(bytes_md5_in);

                str_md5_out = length == MD5Length.L32
                    ? BitConverter.ToString(bytes_md5_out)
                    : BitConverter.ToString(bytes_md5_out, 4, 8);

                str_md5_out = str_md5_out.Replace("-", "");
                return str_md5_out;
            }
        }
        #endregion

        #region HMACMD5
        /// <summary>
        /// HMACMD5 hash
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <param name="key">encrypte key</param>
        /// <returns></returns>
        public static string HMACMD5(string srcString, string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACMD5 md5 = new HMACMD5(secrectKey))
            {
                byte[] bytes_md5_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_md5_out = md5.ComputeHash(bytes_md5_in);
                string str_md5_out = BitConverter.ToString(bytes_md5_out);
                str_md5_out = str_md5_out.Replace("-", "");
                return str_md5_out;
            }
        }

        #endregion

        #region SHA1
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="str">The string to be encrypted</param>
        /// <returns></returns>
        public static string Sha1(string str)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] bytes_sha1_in = Encoding.UTF8.GetBytes(str);
                byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
                string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
                str_sha1_out = str_sha1_out.Replace("-", "");
                return str_sha1_out;
            }
        }
        #endregion

        #region SHA256

        /// <summary>
        /// SHA256 encrypt
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <returns></returns>
        public static string Sha256(string srcString)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes_sha256_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_sha256_out = sha256.ComputeHash(bytes_sha256_in);
                string str_sha256_out = BitConverter.ToString(bytes_sha256_out);
                str_sha256_out = str_sha256_out.Replace("-", "");
                return str_sha256_out;
            }
        }

        #endregion

        #region SHA384

        /// <summary>
        /// SHA384 encrypt
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <returns></returns>
        public static string Sha384(string srcString)
        {
            using (SHA384 sha384 = SHA384.Create())
            {
                byte[] bytes_sha384_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_sha384_out = sha384.ComputeHash(bytes_sha384_in);
                string str_sha384_out = BitConverter.ToString(bytes_sha384_out);
                str_sha384_out = str_sha384_out.Replace("-", "");
                return str_sha384_out;
            }

        }
        #endregion

        #region SHA512
        /// <summary>
        /// SHA512 encrypt
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <returns></returns>
        public static string Sha512(string srcString)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes_sha512_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_sha512_out = sha512.ComputeHash(bytes_sha512_in);
                string str_sha512_out = BitConverter.ToString(bytes_sha512_out);
                str_sha512_out = str_sha512_out.Replace("-", "");
                return str_sha512_out;
            }
        }

        #endregion

        #region HMACSHA1

        /// <summary>
        /// HMAC_SHA1
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <param name="key">encrypte key</param>
        /// <returns></returns>
        public static string HMACSHA1(string srcString, string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACSHA1 hmac = new HMACSHA1(secrectKey))
            {
                hmac.Initialize();

                byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);

                string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
                str_hamc_out = str_hamc_out.Replace("-", "");

                return str_hamc_out;
            }
        }

        #endregion

        #region HMACSHA256

        /// <summary>
        /// HMAC_SHA256 
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <param name="key">encrypte key</param>
        /// <returns></returns>
        public static string HMACSHA256(string srcString, string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACSHA256 hmac = new HMACSHA256(secrectKey))
            {
                hmac.Initialize();

                byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);

                string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
                str_hamc_out = str_hamc_out.Replace("-", "");

                return str_hamc_out;
            }
        }

        #endregion

        #region HMACSHA384

        /// <summary>
        /// HMAC_SHA384
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <param name="key">encrypte key</param>
        /// <returns></returns>
        public static string HMACSHA384(string srcString, string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACSHA384 hmac = new HMACSHA384(secrectKey))
            {
                hmac.Initialize();

                byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);


                string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
                str_hamc_out = str_hamc_out.Replace("-", "");

                return str_hamc_out;
            }
        }

        #endregion

        #region HMACSHA512

        /// <summary>
        /// HMAC_SHA512
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <param name="key">encrypte key</param>
        /// <returns></returns>
        public static string HMACSHA512(string srcString, string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACSHA512 hmac = new HMACSHA512(secrectKey))
            {
                hmac.Initialize();

                byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);

                string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
                str_hamc_out = str_hamc_out.Replace("-", "");

                return str_hamc_out;
            }
        }

        #endregion

        #region Machine Key

        /// <summary>
        /// Create decryptionKey
        /// </summary>
        /// <param name="length">decryption key length range is 16 - 48</param>
        /// <returns>DecryptionKey</returns>
        public static string CreateDecryptionKey(int length) => CreateMachineKey(length);

        /// <summary>
        /// Create validationKey
        /// </summary>
        /// <param name="length"></param>
        /// <returns>ValidationKey</returns>
        public static string CreateValidationKey(int length) => CreateMachineKey(length);

        /// <summary>
        /// 使用加密服务提供程序实现加密生成随机数
        /// 
        /// 说明：
        /// validationKey 的值可以是48到128个字符长，强烈建议使用可用的最长密钥
        /// decryptionKey 的值可以是16到48字符长，建议使用48字符长
        /// 
        /// 使用方式：
        /// string decryptionKey = EncryptManager.CreateMachineKey(48);
        /// string validationKey = EncryptManager.CreateMachineKey(128);
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        private static string CreateMachineKey(int length)
        {
            byte[] random = new byte[length / 2];

            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(random);

            StringBuilder machineKey = new StringBuilder(length);
            for (int i = 0; i < random.Length; i++)
                machineKey.Append(string.Format("{0:X2}", random[i]));
            return machineKey.ToString();
        }
        #endregion

        #region Base64

        #region Base64加密解密
        /// <summary>
        /// Base64 encrypt
        /// </summary>
        /// <param name="input">input value</param>
        /// <returns></returns>
        public static string Base64Encrypt(string input) =>
            Base64Encrypt(input, Encoding.UTF8);

        /// <summary>
        /// Base64 encrypt
        /// </summary>
        /// <param name="input">input value</param>
        /// <param name="encoding">text encoding</param>
        /// <returns></returns>
        public static string Base64Encrypt(string input, Encoding encoding) =>
            Convert.ToBase64String(encoding.GetBytes(input));

        /// <summary>
        /// Base64 decrypt
        /// </summary>
        /// <param name="input">input value</param>
        /// <returns></returns>
        public static string Base64Decrypt(string input) =>
            Base64Decrypt(input, Encoding.UTF8);

        /// <summary>
        /// Base64 decrypt
        /// </summary>
        /// <param name="input">input value</param>
        /// <param name="encoding">text encoding</param>
        /// <returns></returns>
        public static string Base64Decrypt(string input, Encoding encoding) =>
             encoding.GetString(Convert.FromBase64String(input));
        #endregion

        #endregion
    }

}
