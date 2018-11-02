using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// XmlExtension
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static T DeserializeXmlString<T>(this string xml)
            => (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(xml));

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T DeserializeXmlString<T>(this Stream stream)
            => (T)new XmlSerializer(typeof(T)).Deserialize(stream);

        /// <summary>
        /// 序列化到Xml字符串
        /// 说明：此方法序列化复杂类，如果没有声明XmlInclude等特性，可能会引发“使用 XmlInclude 或 SoapInclude 特性静态指定非已知的类型。”的错误。
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToXmlString<T>(this T obj)
            => new StreamReader(obj.ToXmlStream()).ReadToEnd();

        /// <summary>
        /// 序列化到Xml字符串流
        /// 说明：此方法序列化复杂类，如果没有声明XmlInclude等特性，可能会引发“使用 XmlInclude 或 SoapInclude 特性静态指定非已知的类型。”的错误。
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static Stream ToXmlStream<T>(this T obj)
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                new XmlSerializer(typeof(T)).Serialize(stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            stream.Position = 0;
            return stream;
        }


        /// <summary>
        /// 将Xml流转成XDocument
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
#if NET35
        [Obsolete("NET35支持不完整，默认使用UTF8加载数据流，其他编码请考虑将Stream转换为String类型载入")]
#endif
        public static XDocument ToXDocument(this Stream stream)
        {
#if NET35
            return XDocument.Load(stream.ReadToEnd());
#else
            return XDocument.Load(stream);
#endif
        }
        /// <summary>
        /// 将Xml字符串转成XDocument
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static XDocument ToXDocument(this string xml)
            => XDocument.Parse(xml);

        /// <summary>
        /// 获取指定名称的元素的值
        /// </summary>
        /// <param name="xdoc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string SelectValue(this XDocument xdoc, string name)
            => xdoc?.Root?.Element(name)?.Value;
        /// <summary>
        /// 获取指定名称的元素的值
        /// </summary>
        /// <param name="xdoc"></param>
        /// <param name="name">元素名称</param>
        /// <returns></returns>
        public static string SelectValue(this XElement xdoc, string name)
            => xdoc?.Element(name)?.Value;
        /// <summary>
        /// 获取元素集合内指定名称的元素的首要值
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="name">元素名称</param>
        /// <returns></returns>
        public static string SelectValue(this IEnumerable<XElement> nodes, string name)
            => nodes?.Elements(name)?.FirstOrDefault()?.Value;
        /// <summary>
        /// 获取元素集合内指定名称的元素的所有值
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string[] SelectValues(this IEnumerable<XElement> nodes, string name)
            => nodes?.Elements(name)?.Select(d => d.Value).ToArray();
    }
}
