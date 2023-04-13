﻿using Microsoft.Extensions.Options;
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
        public async Task SendEmail(string emailAddress)
        {
            using var client = new SmtpClient();

            client.Host = "smtp.gmail.com";
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

                message.Subject = "Hello from code!";
                message.Body = "Loremn ipsum dolor sit amet ...";

                client.Send(message);
            }
        }
    }
}