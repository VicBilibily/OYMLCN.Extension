using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OYMLCN.Extension.Test
{
    [TestClass]
    public class DemoTest
    {
        [TestMethod]
        public void DemoTestMethod()
        {
            Assert.AreEqual(OYMLCN.Word.WorkPinYinExtension.Pinyin("我们").TotalPinYin.FirstOrDefault(), "women");
        }
    }
}
