using Library.Infrastructure.Data;
using Library.Infrastructure.Models;
using Library.Infrastructure.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityExtensions(this IServiceCollection services)
        {
            services.AddIdentityCore<UserModel>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<EmailConfirmationTokenProvider<UserModel>>("emailconfirmation");
            services.ConfigureIdentityOptions();
            return services;
        }

        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
            });
            
            services.Configure<EmailConfirmationTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromDays(1));
            services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromDays(1)); 

            return services;
        }

    }
}
