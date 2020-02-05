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
        /// 获取url字符串的的协议域名地址（eg：https://www.qq.com）
        /// </summary>
        public static string GetSchemeHost(this Uri uri)
        {
            if (uri == null) return null;
            return $"{uri.Scheme}://{uri.Host}";
        }
    }
}
