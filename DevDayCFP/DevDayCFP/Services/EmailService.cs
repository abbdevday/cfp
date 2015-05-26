using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace DevDayCFP.Services
{
    public class EmailService : IEmailService
    {
        public string SmtpPass { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpHost { get; set; }

        public EmailService()
        {
            SmtpHost = ConfigurationManager.AppSettings["SMTP_Host"];
            SmtpPort = ConfigurationManager.AppSettings["SMTP_Port"];
            SmtpUser = ConfigurationManager.AppSettings["SMTP_Login"];
            SmtpPass = ConfigurationManager.AppSettings["SMTP_Pass"];
        }

        public void SendEmail(IEnumerable<string> emails, string subject, string content)
        {
            using (var smtpClient = new SmtpClient(SmtpHost, Int32.Parse(SmtpPort)))
            {
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(SmtpUser, SmtpPass);
                smtpClient.EnableSsl = true;

                var message = new MailMessage
                {
                    From = new MailAddress("cfp@devday.pl", "DevDay CFP"),
                    Subject = subject,
                    Body = content,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true
                };

                foreach (string email in emails)
                {
                    message.To.Add(new MailAddress(email));
                }

                smtpClient.Send(message);
            }
        }
    }
}