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
        static string _badSqlInputKeywordsRegex;
        private static string BadSqlInputKeywordsRegex
        {
            get
            {
                if (_badSqlInputKeywordsRegex != null)
                    return _badSqlInputKeywordsRegex;
                // SQL的注入关键字符
                string[] strBadChar =
                {
                    "and", "exec", "insert", "select", "delete", "update", "count",
                    "from", "drop", "asc", "char", "or", "%", ";", ":", "\'", "\"", "-",
                    "chr", "mid", "master", "truncate", "char", "declare", "SiteName",
                    "net user", "xp_cmdshell", "/add",
                    "exec master.dbo.xp_cmdshell", "net localgroup administrators"
                };
                // 构造正则表达式
                string str_Regex = ".*(";
                for (int i = 0; i < strBadChar.Length - 1; i++)
                    str_Regex += strBadChar[i] + "|";
                str_Regex += strBadChar[strBadChar.Length - 1] + ").*";
                return _badSqlInputKeywordsRegex = str_Regex;
            }
        }
        /// <summary>
        /// 验证是否存在注入代码
        /// <para>当检测到客户的输入中有攻击性危险字符串,则返回false,有效返回true。</para> 
        /// </summary>
        public static bool IsSafeSqlData(this string inputData)
            => !Regex.IsMatch(inputData.ToLower(), BadSqlInputKeywordsRegex);
        /// <summary>
        /// 检测Sql危险字符
        /// <para>如果没有则返回true，表示当前字符串通过检测</para> 
        /// <para>有Sql危险字符则返回false，你需要拒绝处理该用户提交的内容</para> 
        /// </summary>
        public static bool IsSafeSqlString(this string str)
            => !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        /// <summary>
        /// SQL注入等安全验证
        /// <para>检测是否有危险的可能用于链接的字符串</para> 
        /// </summary>
        public static bool IsSafeUserInfoString(this string str)
            => !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");


    }
}
