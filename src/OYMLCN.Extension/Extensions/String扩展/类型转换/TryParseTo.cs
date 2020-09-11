namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class StringConvertExtension
    {
        #region public static double TryParseToDouble(this string value, double failResult = default)
        /// <summary>
        /// 尝试将表示形式为数字的字符串转换为等效的双精度浮点数，失败时返回默认值
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <param name="failResult"> 转换失败时返回的默认值 </param>
        /// <returns> 与 <paramref name="value"/> 中数字等效的双精度浮点数，如果 <paramref name="value"/> null、空白字符 或 转换失败，则返回默认值 <paramref name="failResult"/>。 </returns>
        public static double TryParseToDouble(this string value, double failResult = default)
        {
            if (value.IsNotNullOrWhiteSpace() && double.TryParse(value, out double res))
                return res;
            return failResult;
        } 
        #endregion
        #region public static double? TryParseToNDouble(this string value)
        /// <summary>
        /// 尝试将表示形式为数字的字符串转换为等效的双精度浮点数，失败时返回 null
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <returns> 与 <paramref name="value"/> 中数字等效的双精度浮点数，如果 <paramref name="value"/> null、空白字符 或 转换失败，则返回 null。 </returns>
        public static double? TryParseToNDouble(this string value)
        {
            if (value.IsNotNullOrWhiteSpace() && double.TryParse(value, out double res))
                return res;
            return null;
        }
        #endregion

        #region public static decimal TryParseToDecimal(this string value, decimal failResult = default)
        /// <summary>
        /// 尝试将数字的指定字符串表示形式转换为等效的十进制数，失败时返回默认值
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <param name="failResult"> 转换失败时返回的默认值 </param>
        /// <returns>
        ///   与 <paramref name="value"/> 中数字等效的十进制数，如果 <paramref name="value"/> 为 null、空白字符 或 转换失败，则返回默认值 <paramref name="failResult"/>。 </returns>
        public static decimal TryParseToDecimal(this string value, decimal failResult = default)
        {
            if (value.IsNotNullOrWhiteSpace() && decimal.TryParse(value, out decimal res))
                return res;
            return failResult;
        }
        #endregion
        #region public static decimal? TryParseToNDecimal(this string value)
        /// <summary>
        /// 尝试将数字的指定字符串表示形式转换为等效的十进制数，失败时返回 null
        /// </summary>
        /// <param name="value"> 包含要转换的数字的字符串 </param>
        /// <returns> 与 <paramref name="value"/> 中数字等效的十进制数，如果 <paramref name="value"/> null、空白字符 或 转换失败，则返回 null。 </returns>
        public static decimal? TryParseToNDecimal(this string value)
        {
            if (value.IsNotNullOrWhiteSpace() && decimal.TryParse(value, out decimal res))
                return res;
            return null;
        } 
        #endregion

    }
}
