using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using OYMLCN.ArgumentChecker;

#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 类型属性辅助扩展
    /// </summary>
    public static class AttributeExtension
    {
        #region public static TAttribute GetAttribute<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
        /// <summary>
        /// 获取特定成员的指定属性
        /// </summary>
        /// <typeparam name="TAttribute"> 属性类型 </typeparam>
        /// <param name="memberInfo"> 成员信息 </param>
        /// <returns> <typeparamref name="TAttribute"/> 指定的属性 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="memberInfo"/> 不能为 null </exception>
        public static TAttribute GetAttribute<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
        {
            memberInfo.ThrowIfNull(nameof(memberInfo));
            return (TAttribute)(memberInfo.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault());
        }
#if Xunit
        [Fact]
        public static void GetAttributeTest()
        {
            MemberInfo info = null;
            Assert.Throws<ArgumentNullException>(() => info.GetAttribute<DescriptionAttribute>());
            // 其他特殊情况特殊处理，这里不测试
        }
#endif 
        #endregion

        #region public static TAttribute[] GetAttributes<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
        /// <summary>
        /// 获取特定成员的指定属性序列
        /// </summary>
        /// <typeparam name="TAttribute"> 属性类型 </typeparam>
        /// <param name="memberInfo"> 成员信息 </param>
        /// <returns> <typeparamref name="TAttribute"/> 指定的属性序列 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="memberInfo"/> 不能为 null </exception>
        public static TAttribute[] GetAttributes<TAttribute>(this MemberInfo memberInfo) where TAttribute : Attribute
        {
            memberInfo.ThrowIfNull(nameof(memberInfo));
            return Array.ConvertAll(memberInfo.GetCustomAttributes(typeof(TAttribute), false), x => (TAttribute)x);
        }
#if Xunit
        [Fact]
        public static void GetAttributesTest()
        {
            MemberInfo info = null;
            Assert.Throws<ArgumentNullException>(() => info.GetAttributes<DescriptionAttribute>());
            // 其他特殊情况特殊处理，这里不测试
        }
#endif
        #endregion


        #region 单元测试类型
#if Xunit
        public class TestClass
        {
            public int IDValue;
            public string NameValue { get; set; }
            public void VoidMethod() { }
        }
        [Description("Test")]
        public class TestDesClass
        {
            [Description("ID")]
            public int IDValue;
            [Description("Name")]
            public string NameValue { get; set; }
            [Description("Method")]
            public void VoidMethod() { }
        }
#endif
        #endregion

        #region public static string GetDescription(this MemberInfo memberInfo)
        /// <summary>
        /// 获取公共成员的描述属性内容
        /// </summary>
        /// <param name="memberInfo"> 成员信息 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="memberInfo"/> 不能为 null </exception>
        public static string GetDescription(this MemberInfo memberInfo)
        {
            memberInfo.ThrowIfNull(nameof(memberInfo));
            var attribute = memberInfo.GetAttribute<DescriptionAttribute>();
            return attribute?.Description ?? string.Empty;
        }
#if Xunit
        [Fact]
        public static void GetDescriptionTest()
        {
            Type type = null;
            Assert.Throws<ArgumentNullException>(() => type.GetDescription());

            type = typeof(TestClass);
            Assert.Empty(type.GetDescription());

            type = typeof(TestDesClass);
            Assert.Equal("Test", type.GetDescription());
        }
#endif
        #endregion

        #region public static string GetFieldDescription(this Type type, string name)
        /// <summary>
        /// 获取公共字段成员的描述属性内容
        /// </summary>
        /// <param name="type"> 要获取成员描述的类类型 </param>
        /// <param name="name"> 要获取公共字段成员描述的名称 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="type"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="name"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="name"/> 不能为空 </exception>
        /// <exception cref="ArgumentException"> 指定名称为 <paramref name="name"/> 的公共字段在 <paramref name="type"/> 的类型中不存在 </exception>
        public static string GetFieldDescription(this Type type, string name)
        {
            type.ThrowIfNull(nameof(type), $"{nameof(type)} 不能为 null");
            name.ThrowIfNullOrEmpty(nameof(name));
            var field = type.GetField(name);
            if (field == null)
                throw new ArgumentException($"指定名称为 {nameof(name)} 的公共字段在 {nameof(type)} 的类型中不存在");
            return field.GetDescription();
        }
#if Xunit
        [Fact]
        public static void GetFieldDescriptionTest()
        {
            Type type = null;
            Assert.Throws<ArgumentNullException>(() => type.GetFieldDescription("Test"));

            type = typeof(TestClass);
            Assert.Throws<ArgumentNullException>(() => type.GetFieldDescription(null));
            Assert.Throws<ArgumentException>(() => type.GetFieldDescription(string.Empty));
            Assert.Empty(type.GetFieldDescription("IDValue"));

            type = typeof(TestDesClass);
            Assert.Equal("ID", type.GetFieldDescription("IDValue"));
            Assert.Throws<ArgumentException>(() => type.GetFieldDescription("NameValue"));
            Assert.Throws<ArgumentException>(() => type.GetFieldDescription("VoidMethod"));
        }
#endif
        #endregion

        #region public static string GetPropertyDescription(this Type type, string name)
        /// <summary>
        /// 获取公共属性成员的描述属性内容
        /// </summary>
        /// <param name="type"> 要获取成员描述的类类型 </param>
        /// <param name="name"> 要获取公共属性成员描述的名称 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="type"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="name"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="name"/> 不能为空 </exception>
        /// <exception cref="ArgumentException"> 指定名称为 <paramref name="name"/> 的公共属性在 <paramref name="type"/> 的类型中不存在 </exception>
        public static string GetPropertyDescription(this Type type, string name)
        {
            type.ThrowIfNull(nameof(type), $"{nameof(type)} 不能为 null");
            name.ThrowIfNullOrEmpty(nameof(name));
            var memberInfos = type.GetProperty(name);
            if (memberInfos == null)
                throw new ArgumentException($"指定名称为 {nameof(name)} 的公共属性在 {nameof(type)} 的类型中不存在");
            return memberInfos.GetDescription();
        }
#if Xunit
        [Fact]
        public static void GetPropertyDescriptionTest()
        {
            Type type = null;
            Assert.Throws<ArgumentNullException>(() => type.GetPropertyDescription("Test"));

            type = typeof(TestClass);
            Assert.Throws<ArgumentNullException>(() => type.GetPropertyDescription(null));
            Assert.Throws<ArgumentException>(() => type.GetPropertyDescription(string.Empty));
            Assert.Empty(type.GetPropertyDescription("NameValue"));

            type = typeof(TestDesClass);
            Assert.Throws<ArgumentException>(() => type.GetPropertyDescription("IDValue"));
            Assert.Equal("Name", type.GetPropertyDescription("NameValue"));
            Assert.Throws<ArgumentException>(() => type.GetPropertyDescription("VoidMethod"));
        }
#endif
        #endregion

        #region public static string GetMethodDescription(this Type type, string name)
        /// <summary>
        /// 获取公共方法成员的描述属性内容
        /// </summary>
        /// <param name="type"> 要获取成员描述的类类型 </param>
        /// <param name="name"> 要获取公共方法成员描述的名称 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="type"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="name"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="name"/> 不能为空 </exception>
        /// <exception cref="ArgumentException"> 指定名称为 <paramref name="name"/> 的公共方法在 <paramref name="type"/> 的类型中不存在 </exception>
        public static string GetMethodDescription(this Type type, string name)
        {
            type.ThrowIfNull(nameof(type), $"{nameof(type)} 不能为 null");
            name.ThrowIfNullOrEmpty(nameof(name));
            var memberInfos = type.GetMethod(name);
            if (memberInfos == null)
                throw new ArgumentException($"指定名称为 {nameof(name)} 的公共方法在 {nameof(type)} 的类型中不存在");
            return memberInfos.GetDescription();
        }
#if Xunit
        [Fact]
        public static void GetMethodDescriptionTest()
        {
            Type type = null;
            Assert.Throws<ArgumentNullException>(() => type.GetMethodDescription("Test"));

            type = typeof(TestClass);
            Assert.Throws<ArgumentNullException>(() => type.GetMethodDescription(null));
            Assert.Throws<ArgumentException>(() => type.GetMethodDescription(string.Empty));
            Assert.Empty(type.GetMethodDescription("VoidMethod"));

            type = typeof(TestDesClass);
            Assert.Throws<ArgumentException>(() => type.GetMethodDescription("IDValue"));
            Assert.Throws<ArgumentException>(() => type.GetMethodDescription("NameValue"));
            Assert.Equal("Method", type.GetMethodDescription("VoidMethod"));
        }
#endif
        #endregion

        #region public static string GetMemberDescription(this Type type, string name)
        /// <summary>
        /// 获取公共成员的描述属性内容
        /// </summary>
        /// <param name="type"> 要获取成员描述的类类型 </param>
        /// <param name="name"> 要获取公共成员描述的名称 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="type"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="name"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="name"/> 不能为空 </exception>
        /// <exception cref="ArgumentException"> 指定名称为 <paramref name="name"/> 的公共成员在 <paramref name="type"/> 的类型中不存在 </exception>
        public static string GetMemberDescription(this Type type, string name)
        {
            type.ThrowIfNull(nameof(type), $"{nameof(type)} 不能为 null");
            name.ThrowIfNullOrEmpty(nameof(name));
            var memberInfos = type.GetMember(name);
            if (!memberInfos.Any())
                throw new ArgumentException($"指定名称为 {nameof(name)} 的公共成员在 {nameof(type)} 的类型中不存在");
            return memberInfos[0].GetDescription();
        }
#if Xunit
        [Fact]
        public static void GetMemberDescriptionTest()
        {
            Type type = null;
            Assert.Throws<ArgumentNullException>(() => type.GetMemberDescription("Test"));

            type = typeof(TestClass);
            Assert.Throws<ArgumentNullException>(() => type.GetMemberDescription(null));
            Assert.Throws<ArgumentException>(() => type.GetMemberDescription(string.Empty));
            Assert.Empty(type.GetMemberDescription("IDValue"));

            type = typeof(TestDesClass);
            Assert.Equal("ID", type.GetMemberDescription("IDValue"));
            Assert.Equal("Name", type.GetMemberDescription("NameValue"));
            Assert.Equal("Method", type.GetMemberDescription("VoidMethod"));

        }
#endif 
        #endregion

    }
}
