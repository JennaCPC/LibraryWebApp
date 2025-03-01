using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Library.Infrastructure.Services.EmailService
{
    public class EmailSender
    {
        private readonly EmailConfiguration emailConfig;

        public EmailSender(IOptions<EmailConfiguration> options)
        {
            this.emailConfig = options.Value;
        }
       
        public MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("", emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format("<a href={0} target='_blank'>Confirm Email</a>", message.Body) };
            return emailMessage;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }

        private async Task SendAsync(MimeMessage message)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(emailConfig.UserName, emailConfig.Password);
                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
