using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 格式相关扩展
    /// </summary>
    public static partial class StringFormatExtension
    {
        /// <summary>
        /// 将Boolean转换为Yes是或No否
        /// </summary>
        /// <param name="boolean"></param>
        /// <param name="cnString">是否返回中文是/否</param>
        /// <returns></returns>
        public static string ToYesOrNo(this bool boolean, bool cnString = true)
            => cnString ? boolean ? "是" : "否" : boolean ? "Yes" : "No";


        /// <summary>
        /// 人民币金额小写转大写
        /// </summary>
        public static string FormatAsUppercaseRMB(this string money)
            => money.ConvertToDouble().ConvertToUppercaseRMB();

    }
}
