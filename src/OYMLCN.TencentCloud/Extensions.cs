using System.Linq;
using TencentCloud.Sms.V20190711.Models;

namespace OYMLCN.Extensions
{
    /// <summary>
    /// TencentCloudExtensions
    /// </summary>
    public static partial class TencentCloudExtension
    {
        /// <summary>
        /// 判断短信发送全部成功
        /// </summary>
        public static bool IsAllOk(this SendSmsResponse response)
            => response.SendStatusSet.All(v => v.Code.EqualsIgnoreCase("Ok"));

    }
}

