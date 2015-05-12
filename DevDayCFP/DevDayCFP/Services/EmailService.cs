using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using DevDayCFP.Models;

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

        public void SendRegistrationEmail(User user, string hostname, string emailTemplate)
        {
            var url = String.Format("{0}/account/activate/{1}", hostname, user.RegistrationToken);

            var smtpClient = new SmtpClient(SmtpHost, Int32.Parse(SmtpPort));
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(SmtpUser, SmtpPass);
            smtpClient.EnableSsl = true;

            var message = new MailMessage
            {
                From = new MailAddress("cfp@devday.pl", "DevDay CFP"),
                Subject = "DevDay 2015 CFP - Account Activation",
                Body = emailTemplate,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(user.Email));

            smtpClient.Send(message);
        }

        public void SendEmail(string email, string subject, string content)
        {
            var smtpClient = new SmtpClient(SmtpHost, Int32.Parse(SmtpPort));
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

            message.To.Add(new MailAddress(email));

            smtpClient.Send(message);
        }
    }
}