using System;
using System.Collections.Generic;
using System.Text;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class StringConvertExtension
    {
        #region public static decimal TryToDecimal(this string value)
        /// <summary>
        /// 将数字的指定字符串表示形式转换为等效的十进制数
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <param name="failResult"></param>
        /// <returns> 与 <paramref name="value"/> 中数字等效的十进制数，如果 <paramref name="value"/> 为 null，则为 0（零） </returns>
        public static decimal TryParseToDecimal(this string value, decimal failResult = 0)
        {
            if (decimal.TryParse(value, out decimal res))
                return res;
            return failResult;
        }
        #endregion
        public static double TryParseToDouble(this string value, double failResult = 0)
        {
            if (double.TryParse(value, out double res))
                return res;
            return failResult;
        }
        public static double? TryParseToNDouble(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return null;
            if (double.TryParse(value, out double res))
                return res;
            return null;
        }

    }
}
