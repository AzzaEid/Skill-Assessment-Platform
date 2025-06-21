using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SkillAssessmentPlatform.Application.Abstract;
using SkillAssessmentPlatform.Infrastructure.ExternalServices.Settings;

namespace SkillAssessmentPlatform.Infrastructure.ExternalServices
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _emailConfig;

        public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> config)
        {
            _logger = logger;
            _emailConfig = config.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Ratify", _emailConfig.SenderEmail));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailConfig.SenderEmail, _emailConfig.SenderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"Email sent to {to} with subject: {subject}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {to} with subject: {subject}");
                throw;
            }
        }
    }
}
