using System;
using System.IO;
using System.IO.Compression;
using OYMLCN.ArgumentChecker;
#if Xunit
#pragma warning disable xUnit1013 // Public method should be marked as test
using Xunit;
#endif

namespace OYMLCN.Extensions
{
    /// <summary>
    /// 数据压缩扩展
    /// </summary>
    public static partial class CompressExtensions
    {
        #region public static void CreateZipFile(this DirectoryInfo sourceDirectoryInfo, string destinationArchiveFileName, CompressionLevel compressionLevel = CompressionLevel.Optimal, bool includeBaseDirectory = false)
        /// <summary> 创建 zip 存档，该存档包含指定目录的文件和目录 </summary>
        /// <param name="sourceDirectoryInfo"> 压缩文件夹信息 </param>
        /// <param name="destinationArchiveFileName"> 压缩文件路径 </param>
        /// <param name="compressionLevel"> 创建时是速度优先还是压缩效率优先的枚举值，默认以 <seealso cref="CompressionLevel.Optimal"/> 最佳方式 压缩 </param>
        /// <param name="includeBaseDirectory"> 是否包含存档根目录，true 则包括目录名称 sourceDirectoryName 的存档，false 则仅包括目录内的文件，默认值为 false </param>
        /// <exception cref="ArgumentNullException"> <paramref name="sourceDirectoryInfo"/> 为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="destinationArchiveFileName"/> 为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="destinationArchiveFileName"/> 为空 <see cref="string.Empty"/> </exception>
        /// <exception cref="PathTooLongException"> 指定的目录或文件名，超出了系统定义的最大长度 </exception>
        /// <exception cref="DirectoryNotFoundException"> sourceDirectory 无效或不存在 （例如，它位于未映射的驱动器上） </exception>
        /// <exception cref="IOException"> <paramref name="destinationArchiveFileName"/> 文件已存在 </exception>
        /// <exception cref="IOException"> 无法打开指定的目录中的文件 </exception>
        /// <exception cref="UnauthorizedAccessException"> <paramref name="destinationArchiveFileName"/> 指定的是目录路径 </exception>
        /// <exception cref="UnauthorizedAccessException"> 调用方没有所需的权限访问指定的 sourceDirectory 目录中的文件 </exception>
        /// <exception cref="NotSupportedException"> zip 存档不支持写入 </exception>
        public static void CreateZipFile(this DirectoryInfo sourceDirectoryInfo, string destinationArchiveFileName, CompressionLevel compressionLevel = CompressionLevel.Optimal, bool includeBaseDirectory = false)
        {
            sourceDirectoryInfo.ThrowIfNull(nameof(sourceDirectoryInfo));
            destinationArchiveFileName.ThrowIfNullOrEmpty(nameof(destinationArchiveFileName));
            ZipFile.CreateFromDirectory(sourceDirectoryInfo.FullName, destinationArchiveFileName, compressionLevel, includeBaseDirectory);
        }
        #endregion

        #region public static void ExtractZipFile(this FileInfo sourceArchiveFileInfo, string destinationDirectoryName)
        /// <summary> 将指定 zip 存档中的所有文件都解压缩到文件系统的一个目录下 </summary>
        /// <param name="sourceArchiveFileInfo"> 要解压缩存档的 <see cref="FileInfo"/> 的实例 </param>
        /// <param name="destinationDirectoryName"> 放置解压缩文件的目录的路径，指定为相对或绝对路径 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="sourceArchiveFileInfo"/> 为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="destinationDirectoryName"/> 为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="destinationDirectoryName"/> 为空 <see cref="string.Empty"/> </exception>
        /// <exception cref="PathTooLongException"> 指定的目录或文件名，超出了系统定义的最大长度 </exception>
        /// <exception cref="DirectoryNotFoundException"> 指定的路径 <paramref name="destinationDirectoryName"/> 无效或不存在 （例如，它位于未映射的驱动器上） </exception>
        /// <exception cref="IOException"> <paramref name="destinationDirectoryName"/> 指定的目录已存在 </exception>
        /// <exception cref="IOException"> 存档中的一个条目名是 <see cref="string.Empty"/>，仅包含空格，或者包含至少一个无效字符 </exception>
        /// <exception cref="IOException"> 提取存档条目将创建在指定的目录之外的文件 <paramref name="destinationDirectoryName"/> （如果条目名中包含父目录访问器，则可能发生这种情况） </exception>
        /// <exception cref="IOException"> 要提取的存档项具有与已经从同一存档提取的条目相同的名称 </exception>
        /// <exception cref="UnauthorizedAccessException"> 调用方没有所需的权限来访问存档或目标目录 </exception>
        /// <exception cref="NotSupportedException"> <paramref name="destinationDirectoryName"/> 包含无效格式 </exception>
        /// <exception cref="FileNotFoundException"> sourceArchiveFile 未找到 </exception>
        /// <exception cref="InvalidDataException"> 指定的存档 sourceArchiveFile 不是有效的 zip 存档 </exception>
        /// <exception cref="InvalidDataException"> 存档条目未找到或已损坏 </exception>
        /// <exception cref="InvalidDataException"> 使用了一种不支持的压缩方法压缩存档条目 </exception>
        public static void ExtractZipFile(this FileInfo sourceArchiveFileInfo, string destinationDirectoryName)
        {
            sourceArchiveFileInfo.ThrowIfNull(nameof(sourceArchiveFileInfo));
            destinationDirectoryName.ThrowIfNullOrEmpty(nameof(destinationDirectoryName));
            ZipFile.ExtractToDirectory(sourceArchiveFileInfo.FullName, destinationDirectoryName);
        } 
        #endregion

    }
}
