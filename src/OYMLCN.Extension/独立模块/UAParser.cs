using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN
{
    /// <summary>
    /// 基于 https://github.com/faisalman/ua-parser-js 0.7.21
    /// 重新编写为 .Net C# 版本
    /// </summary>
    public class UAParser
    {
        /// <summary>
        /// 将用户代理字符串转化为可分析的数据实体
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static UAParser Parse(string userAgent)
            => new UAParser(userAgent);


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="userAgent"></param>
        private UAParser(string userAgent)
        {

        }



    }
}
