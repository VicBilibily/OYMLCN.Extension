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
        private static readonly string[] _ConvertToBooleanTrueResultValues
            = new[] { "1", "true", "yes", "ok", "checked", "是", "对" };

        #region public static bool ConvertToBoolean(this string input)
        /// <summary>
        /// 将字符串转换为Boolean类型（当字符串是“true/1/yes/ok/checked/是/对”(不区分大小写)时返回true）
        /// </summary>
        /// <param name="input"> 传入的字符串 </param>
        /// <returns> <seealso cref="bool"/> 的值 true/false </returns>
        public static bool ConvertToBoolean(this string input)
            => input.EqualsIgnoreCase(_ConvertToBooleanTrueResultValues);
#if Xunit
        [Fact]
        public static void ConvertToBooleanTest()
        {
            string str = null;
            Assert.False(str.ConvertToBoolean());
            Assert.False("0".ConvertToBoolean());
            Assert.True("1".ConvertToBoolean());
            Assert.True("TRUE".ConvertToBoolean());
            Assert.True("YES".ConvertToBoolean());
            Assert.True("CHECKED".ConvertToBoolean());
            Assert.True("是".ConvertToBoolean());
            Assert.True("对".ConvertToBoolean());
            Assert.False("---".ConvertToBoolean());
        }
#endif
        #endregion

        #region public static bool? ConvertToNullableBoolean(this string input)
        /// <summary>
        /// 将字符串转换为Boolean类型（当字符串是“true/1/ok/yes/checked/是/对”(不区分大小写)时返回true，为空时返回null）
        /// </summary>
        /// <param name="input"> 传入的字符串 </param>
        /// <returns> 
        /// <para> 如果 <paramref name="input"/> 的值为 null，则返回值为 null </para>
        /// <para> 否则是 <seealso cref="bool"/> 的值 true/false </para>
        /// </returns>
        public static bool? ConvertToNullableBoolean(this string input)
            => input?.ConvertToBoolean();
#if Xunit
        [Fact]
        public static void ConvertToNullableBooleanTest()
        {
            string str = null;
            Assert.Null(str.ConvertToNullableBoolean());

            // 其他测试结果同 ConvertToBooleanTest
        }
#endif
        #endregion

    }
}
