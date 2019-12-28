using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OYMLCN.Extension.TestForDotNet
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var page = new Helpers.PaginationHelpers(9, 1, 10);
        }
    }
}
