using System.Reflection;

namespace OYMLCN.Tools
{
    /// <summary>
    /// 应用程序相关操作
    /// </summary>
    public static class Application
    {
        /// <summary>
        /// 获取可执行程序的完整路径
        /// </summary>
        public static string GetExecutablePath()
            // 不建议直接使用 Application.ExecutablePath
            // 因为在文件路径中含有特殊字符时会发生异常情况
            // 详情请看 https://stackoverflow.com/questions/12945805/odd-c-sharp-path-issue
            => Assembly.GetEntryAssembly()?.Location;
    }
}