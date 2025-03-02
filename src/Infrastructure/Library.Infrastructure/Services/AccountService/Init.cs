using Library.Application.Interfaces;
using Library.Infrastructure.Models;
using Library.Infrastructure.Services.EmailService;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure.Services.AccountService
{
    public partial class AccountService : IAccountService
    {
        private readonly UserManager<UserModel> userManager;
        private readonly EmailSender emailSender;

        public AccountService(UserManager<UserModel> userManager, EmailSender emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }        

    }
}
