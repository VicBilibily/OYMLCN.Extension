using System;
using System.Collections.Generic;
using System.Linq;
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
        #region public static string[] ToStringArray(this string source)
        /// <summary>
        /// 将字符串拆分成为每一个单字
        /// </summary>
        /// <param name="source"> 源字符串 </param>
        /// <returns> 返回字符串对象中每个字符的 <see cref="string"/> 实例组成的数组 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source"/> 为 null </exception>
        public static string[] ToStringArray(this string source)
            => source.Select(v => v.ToString()).ToArray();
#if Xunit
        [Fact]
        public static void StringToArrayTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.ToStringArray());

            str = "Yes";
            Assert.Equal(new[] { "Y", "e", "s" }, str.ToStringArray());

            str = "你好，世界！";
            Assert.Equal(new[] { "你", "好", "，", "世", "界", "！" }, str.ToStringArray());
        }
#endif
        #endregion


        #region public static T[] ConvertToIdArray<T>(this string idsStr)
        /// <summary>
        /// 把以英文逗号 (,) 分割的 ID 字符串提取为 ID 数组
        /// <para> 当有任意值转换失败或格式不正确，将会跳过该值并返回所有转换成功的 ID </para>
        /// </summary>
        /// <param name="idsStr"> 以英文逗号 (,) 分割 ID 的字符串对象 </param>
        /// <returns> 
        /// <para> 如果 <paramref name="idsStr"/> 为 null 则会返回指定对象的空数组 </para>
        /// <para> 否则将尝试把以英文逗号 (,) 分割的 ID 字符串提取为 ID 数组 </para>
        /// <para> 当有任意值转换失败或格式不正确，将会跳过该值并返回所有转换成功的 ID </para>
        /// </returns>
        public static T[] ConvertToIdArray<T>(this string idsStr)
        {
            if (string.IsNullOrEmpty(idsStr)) return new T[0];

            List<T> list = new List<T>();
            Type typeFromHandle = typeof(T);
            foreach (string value in idsStr.Split(',').Select(v => v.Trim()))
                try
                {
                    list.Add((T)Convert.ChangeType(value, typeFromHandle));
                }
                catch (InvalidCastException) { }
                catch (FormatException) { }
            return list.ToArray();
        }
#if Xunit
        [Fact]
        public static void ConvertToIdArrayTest()
        {
            string str = null;
            Assert.Equal(new int[0], str.ConvertToIdArray<int>());

            str = "123456";
            Assert.Equal(new[] { 123456 }, str.ConvertToIdArray<int>());

            str = "0,1,2,3,4,5,6, 7, 8, 9";
            Assert.Equal(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, str.ConvertToIdArray<int>());
            Assert.Equal(new long[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, str.ConvertToIdArray<long>());

            // 这只是测试一下，一般不会用小数作为 ID 值
            str = "0.12,1,2,2.22, 3.14,";
            Assert.Equal(new double[] { 0.12, 1, 2, 2.22, 3.14 }, str.ConvertToIdArray<double>());
            Assert.Equal(new decimal[] { 0.12M, 1, 2, 2.22M, 3.14M }, str.ConvertToIdArray<decimal>());

            // 这只是测试一下，并不支持GUID直接转换
            var guid = Guid.NewGuid();
            str = $"0,1, {guid.ToString()}";
            var guids = str.ConvertToIdArray<Guid>();
            Assert.Equal(new Guid[0], guids);
            Assert.NotEqual(new[] { guid }, guids);
        }
#endif
        #endregion


        #region public static string ConvertToString(this char[] chars)
        /// <summary>
        /// 将 Unicode 字符数组拼接为字符串
        /// </summary>
        /// <param name="chars"> Unicode 字符数组 </param>
        public static string ConvertToString(this char[] chars)
            => new string(chars);
#if Xunit
        [Fact]
        public static void CharArrayConvertToStringTest()
        {
            char[] chars = null;
            Assert.Equal(string.Empty, chars.ConvertToString());

            chars = new char[0];
            Assert.Equal(string.Empty, chars.ConvertToString());

            chars = new[] { 'Y', 'e', 's' };
            Assert.Equal("Yes", chars.ConvertToString());
        }
#endif 
        #endregion

    }
}
