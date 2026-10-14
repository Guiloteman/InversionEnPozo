using System.Net;
using System.Net.Mail;
using InversionEnPozo.Api.Servicios.Interfaces;
using Microsoft.Extensions.Configuration;

namespace InversionEnPozo.Api.Servicios
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarEmailAsync(string destinatario, string asunto, string mensajeHtml)
        {
            var smtpServer = _configuration["SmtpSettings:Server"];
            var port = int.Parse(_configuration["SmtpSettings:Port"] ?? "587");
            var senderName = _configuration["SmtpSettings:SenderName"];
            var senderEmail = _configuration["SmtpSettings:SenderEmail"];
            var password = _configuration["SmtpSettings:Password"];

            using var client = new SmtpClient(smtpServer, port)
            {
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail!, senderName),
                Subject = asunto,
                Body = mensajeHtml,
                IsBodyHtml = true
            };

            mailMessage.To.Add(destinatario);
            await client.SendMailAsync(mailMessage);
        }
    }
}