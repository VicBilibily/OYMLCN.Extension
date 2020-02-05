using OYMLCN.ArgumentChecker;
using System;
using System.Collections.Generic;
using System.Linq;
#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 可枚举迭代扩展
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary> 确定序列是否不包含任何元素或引用 null </summary>
        /// <returns> 如果源序列不包含任何元素或引用 null，则为 true；否则为 false。 </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
            => source == null || source.Any() == false;
#if Xunit
        [Fact]
        public static void IsNullOrEmptyTest()
        {
            List<object> obj = null;
            Assert.True(obj.IsNullOrEmpty());

            obj = default;
            Assert.True(obj.IsNullOrEmpty());

            obj = new List<object>();
            Assert.True(obj.IsNullOrEmpty());

            obj.Add(new object());
            Assert.False(obj.IsNullOrEmpty());
        }
#endif

        /// <summary> 确定序列是否不包含任何元素 </summary>
        /// <returns> 如果源序列不包含任何元素，则为 true；否则为 false。 </returns>
        /// <exception cref="ArgumentNullException"> 源序列不能为 null </exception>
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            source.ThrowIfNull(nameof(source), "源序列不能为 null");
            return source.Any() == false;
        }
#if Xunit
        [Fact]
        public static void IsEmptyTest()
        {
            List<object> obj = null;
            Assert.Throws<ArgumentNullException>(() => obj.IsEmpty());

            obj = default;
            Assert.Throws<ArgumentNullException>(() => obj.IsEmpty());

            obj = new List<object>();
            Assert.True(obj.IsEmpty());

            obj.Add(new object());
            Assert.False(obj.IsEmpty());
        }
#endif

        /// <summary> 确定序列是否包含任何元素
        /// <para> 建议直接使用内置扩展方法 Any() </para>
        /// </summary>
        /// <returns> 如果源序列包含任何元素，则为 true；否则为 false。 </returns>
        /// <exception cref="ArgumentNullException"> 源序列不能为 null </exception>
        public static bool IsNotEmpty<T>(this IEnumerable<T> source)
        {
            source.ThrowIfNull(nameof(source), "源序列不能为 null");
            return source.Any();
        }
#if Xunit
        [Fact]
        public static void IsNotEmptyTest()
        {
            List<object> obj = null;
            Assert.Throws<ArgumentNullException>(() => obj.IsNotEmpty());

            obj = default;
            Assert.Throws<ArgumentNullException>(() => obj.IsNotEmpty());

            obj = new List<object>();
            Assert.False(obj.IsNotEmpty());

            obj.Add(new object());
            Assert.True(obj.IsNotEmpty());
        }
#endif

    }
}
