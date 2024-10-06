using Data;
using Data.Context;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Services
{
    public interface IComunicationService
    {
        bool SendEmail(string email, string body, string subject);
        bool SendEmail(List<string> email, string body, string subject);
    }
    public class ComunicationService : IComunicationService
    {
        private readonly DataContext _db;
        public ComunicationService(DataContext db)
        {
            _db = db;
        }

        public bool SendEmail(string emailClient, string body, string subject)

        {
            string server = _db.AppConfigurations.FirstOrDefault(x => x.Key == "ServerSMTP")?.Value;
            string user = _db.AppConfigurations.FirstOrDefault(x => x.Key == "Email")?.Value;
            string port = _db.AppConfigurations.FirstOrDefault(x => x.Key == "PortSMTP")?.Value;
            string pass = _db.AppConfigurations.FirstOrDefault(x => x.Key == "Password")?.Value;


            try
            {
                SmtpClient smtpClient = new SmtpClient
                {
                    Port = int.Parse(port),
                    Host = server,
                    //Timeout = 60000,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(user, pass),
                    TargetName = "Estrelas Ao Palco - Inscrições"
                };

                MailMessage email = new MailMessage();

                email.To.Add(new MailAddress(emailClient));
                email.From = new MailAddress(user);
                email.Subject = subject;
                email.Body = body;
                email.IsBodyHtml = true;
                email.Priority = MailPriority.High;
                smtpClient.Send(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool SendEmail(List<string> emails, string body, string subject)

        {
            string server = _db.AppConfigurations.FirstOrDefault(x => x.Key == "ServerSMTP")?.Value;
            string user = _db.AppConfigurations.FirstOrDefault(x => x.Key == "Email")?.Value;
            string port = _db.AppConfigurations.FirstOrDefault(x => x.Key == "PortSMTP")?.Value;
            string pass = _db.AppConfigurations.FirstOrDefault(x => x.Key == "Password")?.Value;


            try
            {
                SmtpClient smtpClient = new SmtpClient
                {
                    Port = int.Parse(port),
                    Host = server,
                    Timeout = 60000,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(user, pass)
                };

                MailMessage email = new MailMessage();
                foreach (var e in emails)
                {
                    email.To.Add(new MailAddress(e));
                }
                email.From = new MailAddress(user);
                email.Subject = subject;
                email.Body = body;
                email.IsBodyHtml = true;
                email.Priority = MailPriority.High;
                smtpClient.Send(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

