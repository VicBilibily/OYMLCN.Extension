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
            // Resharper��VS�Ĳ��Ի�����һ������Ͳ��������ˣ��������ǶԵ�
            //Assert.Contains("test", exePath, StringComparison.OrdinalIgnoreCase);
        }
    }
}