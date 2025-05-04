using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace TUQA_Shop.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string Message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("abusharktuqa@gmail.com","")
            };
            return client.SendMailAsync(
                new MailMessage(from: "abusharktuqa@gmail.com",
                to: email,
                subject,
                Message
                )
                {
                    IsBodyHtml = true
                });
        }
    }
}
