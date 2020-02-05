using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OYMLCN.Extensions
{
    public static class ObjectCloneExtension
    {
        /// <summary>
        /// Json序列化的方式实现深拷贝
        /// </summary>
        public static T JsonDeepClone<T>(this T t) where T : class, new()
            => t.ToJsonString().DeserializeJsonToObject<T>();
        /// <summary>
        /// XML序列化的方式实现深拷贝
        /// </summary>
        public static T XmlDeepClone<T>(this T t) where T : class, new()
        {
            //创建Xml序列化对象
            XmlSerializer xml = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())//创建内存流
            {
                //将对象序列化到内存中
                xml.Serialize(ms, t);
                ms.Position = default;//将内存流的位置设为0
                return (T)xml.Deserialize(ms);//继续反序列化
            }
        }
        /// <summary>
        /// 二进制序列化的方式进行深拷贝
        /// <para>确保需要拷贝的类里的所有成员已经标记为 [Serializable] 如果没有加该特性特报错</para>
        /// </summary>
        public static T BinaryDeepCopy<T>(this T t) where T : class, new()
        {
            //创建二进制序列化对象
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())//创建内存流
            {
                //将对象序列化到内存中
                bf.Serialize(ms, t);
                ms.Position = default;//将内存流的位置设为0
                return (T)bf.Deserialize(ms);//继续反序列化
            }
        }
        /// <summary>
        /// Reflection方式进行深拷贝
        /// </summary>
        public static T DeepClone<T>(this T obj) where T : class, new()
        {
            Type type = obj.GetType();
            // 对于没有公共无参构造函数的类型此处会报错
            T returnObj = Activator.CreateInstance(type) as T;
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                var fieldValue = field.GetValue(obj);
                // 值类型，字符串，枚举类型直接把值复制，不存在浅拷贝
                if (fieldValue.GetType().IsValueType || fieldValue.GetType().Equals(typeof(System.String)) || fieldValue.GetType().IsEnum)
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
                if (propertyValue.GetType().IsValueType || propertyValue.GetType().Equals(typeof(String)) || propertyValue.GetType().IsEnum)
                    property.SetValue(returnObj, propertyValue);
                else
                    property.SetValue(returnObj, DeepClone(propertyValue));
            }
            return returnObj;
        }
        /// <summary>
        /// Reflection方式进行浅拷贝（仅克隆值类型）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Clone<T>(this T obj) where T : class, new()
        {
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

    }
}
