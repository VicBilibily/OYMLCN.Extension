using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OYMLCN.Extensions
{
    public static class DirectoryInfoExtension
    {
        #region DirectoryInfo

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

    }
}
