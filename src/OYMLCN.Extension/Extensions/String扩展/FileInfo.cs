using System;
using System.IO;
using System.Security;

#if Xunit
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 文件操作相关扩展
    /// </summary>
    public static partial class FileInfoExtension
    {
        #region public static FileInfo GetFileInfo(this string fileName)
        /// <summary>
        /// 初始化作为文件路径的包装的 <seealso cref="FileInfo"/> 类的新实例，
        /// 如果路径有问题则返回 null
        /// </summary>
        /// <returns> <seealso cref="FileInfo"/> 类的新实例 </returns>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限 </exception>
        /// <exception cref="UnauthorizedAccessException"> 访问 <paramref name="fileName"/> 被拒绝 </exception>
        public static FileInfo GetFileInfo(this string fileName)
        {
            if (fileName == null) return null;
            try { return new FileInfo(fileName); }
            catch (SecurityException) { throw; }
            catch (UnauthorizedAccessException) { throw; }
            catch { return null; }
        }
#if Xunit
        [Fact]
        public static void GetFileInfoTest()
        {
            string filePath = null;
            Assert.Null(filePath.GetFileInfo());

            filePath = "X:/TestFile.file";
            Assert.IsType<FileInfo>(filePath.GetFileInfo());

            // 仅扩展创建内置类型的方式，不需要再次进行复杂的内部测试
        }
#endif
        #endregion

        #region public static DirectoryInfo GetDirectoryInfo(this string path)
        /// <summary>
        /// 初始化指定路径上的 <seealso cref="DirectoryInfo"/> 类的新实例，
        /// 如果路径有问题则返回 null
        /// </summary>
        /// <returns> <seealso cref="DirectoryInfo"/> 类的新实例 </returns>
        /// <exception cref="SecurityException"> 调用方没有所要求的权限。 </exception>
        public static DirectoryInfo GetDirectoryInfo(this string path)
        {
            if (path == null) return null;
            try { return new DirectoryInfo(path); }
            catch (SecurityException) { throw; }
            catch { return null; }
        }
#if Xunit
        [Fact]
        public static void GetDirectoryInfoTest()
        {
            string filePath = null;
            Assert.Null(filePath.GetDirectoryInfo());

            filePath = "X:/TestPath";
            Assert.IsType<DirectoryInfo>(filePath.GetDirectoryInfo());

            // 仅扩展创建内置类型的方式，不需要再次进行复杂的内部测试
        }
#endif 
        #endregion

    }
}
