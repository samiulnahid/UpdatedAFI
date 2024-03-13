using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Services
{
    public interface IEmailService
    {
        bool Send(Email email);
    }

    public class EmailService : IEmailService
    {
        public bool Send(Email email)
        {
            try
            {
                var host = ConfigurationManager.AppSettings["SMTPhost"];
                var port = ConfigurationManager.AppSettings["SMTPport"];
                var username = ConfigurationManager.AppSettings["SMTPuserName"];
                var password = ConfigurationManager.AppSettings["SMTPpassword"];
                var authMethod = ConfigurationManager.AppSettings["SMTPauthenticationMethod"];
                var tls = ConfigurationManager.AppSettings["SMTPstartTls"];
                var formEmail = ConfigurationManager.AppSettings["SMTPfromEmail"];

                int _port = string.IsNullOrEmpty(port) ? 0 : Convert.ToInt32(port);

                var client = new SmtpClient(host: host, port: _port);
                client.Credentials = new NetworkCredential(username, password);

                var message = new MailMessage();
                message.Subject = email.Subject;
                message.Body = email.Body;
                message.IsBodyHtml = email.IsBodyHtml;
                message.From = string.IsNullOrWhiteSpace(email.FromName)
                    ? new MailAddress(email.FromEmail)
                    : new MailAddress(email.FromEmail, email.FromName);

                foreach (var attachement in email.Attachments)
                {
                    message.Attachments.Add(attachement);
                }

                foreach (var address in email.To)
                {
                    if (string.IsNullOrWhiteSpace(address))
                        continue;

                    message.To.Add(address);
                }

                foreach (var address in email.Cc)
                {
                    if (string.IsNullOrWhiteSpace(address))
                        continue;

                    message.CC.Add(address);
                }

                foreach (var address in email.Bcc)
                {
                    if (string.IsNullOrWhiteSpace(address))
                        continue;

                    message.Bcc.Add(address);
                }

                client.Send(message);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"{nameof(EmailService)}: Error sending email", ex, this);
                throw;
            }
            return true;
        }
    }
}
