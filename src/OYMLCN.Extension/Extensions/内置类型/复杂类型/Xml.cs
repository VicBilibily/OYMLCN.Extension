using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using OYMLCN.ArgumentChecker;
#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// XML 文档相关扩展
    /// </summary>
    public static class XmlExtensions
    {
        #region public static Stream XmlSerializeToStream<T>(this T value) where T : class
        /// <summary>
        /// 将当前的对象实例 <paramref name="value"/> 序列化为 XML 数据流
        /// </summary>
        /// <typeparam name="T"> 对象类型，必须是一个类 </typeparam>
        /// <param name="value"> 要进行 XML 序列化的对象 </param>
        /// <returns> 使用默认参数序列化后的 XML 数据流 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        /// <exception cref="InvalidOperationException"> 序列化时发生错误 </exception>
        public static Stream XmlSerializeToStream<T>(this T value) where T : class
        {
            value.ThrowIfNull(nameof(value));
            var stream = new MemoryStream();
            // 将对象序列化到内存中
            new XmlSerializer(typeof(T)).Serialize(stream, value);
            // 将内存流的位置设为0
            stream.Position = default;
            return stream;
        }
        #endregion
        #region public static string XmlSerializeToDocument<T>(this T value) where T : class
        /// <summary>
        /// 将当前的对象实例 <paramref name="value"/> 序列化为 XML 标准文档
        /// </summary>
        /// <typeparam name="T"> 对象类型，必须是一个类 </typeparam>
        /// <param name="value"> 要进行 XML 序列化的对象 </param>
        /// <returns> 使用默认参数序列化后的 XML 标准文档 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        /// <exception cref="InvalidOperationException"> 序列化时发生错误 </exception>
        /// <exception cref="OutOfMemoryException"> 序列化为字符串时内存不足 </exception>
        public static string XmlSerializeToDocument<T>(this T value) where T : class
        {
            value.ThrowIfNull(nameof(value));
            using (var stream = value.XmlSerializeToStream())
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
        #endregion

        #region public static string XmlSerializeToString<T>(this T value, Encoding encoding) where T : class
        /// <summary>
        /// 使用 <paramref name="encoding"/> 指定的编码，将当前的对象实例 <paramref name="value"/> 序列化为 XML 表示的字符串
        /// </summary>
        /// <typeparam name="T"> 对象类型，必须是一个类 </typeparam>
        /// <param name="value"> 要进行 XML 序列化的对象 </param>
        /// <param name="encoding"> 一个 <see cref="Encoding"/> 编码方式 </param>
        /// <returns> 使用默认参数序列化后的 XML 标准文档 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        /// <exception cref="InvalidOperationException"> 序列化时发生错误 </exception>
        /// <exception cref="OutOfMemoryException"> 序列化为字符串时内存不足 </exception>
        public static string XmlSerializeToString<T>(this T value, Encoding encoding) where T : class
        {
            value.ThrowIfNull(nameof(value));

            XmlWriterSettings settings = new XmlWriterSettings();
            // 去除 XML 声明
            settings.OmitXmlDeclaration = true;
            settings.Encoding = encoding;
            using (MemoryStream stream = new MemoryStream())
            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                // 去除默认命名空间 xmlns:xsd 和 xmlns:xsi
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer formatter = new XmlSerializer(typeof(T));
                formatter.Serialize(writer, value, ns);
                return encoding.GetString(stream.ToArray());
            }
        }
        #endregion
        #region public static string XmlSerializeToString<T>(this T value) where T : class
        /// <summary>
        /// 使用默认编码 <see cref="Encoding.Default"/>，将当前的对象实例 <paramref name="value"/> 序列化为 XML 表示的字符串
        /// </summary>
        /// <typeparam name="T"> 对象类型，必须是一个类 </typeparam>
        /// <param name="value"> 要进行 XML 序列化的对象 </param>
        /// <returns> 使用默认参数序列化后的 XML 标准文档 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> 不能为 null </exception>
        /// <exception cref="InvalidOperationException"> 序列化时发生错误 </exception>
        /// <exception cref="OutOfMemoryException"> 序列化为字符串时内存不足 </exception>
        public static string XmlSerializeToString<T>(this T value) where T : class
            => XmlSerializeToString(value, Encoding.Default);
        #endregion


        #region public static T XmlDeserialize<T>(this Stream stream) where T : class, new()
        /// <summary>
        /// 反序列化包含 XML 数据的字节流为指定的实体对象
        /// </summary>
        /// <typeparam name="T"> 对象类型，必须是一个类 </typeparam>
        /// <param name="stream"> 包含 XML 数据形式的字节流 </param>
        /// <returns> 指定的 <typeparamref name="T"/> 对象实例 </returns>
        public static T XmlDeserialize<T>(this Stream stream) where T : class, new()
        {
            stream.ThrowIfNull(nameof(stream));
            return new XmlSerializer(typeof(T)).Deserialize(stream) as T;
        }
        #endregion
        #region public static T XmlDeserialize<T>(this string xml) where T : class, new()
        /// <summary>
        /// 反序列化包含 XML 文档字符串为指定的实体对象
        /// </summary>
        /// <typeparam name="T"> 对象类型，必须是一个类 </typeparam>
        /// <param name="xml"> 包含 XML 数据形式的字符串 </param>
        /// <returns> 指定的 <typeparamref name="T"/> 对象实例 </returns>
        public static T XmlDeserialize<T>(this string xml) where T : class, new()
        {
            xml.ThrowIfNull(nameof(xml));
            return (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(xml));
        }
        #endregion
        #region public static T XmlDeserialize<T>(this string xml, Encoding encoding) where T : class, new()
        /// <summary>
        /// 使用指定编码反序列化包含 XML 文档字符串为指定的实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"> 包含 XML 数据形式的字符串 </param>
        /// <param name="encoding"> 一个 <see cref="Encoding"/> 编码方式 </param>
        /// <returns> 指定的 <typeparamref name="T"/> 对象实例 </returns>
        public static T XmlDeserialize<T>(this string xml, Encoding encoding) where T : class, new()
        {
            xml.ThrowIfNull(nameof(xml));
            return (T)new XmlSerializer(typeof(T)).Deserialize(xml.GetEncodingBytes(encoding).ToStream());
        }
        #endregion

#if Xunit
        public class XmlClassTest
        {
            public int ID;
            public string Value { get; set; }
            public XmlObjectClass Object { get; set; }
        }
        public class XmlObjectClass
        {
            public int ID;
            public string ClassName { get; set; }
        }
        [Fact]
        public static void XmlSerializeAndDeserializeTest()
        {
            XmlClassTest classTest = null;
            Assert.Throws<ArgumentNullException>(() => classTest.XmlSerializeToStream());
            Assert.Throws<ArgumentNullException>(() => classTest.XmlSerializeToDocument());
            Assert.Throws<ArgumentNullException>(() => classTest.XmlSerializeToString());

            classTest = new XmlClassTest()
            {
                ID = 0,
                Value = DateTime.Now.ToString(),
                Object = new XmlObjectClass()
                {
                    ID = 1,
                    ClassName = DateTime.Today.ToDateString()
                }
            };

            #region 序列化
            Stream stream = classTest.XmlSerializeToStream();
            Assert.NotNull(stream);
            Assert.True(stream.Length > 0);

            string document = classTest.XmlSerializeToDocument();
            Assert.NotEmpty(document);
            Assert.StartsWith("<?xml", document);

            string xmlString = classTest.XmlSerializeToString();
            Assert.NotNull(xmlString);
            Assert.StartsWith("<XmlClassTest>", xmlString);

            string utf8XmlString = classTest.XmlSerializeToString(Encoding.UTF8);
            Assert.NotNull(utf8XmlString);
            Assert.StartsWith("<XmlClassTest>", utf8XmlString);
            #endregion

            #region 反序列化
            var desResult = stream.XmlDeserialize<XmlClassTest>();
            Assert.NotSame(classTest, desResult);
            Assert.Equal(classTest.ID, desResult.ID);
            Assert.Equal(classTest.Value, desResult.Value);
            Assert.NotSame(classTest.Object, desResult.Object);
            Assert.Equal(classTest.Object.ID, desResult.Object.ID);
            Assert.Equal(classTest.Object.ClassName, desResult.Object.ClassName);

            desResult = document.XmlDeserialize<XmlClassTest>();
            Assert.NotSame(classTest, desResult);
            Assert.Equal(classTest.ID, desResult.ID);
            Assert.Equal(classTest.Value, desResult.Value);
            Assert.NotSame(classTest.Object, desResult.Object);
            Assert.Equal(classTest.Object.ID, desResult.Object.ID);
            Assert.Equal(classTest.Object.ClassName, desResult.Object.ClassName);

            desResult = xmlString.XmlDeserialize<XmlClassTest>();
            Assert.NotSame(classTest, desResult);
            Assert.Equal(classTest.ID, desResult.ID);
            Assert.Equal(classTest.Value, desResult.Value);
            Assert.NotSame(classTest.Object, desResult.Object);
            Assert.Equal(classTest.Object.ID, desResult.Object.ID);
            Assert.Equal(classTest.Object.ClassName, desResult.Object.ClassName);

            desResult = utf8XmlString.XmlDeserialize<XmlClassTest>(Encoding.UTF8);
            Assert.NotSame(classTest, desResult);
            Assert.Equal(classTest.ID, desResult.ID);
            Assert.Equal(classTest.Value, desResult.Value);
            Assert.NotSame(classTest.Object, desResult.Object);
            Assert.Equal(classTest.Object.ID, desResult.Object.ID);
            Assert.Equal(classTest.Object.ClassName, desResult.Object.ClassName);
            #endregion
        }
#endif


        #region 以往的 XML 文档操作扩展
        /// <summary>
        /// 将Xml流转成XDocument
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static XDocument ToXDocument(this Stream stream)
            => XDocument.Load(stream);
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
        #endregion
    }
}