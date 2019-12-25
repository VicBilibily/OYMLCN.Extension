using System;
using System.Net;
using System.Net.Mail;

namespace OYMLCN
{
    /// <summary>
    /// 邮件发送
    /// </summary>
    public class EmailSender
    {
        /// <summary>
        /// 仅用于继承
        /// </summary>
        protected EmailSender() { }
        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="displayName">显示名称</param>
        /// <param name="smtp">SMTP地址</param>
        /// <param name="userName">邮箱地址/用户名</param>
        /// <param name="password">密码</param>
        /// <param name="port">端口（默认25）</param>
        public EmailSender(string displayName, string smtp, string userName, string password, int port = 25)
        {
            DisplayName = displayName;
            SMTP = smtp;
            UserName = userName;
            Password = password;
            Port = port;
            if (string.IsNullOrWhiteSpace(DisplayName))
                throw new Exception("邮件配置错误：From（DisplayName）未配置");
            if (string.IsNullOrWhiteSpace(SMTP))
                throw new Exception("邮件配置错误：SMTP节点未配置");
            if (string.IsNullOrWhiteSpace(UserName))
                throw new Exception("邮件配置错误：Username节点未配置");
            if (string.IsNullOrWhiteSpace(Password))
                throw new Exception("邮件配置错误：Password节点未配置");
            if (Port <= 0)
                throw new Exception("邮件配置错误：Port节点配置错误");
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        protected string DisplayName;
        /// <summary>
        /// SMTP地址
        /// </summary>
        protected string SMTP;
        /// <summary>
        /// 邮箱地址/用户名
        /// </summary>
        protected string UserName;
        /// <summary>
        /// 密码
        /// </summary>
        protected string Password;
        /// <summary>
        /// 端口
        /// </summary>
        protected int Port;

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">正文HTML</param>
        /// <param name="targets">目标邮箱</param>
        /// <returns>成功返回true，否则报错</returns>
        public bool SendEmail(string subject, string body, params string[] targets)
        {
            using (SmtpClient client = new SmtpClient())
            {
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = SMTP;
                client.Port = Port;
                client.Credentials = new NetworkCredential(UserName, Password);
                using (MailMessage msg = new MailMessage())
                {
                    msg.From = new MailAddress(UserName, DisplayName);
                    foreach (var target in targets)
                        msg.To.Add(target);
                    msg.Subject = subject;
                    msg.Priority = MailPriority.High;
                    msg.IsBodyHtml = true;
                    msg.Body = body;

                    client.Send(msg);
                }
            }
            return true;
        }

    }
}
