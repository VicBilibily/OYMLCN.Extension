using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// FileExtension
    /// </summary>
    public static class FileExtensions
    {
        #region FileInfo
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static FileInfo GetFileInfo(this string filePath)
        {
            try { return new FileInfo(filePath); }
            catch { return null; }
        }
        /// <summary>
        /// 获取文件流FileStream
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static FileStream ReadToStream(this FileInfo file)
            => file?.Exists ?? false ?
            new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite) : null;


        /// <summary>
        /// 将文本信息保存到文件
        /// <para>如果文件已经存在则直接覆盖</para>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content">文本内容</param>
        public static void WriteAllText(this FileInfo file, string content)
        {
            if (!file.Exists)
                file.Directory.Create();
            File.WriteAllText(file.FullName, content);
        }
        /// <summary>
        /// 向文本文件追加文本
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content">文本内容</param>
        /// <param name="appendOnEnd">在文件结尾添加，false时在文件开头添加</param>
        public static void Append(this FileInfo file, string content, bool appendOnEnd = true)
        {
            if (!file.Exists)
                file.Directory.Create();

            if (appendOnEnd)
                using (var writer = file.AppendText())
                    writer.Write(content);
            else
            {
                StringBuilder str = new StringBuilder();
                var temp = file.ReadAllText();
                str.Append(content);
                str.Append(temp);
                file.WriteAllText(str.ToString());
            }
        }
        /// <summary>
        /// 向文本文件追加文本
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content">文本内容</param>
        /// <param name="appendOnEnd">在文件结尾添加，false时在文件开头添加</param>
        public static void AppendLine(this FileInfo file, string content, bool appendOnEnd = true)
        {
            if (!file.Exists)
                file.Directory.Create();

            if (appendOnEnd)
                using (var writer = file.AppendText())
                    writer.WriteLine(content);
            else
            {
                StringBuilder str = new StringBuilder();
                var temp = file.ReadAllText();
                str.AppendLine(content);
                str.Append(temp);
                file.WriteAllText(str.ToString());
            }
        }

        /// <summary>
        /// 读取文本文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ReadAllText(this FileInfo file)
        {
            if (!file.Exists)
                return string.Empty;
            return File.ReadAllText(file.FullName);
        }
        #endregion

        #region DirectoryInfo
        /// <summary>
        /// 获取文件夹信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DirectoryInfo GetDirectoryInfo(this string path)
        {
            try { return new DirectoryInfo(path); }
            catch { return null; }
        }
        /// <summary>
        /// 获取文件夹总大小(字节)
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static long GetAllLength(this DirectoryInfo directory)
        {
            if (!directory.Exists)
                return 0;
            long len = 0;
            foreach (FileInfo fi in directory.GetFiles())
                len += fi.Length;
            DirectoryInfo[] dis = directory.GetDirectories();
            if (dis.Length > 0)
                for (int i = 0; i < dis.Length; i++)
                    len += GetAllLength(dis[i]);
            return len;
        }
        /// <summary>
        /// 将指定文件夹下面的所有内容copy到目标文件夹下面
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="target">目标路径路径</param>
        public static void FolderCopy(this DirectoryInfo directory, string target)
        {
            if (!directory.Exists)
                return;
            if (target[target.Length - 1] != Path.DirectorySeparatorChar)
                target += Path.DirectorySeparatorChar;
            if (!Directory.Exists(target))
                Directory.CreateDirectory(target);
            string[] fileList = Directory.GetFileSystemEntries(directory.FullName);
            foreach (string file in fileList)
            {
                if (Directory.Exists(file))
                    file.GetDirectoryInfo().FolderCopy(target + Path.GetFileName(file));
                else
                    File.Copy(file, target + Path.GetFileName(file), true);
            }
        }
        #endregion


        #region FileHash
        /// <summary>
        /// 获取路径文件的MD5摘要值(文件不存在时返回空值)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetMD5Hash(this FileInfo file)
        {
            if (file.Exists)
                using (FileStream temp = new FileStream(file.FullName, FileMode.Open))
                    return temp.GetMD5Hash();
            else
                return string.Empty;
        }
        /// <summary>
        /// 计算文件流的MD5摘要值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetMD5Hash(this Stream stream)
        {
            MD5 md5 = MD5.Create();
            byte[] retVal = md5.ComputeHash(stream);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
                sb.Append(retVal[i].ToString("x2"));
            return sb.ToString();
        }
        /// <summary>
        /// 获取路径文件的SHA1摘要值(文件不存在时返回空值)
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetSHA1Hash(this FileInfo file)
        {
            if (file.Exists)
                using (FileStream temp = new FileStream(file.FullName, FileMode.Open))
                    return temp.GetSHA1Hash();
            else
                return string.Empty;
        }
        /// <summary>
        /// 计算文件流的SHA1摘要值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetSHA1Hash(this Stream stream)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] retVal = sha1.ComputeHash(stream);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
                sb.Append(retVal[i].ToString("x2"));
            return sb.ToString();
        }
        #endregion

        #region TextReader ReadLines
        /// <summary>
        /// 获取文本全部行
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToLines(this TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
                yield return line;
        }
        /// <summary>
        /// 获取文本全部非空行
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<string> NonEmptyLines(this TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line == "") continue;
                yield return line;
            }
        }
        /// <summary>
        /// 获取文本全部非空格行
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<string> NonWhiteSpaceLines(this TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.IsNullOrWhiteSpace()) continue;
                yield return line;
            }
        }
        #endregion

    }
}
