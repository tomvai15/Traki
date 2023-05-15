using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Traki.Domain.Services.Email;

namespace Traki.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEmail(string emailAddress, string subject, string body)
        {
            using var client = new SmtpClient();

            client.Host = _emailSettings.Host;
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(_emailSettings.Address, _emailSettings.Password);
            using (var message = new MailMessage(
                from: new MailAddress(_emailSettings.Address, "TrakiAPP"),
                to: new MailAddress(emailAddress, "THEIR NAME")
                ))
            {
                message.Subject = subject;
                message.Body = body;

                client.Send(message);
            }
        }
    }
}
