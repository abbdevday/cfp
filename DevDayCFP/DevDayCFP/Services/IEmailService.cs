namespace DevDayCFP.Services
{
    public interface IEmailService
    {
        void SendEmail(string email, string subject, string content);
    }
}