using System;
using System.IO;
using System.Security;
using System.Text;
using OYMLCN.ArgumentChecker;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 文件操作相关扩展
    /// </summary>
    public static partial class FileInfoExtension
    {
        #region public static FileStream ReadToStream(this FileInfo file)
        /// <summary>
        /// 从文件信息创建 <see cref="FileStream"/> 类的只读实例
        /// </summary>
        /// <param name="file"> 文件信息实例 </param>
        /// <returns> <see cref="FileStream"/> 类的实例 </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="file"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="file"/> 是一个空字符串 ("")，仅包含空格，或者包含一个或多个无效字符。 </exception>
        /// <exception cref="ArgumentException"> <paramref name="file"/> 指向非文件设备，如 NTFS 环境中的“con:”、“com1:”、“lpt1:”等 </exception>
        /// <exception cref="NotSupportedException"> <paramref name="file"/> 指向非文件设备，如非 NTFS 环境中的“con:”、“com1:”、“lpt1:”等 </exception>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径无效，例如位于未映射的驱动器上 </exception>
        /// <exception cref="PathTooLongException"> 指定的路径和/或文件名超过了系统定义的最大长度 </exception>
        public static FileStream ReadToStream(this FileInfo file)
        {
            file.ThrowIfNull(nameof(file));
            return new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
        #endregion

        #region public static void WriteAllText(this FileInfo file, string contents)
        /// <summary>
        /// 基于文件信息创建一个新文件，向其中写入指定的字符串，然后关闭文件
        /// <para> 如果目标文件已存在，则覆盖该文件 </para>
        /// </summary>
        /// <param name="file"> 文件信息 </param>
        /// <param name="contents"> 要写入的字符串 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="file"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="contents"/> 不能为 null </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径无效，例如位于未映射的驱动器上 </exception>
        /// <exception cref="PathTooLongException"> 指定的路径和/或文件名超过了系统定义的最大长度 </exception>
        /// <exception cref="IOException"> 打开文件时发生 I/O 错误 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个只读文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个隐藏文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个目录 </exception>
        /// <exception cref="UnauthorizedAccessException"> 当前平台不支持此操作 或 调用方没有所要求的权限 </exception>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        public static void WriteAllText(this FileInfo file, string contents)
        {
            file.ThrowIfNull(nameof(file));
            contents.ThrowIfNull(nameof(contents));
            if (!file.Exists) file.Directory.Create();
            File.WriteAllText(file.FullName, contents);
        }
        #endregion
        #region public static void WriteAllText(this FileInfo file, string contents, Encoding encoding)
        /// <summary>
        /// 基于文件信息创建一个新文件，向其中写入指定的字符串，然后关闭文件
        /// <para> 如果目标文件已存在，则覆盖该文件 </para>
        /// </summary>
        /// <param name="file"> 文件信息 </param>
        /// <param name="contents"> 要写入的字符串 </param>
        /// <param name="encoding"> 应用于字符串的编码 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="file"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="contents"/> 不能为 null </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径无效，例如位于未映射的驱动器上 </exception>
        /// <exception cref="PathTooLongException"> 指定的路径和/或文件名超过了系统定义的最大长度 </exception>
        /// <exception cref="IOException"> 打开文件时发生 I/O 错误 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个只读文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个隐藏文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个目录 </exception>
        /// <exception cref="UnauthorizedAccessException"> 当前平台不支持此操作 或 调用方没有所要求的权限 </exception>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        public static void WriteAllText(this FileInfo file, string contents, Encoding encoding)
        {
            file.ThrowIfNull(nameof(file));
            contents.ThrowIfNull(nameof(contents));
            if (!file.Exists) file.Directory.Create();
            File.WriteAllText(file.FullName, contents, encoding);
        }
        #endregion

        #region public static void Append(this FileInfo file, string contents, bool appendEnd = true)
        /// <summary>
        /// 向当前文件追加文本内容
        /// </summary>
        /// <param name="file"> 文件信息 </param>
        /// <param name="contents"> 要追加的文本字符串 </param>
        /// <param name="appendEnd">在文件结尾添加，false时在文件开头添加</param>
        /// <exception cref="ArgumentNullException"> <paramref name="file"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="contents"/> 不能为 null </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径无效，例如位于未映射的驱动器上 </exception>
        /// <exception cref="PathTooLongException"> 指定的路径和/或文件名超过了系统定义的最大长度 </exception>
        /// <exception cref="IOException"> 打开文件时发生 I/O 错误 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个只读文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个隐藏文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个目录 </exception>
        /// <exception cref="UnauthorizedAccessException"> 当前平台不支持此操作 或 调用方没有所要求的权限 </exception>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        public static void Append(this FileInfo file, string contents, bool appendEnd = true)
        {
            file.ThrowIfNull(nameof(file));
            contents.ThrowIfNull(nameof(contents));
            if (!file.Exists) file.Directory.Create();

            if (appendEnd)
                using (var writer = file.AppendText())
                    writer.Write(contents);
            else
            {
                StringBuilder str = new StringBuilder();
                var temp = file.ReadAllText();
                str.Append(contents);
                str.Append(temp);
                file.WriteAllText(str.ToString());
            }
        }
        #endregion
        #region public static void Append(this FileInfo file, string contents, Encoding encoding, bool appendEnd = true)
        /// <summary>
        /// 向当前文件追加文本内容
        /// </summary>
        /// <param name="file"> 文件信息 </param>
        /// <param name="contents"> 要追加的文本字符串 </param>
        /// <param name="encoding"> 应用于字符串的编码 </param>
        /// <param name="appendEnd">在文件结尾添加，false时在文件开头添加</param>
        /// <exception cref="ArgumentNullException"> <paramref name="file"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="contents"/> 不能为 null </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径无效，例如位于未映射的驱动器上 </exception>
        /// <exception cref="PathTooLongException"> 指定的路径和/或文件名超过了系统定义的最大长度 </exception>
        /// <exception cref="IOException"> 打开文件时发生 I/O 错误 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个只读文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个隐藏文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个目录 </exception>
        /// <exception cref="UnauthorizedAccessException"> 当前平台不支持此操作 或 调用方没有所要求的权限 </exception>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        public static void Append(this FileInfo file, string contents, Encoding encoding, bool appendEnd = true)
        {
            file.ThrowIfNull(nameof(file));
            contents.ThrowIfNull(nameof(contents));
            if (!file.Exists) file.Directory.Create();

            if (appendEnd)
            {
                using (var fs = new FileStream(file.FullName, FileMode.Append, FileAccess.ReadWrite, FileShare.ReadWrite))
                using (var writer = new StreamWriter(fs, encoding))
                    writer.Write(contents);
            }
            else
            {
                StringBuilder str = new StringBuilder();
                var temp = file.ReadAllText(encoding);
                str.Append(contents);
                str.Append(temp);
                file.WriteAllText(str.ToString(), encoding);
            }
        }
        #endregion

        #region public static void AppendLine(this FileInfo file, string contents,bool appendEnd = true)
        /// <summary>
        /// 向当前文件追加文本内容
        /// </summary>
        /// <param name="file"> 文件信息 </param>
        /// <param name="contents"> 要追加的文本字符串 </param>
        /// <param name="appendEnd">在文件结尾添加，false时在文件开头添加</param>
        /// <exception cref="ArgumentNullException"> <paramref name="file"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="contents"/> 不能为 null </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径无效，例如位于未映射的驱动器上 </exception>
        /// <exception cref="PathTooLongException"> 指定的路径和/或文件名超过了系统定义的最大长度 </exception>
        /// <exception cref="IOException"> 打开文件时发生 I/O 错误 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个只读文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个隐藏文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个目录 </exception>
        /// <exception cref="UnauthorizedAccessException"> 当前平台不支持此操作 或 调用方没有所要求的权限 </exception>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        public static void AppendLine(this FileInfo file, string contents, bool appendEnd = true)
        {
            file.ThrowIfNull(nameof(file));
            contents.ThrowIfNull(nameof(contents));
            if (!file.Exists) file.Directory.Create();

            if (appendEnd)
                using (var writer = file.AppendText())
                    writer.WriteLine(contents);
            else
            {
                StringBuilder str = new StringBuilder();
                var temp = file.ReadAllText();
                str.AppendLine(contents);
                str.Append(temp);
                file.WriteAllText(str.ToString());
            }
        }
        #endregion
        #region public static void AppendLine(this FileInfo file, string contents, Encoding encoding, bool appendEnd = true)
        /// <summary>
        /// 向当前文件追加文本内容
        /// </summary>
        /// <param name="file"> 文件信息 </param>
        /// <param name="contents"> 要追加的文本字符串 </param>
        /// <param name="encoding"> 应用于字符串的编码 </param>
        /// <param name="appendEnd">在文件结尾添加，false时在文件开头添加</param>
        /// <exception cref="ArgumentNullException"> <paramref name="file"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="contents"/> 不能为 null </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径无效，例如位于未映射的驱动器上 </exception>
        /// <exception cref="PathTooLongException"> 指定的路径和/或文件名超过了系统定义的最大长度 </exception>
        /// <exception cref="IOException"> 打开文件时发生 I/O 错误 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个只读文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个隐藏文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个目录 </exception>
        /// <exception cref="UnauthorizedAccessException"> 当前平台不支持此操作 或 调用方没有所要求的权限 </exception>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        public static void AppendLine(this FileInfo file, string contents, Encoding encoding, bool appendEnd = true)
        {
            file.ThrowIfNull(nameof(file));
            contents.ThrowIfNull(nameof(contents));
            if (!file.Exists) file.Directory.Create();

            if (appendEnd)
            {
                using (var fs = new FileStream(file.FullName, FileMode.Append, FileAccess.ReadWrite, FileShare.ReadWrite))
                using (var writer = new StreamWriter(fs, encoding))
                    writer.WriteLine(contents);
            }
            else
            {
                StringBuilder str = new StringBuilder();
                var temp = file.ReadAllText(encoding);
                str.AppendLine(contents);
                str.Append(temp);
                file.WriteAllText(str.ToString(), encoding);
            }
        }
        #endregion

        #region public static string ReadAllText(this FileInfo file)
        /// <summary>
        ///  打开一个文本文件，读取文件中的所有文本，然后关闭此文件
        ///  <para> 如果文件不存在则返回空字符串 </para>
        /// </summary>
        /// <param name="file"> 文件信息 </param>
        /// <returns> 文件中所有文本字符串 </returns>
        /// <exception cref="ArgumentException"> <paramref name="file"/> 不能为 null </exception>
        /// <exception cref="PathTooLongException"> 指定的路径和/或文件名超过了系统定义的最大长度 </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径无效，例如位于未映射的驱动器上 </exception>
        /// <exception cref="IOException"> 打开文件时发生 I/O 错误 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个只读文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个隐藏文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个目录 </exception>
        /// <exception cref="UnauthorizedAccessException"> 当前平台不支持此操作 或 调用方没有所要求的权限 </exception>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        public static string ReadAllText(this FileInfo file)
        {
            file.ThrowIfNull(nameof(file));
            if (!file.Exists) return string.Empty;
            return File.ReadAllText(file.FullName);
        }
        #endregion
        #region public static string ReadAllText(this FileInfo file, Encoding encoding)
        /// <summary>
        ///  打开一个文本文件，读取文件中的所有文本，然后关闭此文件
        ///  <para> 如果文件不存在则返回空字符串 </para>
        /// </summary>
        /// <param name="file"> 文件信息 </param>
        /// <param name="encoding"> 应用于字符串的编码 </param>
        /// <returns> 文件中所有文本字符串 </returns>
        /// <exception cref="ArgumentException"> <paramref name="file"/> 不能为 null </exception>
        /// <exception cref="PathTooLongException"> 指定的路径和/或文件名超过了系统定义的最大长度 </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径无效，例如位于未映射的驱动器上 </exception>
        /// <exception cref="IOException"> 打开文件时发生 I/O 错误 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个只读文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个隐藏文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个目录 </exception>
        /// <exception cref="UnauthorizedAccessException"> 当前平台不支持此操作 或 调用方没有所要求的权限 </exception>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        public static string ReadAllText(this FileInfo file, Encoding encoding)
        {
            file.ThrowIfNull(nameof(file));
            if (!file.Exists) return string.Empty;
            return File.ReadAllText(file.FullName, encoding);
        }
        #endregion

        #region public static string ReadAllLines(this FileInfo file)
        /// <summary>
        ///  打开一个文本文件，读取文件中的所有文本行序列，然后关闭此文件
        ///  <para> 如果文件不存在则返回空字符串序列 </para>
        /// </summary>
        /// <param name="file"> 文件信息 </param>
        /// <returns> 文件中所有文本字符串行序列 </returns>
        /// <exception cref="ArgumentException"> <paramref name="file"/> 不能为 null </exception>
        /// <exception cref="PathTooLongException"> 指定的路径和/或文件名超过了系统定义的最大长度 </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径无效，例如位于未映射的驱动器上 </exception>
        /// <exception cref="IOException"> 打开文件时发生 I/O 错误 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个只读文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个隐藏文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个目录 </exception>
        /// <exception cref="UnauthorizedAccessException"> 当前平台不支持此操作 或 调用方没有所要求的权限 </exception>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        public static string[] ReadAllLines(this FileInfo file)
        {
            file.ThrowIfNull(nameof(file));
            if (!file.Exists) return new string[0];
            return File.ReadAllLines(file.FullName);
        }
        #endregion
        #region public static string ReadAllLines(this FileInfo file, Encoding encoding)
        /// <summary>
        ///  打开一个文本文件，读取文件中的所有文本行序列，然后关闭此文件
        ///  <para> 如果文件不存在则返回空字符串序列 </para>
        /// </summary>
        /// <param name="file"> 文件信息 </param>
        /// <param name="encoding"> 应用于字符串的编码 </param>
        /// <returns> 文件中所有文本字符串行序列 </returns>
        /// <exception cref="ArgumentException"> <paramref name="file"/> 不能为 null </exception>
        /// <exception cref="PathTooLongException"> 指定的路径和/或文件名超过了系统定义的最大长度 </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径无效，例如位于未映射的驱动器上 </exception>
        /// <exception cref="IOException"> 打开文件时发生 I/O 错误 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个只读文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个隐藏文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="file"/> 指定了一个目录 </exception>
        /// <exception cref="UnauthorizedAccessException"> 当前平台不支持此操作 或 调用方没有所要求的权限 </exception>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        public static string[] ReadAllLines(this FileInfo file, Encoding encoding)
        {
            file.ThrowIfNull(nameof(file));
            if (!file.Exists) return new string[0];
            return File.ReadAllLines(file.FullName, encoding);
        }
        #endregion

    }
}
