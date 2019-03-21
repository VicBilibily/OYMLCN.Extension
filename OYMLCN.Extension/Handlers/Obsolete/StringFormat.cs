using OYMLCN.Extensions;

namespace OYMLCN.Handlers
{
    /// <summary>
    /// 字符串格式相关操作
    /// </summary>
    public class StringFormatHandler
    {
        private string Str;
        internal StringFormatHandler(string str) => Str = str;

        #region Unicode
        /// <summary>
        /// 普通字符串转Unicode字符串
        /// </summary>
        public string String2Unicode => Str.ToUnicode();
        /// <summary>
        /// Unicode字符串转普通字符串
        /// </summary>
        public string Unicode2String => Str.UnicodeToString();
        #endregion

        #region Is判断
        /// <summary>
        /// 验证居民身份证号码
        /// 验证支持：GB11643-1989、GB11643-1999
        /// </summary>
        public bool IsChineseIDCard => Str.FormatIsChineseIDCard();
        /// <summary>
        /// 判断字符串是否是邮箱地址
        /// </summary>
        public bool IsEmailAddress => Str.FormatIsEmailAddress();
        /// <summary>
        /// 判断字符串是否是手机号码
        /// </summary>
        public bool IsMobilePhoneNumber => Str.FormatIsMobilePhoneNumber();
        /// <summary>
        /// 判断字符是不是汉字
        /// </summary>
        public bool IsChineseRegString => Str.FormatIsChineseRegString();
        #endregion

        #region 数字的判断
        /// <summary>
        /// 判断文本是否为数字
        /// </summary>
        public bool IsNumeric => Str.FormatIsNumeric();
        /// <summary>
        /// 判断文本是否为整数
        /// </summary>
        public bool IsInteger => Str.FormatIsInteger();
        /// <summary>
        /// 判断文本是否为正数
        /// </summary>
        public bool IsUnsignNumeric => Str.FormatIsUnsignNumeric();
        /// <summary>
        /// 获取文本中的数字
        /// </summary>
        public string Numeric => Str.FormatAsNumeric();
        /// <summary>
        /// 获取文本中的整数部分
        /// </summary>
        public string IntegerNumeric => Str.FormatAsIntegerNumeric();
        #endregion

    }
}
