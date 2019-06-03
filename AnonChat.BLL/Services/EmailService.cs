using AnonChat.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AnonChat.BLL.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("yanastalin99@gmail.com", "fuck164466")
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("yanastalin99@gmail.com")
            };
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            return client.SendMailAsync(mailMessage);
        }
    }
}
