using Microsoft.Extensions.Options;
using Services.Models.Settings;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IEmailSenderService
    {

        Task SendEmailAsync(string to, string subject, string body);
    }

    public class EmailSenderService : IEmailSenderService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailSenderService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
            {
                client.EnableSsl = _smtpSettings.EnableSsl;

                var mailMessage = new MailMessage(_smtpSettings.From, to, subject, body);
                mailMessage.IsBodyHtml = true;

                await client.SendMailAsync(mailMessage);
            }
        }
    }


}
