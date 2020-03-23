using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    public static partial class SystemTypeExtension
    {
        /// <summary>
        /// 把秒转换成分钟
        /// </summary>
        /// <param name="second">秒数</param>
        /// <returns>分钟数</returns>
        public static int SecondToMinute(this int second)
            => Convert.ToInt32(Math.Ceiling(second / (decimal)60));

        /// <summary>
        /// 人民币金额小写转大写
        /// </summary>
        public static string ConvertToRMBUppercase(this decimal money, bool endSymbol = true)
        {
            var valueString = money.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            var temp = Regex.Replace(valueString, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            var result = Regex.Replace(temp, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟万亿兆京垓秭穰"[m.Value[0] - '-'].ToString());
            if (endSymbol && result.EndsWith("元")) result += "整";
            return result;
        }
        /// <summary>
        /// 人民币金额小写转大写
        /// </summary>
        public static string ConvertToRMBUppercase(this double money)
            => ConvertToRMBUppercase((decimal)money);
        /// <summary>
        /// 人民币金额小写转大写
        /// </summary>
        public static string ConvertToRMBUppercase(this int money)
            => ConvertToRMBUppercase((decimal)money);


    }
}
