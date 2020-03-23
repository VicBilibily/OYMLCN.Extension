using OYMLCN.ArgumentChecker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// DirectoryInfoExtension
    /// </summary>
    public static class DirectoryInfoExtension
    {
        #region public static long GetTotalLength(this DirectoryInfo dir)
        /// <summary>
        /// 获取文件夹总大小(字节)
        /// <para> 如果文件夹不存在则返回 0 </para>
        /// </summary>
        /// <param name="dir"> 文件夹信息 </param>
        /// <returns> 文件夹内文件总大小(字节) </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dir"/> 不能为 null </exception>
        public static long GetTotalLength(this DirectoryInfo dir)
        {
            dir.ThrowIfNull(nameof(dir));
            if (!dir.Exists) return 0;
            long len = 0;
            foreach (FileInfo fi in dir.GetFiles())
                len += fi.Length;
            DirectoryInfo[] dis = dir.GetDirectories();
            if (dis.Length > 0)
                for (int i = 0; i < dis.Length; i++)
                    len += GetTotalLength(dis[i]);
            return len;
        }
        #endregion

        #region public static void Copy(this DirectoryInfo dir, string target)
        /// <summary>
        /// 将指定文件夹下面的所有内容复制到目标文件夹下面
        /// </summary>
        /// <param name="dir"> 文件夹信息 </param>
        /// <param name="target"> 目标路径路径 </param>
        /// <exception cref="ArgumentNullException"> <paramref name="dir"/> 不能为 null </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="target"/> 不能为 null </exception>
        /// <exception cref="ArgumentException"> <paramref name="target"/> 不能为空 </exception>
        public static void Copy(this DirectoryInfo dir, string target)
        {
            dir.ThrowIfNull(nameof(dir));
            target.ThrowIfNullOrEmpty(nameof(target));
            if (!dir.Exists) return;
            if (target[target.Length - 1] != Path.DirectorySeparatorChar)
                target += Path.DirectorySeparatorChar;
            if (!Directory.Exists(target))
                Directory.CreateDirectory(target);
            string[] fileList = Directory.GetFileSystemEntries(dir.FullName);
            foreach (string file in fileList)
            {
                if (Directory.Exists(file))
                    file.GetDirectoryInfo().Copy(target + Path.GetFileName(file));
                else
                    File.Copy(file, target + Path.GetFileName(file), true);
            }
        } 
        #endregion

    }
}
