using OYMLCN.ArgumentChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    public static partial class SystemTypeExtension
    {
        ///// <summary>
        ///// 把秒转换成分钟
        ///// </summary>
        ///// <param name="second">秒数</param>
        ///// <returns>分钟数</returns>
        //public static int SecondsToMinutes(this int second)
        //{
        //    second.ThrowIfNegative(nameof(second));
        //    return Convert.ToInt32(Math.Ceiling(second / (decimal)60));
        //}
        //#if Xunit
        //        [Fact]
        //        public static void SecondToMinuteTest()
        //        {
        //            Assert.Throws<ArgumentOutOfRangeException>(() => (-1).SecondsToMinutes());
        //            Assert.Equal(0, 0.SecondsToMinutes());
        //            Assert.Equal(1, 1.SecondsToMinutes());
        //            Assert.Equal(1, 30.SecondsToMinutes());
        //            Assert.Equal(1, 55.SecondsToMinutes());
        //            Assert.Equal(1, 60.SecondsToMinutes());
        //            Assert.Equal(2, 61.SecondsToMinutes());
        //        }
        //#endif


        #region public static string ToRMBUpperString(this double money, bool endSymbol = true)
        /// <summary>
        /// 将数值格式化为人民币金额大写
        /// </summary>
        /// <param name="money"> 金额数值 </param>
        /// <param name="endSymbol"> 结尾如果是整数是否输出为 元整 </param>
        /// <returns> 人民币金额的大写表现形式 </returns>
        public static string ToRMBUpperString(this double money, bool endSymbol = true)
        {
            var valueString = money.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var temp = Regex.Replace(valueString, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var result = Regex.Replace(temp, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            if (endSymbol && result.EndsWith('元')) result += "整";
            return result;
        }
#if Xunit
        [Fact]
        public static void ToRMBUpperStringTest()
        {
            Assert.Equal("壹佰零壹万零壹佰贰拾叁元贰角伍分", 1010123.25D.ToRMBUpperString());
            Assert.Equal("壹仟元", 1000.ToRMBUpperString(endSymbol: false));
            Assert.Equal("壹仟兆元整", 1000000000000000D.ToRMBUpperString());
        }
#endif
        #endregion

        #region public static string ToRMBUpperString(this decimal money, bool endSymbol = true)
        /// <summary>
        /// 将数值格式化为人民币金额大写
        /// </summary>
        /// <param name="money"> 金额数值 </param>
        /// <param name="endSymbol"> 结尾如果是整数是否输出为 元整 </param>
        /// <returns> 人民币金额的大写表现形式 </returns>
        public static string ToRMBUpperString(this decimal money, bool endSymbol = true)
            => money.ToDouble().ToRMBUpperString(endSymbol);
        #endregion
        #region public static string ToRMBUpperString(this int money, bool endSymbol = true)
        /// <summary>
        /// 将数值格式化为人民币金额大写
        /// </summary>
        /// <param name="money"> 金额数值 </param>
        /// <param name="endSymbol"> 结尾如果是整数是否输出为 元整 </param>
        /// <returns> 人民币金额的大写表现形式 </returns>
        public static string ToRMBUpperString(this int money, bool endSymbol = true)
            => money.ToDouble().ToRMBUpperString(endSymbol); 
        #endregion

    }
}
