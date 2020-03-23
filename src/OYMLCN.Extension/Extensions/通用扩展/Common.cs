#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 通用扩展
    /// </summary>
    public static class CommonExtension
    {
        #region public static bool IsNull<T>(this T obj)
        /// <summary>
        /// 确定当前的值或引用就是 null
        /// </summary>
        /// <returns> 如果当前的值或引用是 null，则为 true；否则为 false。 </returns>
        public static bool IsNull<T>(this T obj)
            => obj == null;
#if Xunit
        [Fact]
        public static void IsNullTest()
        {
            object obj = null;
            Assert.True(obj.IsNull());

            obj = default;
            Assert.True(obj.IsNull());

            obj = new object();
            Assert.False(obj.IsNull());
        }
#endif
        #endregion
        #region public static bool IsNotNull<T>(this T obj)
        /// <summary>
        /// 确定当前的值或引用不是 null
        /// </summary>
        /// <returns> 如果当前的值或引用不是 null，则为 true；否则为 false。 </returns>
        public static bool IsNotNull<T>(this T obj)
            => obj != null;
#if Xunit
        [Fact]
        public static void IsNotNullTest()
        {
            object obj = null;
            Assert.False(obj.IsNotNull());

            obj = default;
            Assert.False(obj.IsNotNull());

            obj = new object();
            Assert.True(obj.IsNotNull());
        }
#endif 
        #endregion

    }
}
