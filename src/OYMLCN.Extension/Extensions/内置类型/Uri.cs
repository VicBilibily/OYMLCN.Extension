using System;
using OYMLCN.ArgumentChecker;

#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// SystemTypeExtension
    /// </summary>
    public static partial class SystemTypeExtension
    {
        #region public static string GetSchemeHost(this Uri uri)
        /// <summary>
        /// 获取 <paramref name="uri"/> 的协议和域名地址（eg：https://www.qq.com）
        /// </summary>
        /// <param name="uri"> <see cref="Uri"/> 实例 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="uri"/> 不能为 null </exception>
        public static string GetSchemeHost(this Uri uri)
        {
            uri.ThrowIfNull(nameof(uri));
            return $"{uri.Scheme}://{uri.Host}";
        }
#if Xunit
        [Fact]
        public static void GetSchemeHostTest()
        {
            Uri uri = null;
            Assert.Throws<ArgumentNullException>(() => uri.GetSchemeHost());
            uri = new Uri("https://www.qq.com/index.shtml");
            Assert.Equal("https://www.qq.com", uri.GetSchemeHost());
        }
#endif 
        #endregion

    }
}
