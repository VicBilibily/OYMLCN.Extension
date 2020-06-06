using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using OYMLCN.ArgumentChecker;
using System.Runtime.Serialization;
using System.Linq;
using System.Collections.Generic;
#if Xunit
using System.Data;
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 对象克隆扩展
    /// </summary>
    public static class ObjectCloneExtension
    {
        #region public static T JsonDeepClone<T>(this T value) where T : class, new()
        /// <summary>
        /// 采用 Json 序列化的方式实现深度对象拷贝
        /// </summary>
        /// <typeparam name="T"> 对象实例的类型 </typeparam>
        /// <param name="obj"> 要进行深度拷贝的对象实例 </param>
        /// <returns> 一个与 <paramref name="obj"/> 对象实例结构数据相同的新独立实例 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="obj"/> 不能为 null </exception>
        public static T JsonDeepClone<T>(this T obj) where T : class, new()
        {
            obj.ThrowIfNull(nameof(obj));
            return obj.JsonSerialize().JsonDeserialize<T>();
        }
#if Xunit
        public class DeepCloneClassTest
        {
            public int ID;
            public string Value { get; set; }
            public DeepCloneClass Object { get; set; }
        }
        public class DeepCloneClass
        {
            public int ID;
            public string ClassName { get; set; }
        }
        [Fact]
        public static void JsonDeepCloneTest()
        {
            DeepCloneClassTest classTest = null;
            Assert.Throws<ArgumentNullException>(() => classTest.JsonDeepClone());

            classTest = new DeepCloneClassTest()
            {
                ID = 0,
                Value = DateTime.Now.ToString(),
                Object = new DeepCloneClass()
                {
                    ID = 1,
                    ClassName = DateTime.Today.ToDateString()
                }
            };
            DeepCloneClassTest cloneClass = classTest.JsonDeepClone();
            Assert.NotSame(classTest, cloneClass);
            Assert.Equal(classTest.ID, cloneClass.ID);
            Assert.Equal(classTest.Value, cloneClass.Value);
            Assert.NotSame(classTest.Object, cloneClass.Object);
            Assert.Equal(classTest.Object.ID, cloneClass.Object.ID);
            Assert.Equal(classTest.Object.ClassName, cloneClass.Object.ClassName);
        }
#endif
        #endregion
        #region public static T XmlDeepClone<T>(this T obj) where T : class, new()
        /// <summary>
        /// 采用 XML 序列化的方式实现深度对象拷贝
        /// </summary>
        /// <typeparam name="T"> 对象实例的类型 </typeparam>
        /// <param name="obj"> 要进行深度拷贝的对象实例 </param>
        /// <returns> 一个与 <paramref name="obj"/> 对象实例结构数据相同的新独立实例 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="obj"/> 不能为 null </exception>
        public static T XmlDeepClone<T>(this T obj) where T : class, new()
        {
            obj.ThrowIfNull(nameof(obj));
            using (var stream = obj.XmlSerializeToStream())
                return stream.XmlDeserialize<T>();
        }
#if Xunit
        [Fact]
        public static void XmlDeepCloneTest()
        {
            DeepCloneClassTest classTest = null;
            Assert.Throws<ArgumentNullException>(() => classTest.XmlDeepClone());

            classTest = new DeepCloneClassTest()
            {
                ID = 0,
                Value = DateTime.Now.ToString(),
                Object = new DeepCloneClass()
                {
                    ID = 1,
                    ClassName = DateTime.Today.ToDateString()
                }
            };
            DeepCloneClassTest cloneClass = classTest.XmlDeepClone();
            Assert.NotSame(classTest, cloneClass);
            Assert.Equal(classTest.ID, cloneClass.ID);
            Assert.Equal(classTest.Value, cloneClass.Value);
            Assert.NotSame(classTest.Object, cloneClass.Object);
            Assert.Equal(classTest.Object.ID, cloneClass.Object.ID);
            Assert.Equal(classTest.Object.ClassName, cloneClass.Object.ClassName);
        }
#endif
        #endregion
        #region public static T BinaryDeepClone<T>(this T obj) where T : class, new()
        /// <summary>
        /// 采用二进制序列化的方式实现深度对象拷贝
        /// <para> 确保需要拷贝的类里的所有成员已经标记为 [Serializable]，如果没有加该特性特则报错 </para>
        /// </summary>
        /// <typeparam name="T"> 对象实例的类型 </typeparam>
        /// <param name="obj"> 要进行深度拷贝的对象实例 </param>
        /// <returns> 一个与 <paramref name="obj"/> 对象实例结构数据相同的新独立实例 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="obj"/> 不能为 null </exception>
        /// <exception cref="SerializationException"> <typeparamref name="T"/> 未标记为 [Serializable] 可序列化的，或者 <typeparamref name="T"/> 内包含为标记为[Serializable] 可序列化的类型 </exception>
        public static T BinaryDeepClone<T>(this T obj) where T : class, new()
        {
            obj.ThrowIfNull(nameof(obj));
            //创建二进制序列化对象
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())//创建内存流
            {
                //将对象序列化到内存中
                bf.Serialize(ms, obj);
                ms.Position = default;//将内存流的位置设为0
                return (T)bf.Deserialize(ms);//继续反序列化
            }
        }
