using Microsoft.AspNetCore.Server.IIS.Core;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Riode.WebUI.AppCode.Extensions
{
    public static partial class NetworkExtension
    {
        public static bool SendMail(this IConfiguration configuration,
            string to,
            string subject,
            string body,
            bool appendCC = false)
        {
            try
            {
                string fromMail = configuration["emailAccount: userName"];
                string displayName = configuration["emailAccount: displayName"];
                string smtpServer = configuration["emailAccount: smtpServer"];
                int smtpPort = Convert.ToInt32(configuration["emailAccount: smtpPort"]);
                string password = configuration["emailAccount: password"];
                string cc = configuration["emailAccount: cc"];

                using (MailMessage message = new MailMessage(new MailAddress(fromMail, displayName), new MailAddress(to))
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                })
                {
                    if (!string.IsNullOrWhiteSpace(cc) && appendCC)
                        message.CC.Add(cc);

                    SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
                    smtpClient.Credentials = new NetworkCredential(fromMail, password);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(message);
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
