using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace UdbService.Services
{
    public class AuthMessageSender : IEmailSender
    {
        public AuthMessageSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public EmailSettings _emailSettings { get; }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            Execute(email, subject, htmlMessage).Wait();
            return Task.FromResult(0);
        }

        public async Task Execute(string email, string subject, string htmlMessage)
        {
            try
            {
               var From = new MailAddress("sha0ffat@gmail.com", "sha0ffat@gmail.com");
                MailMessage mail = new MailMessage
                {
                   From = From
                    
                };

                mail.To.Add(new MailAddress(email));
                mail.Subject = subject;
                mail.Body = htmlMessage;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com"))
                {
                    smtp.Port = 587;
                    smtp.Credentials = new NetworkCredential("sha0ffat@gmail.com", "Gma$7831132");
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
