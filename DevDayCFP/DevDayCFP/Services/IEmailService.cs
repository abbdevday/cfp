using System.Collections.Generic;

namespace DevDayCFP.Services
{
    public interface IEmailService
    {
        void SendEmail(IEnumerable<string> emails, string subject, string content);
    }
}