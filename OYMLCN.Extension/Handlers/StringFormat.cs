using OYMLCN.Extensions;
using System;
using System.Text;
using System.Text.RegularExpressions;

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
        public string String2Unicode
        {
            get
            {
                byte[] bytes = Encoding.Unicode.GetBytes(Str);
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i += 2)
                    stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
                return stringBuilder.ToString();
            }
        }
        /// <summary>
        /// Unicode字符串转普通字符串
        /// </summary>
        public string Unicode2String =>
            new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled)
                .Replace(Str, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        #endregion

        #region Is判断
        /// <summary>
        /// 验证居民身份证号码
        /// 验证支持：GB11643-1989、GB11643-1999
        /// </summary>
        public bool IsChineseIDCard
        {
            get
            {
                string address = "11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,51,52,53,54,50,61,62,63,64,65,71,81,82";
                long n = 0;

                if (Str.Length == 15)
                {
                    if (long.TryParse(Str, out n) == false || n < Math.Pow(10, 14))
                        return false;//数字验证
                    if (address.IndexOf(Str.Remove(2)) == -1)
                        return false;//省份验证
                    if (Str.Substring(6, 6).Insert(4, "-").Insert(2, "-").AsType().NullableDatetime.IsNull())
                        return false;//生日验证  
                    return true;//符合GB11643-1989标准
                }
                else if (Str.Length == 18)
                {
                    if (long.TryParse(Str.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Str.Replace('x', '0').Replace('X', '0'), out n) == false)
                        return false;//数字验证  
                    if (address.IndexOf(Str.Remove(2)) == -1)
                        return false;//省份验证  
                    if (Str.Substring(6, 8).Insert(6, "-").Insert(4, "-").AsType().NullableDatetime.IsNull())
                        return false;//生日验证  
                    string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
                    string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
                    char[] Ai = Str.Remove(17).ToCharArray();
                    int sum = 0;
                    for (int i = 0; i < 17; i++)
                        sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
                    if (arrVarifyCode[sum % 11] != Str.Substring(17, 1).ToLower())
                        return false;//校验码验证
                    return true;//符合GB11643-1999标准
                }
                return false;
                //throw new FormatException("身份证长度不正确或暂未支持该身份证验证");
            }

        }
        /// <summary>
        /// 判断字符串是否是邮箱地址
        /// </summary>
        public bool IsEmailAddress =>
            Str.IsNullOrEmpty() ? false : new Regex(@"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?").IsMatch(Str.Trim());
        /// <summary>
        /// 判断字符串是否是手机号码
        /// </summary>
        public bool IsMobilePhoneNumber =>
            Str.IsNullOrEmpty() ? false : new Regex(@"^1[0-9]{10}$").IsMatch(Str.Trim());
        /// <summary>
        /// 判断字符是不是汉字
        /// </summary>
        public bool IsChineseRegString => Regex.IsMatch(Str, @"[\u4e00-\u9fbb]+$");
        #endregion

        #region 数字的判断
        /// <summary>
        /// 判断文本是否为数字
        /// </summary>
        public bool IsNumeric => Regex.IsMatch(Str, @"^[+-]?\d*[.]?\d*$", RegexOptions.Compiled);
        /// <summary>
        /// 判断文本是否为整数
        /// </summary>
        public bool IsInteger => Regex.IsMatch(Str, @"^[+-]?\d*$", RegexOptions.Compiled);
        /// <summary>
        /// 判断文本是否为正数
        /// </summary>
        public bool IsUnsignNumeric => Regex.IsMatch(Str, @"^\d*[.]?\d*$", RegexOptions.Compiled);
        /// <summary>
        /// 获取文本中的数字
        /// </summary>
        public string Numeric => Str.IsNullOrEmpty() ? null : Regex.Match(Str, @"[+-]?\d+(\.\d+)?", RegexOptions.Compiled).Value;
        /// <summary>
        /// 获取文本中的整数部分
        /// </summary>
        public string IntegerNumeric => Numeric?.SplitThenGetFirst(".");
        #endregion

    }
}
