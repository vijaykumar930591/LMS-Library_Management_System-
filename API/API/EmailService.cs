

using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace API
{
    public class EmailService
    {
        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public string? FromEmail { get; private set; }

        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var fromEmail = Configuration.GetSection("Constants:FromEmail").Value ?? string.Empty;
                var fromEmailPassword = Configuration.GetSection("Constants:EmailAccountPassword").Value ?? string.Empty;

                var message = new MailMessage()
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                message.To.Add(toEmail);

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, fromEmailPassword),
                    EnableSsl = true,
                };

                smtpClient.Send(message);
            }
            catch (SmtpException smtpException)
            {
                // Handle SMTP-related exceptions
                Console.WriteLine($"SMTP Exception: {smtpException.Message}");
                // You can log the exception, send a notification, or perform other error-handling tasks.
                // Consider logging the exception using a logging framework like Serilog or NLog.
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}