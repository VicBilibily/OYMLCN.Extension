using OYMLCN.ArgumentChecker;
using System;
using System.Globalization;
using System.Text;
#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class StringConvertExtension
    {
        #region public static byte[] GetEncodingBytes(this string str, Encoding encoding)
        /// <summary>
        /// 将指定字符串中的所有字符编码为一个字节序列
        /// </summary>
        /// <param name="str"> 字符串对象 </param>
        /// <param name="encoding"> 所要指定使用的字符编码方式 </param>
        /// <returns> 字符串中的所有字符按指定编码的一个字节序列 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str"/> 的字符串对象不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="encoding"/> 所指定的字符编码方式不能为 null </exception>
        public static byte[] GetEncodingBytes(this string str, Encoding encoding)
        {
            str.ThrowIfNull(nameof(str));
            encoding.ThrowIfNull(nameof(encoding), $"{nameof(encoding)} 所指定的字符编码方式不能为 null");
            return encoding.GetBytes(str);
        }
#if Xunit
        [Fact]
        public static void GetEncodingBytesTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.GetEncodingBytes(null));
            Assert.Throws<ArgumentNullException>(() => str.GetEncodingBytes(Encoding.Default));

            str = string.Empty;
            Assert.Empty(str.GetEncodingBytes(Encoding.Default));

            str = "Hello World!";
            Assert.NotEmpty(str.GetEncodingBytes(Encoding.Default));

            // 内置方法进行编码的各种方式，不属于本方法测试范围
        }
#endif
        #endregion

        #region public static string GetEncodingString(this byte[] bytes, Encoding encoding)
        /// <summary>
        /// 将指定字节数组中的所有字节按指定编码解码为一个字符串
        /// </summary>
        /// <param name="bytes"> 字节数组 </param>
        /// <param name="encoding"> 所要指定解码使用的字符编码方式 </param>
        /// <returns> 指定字节数组的所有字节按指定编码解码的一个字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="bytes"/> 的直接数组对象不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="encoding"/> 所指定的字符编码方式不能为 null </exception>
        public static string GetEncodingString(this byte[] bytes, Encoding encoding)
        {
            bytes.ThrowIfNull(nameof(bytes));
            encoding.ThrowIfNull(nameof(encoding), $"{nameof(encoding)} 所指定的字符编码方式不能为 null");
            return encoding.GetString(bytes);
        }
#if Xunit
        [Fact]
        public static void GetEncodingStringTest()
        {
            byte[] bytes = null;
            Assert.Throws<ArgumentNullException>(() => bytes.GetEncodingString(null));
            Assert.Throws<ArgumentNullException>(() => bytes.GetEncodingString(Encoding.Default));

            bytes = new byte[0];
            Assert.Equal(string.Empty, bytes.GetEncodingString(Encoding.Default));

            var str = "Hello World!";
            bytes = str.GetEncodingBytes(Encoding.Default);
            Assert.Equal(str, bytes.GetEncodingString(Encoding.Default));

            // 内置方法进行编码的各种方式，不属于本方法测试范围
        }
#endif
        #endregion


        #region public static byte[] GetUTF8Bytes(this string str)
        /// <summary>
        /// 将指定字符串中的所有字符以 UTF8 的格式编码为一个字节序列
        /// </summary>
        /// <param name="str"> 字符串对象 </param>
        /// <returns> 字符串中的所有字符按 UTF8 的格式编码的一个字节序列 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str"/> 的字符串对象不能为 null </exception>
        public static byte[] GetUTF8Bytes(this string str)
            => Encoding.UTF8.GetBytes(str);
#if Xunit
        [Fact]
        public static void GetUTF8BytesTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.GetUTF8Bytes());

            str = string.Empty;
            Assert.Empty(str.GetUTF8Bytes());

            str = "Hello World!";
            var bytes = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 33 };
            Assert.Equal(bytes, str.GetUTF8Bytes());
        }
#endif
        #endregion

        #region public static string GetUTF8String(this byte[] bytes)
        /// <summary>
        /// 将指定字节数组中的所有字节按 UTF8 的格式解码为一个字符串
        /// </summary>
        /// <param name="bytes"> 字节数组 </param>
        /// <returns> 指定字节数组的所有字节按 UTF8 的格式解码的一个字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="bytes"/> 的直接数组对象不能为 null </exception>
        public static string GetUTF8String(this byte[] bytes)
            => Encoding.UTF8.GetString(bytes);
#if Xunit
        [Fact]
        public static void GetUTF8StringTest()
        {
            byte[] bytes = null;
            Assert.Throws<ArgumentNullException>(() => bytes.GetUTF8String());

            bytes = new byte[0];
            Assert.Equal(string.Empty, bytes.GetUTF8String());

            bytes = new byte[] { 72, 101, 108, 108, 111, 32, 87, 111, 114, 108, 100, 33 };
            Assert.Equal("Hello World!", bytes.GetUTF8String());
        }
#endif
        #endregion


        #region public static byte[] HexToBytes(this string hex)
        /// <summary>
        /// 将16进制字符串转换成为字节数组
        /// </summary>
        /// <param name="hex"> 16进制字符串 </param>
        /// <returns> 字节数组 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="hex"/> 不能为 null </exception>
        /// <exception cref="FormatException"> <paramref name="hex"/> 不是16进制字符或格式不正确 </exception>
        public static byte[] HexToBytes(this string hex)
        {
            hex.ThrowIfNull(nameof(hex));

            if (hex.Length == 0)
                return new byte[] { 0 };
            if (hex.Length % 2 == 1)
                hex = "0" + hex;
            byte[] result = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length / 2; i++)
                result[i] = byte.Parse(hex.Substring(2 * i, 2), NumberStyles.AllowHexSpecifier);
            return result;
        }
#if Xunit
        [Fact]
        public static void HexToBytesTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.HexToBytes());

            str = string.Empty;
            Assert.Equal(new byte[] { 0 }, str.HexToBytes());

            str = "3031";
            Assert.Equal(new byte[] { 0x30, 0x31 }, str.HexToBytes());

            str = "Hello World!";
            Assert.Throws<FormatException>(() => str.HexToBytes());
        }
        [Fact]
        public static void HexWithUTF8Test()
        {
            string str = "Hello World!";
            var bytes = str.GetUTF8Bytes();
            var hexStr = bytes.HexToString();
            var hexBytes = hexStr.HexToBytes();
            Assert.Equal(bytes, hexBytes);
            hexStr = hexBytes.GetUTF8String();
            Assert.Equal(str, hexStr);
        }
#endif
        #endregion

        #region public static string HexToString(this byte[] bytes)
        /// <summary>
        /// 将字节数组转换成为16进制字符串
        /// </summary>
        /// <param name="bytes"> 字节数组 </param>
        /// <returns> 使用16进制表示的字符串 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="bytes"/> 不能为 null </exception>
        public static string HexToString(this byte[] bytes)
        {
            bytes.ThrowIfNull(nameof(bytes));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                sb.Append(bytes[i].ToString("X2"));
            return sb.ToString();
        }
#if Xunit
        [Fact]
        public static void HexToStringTest()
        {
            byte[] bytes = null;
            Assert.Throws<ArgumentNullException>(() => bytes.HexToString());

            bytes = new byte[0];
            Assert.Equal(string.Empty, bytes.HexToString());

            bytes = new byte[] { 0x30, 0x31 };
            Assert.Equal("3031", bytes.HexToString());
        }
#endif  
        #endregion

    }
}
