using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
using System.Threading.Tasks;

namespace AcmeCaseAPI.Application.Common.Email
{
    public class Email : IEmail
    {
        private readonly IConfiguration _config;

        public Email(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string to, string subject, string html)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetValue<string>("SmtpSettings:From")));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.GetValue<string>("SmtpSettings:Host"), Convert.ToInt32(_config.GetValue<string>("SmtpSettings:Port")), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetValue<string>("SmtpSettings:User"), _config.GetValue<string>("SmtpSettings:Pass"));
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
