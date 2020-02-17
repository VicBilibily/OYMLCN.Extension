using OYMLCN.Encrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using OYMLCN.Extensions;
using OYMLCN.ArgumentChecker;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using OYMLCN.Exceptions;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// EncryptHelper (base on Lvcc NETCore.Encrypt 2.0.8)
    /// </summary>
    public static partial class EncryptHelper
    {
        /// <summary>
        /// 生成 RSA 公钥和私钥
        /// </summary>
        /// <param name="rsaSize">密钥长度</param>
        public static RSAKey CreateRsaKey(RsaSize rsaSize = RsaSize.R2048)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.KeySize = (int)rsaSize;
                string publicKey = rsa.ToJsonString(false);
                string privateKey = rsa.ToJsonString(true);
                return new RSAKey()
                {
                    PublicKey = publicKey,
                    PrivateKey = privateKey,
                    Exponent = rsa.ExportParameters(false).Exponent.HexToString(),
                    Modulus = rsa.ExportParameters(false).Modulus.HexToString()
                };
            }
        }
        /// <summary>
        /// 生成 RSA 公钥和私钥
        /// </summary>
        public static RSAKey CreateRsaKey(RSA rsa)
        {
            rsa.ThrowIfNull(nameof(rsa));

            string publicKey = rsa.ToJsonString(false);
            string privateKey = rsa.ToJsonString(true);
            return new RSAKey()
            {
                PublicKey = publicKey,
                PrivateKey = privateKey,
                Exponent = rsa.ExportParameters(false).Exponent.HexToString(),
                Modulus = rsa.ExportParameters(false).Modulus.HexToString()
            };
        }

        /// <summary>
        /// 从 JSON 加载 RSA 密钥
        /// </summary>
        /// <param name="rsaKey">rsa json string</param>
        /// <returns></returns>
        public static RSA LoadRSAFromJsonString(string rsaKey)
        {
            rsaKey.ThrowIfEmpty(nameof(rsaKey));
            RSA rsa = RSA.Create();
            rsa.FromJsonString(rsaKey);
            return rsa;
        }
        /// <summary>
        /// 从 PEM 加载 RSA 密钥
        /// </summary>
        /// <param name="pem"></param>
        /// <returns></returns>
        public static RSA RSAFromPem(string pem)
        {
            pem.ThrowIfEmpty(nameof(pem));
            return RsaProvider.FromPem(pem);
        }
        /// <summary>
        /// 创建 RSA 导出 PEM 密钥
        /// </summary>
        public static (string publicPem, string privatePem) RSAToPem(RSAKey rsaKey, bool isPKCS8)
        {
            rsaKey = rsaKey ?? CreateRsaKey();
            using (RSA rsa = RSA.Create())
            {
                rsa.FromJsonString(rsaKey.PrivateKey);
                var publicPem = RsaProvider.ToPem(rsa, false, isPKCS8);
                var privatePem = RsaProvider.ToPem(rsa, true, isPKCS8);
                return (publicPem, privatePem);
            }
        }

        #region InternalExtensions
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
                var paramsJson = jsonString.DeserializeJsonToObject<RSAParametersJson>();
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
        internal static void FromLvccXmlString(this RSA rsa, string xmlString)
        {
            RSAParameters parameters = new RSAParameters();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
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
            else
                throw new Exception("Invalid XML RSA key.");
            rsa.ImportParameters(parameters);
        }
        /// <summary>
        /// 获取RSA Key序列化XML
        /// </summary>
        /// <param name="rsa">RSA实例<see cref="RSA"/></param>
        /// <param name="includePrivateParameters">是否包含私钥</param>
        /// <returns></returns>
        internal static string ToLvccXmlString(this RSA rsa, bool includePrivateParameters)
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
        /// <summary>
        /// 数组子数据操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        internal static T[] Sub<T>(this T[] arr, int start, int count)
        {
            T[] val = new T[count];
            for (var i = 0; i < count; i++)
                val[i] = arr[start + i];
            return val;
        }
        #endregion
        #region RSA Provider
        /// <summary>
        /// RSA provider
        /// https://github.com/xiangyuecn/RSA-csharp
        /// </summary>
        internal class RsaProvider
        {
            static Regex _PEMCode = new Regex(@"--+.+?--+|\s+");
            static byte[] _SeqOID = new byte[] { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            static byte[] _Ver = new byte[] { 0x02, 0x01, 0x00 };

            /// <summary>
            /// Convert pem to rsa，support PKCS#1、PKCS#8 
            /// </summary>
            internal static RSA FromPem(string pem)
            {
                //var rsaParams = new CspParameters();
                //rsaParams.Flags = CspProviderFlags.UseMachineKeyStore;
                //var rsa = new RSACryptoServiceProvider(rsaParams);

                var rsa = RSA.Create();
                var param = new RSAParameters();

                var base64 = _PEMCode.Replace(pem, "");
                var data = Convert.FromBase64String(base64);
                if (data == null)
                    throw new Exception("Pem content invalid ");
                var idx = 0;
                //read  length
                Func<byte, int> readLen = (first) =>
                {
                    if (data[idx] == first)
                    {
                        idx++;
                        if (data[idx] == 0x81)
                        {
                            idx++;
                            return data[idx++];
                        }
                        else if (data[idx] == 0x82)
                        {
                            idx++;
                            return (((int)data[idx++]) << 8) + data[idx++];
                        }
                        else if (data[idx] < 0x80)
                            return data[idx++];
                    }
                    throw new Exception("Not found any content in pem file");
                };
                //read module length
                Func<byte[]> readBlock = () =>
                {
                    var len = readLen(0x02);
                    if (data[idx] == 0x00)
                    {
                        idx++;
                        len--;
                    }
                    var val = data.Sub(idx, len);
                    idx += len;
                    return val;
                };

                Func<byte[], bool> eq = (byts) =>
                {
                    for (var i = 0; i < byts.Length; i++, idx++)
                    {
                        if (idx >= data.Length)
                            return false;
                        if (byts[i] != data[idx])
                            return false;
                    }
                    return true;
                };

                if (pem.Contains("PUBLIC KEY"))
                {
                    /****Use public key****/
                    readLen(0x30);
                    if (!eq(_SeqOID))
                        throw new Exception("Unknown pem format");
                    readLen(0x03);
                    idx++;
                    readLen(0x30);
                    //Modulus
                    param.Modulus = readBlock();
                    //Exponent
                    param.Exponent = readBlock();
                }
                else if (pem.Contains("PRIVATE KEY"))
                {
                    /****Use private key****/
                    readLen(0x30);
                    //Read version
                    if (!eq(_Ver))
                        throw new Exception("Unknown pem version");
                    //Check PKCS8
                    var idx2 = idx;
                    if (eq(_SeqOID))
                    {
                        //Read one byte
                        readLen(0x04);
                        readLen(0x30);
                        //Read version
                        if (!eq(_Ver))
                            throw new Exception("Pem version invalid");
                    }
                    else
                        idx = idx2;
                    //Reda data
                    param.Modulus = readBlock();
                    param.Exponent = readBlock();
                    param.D = readBlock();
                    param.P = readBlock();
                    param.Q = readBlock();
                    param.DP = readBlock();
                    param.DQ = readBlock();
                    param.InverseQ = readBlock();
                }
                else
                    throw new Exception("pem need 'BEGIN' and  'END'");
                rsa.ImportParameters(param);
                return rsa;
            }

            /// <summary>
            /// Converter Rsa to pem ,
            /// </summary>
            /// <param name="rsa"><see cref="RSACryptoServiceProvider"/></param>
            /// <param name="includePrivateParameters">if false only return publick key</param>
            /// <param name="isPKCS8">default is false,if true return PKCS#8 pem else return PKCS#1 pem </param>
            /// <returns></returns>
            internal static string ToPem(RSA rsa, bool includePrivateParameters, bool isPKCS8 = false)
            {
                var ms = new MemoryStream();
                Action<int> writeLenByte = (len) =>
                {
                    if (len < 0x80)
                        ms.WriteByte((byte)len);
                    else if (len <= 0xff)
                    {
                        ms.WriteByte(0x81);
                        ms.WriteByte((byte)len);
                    }
                    else
                    {
                        ms.WriteByte(0x82);
                        ms.WriteByte((byte)(len >> 8 & 0xff));
                        ms.WriteByte((byte)(len & 0xff));
                    }
                };
                //write moudle data
                Action<byte[]> writeBlock = (byts) =>
                {
                    var addZero = (byts[0] >> 4) >= 0x8;
                    ms.WriteByte(0x02);
                    var len = byts.Length + (addZero ? 1 : 0);
                    writeLenByte(len);
                    if (addZero)
                        ms.WriteByte(0x00);
                    ms.Write(byts, 0, byts.Length);
                };
                Func<int, byte[], byte[]> writeLen = (index, byts) =>
                {
                    var len = byts.Length - index;
                    ms.SetLength(0);
                    ms.Write(byts, 0, index);
                    writeLenByte(len);
                    ms.Write(byts, index, len);
                    return ms.ToArray();
                };
                if (!includePrivateParameters)
                {
                    /****Create public key****/
                    var param = rsa.ExportParameters(false);
                    ms.WriteByte(0x30);
                    var index1 = (int)ms.Length;
                    // Encoded OID sequence for PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
                    ms.WriteAll(_SeqOID);
                    //Start with 0x00 
                    ms.WriteByte(0x03);
                    var index2 = (int)ms.Length;
                    ms.WriteByte(0x00);
                    //Content length
                    ms.WriteByte(0x30);
                    var index3 = (int)ms.Length;
                    //Write Modulus
                    writeBlock(param.Modulus);
                    //Write Exponent
                    writeBlock(param.Exponent);
                    var bytes = ms.ToArray();
                    bytes = writeLen(index3, bytes);
                    bytes = writeLen(index2, bytes);
                    bytes = writeLen(index1, bytes);
                    return "-----BEGIN PUBLIC KEY-----\n" + TextBreak(Convert.ToBase64String(bytes), 64) + "\n-----END PUBLIC KEY-----";
                }
                else
                {
                    /****Create private key****/
                    var param = rsa.ExportParameters(true);
                    //Write total length
                    ms.WriteByte(0x30);
                    int index1 = (int)ms.Length;
                    //Write version
                    ms.WriteAll(_Ver);
                    //PKCS8 
                    int index2 = -1, index3 = -1;
                    if (isPKCS8)
                    {
                        ms.WriteAll(_SeqOID);
                        ms.WriteByte(0x04);
                        index2 = (int)ms.Length;
                        ms.WriteByte(0x30);
                        index3 = (int)ms.Length;
                        ms.WriteAll(_Ver);
                    }
                    //Write data
                    writeBlock(param.Modulus);
                    writeBlock(param.Exponent);
                    writeBlock(param.D);
                    writeBlock(param.P);
                    writeBlock(param.Q);
                    writeBlock(param.DP);
                    writeBlock(param.DQ);
                    writeBlock(param.InverseQ);
                    var bytes = ms.ToArray();
                    if (index2 != -1)
                    {
                        bytes = writeLen(index3, bytes);
                        bytes = writeLen(index2, bytes);
                    }
                    bytes = writeLen(index1, bytes);
                    var flag = " PRIVATE KEY";
                    if (!isPKCS8)
                        flag = " RSA" + flag;
                    return "-----BEGIN" + flag + "-----\n" + TextBreak(Convert.ToBase64String(bytes), 64) + "\n-----END" + flag + "-----";
                }
            }
            /// <summary>
            /// Text break method
            /// </summary>
            private static string TextBreak(string text, int line)
            {
                var idx = 0;
                var len = text.Length;
                var str = new StringBuilder();
                while (idx < len)
                {
                    if (idx > 0)
                        str.Append('\n');
                    if (idx + line >= len)
                        str.Append(text.Substring(idx));
                    else
                        str.Append(text.Substring(idx, line));
                    idx += line;
                }
                return str.ToString();
            }
        }
        #endregion

        /// <summary>
        /// 计算 RSA 签名
        /// </summary>
        public static string SignAsRSA(string content, string privateKey, HashAlgorithmName hashAlgorithmName, RSASignaturePadding rSASignaturePadding, Encoding encoding)
        {
            content.ThrowIfEmpty(nameof(content));
            privateKey.ThrowIfEmpty(nameof(privateKey));
            rSASignaturePadding.ThrowIfNull(nameof(rSASignaturePadding));

            byte[] dataBytes = encoding.GetBytes(content);
            using (RSA rsa = RSA.Create())
            {
                rsa.FromJsonString(privateKey);
                var signBytes = rsa.SignData(dataBytes, hashAlgorithmName, rSASignaturePadding);
                return Convert.ToBase64String(signBytes);
            }
        }
        /// <summary>
        /// 检查 RSA 签名的有效性
        /// </summary>
        public static bool VerifyRSASign(string content, string signStr, string publickKey, HashAlgorithmName hashAlgorithmName, RSASignaturePadding rSASignaturePadding, Encoding encoding)
        {
            content.ThrowIfEmpty(nameof(content));
            signStr.ThrowIfEmpty(nameof(signStr));

            byte[] dataBytes = encoding.GetBytes(content);
            byte[] signBytes = Convert.FromBase64String(signStr);
            using (RSA rsa = RSA.Create())
            {
                rsa.FromJsonString(publickKey);
                return rsa.VerifyData(dataBytes, signBytes, hashAlgorithmName, rSASignaturePadding);
            }
        }
        /// <summary>
        /// RSA 加密 
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="srcString">明文字符串</param>
        /// <param name="padding">rsa 加密填充模式 <see cref="RSAEncryptionPadding"/> 在 linux/mac openssl 时为 RSAEncryptionPadding.Pkcs1 </param>
        /// <param name="isPemKey">PEM密钥(false)</param>
        /// <returns>encrypted string</returns>
        public static string RSAEncrypt(string publicKey, string srcString, RSAEncryptionPadding padding, bool isPemKey = false)
        {
            publicKey.ThrowIfEmpty(nameof(publicKey));
            srcString.ThrowIfEmpty(nameof(srcString));
            padding.ThrowIfNull(nameof(padding));

            RSA rsa;
            if (isPemKey)
                rsa = RsaProvider.FromPem(publicKey);
            else
            {
                rsa = RSA.Create();
                rsa.FromJsonString(publicKey);
            }

            using (rsa)
            {
                var maxLength = GetMaxRsaEncryptLength(rsa, padding);
                var rawBytes = Encoding.UTF8.GetBytes(srcString);

                if (rawBytes.Length > maxLength)
                    throw new EncryptOutofMaxlengthException($"'{srcString}' is out of max encrypt length {maxLength}", maxLength, rsa.KeySize, padding);

                byte[] encryptBytes = rsa.Encrypt(rawBytes, padding);
                return encryptBytes.HexToString();
            }
        }
        /// <summary>
        /// RSA 解密 
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="srcString">密文字符串</param>
        /// <param name="padding">rsa 加密填充模式 <see cref="RSAEncryptionPadding"/> 在 linux/mac openssl 时为 RSAEncryptionPadding.Pkcs1 </param>
        /// <param name="isPemKey">PEM密钥(false)</param>
        /// <returns>encrypted string</returns>
        public static string RSADecrypt(string privateKey, string srcString, RSAEncryptionPadding padding, bool isPemKey = false)
        {
            privateKey.ThrowIfEmpty(nameof(privateKey));
            srcString.ThrowIfEmpty(nameof(srcString));
            padding.ThrowIfNull(nameof(padding));

            RSA rsa;
            if (isPemKey)
                rsa = RsaProvider.FromPem(privateKey);
            else
            {
                rsa = RSA.Create();
                rsa.FromJsonString(privateKey);
            }

            using (rsa)
            {
                byte[] srcBytes = srcString.GetUTF8Bytes();
                byte[] decryptBytes = rsa.Decrypt(srcBytes, padding);
                return Encoding.UTF8.GetString(decryptBytes);
            }
        }

        private static int GetMaxRsaEncryptLength(RSA rsa, RSAEncryptionPadding padding)
        {
            var offset = 0;
            if (padding.Mode == RSAEncryptionPaddingMode.Pkcs1)
                offset = 11;
            else
            {
                if (padding.Equals(RSAEncryptionPadding.OaepSHA1))
                    offset = 42;
                if (padding.Equals(RSAEncryptionPadding.OaepSHA256))
                    offset = 66;
                if (padding.Equals(RSAEncryptionPadding.OaepSHA384))
                    offset = 98;
                if (padding.Equals(RSAEncryptionPadding.OaepSHA512))
                    offset = 130;
            }
            var keySize = rsa.KeySize;
            var maxLength = keySize / 8 - offset;
            return maxLength;
        }

    }
}
