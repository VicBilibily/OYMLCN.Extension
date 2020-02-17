using System;
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

        #region public static string FormatAsUppercaseRMB(this string money, bool endSymbol = true)
        /// <summary>
        /// 将人民币字符串小写金额转换成大写形式
        /// </summary>
        /// <param name="money"> 人民币字符串小写金额 </param>
        /// <param name="endSymbol"> 是否在整数后面输出 元整 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="money"/> 不能为 null </exception>
        /// <exception cref="FormatException"> <paramref name="money"/> 不是人民币小数标识的字符串 </exception>
        public static string FormatAsUppercaseRMB(this string money, bool endSymbol = true)
        {
            money.ThrowIfNull(nameof(money));
            return money.RemoveValuesRegexMatches("￥", ",", "\\s")
                .ConvertToDouble().ConvertToUppercaseRMB(endSymbol);
        }
#if Xunit
        [Fact]
        public static void FormatAsUppercaseRMBTest()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.FormatAsUppercaseRMB());

            str = "1,010,123.25";
            Assert.Equal("壹佰零壹万零壹佰贰拾叁元贰角伍分", str.FormatAsUppercaseRMB());

            str = "￥1000000000000000";
            Assert.Equal("壹仟兆元整", str.FormatAsUppercaseRMB());

            str = "$10000000";
            Assert.Throws<FormatException>(() => str.FormatAsUppercaseRMB());
        }
#endif 
        #endregion

    }
}
