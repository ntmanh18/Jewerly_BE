using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace Internship.Business.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmail(
            
            EmailSendingFormat sendingFormat)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("SmtpSettings:Username").Value));
            email.To.Add(MailboxAddress.Parse(sendingFormat.customer));
            email.Subject = sendingFormat.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = sendingFormat.Information };


            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("SmtpSettings:Host").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("SmtpSettings:Username").Value, _config.GetSection("SmtpSettings:Password").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            
        }
    }
}
