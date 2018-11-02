//using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
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
        static string AssemblyPath => Assembly.GetExecutingAssembly().CodeBase.Substring(8);

        /// <summary>
        /// 检查执行程序签名
        /// </summary>
        /// <returns>如通过则返回证书信息，否则为Null</returns>
        public static X509Certificate2 CheckEXE() => CheckDLL(Helpers.SystemHelpers.ExecutablePath);

        /// <summary>
        /// 检查指定程序集签名
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns>如通过则返回证书信息，否则为Null</returns>
        public static X509Certificate2 CheckDLL(string filepath)
        {
            // 通过系统内置的API验证进行证书有效性验证
            if (WinTrust.VerifyEmbeddedSignature(filepath))
            {
                // 获取证书更多的信息进行进一步验证
                // 通过则返回证书以进一步自定义验证
                X509Certificate2 cert = new X509Certificate2(filepath);
                return cert.Verify() ? cert : null;
            }
            return null;
        }

        /// <summary>
        /// 检查执行程序签名是否与指定程序集签名相同
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static bool CheckEXEAndDLLSignWithSameCodeSigningCertificate(string filepath)
        {
            var thisAssembly = CheckDLL(filepath);
            var currentProcess = CheckEXE();
            if (thisAssembly != null && currentProcess != null &&
                thisAssembly.Subject == currentProcess.Subject &&
                thisAssembly.Thumbprint == currentProcess.Thumbprint
                )
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取当前用户已注册的有效证书
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<X509Certificate2> CurrentUserCertificates
        {
            get
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);//获取本地计算机受信任的根证书的储存区
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection collection = store.Certificates;//获取储存区上的所有证书
                                                                           //X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByIssuerName, "demo", false);//找到所有demo颁发的证书
                foreach (var item in collection)
                    if (item.Verify())
                        yield return item;
            }
        }

    }
}
