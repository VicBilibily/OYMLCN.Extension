using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OYMLCN.ArgumentChecker;
#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 格式相关扩展
    /// </summary>
    public static partial class StringFormatExtension
    {
        #region public static string ToYesOrNo(this bool boolean, bool cnString = true)
        /// <summary>
        /// 将 Boolean 转换为 Yes/是 或 No/否
        /// </summary>
        /// <param name="boolean"> Boolean 值 </param>
        /// <param name="cnString"> 是否使用中文是/否 </param>
        /// <returns>
        /// <para> 根据 <paramref name="boolean"/> 的值返回 是/否 </para>
        /// <para> 若 <paramref name="cnString"/> 为 false，则返回 Yes/No </para>
        /// </returns>
        public static string ToYesOrNo(this bool boolean, bool cnString = true)
            => cnString ? boolean ? "是" : "否" : boolean ? "Yes" : "No";
#if Xunit
        [Fact]
        public static void ToYesOrNoTest()
        {
            Assert.Equal("是", true.ToYesOrNo());
            Assert.Equal("否", false.ToYesOrNo());
            Assert.Equal("Yes", true.ToYesOrNo(false));
            Assert.Equal("No", false.ToYesOrNo(false));
        }
#endif
        #endregion

        #region public static string FormatAsRMBUpperCase(this string money, bool endSymbol = true)
        /// <summary>
        /// 将人民币字符串小写金额转换成大写形式
        /// </summary>
        /// <param name="money"> 人民币字符串小写金额 </param>
        /// <param name="endSymbol"> 是否在整数后面输出 元整 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="money"/> 不能为 null </exception>
        /// <exception cref="FormatException"> <paramref name="money"/> 不是人民币小数标识的字符串 </exception>
        public static string FormatAsRMBUpperCase(this string money, bool endSymbol = true)
        {
            money.ThrowIfNull(nameof(money));
            return money.RemoveValuesRegexMatches("￥", ",", "\\s")
                .ConvertToDouble().ToRMBUpperString(endSymbol);
        }
#if Xunit
        [Fact]
        public static void FormatAsRMBUpperCaseTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.FormatAsRMBUpperCase());

            str = "1,010,123.25";
            Assert.Equal("壹佰零壹万零壹佰贰拾叁元贰角伍分", str.FormatAsRMBUpperCase());

            str = "￥1000000000000000";
            Assert.Equal("壹仟兆元整", str.FormatAsRMBUpperCase());

            str = "￥1000";
            Assert.Equal("壹仟元", str.FormatAsRMBUpperCase(endSymbol: false));

            str = "$10000000";
            Assert.Throws<FormatException>(() => str.FormatAsRMBUpperCase());
        }
#endif
        #endregion


        #region public static string FirstCharToLower(this string input)
        /// <summary>
        /// 把字符串对象的首字母转换为小写表现形式
        /// </summary>
        /// <param name="input"> 要转换的字符串 </param>
        /// <returns> 如果首字母为大写则转换为小写后返回新的字符串，小写则不处理原样返回 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static string FirstCharToLower(this string input)
        {
            input.ThrowIfNull(nameof(input));
            var arr = input.ToCharArray();
            if (arr.Any())
            {
                var first = arr[0];
                if (char.IsUpper(first))
                {
                    arr[0] = char.ToLower(first);
                    return new string(arr);
                }
            }
            return input;
        }
#if Xunit
        [Fact]
        public static void FirstCharToLowerTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.FirstCharToLower());

            str = "Hello World!";
            Assert.Equal("hello World!", str.FirstCharToLower());

            str = "yes";
            Assert.Equal(str, str.FirstCharToLower());

            str = "你好，世界！";
            Assert.Equal(str, str.FirstCharToLower());
        }
#endif 
        #endregion

        #region public static string FirstCharToUpper(this string input)
        /// <summary>
        /// 把字符串对象的首字母转换为大写表现形式
        /// </summary>
        /// <param name="input"> 要转换的字符串 </param>
        /// <returns> 如果首字母为小写则转换为大写后返回新的字符串，大写则不处理原样返回 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="input"/> 不能为 null </exception>
        public static string FirstCharToUpper(this string input)
        {
            input.ThrowIfNull(nameof(input));
            var arr = input.ToCharArray();
            if (arr.Any())
            {
                var first = arr[0];
                if (char.IsLower(first))
                {
                    arr[0] = char.ToUpper(first);
                    return new string(arr);
                }
            }
            return input;
        }
#if Xunit
        [Fact]
        public static void FirstCharToUpperTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.FirstCharToUpper());

            str = "hello world!";
            Assert.Equal("Hello world!", str.FirstCharToUpper());

            str = "yes";
            Assert.Equal("Yes", str.FirstCharToUpper());

            str = "你好，世界！";
            Assert.Equal(str, str.FirstCharToUpper());
        }
#endif
        #endregion

        #region public static string CamelCaseToUnderline(this string input)
        /// <summary>
        /// 驼峰转下划线
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CamelCaseToUnderline(this string input)
        {
            input.ThrowIfNull(nameof(input));

            if (input.IsEmpty()) return "";
            return new Regex("([a-z])([A-Z])").Replace(input, "$1_$2").ToLower();
        }
#if Xunit
        [Fact]
        public static void CamelCaseToUnderlineTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.CamelCaseToUnderline());
            str = "CamelCase";
            Assert.Equal("camel_case", str.CamelCaseToUnderline());
        }
#endif 
        #endregion

    }
}
