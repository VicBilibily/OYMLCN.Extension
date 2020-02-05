using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    public static partial class SystemTypeExtension
    {
        /// <summary>
        /// 将枚举值转换为字符串值（替换 _ 标头）
        /// 用于部分不能使用数字作为枚举值 用 _ 作为开头（某些较旧的源码里有这种习惯……）
        /// </summary>
        /// <param name="enumClass"></param>
        /// <returns></returns>
        public static string EnumToString(this Enum enumClass)
            => enumClass.ToString().TrimStart('_');

    }
}
