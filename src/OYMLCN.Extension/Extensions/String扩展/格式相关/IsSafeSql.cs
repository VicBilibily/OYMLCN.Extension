using System.Text.RegularExpressions;

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
        #region public static bool IsSafeSqlData(this string input)
        /// <summary>
        /// 验证是否存在 SQL 注入代码
        /// <para> 建议采用参数化的方式防止 SQL 注入，本方法通过检查关键词的方式判断，可能存在误报情况 </para>
        /// <para>（eg："Hello World!" 中存在 or 关键词）</para>
        /// </summary>
        /// <param name="input"> 要检查的字符串 </param>
        /// <returns> 当检测到客户的输入中有含有危险字符串，则返回false，否则返回true。 </returns>
        public static bool IsSafeSqlData(this string input)
        {
            if (input.IsNullOrWhiteSpace()) return true;
            if (BadSqlInputKeywordsRegex == null)
            {
                // SQL的注入关键字符
                string[] strBadChar =
                {
                    "and", "exec", "insert", "select", "delete", "update", "count",
                    "from", "drop", "asc", "char", "or", "%", ";", ":", "\'", "\"", "-",
                    "chr", "mid", "master", "truncate", "char", "declare", // "SiteName",
                    "net user", "xp_cmdshell", "/add",
                    "exec master.dbo.xp_cmdshell", "net localgroup administrators"
                };
                // 构造正则表达式
                string str_Regex = ".*(";
                for (int i = 0; i < strBadChar.Length - 1; i++)
                    str_Regex += strBadChar[i] + "|";
                str_Regex += strBadChar[strBadChar.Length - 1] + ").*";
                BadSqlInputKeywordsRegex = str_Regex;
            }
            return !Regex.IsMatch(input.ToLower(), BadSqlInputKeywordsRegex);
        }
        private static string BadSqlInputKeywordsRegex;
#if Xunit
        [Fact]
        public static void IsSafeSqlDataTest()
        {
            string str = null;
            Assert.True(str.IsSafeSqlData());
            str = "Hello World!"; // 匹配到 or，令杀错勿放过
            Assert.False(str.IsSafeSqlData());
            str = "你好，世界!";
            Assert.True(str.IsSafeSqlData());

            string[] strBadChar =
            {
                "and", "exec", "insert", "select", "delete", "update", "count",
                "from", "drop", "asc", "char", "or", "%", ";", ":", "\'", "\"", "-",
                "chr", "mid", "master", "truncate", "char", "declare", //"SiteName",
                "net user", "xp_cmdshell", "/add",
                "exec master.dbo.xp_cmdshell", "net localgroup administrators"
            };
            strBadChar.ForEach((data, idx) =>
            {
                Assert.False($"{data} {idx}".IsSafeSqlData(), $"{data} {idx}");
            });
        }
#endif
        #endregion

        #region public static bool IsSafeSqlString(this string input)
        /// <summary>
        /// 验证字符串是否包含 SQL 危险符号
        /// <para> 如果没有则返回 true，表示当前字符串通过检测 </para> 
        /// <para> 有Sql危险字符则返回 false，你应当不再处理该用户提交的内容 </para> 
        /// </summary>
        /// <param name="input"> 要检查的字符串 </param>
        /// <returns> 如果 <paramref name="input"/> 验证通过，不包含 SQL 危险符号，则返回 true，否则为 false </returns>
        public static bool IsSafeSqlString(this string input)
        {
            if (input.IsNullOrWhiteSpace()) return true;
            return !Regex.IsMatch(input, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
#if Xunit
        [Fact]
        public static void IsSafeSqlStringTest()
        {
            string str = null;
            Assert.True(str.IsSafeSqlString());
            str = "Hello World";
            Assert.True(str.IsSafeSqlString());
            str = "Hello World!";
            Assert.False(str.IsSafeSqlString());
            string[] strBadChar = new[] { "-", ";", ",", "/", "(", ")", "[", "]", "}", "{", "%", "@", "*", "!", "'" };
            strBadChar.ForEach((input) => Assert.False($"Hello{input}".IsSafeSqlString()));
        }
#endif 
        #endregion

        #region public static bool IsSafeInfoString(this string input)
        /// <summary>
        /// 用户信息 SQL 注入安全验证
        /// <para> 检测是否有危险的可能用于注入字符符号 </para> 
        /// </summary>
        /// <param name="input"> 要检查的用户字符串 </param>
        /// <returns> 如果 <paramref name="input"/> 验证通过，不包含危险字符时返回 true，否则为 false </returns>
        public static bool IsSafeInfoString(this string input)
        {
            if (input.IsNullOrWhiteSpace()) return false;
            //return !Regex.IsMatch(input, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
            return !Regex.IsMatch(input, @"^\s*$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        }
#if Xunit
        [Fact]
        public static void IsSafeInfoStringTest()
        {
            // 字符串为 null
            string str = null;
            Assert.False(str.IsSafeInfoString());

            // 字符串为空或由空格组成
            str = string.Empty;
            Assert.False(str.IsSafeInfoString());
            str = "  ";
            Assert.False(str.IsSafeInfoString());
            // 字符串中含有空格
            str = "Hello World!";
            Assert.False(str.IsSafeInfoString());

            // 合规的字符串
            str = "HelloWorld!";
            Assert.True(str.IsSafeInfoString());

            // 远古漏洞，现在已不受影响
            str = @"c:\con\con";
            //Assert.False(str.IsSafeInfoString());
            Assert.True(str.IsSafeInfoString());

            // 百分号（%），逗号（,），星号（*），双引号（"），制表符（\t），小于号（<），大于号(>)和连接符号（&）
            string[] strBadChar = new[] { "%", ",", "*", "\"", "\t", "<", ">", "&" };
            strBadChar.ForEach((input) => Assert.False($"Hi{input}World".IsSafeInfoString()));

            // 包含 游客 的字符串
            str = "我是游客";
            Assert.False(str.IsSafeInfoString());
            // 开头为 Guest 的字符串
            str = "GuestLogout";
            Assert.False(str.IsSafeInfoString());
        }
#endif 
        #endregion

    }
}
