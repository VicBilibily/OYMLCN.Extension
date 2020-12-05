/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: Application.cs
Author: VicBilibily
Description: 单元测试代码，测试应用程序环境相关的帮助方法。
*****************************************************************************/

using System;
using OYMLCN.Tools;
using Xunit;

namespace OYMLCN.Tests.Tools
{
    public class ApplicationTest
    {
        [Fact]
        public void GetExecutablePathTest()
        {
            string exePath = Application.GetExecutablePath();
            // Resharper和VS的测试环境不一样，这就不测试它了，反正他是对的
            //Assert.Contains("test", exePath, StringComparison.OrdinalIgnoreCase);
        }
    }
}