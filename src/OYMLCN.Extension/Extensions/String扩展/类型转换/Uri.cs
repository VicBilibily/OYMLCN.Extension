using System;

#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class StringConvertExtension
    {
        #region public static Uri ConvertToUri(this string uriString)
        /// <summary>
        /// 用指定的 URI 初始化 <seealso cref="Uri" /> 类的新实例，格式不正确时返回 null
        /// </summary>
        /// <param name="uriString"></param>
        /// <returns> 如果能够创建新的 <seealso cref="Uri" /> 实例，则返回实例引用，否则返回null </returns>
        public static Uri ConvertToUri(this string uriString)
        {
            if (uriString.IsNull()) return null;
            try
            {
                return new Uri(uriString);
            }
            catch (UriFormatException)
            {
                return null;
            }
        }
#if Xunit
        [Fact]
        public static void ConvertToUriTest()
        {
            string str = null;
            Assert.Null(str.ConvertToUri());

            str = "Hello World!";
            Assert.Null(str.ConvertToUri());

            str = "http://www.qq.com";
            Assert.Equal(str.ConvertToUri(), new Uri("http://www.qq.com"));
        }
#endif
        #endregion

    }
}
