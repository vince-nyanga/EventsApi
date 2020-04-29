using System;
using System.Net.Mail;
using System.Threading.Tasks;
using EventsApi.Core.Interfaces;

namespace EventsApi.Infrastracture.Services
{
    public class LocalSmtpEmailService : IEmailService
    {
 
        public async Task SendEmailAsync(string emailAddress, string emailSubject, string emailBody)
        {
            using (var client = new SmtpClient("localhost"))
            {
                var mailMessage = new MailMessage("noreply@eventsapi.com", emailAddress, emailSubject, emailBody);
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
