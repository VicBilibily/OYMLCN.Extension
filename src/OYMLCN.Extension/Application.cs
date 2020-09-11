using System.Reflection;

#if Xunit
using Xunit;
#endif

namespace OYMLCN
{
    /// <summary>
    /// 程序相关操作
    /// </summary>
    public static class Application
    {
        #region public static string GetExecutablePath()
        /// <summary>
        /// 获取可执行程序的完整路径
        /// </summary>
        // 不建议直接使用 Application.ExecutablePath
        // 因为在文件路径中含有特殊字符时会发生异常情况
        // 详情请看 https://stackoverflow.com/questions/12945805/odd-c-sharp-path-issue
        public static string GetExecutablePath()
            => Assembly.GetEntryAssembly()?.Location;
#if Xunit
        [Fact]
        public static void GetExecutablePathTest()
        {
            Assert.Contains("microsoft.testplatform.testhost", Application.GetExecutablePath());
        }
#endif
        #endregion

    }
}
