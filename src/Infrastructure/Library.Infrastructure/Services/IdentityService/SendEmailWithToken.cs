using System.Text.Encodings.Web;
using Library.Infrastructure.Services.EmailService;
using Microsoft.AspNetCore.WebUtilities;

namespace Library.Infrastructure.Services.IdentityService
{
    public partial class IdentityService
    {
        public async Task SendEmailWithTokenAsync(string email, string token, string clientUri, string subject)
        {
            var param = new Dictionary<string, string?>
                {
                    {"email", email },
                    {"token", token }
                };
            var callback = QueryHelpers.AddQueryString(clientUri, param);
            var message = new EmailMessage([email], subject, HtmlEncoder.Default.Encode(callback));
            await emailSender.SendEmailAsync(message);
        }
    }
}
