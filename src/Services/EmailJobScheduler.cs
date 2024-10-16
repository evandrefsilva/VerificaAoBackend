using Hangfire;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public interface IEmailJobScheduler
    {
        void ScheduleEmail(string to, string subject, string templatePath, object model);
    }
    public class EmailJobScheduler
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IEmailSenderService _emailSenderService;

        public EmailJobScheduler(IBackgroundJobClient backgroundJobClient, IEmailSenderService emailSender)
        {
            _backgroundJobClient = backgroundJobClient;
            _emailSenderService = emailSender;
        }

        public void ScheduleEmail(string to, string subject, string templatePath,string emailType, object model)
        {
            string emailBody = EmailTemplateHelper.BuildEmail(templatePath, emailType, model);

            _backgroundJobClient.Enqueue(() => _emailSenderService.SendEmailAsync(to, subject, emailBody));
        }
    }

}
