using OYMLCN.Extensions;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;

namespace OYMLCN.Helpers
{
    /// <summary>
    /// 进程或线程相关操作
    /// </summary>
    public static partial class ProcessHelpers
    {
        /// <summary>
        /// 挂起线程（Thread.Sleep一年）
        /// </summary>
        public static void Hold()
        {
            while (true)
                Thread.Sleep(1000 * 60 * 60 * 365);
        }
        /// <summary>
        /// 杀掉指定名称的所有程序
        /// </summary>
        /// <param name="processName">程序名称</param>
        public static void Kill(string processName)
        {
            var ps = Process.GetProcesses();
            foreach (var p in ps)
                if (p.ProcessName == processName)
                    p.Kill();
        }

#if NET461
        /// <summary>
        /// 确定当前主体是否属于具有指定 Administrator 的 Windows 用户组
        /// </summary>
        /// <returns>如果当前主体是指定的 Administrator 用户组的成员，则为 true；否则为 false。</returns>
        public static bool IsAdministrator
        {
            get
            {
                bool result;
                try
                {
                    WindowsIdentity identity = WindowsIdentity.GetCurrent();
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    result = principal.IsInRole(WindowsBuiltInRole.Administrator);

                    //http://www.cnblogs.com/Interkey/p/RunAsAdmin.html
                    //AppDomain domain = Thread.GetDomain();
                    //domain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                    //WindowsPrincipal windowsPrincipal = (WindowsPrincipal)Thread.CurrentPrincipal;
                    //result = windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
                }
                catch
                {
                    result = false;
                }
                return result;
            }
        }
        /// <summary>
        /// 以管理员身份执行程序
        /// </summary>
        /// <param name="file"></param>
        /// <param name="args"></param>
        public static void RunAsAdministrator(FileInfo file, params string[] args)
        {
            if (IsAdministrator)
                try
                {
                    Process.Start(file.FullName, args.Join(" "));
                }
                catch { }
            else
            {
                var startInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    WorkingDirectory = file.DirectoryName,
                    FileName = file.FullName,
                    Arguments = args.Join(" "),
                    //设置启动动作,确保以管理员身份运行
                    Verb = "runas"
                };
                try
                {
                    Process.Start(startInfo);
                }
                catch
                {
                    return;
                }
            }
        }
#endif
    }
}
