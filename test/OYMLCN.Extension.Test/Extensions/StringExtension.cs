
// .Net自带方法扩展，不做测试，此文件只作为测试覆盖引用
using Xunit;
using OYMLCN.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Tests.Extensions
{
    public class StringExtensionTest
    {
        [Fact]
        public void IsNullOrEmptyTest()
        {
            StringExtension.IsNullOrEmpty(null);
            string.Empty.IsNullOrEmpty();
        }




        #region Obsolete
        [Obsolete, Fact]
        public void IsNotNullOrEmptyTest()
        {
            StringExtension.IsNotNullOrEmpty(null);
            string.Empty.IsNotNullOrEmpty();
        } 
        #endregion

    }
}
