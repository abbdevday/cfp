using DevDayCFP.Models;

namespace DevDayCFP.Services
{
    public interface IEmailService
    {
        void SendRegistrationEmail(User user, string hostName);
        void SendEmail(string email, string subject, string content);
    }
}