#if Xunit
        [Serializable]
        public class DeepCloneSerializableFailTest
        {
            public int ID { get; set; }
            public string Value { get; set; }
            public DeepCloneClass Object { get; set; }
        }
        [Serializable]
        public class DeepCloneSerializableTest
        {
            public int ID { get; set; }
            public string Value { get; set; }
            public DeepCloneSerializableClass Object { get; set; }
        }
        [Serializable]
        public class DeepCloneSerializableClass
        {
            public int ID { get; set; }
            public string ClassName { get; set; }
        }
        [Fact]
        public static void BinaryDeepCopyTest()
        {
            DeepCloneClassTest fail = null;
            Assert.Throws<ArgumentNullException>(() => fail.BinaryDeepClone());
            fail = new DeepCloneClassTest();
            Assert.Throws<SerializationException>(() => fail.BinaryDeepClone());

            DeepCloneSerializableFailTest fail2 = new DeepCloneSerializableFailTest();
            Assert.Throws<SerializationException>(() => fail2.BinaryDeepClone());

            DeepCloneSerializableTest test = new DeepCloneSerializableTest()
            {
                ID = 0,
                Value = DateTime.Now.ToString(),
                Object = new DeepCloneSerializableClass()
                {
                    ID = 1,
                    ClassName = DateTime.Today.ToDateString()
                }
            };
            DeepCloneSerializableTest clone = test.BinaryDeepClone();
            Assert.NotSame(test, clone);
            Assert.Equal(test.ID, clone.ID);
            Assert.Equal(test.Value, clone.Value);
            Assert.NotSame(test.Object, clone.Object);
            Assert.Equal(test.Object.ID, clone.Object.ID);
            Assert.Equal(test.Object.ClassName, clone.Object.ClassName);
        }
#endif
        #endregion

        #region public static T DeepClone<T>(this T obj) where T : class, new()
        /// <summary>
        /// 采用 Reflection 对象反射的方式实现深度对象拷贝
        /// </summary>
        /// <typeparam name="T"> 对象实例的类型 </typeparam>
        /// <param name="obj"> 要进行深度拷贝的对象实例 </param>
        /// <returns> 一个与 <paramref name="obj"/> 对象实例结构数据相同的新独立实例 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="obj"/> 不能为 null </exception>
        public static T DeepClone<T>(this T obj) where T : class, new()
        {
            obj.ThrowIfNull(nameof(obj));
            Type type = obj.GetType();
            // 对于没有公共无参构造函数的类型此处会报错
            T returnObj = Activator.CreateInstance(type) as T;
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                var fieldValue = field.GetValue(obj);
                if (fieldValue == null) continue;
                // 值类型，字符串，枚举类型直接把值复制，不存在浅拷贝
                if (fieldValue.GetType().IsValueType || fieldValue is string || fieldValue.GetType().IsEnum)
                    field.SetValue(returnObj, fieldValue);
                else
                    field.SetValue(returnObj, DeepClone(fieldValue));
            }
            // 属性
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                var propertyValue = property.GetValue(obj);
                if (propertyValue == null) continue;
                if (propertyValue.GetType().IsValueType || propertyValue is string || propertyValue.GetType().IsEnum)
                    property.SetValue(returnObj, propertyValue);
                else
                    property.SetValue(returnObj, DeepClone(propertyValue));
            }
            return returnObj;
        }
#if Xunit
        [Fact]
        public static void DeepCloneTest()
        {
            DeepCloneClassTest classTest = null;
            Assert.Throws<ArgumentNullException>(() => classTest.DeepClone());

            classTest = new DeepCloneClassTest()
            {
                ID = 0,
                Value = DateTime.Now.ToString(),
                Object = new DeepCloneClass()
                {
                    ID = 1,
                    ClassName = DateTime.Today.ToDateString()
                }
            };
            DeepCloneClassTest cloneClass = classTest.DeepClone();
            Assert.NotSame(classTest, cloneClass);
            Assert.Equal(classTest.ID, cloneClass.ID);
            Assert.Equal(classTest.Value, cloneClass.Value);
            Assert.NotSame(classTest.Object, cloneClass.Object);
            Assert.Equal(classTest.Object.ID, cloneClass.Object.ID);
            Assert.Equal(classTest.Object.ClassName, cloneClass.Object.ClassName);
        }
#endif 
        #endregion
        #region public static T Clone<T>(this T obj) where T : class, new()
        /// <summary>
        /// 采用 Reflection 对象反射的方式实现引用对象拷贝（仅克隆值类型，引用类型保持不变）
        /// </summary>
        /// <typeparam name="T"> 对象实例的类型 </typeparam>
        /// <param name="obj"> 要进行深度拷贝的对象实例 </param>
        /// <returns> 一个与 <paramref name="obj"/> 对象实例结构数据相同的新独立实例 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="obj"/> 不能为 null </exception>
        public static T Clone<T>(this T obj) where T : class, new()
        {
            obj.ThrowIfNull(nameof(obj));
            Type type = obj.GetType();
            // 对于没有公共无参构造函数的类型此处会报错
            T returnObj = Activator.CreateInstance(type) as T;
            //字段
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                field.SetValue(returnObj, field.GetValue(obj));
            }
            //属性
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                property.SetValue(returnObj, property.GetValue(obj));
            }
            return returnObj;
        }
