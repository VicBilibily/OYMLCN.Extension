#if NET461
//using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace OYMLCN.WinTrust
{
    /// <summary>
    /// 数字证书签名验证工具
    /// </summary>
    public static class AuthenticodeTools
    {
        ///// <summary>
        ///// 执行程序路径
        ///// </summary>
        ////static string CurrentProcessModulePath => Process.GetCurrentProcess().MainModule.FileName;
        //static string ExecutablePath => Assembly.GetEntryAssembly().Location;
        /// <summary>
        /// 本程序集路径
        /// </summary>
        ///<returns></returns>
        static string GetAssemblyPath => Assembly.GetExecutingAssembly().CodeBase.Substring(8);

        /// <summary>
        /// 检查执行程序签名
        /// </summary>
        /// <returns>如通过则返回证书信息，否则为Null</returns>
        public static X509Certificate2 CheckEXE() => CheckDLL(Helpers.SystemHelpers.ExecutablePath);

        /// <summary>
        /// 检查指定程序集签名
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <returns>如通过则返回证书信息，否则为Null</returns>
        public static X509Certificate2 CheckDLL(string filename)
        {
            // 通过系统内置的API验证进行证书有效性验证
            if (WinTrust.VerifyEmbeddedSignature(filename))
            {
                // 获取证书更多的信息进行进一步验证
                // 通过则返回证书以进一步自定义验证
                X509Certificate2 cert = new X509Certificate2(filename);
                return cert.Verify() ? cert : null;
            }
            return null;
        }
    }
}
#endif