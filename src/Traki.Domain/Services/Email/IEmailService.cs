namespace Traki.Domain.Services.Email
{
    public interface IEmailService
    {
        Task SendEmail(string emailAddress);
    }
}