#if Xunit
        [Fact]
        public static void CloneTest()
        {
            DeepCloneClassTest classTest = null;
            Assert.Throws<ArgumentNullException>(() => classTest.Clone());

            classTest = new DeepCloneClassTest()
            {
                ID = 0,
                Value = DateTime.Now.ToString(),
                Object = new DeepCloneClass()
                {
                    ID = 1,
                    ClassName = DateTime.Today.ToDateString()
                }
            };
            DeepCloneClassTest cloneClass = classTest.Clone();
            Assert.NotSame(classTest, cloneClass);
            Assert.Equal(classTest.ID, cloneClass.ID);
            Assert.Equal(classTest.Value, cloneClass.Value);
            Assert.Same(classTest.Object, cloneClass.Object);
            Assert.Equal(classTest.Object.ID, cloneClass.Object.ID);
            Assert.Equal(classTest.Object.ClassName, cloneClass.Object.ClassName);
        }
#endif
        #endregion

        #region public static T AutoCopyTo<T>(this object obj) where T : class, new()
        /// <summary>
        /// 采用 Reflection 对象反射的方式实现引用对象跨目标类型拷贝
        /// </summary>
        /// <typeparam name="T"> 对象实例的类型 </typeparam>
        /// <param name="obj"> 要进行拷贝的对象实例 </param>
        /// <param name="onlyValueType"> 只拷贝值类型，默认false </param>
        /// <returns> 一个与 <paramref name="obj"/> 对象的数据结构相同的新目标对象实例 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="obj"/> 不能为 null </exception>
        public static T AutoCopyTo<T>(this object obj, bool onlyValueType = false) where T : class, new()
        {
            obj.ThrowIfNull(nameof(obj));
            var sourceType = obj.GetType();
            var returnType = typeof(T);
            T returnObj = Activator.CreateInstance(returnType) as T;
            // 对于没有公共无参构造函数的类型会报错
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var sourceField = sourceType.GetFields(flags);
            var returnField = returnType.GetFields(flags);
            var sourceProperty = sourceType.GetProperties(flags);
            var returnProperty = returnType.GetProperties(flags);
            for (int i = 0; i < sourceField.Length; i++)
            {
                FieldInfo field = sourceField[i];
                var fieldValue = field.GetValue(obj);
                var setField = returnField.FirstOrDefault(v => v.Name == field.Name && v.FieldType == field.FieldType);
                if (setField == null || fieldValue == null) continue;
                if (fieldValue.GetType().IsValueType || fieldValue is string || fieldValue.GetType().IsEnum)
                    setField.SetValue(returnObj, fieldValue);
                else if (onlyValueType == false)
                    setField.SetValue(returnObj, DeepClone(fieldValue));
            }
            for (int i = 0; i < sourceProperty.Length; i++)
            {
                PropertyInfo property = sourceProperty[i];
                var propertyValue = property.GetValue(obj);
                var setProperty = returnProperty.FirstOrDefault(v => v.Name == property.Name && v.PropertyType == property.PropertyType);
                if (setProperty == null || propertyValue == null) continue;
                if (propertyValue.GetType().IsValueType || propertyValue is string || propertyValue.GetType().IsEnum)
                    setProperty.SetValue(returnObj, propertyValue);
                else if (onlyValueType == false)
                    setProperty.SetValue(returnObj, DeepClone(propertyValue));
            }
            return returnObj;
        }
        #endregion
        #region public static T AutoCopyValues<T>(this T obj) where T : class, new()
        /// <summary>
        /// 采用 Reflection 对象反射的方式实现引用对象跨目标类型拷贝值
        /// </summary>
        /// <typeparam name="T"> 对象实例的类型 </typeparam>
        /// <param name="obj"> 要进行拷贝的对象实例 </param>
        /// <returns> 一个与 <paramref name="obj"/> 对象的数据结构相同的新目标对象实例 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="obj"/> 不能为 null </exception>
        public static T AutoCopyValues<T>(this T obj) where T : class, new()
            => AutoCopyTo<T>(obj, true);
        #endregion
        #region public static List<T> AutoCopyValues<T>(this List<T> list) where T : class, new()
        /// <summary>
        /// 采用 Reflection 对象反射的方式实现引用对象跨目标类型拷贝值
        /// </summary>
        /// <typeparam name="T"> 对象实例的类型 </typeparam>
        /// <param name="list"> 要进行拷贝的对象实例集合 </param>
        /// <returns> 一个与 <paramref name="list"/> 集合内对象的数据结构相同的新目标对象实例 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="list"/> 不能为 null </exception>
        public static List<T> AutoCopyValues<T>(this List<T> list) where T : class, new()
            => list.Select(obj => AutoCopyTo<T>(obj, true)).ToList();
        #endregion

    }
}
