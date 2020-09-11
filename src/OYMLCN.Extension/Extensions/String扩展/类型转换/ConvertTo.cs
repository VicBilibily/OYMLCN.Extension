using System;
using System.ComponentModel;

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
        #region public static T ConvertTo<T>(this string input) where T : struct
        /// <summary>
        /// string转换为值类型
        /// </summary>
        /// <param name="input"> 传入的字符串 </param>
        /// <returns> 你所指定的 <typeparamref name="T"/> 值类型 </returns>
        /// <exception cref="ArgumentException"> 字符串格式不符合目标类型要求或不是 <typeparamref name="T"/> 的有效值，转换失败 </exception>
        /// <exception cref="NotSupportedException"> 无法将字符串转换为指定的类型 </exception>
        /// <exception cref="NullReferenceException"> 不能将 null 强制类型转换为指定的类型 </exception>
        /// <exception cref="FormatException"> 字符串类型不是指定类型的输出格式 </exception>
        public static T ConvertTo<T>(this string input) where T : struct
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null)
                return (T)converter.ConvertFromString(input);
            throw new NotSupportedException();
        }
#if Xunit
        [Fact]
        public static void ConvertToTest()
        {
            string str = null;
            Assert.Throws<NotSupportedException>(() => str.ConvertTo<int>());

            #region Boolean ( 不能对数值直接转换，仅能转换 True / False )
            Assert.False("false".ConvertTo<bool>());
            Assert.False("False".ConvertTo<bool>());
            Assert.False("FALSE".ConvertTo<bool>());
            Assert.Throws<FormatException>(() => "0".ConvertTo<bool>());
            Assert.True("True".ConvertTo<bool>());
            Assert.Throws<FormatException>(() => "1".ConvertTo<bool>());
            #endregion

            #region Byte
            Assert.Equal((byte)0, "0".ConvertTo<byte>());
            Assert.Equal((byte)255, "255".ConvertTo<byte>());
            Assert.Throws<ArgumentException>(() => "-1".ConvertTo<byte>());
            Assert.Throws<ArgumentException>(() => "256".ConvertTo<byte>());
            #endregion

            #region Char
            Assert.Equal('\u00A9', "©".ConvertTo<char>());
            Assert.Throws<FormatException>(() => "00".ConvertTo<char>());
            #endregion

            #region Num
            Assert.Equal(79228162514264337593543950335M, "79228162514264337593543950335".ConvertTo<decimal>());

            Assert.Equal(1.7976931348623157E+308, "1.7976931348623157E+308".ConvertTo<double>());
            Assert.Equal(3.40282347E+38F, "3.40282347E+38".ConvertTo<float>());

            Assert.Equal(127, "127".ConvertTo<sbyte>());

            Assert.Equal(32767, "32767".ConvertTo<short>());
            Assert.Equal(65535, "65535".ConvertTo<ushort>());

            Assert.Equal(2147483647, "2147483647".ConvertTo<int>());
            Assert.Equal(9223372036854775807, "9223372036854775807".ConvertTo<long>());
            Assert.Equal(4294967295, "4294967295".ConvertTo<uint>());
            Assert.Equal(18446744073709551615, "18446744073709551615".ConvertTo<ulong>());

            Assert.Throws<ArgumentException>(() => "79228162514264337593543950336".ConvertTo<decimal>());
            Assert.Throws<ArgumentException>(() => "128".ConvertTo<sbyte>());
            Assert.Throws<ArgumentException>(() => "32768".ConvertTo<short>());
            Assert.Throws<ArgumentException>(() => "65536".ConvertTo<ushort>());
            Assert.Throws<ArgumentException>(() => "2147483648".ConvertTo<int>());
            Assert.Throws<ArgumentException>(() => "9223372036854775808".ConvertTo<long>());
            Assert.Throws<ArgumentException>(() => "4294967296".ConvertTo<uint>());
            Assert.Throws<ArgumentException>(() => "18446744073709551616".ConvertTo<ulong>());
            #endregion

            #region DateTime
            str = null;
            Assert.Throws<NotSupportedException>(() => str.ConvertTo<DateTime>());
            Assert.Throws<NotSupportedException>(() => str.ConvertTo<DateTimeOffset>());
            str = string.Empty;
            Assert.Equal(DateTime.MinValue, str.ConvertTo<DateTime>());
            Assert.Equal(DateTimeOffset.MinValue, str.ConvertTo<DateTimeOffset>());
            str = "Hello World!";
            Assert.Throws<FormatException>(() => str.ConvertTo<DateTime>());
            Assert.Throws<FormatException>(() => str.ConvertTo<DateTimeOffset>());

            var nowDT = DateTime.Now;
            str = nowDT.ToString();
            Assert.Equal(DateTime.Parse(str), str.ConvertTo<DateTime>());

            var nowDTOS = DateTimeOffset.Now;
            str = nowDT.ToString();
            Assert.Equal(DateTimeOffset.Parse(str), str.ConvertTo<DateTimeOffset>());
            #endregion

        }
#endif
        #endregion
        #region public static T? ConvertToNullable<T>(this string input) where T : struct
        /// <summary>
        /// string转换为可空结构性类型
        /// </summary>
        /// <param name="input"> 传入的字符串 </param>
        /// <returns> 
        /// <para> 如果 <paramref name="input"/> 的值为 null，则返回值为 null </para>
        /// <para> 否则是你所指定的 <typeparamref name="T"/> 值类型 </para>
        /// </returns>
        public static T? ConvertToNullable<T>(this string input) where T : struct
        {
            if (input == null) return new T?(); ;
            try { return ConvertTo<T>(input); }
            catch { return new T?(); }
        }
#if Xunit
        [Fact]
        public static void ConvertToNullableTest()
        {
            string str = null;
            Assert.Null(str.ConvertToNullable<int>());
            str = "0";
            Assert.Equal(0, str.ConvertToNullable<int>());

            // 其他测试结果同 ConvertToTest
        }
#endif
        #endregion

    }
}
