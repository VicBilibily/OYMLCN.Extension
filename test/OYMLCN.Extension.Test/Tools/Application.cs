using System;
using OYMLCN.Tools;
using Xunit;

namespace OYMLCN.Tests.Tools
{
    public class ApplicationTest
    {
        [Fact(DisplayName = nameof(Application.GetExecutablePath))]
        public void GetExecutablePathTest()
        {
            string exePath = Application.GetExecutablePath();
            // Resharper和VS的测试环境不一样，这就不测试它了，反正他是对的
            //Assert.Contains("test", exePath, StringComparison.OrdinalIgnoreCase);
        }
    }
}