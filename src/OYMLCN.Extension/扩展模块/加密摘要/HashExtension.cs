using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    public static class EncryptHashExtension
    {
        internal static string ComputeHash<T>(this T encryptor, string str) where T : HashAlgorithm
        {
            var buffer = str.GetUTF8Bytes();
            byte[] resultHash = encryptor.ComputeHash(buffer);
            string hashString = BitConverter.ToString(resultHash).ToLower();
            hashString = hashString.Replace("-", "");
            return hashString;
        }
        internal static string ComputeHashBase64<T>(this T encryptor, string str) where T : HMAC
        {
            byte[] buffer = str.GetUTF8Bytes();
            byte[] resultHash = encryptor.ComputeHash(buffer);
            return resultHash.ConvertToBase64String();
        }


    }
}
