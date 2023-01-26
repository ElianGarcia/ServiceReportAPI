using System.Net.Mail;
using System.Net;

namespace ServiceReportAPI.Utils
{
    public class Mails
    {
        public static string[] GetHTMLContent(string url)
        {
            string[] lines = File.ReadAllLines(url);
            Console.WriteLine(String.Join(Environment.NewLine, lines));
            return lines;
        }

        public static async Task SendMail(string mail, string asunto, string cuerpo)
        {
            var fromAddress = new MailAddress(ConfigurationManager.AppSetting["Mail:From"], ConfigurationManager.AppSetting["Mail:Username"]);
            var toAddress = new MailAddress(mail);
            string fromPassword = ConfigurationManager.AppSetting["Mail:Password"];
            string subject = asunto;
            string body = cuerpo;

            var smtp = new SmtpClient
            {
                Host = ConfigurationManager.AppSetting["Mail:Host"],
                Port = Convert.ToInt32(ConfigurationManager.AppSetting["Mail:Port"]),
                EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSetting["Mail:EnableSsl"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
