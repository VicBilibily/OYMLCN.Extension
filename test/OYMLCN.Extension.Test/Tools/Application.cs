/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: Application.cs
Author: VicBilibily
Description: ��Ԫ���Դ��룬����Ӧ�ó��򻷾���صİ���������
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
            // Resharper��VS�Ĳ��Ի�����һ������Ͳ��������ˣ��������ǶԵ�
            //Assert.Contains("test", exePath, StringComparison.OrdinalIgnoreCase);
        }
    }
}