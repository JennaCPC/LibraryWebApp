using Library.Application.Interfaces;
using Library.Infrastructure.Models;
using Library.Infrastructure.Services.EmailService;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure.Services.IdentityService
{
    public partial class IdentityService : IIdentityService
    {
        private readonly UserManager<UserModel> userManager;
        private readonly EmailSender emailSender;

        public IdentityService(UserManager<UserModel> userManager, EmailSender emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }        

    }
}
