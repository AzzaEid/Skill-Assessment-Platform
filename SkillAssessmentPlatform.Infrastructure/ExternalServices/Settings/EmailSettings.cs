namespace SkillAssessmentPlatform.Infrastructure.ExternalServices.Settings
{  /// need to enhance !!
    public class EmailSettings
    {
        /*
        private readonly IConfiguration _configuration;

        public EmailSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string message)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("CREDHUB", _configuration["EmailSettings:SenderEmail"]));
            email.To.Add(new MailboxAddress("", to));
            email.Subject = subject;

            email.Body = new TextPart("html") { Text = message };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _configuration["EmailSettings:SmtpServer"],
                int.Parse(_configuration["EmailSettings:SmtpPort"]),
                SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(
                _configuration["EmailSettings:SenderEmail"],
                _configuration["EmailSettings:SenderPassword"]
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
        /*/
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
    }
}
