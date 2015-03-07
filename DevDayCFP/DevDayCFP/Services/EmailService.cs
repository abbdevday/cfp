using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using DevDayCFP.Models;

namespace DevDayCFP.Services
{
    public class EmailService : IEmailService
    {
        public void SendRegistrationEmail(User user, string hostname)
        {
            var url = String.Format("{0}/account/activate/{1}", hostname, user.RegistrationToken);

            var smtpClient = new SmtpClient();

            var message = new MailMessage
            {
                From = new MailAddress("contact@devday.pl"),
                Subject = "DevDay 2015 CFP - Account Activation",
                Body = string.Format(@"<h1>Hey</h1>
                        <a href='{0}'>Click to activate account</a>", url),
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(user.Email));

            smtpClient.Send(message);
        }
    }
}