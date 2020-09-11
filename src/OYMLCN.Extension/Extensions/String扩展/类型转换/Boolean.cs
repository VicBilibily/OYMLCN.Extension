#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class StringConvertExtension
    {
        private static readonly string[] _TrueStringValues
            = { "1", "true", "yes", "ok", "checked", "是", "对" };

        #region public static bool IsTrueStringValue(this string input)
        /// <summary>
        /// 将字符串转换为Boolean类型（当字符串是“true/1/yes/ok/checked/是/对”(不区分大小写)时返回true）
        /// </summary>
        /// <param name="input"> 传入的字符串 </param>
        /// <returns> <seealso cref="bool"/> 的值 true/false </returns>
        public static bool IsTrueStringValue(this string input)
            => input.EqualsIgnoreCase(_TrueStringValues);
#if Xunit
        [Fact]
        public static void IsTrueStringValueTest()
        {
            string str = null;
            Assert.False(str.IsTrueStringValue());
            Assert.False("0".IsTrueStringValue());
            Assert.True("1".IsTrueStringValue());
            Assert.True("TRUE".IsTrueStringValue());
            Assert.True("YES".IsTrueStringValue());
            Assert.True("CHECKED".IsTrueStringValue());
            Assert.True("是".IsTrueStringValue());
            Assert.True("对".IsTrueStringValue());
            Assert.False("---".IsTrueStringValue());
        }
#endif
        #endregion

    }
}
