using Library.Application.Interfaces;
using Library.Infrastructure.Models;
using Library.Infrastructure.Services.EmailService;
using Library.Shared.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Library.Infrastructure.Services.Account
{
    public partial class AccountService : IAccountService
    {
        private readonly UserManager<UserModel> userManager;        
        private readonly EmailSender emailSender;
        private readonly IEmailTemplateProvider templateProvider;

        public AccountService(UserManager<UserModel> userManager, EmailSender emailSender,IEmailTemplateProvider templateProvider)
        {
            this.userManager = userManager;            
            this.emailSender = emailSender;
            this.templateProvider = templateProvider;
        }

        public static string GetCallbackUrl(string email, string token, string clientUri)
        {
            var param = new Dictionary<string, string?>
                {
                    {"email", email },
                    {"token", TokenCodec.EncodeToken(token) }
                };
            return QueryHelpers.AddQueryString(clientUri, param);
        }
    }
}
