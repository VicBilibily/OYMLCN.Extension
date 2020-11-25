/*****************************************************************************
Copyright: VicBilibily, 2008-2021 OYMLCN.
License: The MIT License. 
File name: StringExtension.cs
Author: VicBilibily
Description: 单元测试代码，此文件只作为测试覆盖引用，并未实现完整的过程测试。
*****************************************************************************/

// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable CheckNamespace

using OYMLCN.Extensions;
using System.Globalization;
using Xunit;

namespace OYMLCN.Tests.Extensions
{
    public partial class SystemStringExtensionTest
    {
        [Fact]
        public void ConvertToTest()
        {
            var provider = CultureInfo.CurrentCulture;
            var fromBase = 10;

            string str = null;
            str.ToBoolean();
            str = "0";
            str.ToByte(); str.ToByte(provider); str.ToByte(fromBase);
            str.ToChar();
            str.ToDecimal(); str.ToDecimal(provider);
            str.ToDouble(); str.ToDouble(provider);
            str.ToInt16(); str.ToInt16(provider); str.ToInt16(fromBase);
            str.ToInt32(); str.ToInt32(provider); str.ToInt32(fromBase);
            str.ToInt64(); str.ToInt64(provider); str.ToInt64(fromBase);
            str.ToSByte(); str.ToSByte(provider); str.ToSByte(fromBase);
            str.ToSingle(); str.ToSingle(provider);
            str.ToUInt16(); str.ToUInt16(provider); str.ToUInt16(fromBase);
            str.ToUInt32(); str.ToUInt32(provider); str.ToUInt32(fromBase);
            str.ToUInt64(); str.ToUInt64(provider); str.ToUInt64(fromBase);
            str = null;
            str.ToDateTime(); str.ToDateTime(provider);
        }
    }
}
