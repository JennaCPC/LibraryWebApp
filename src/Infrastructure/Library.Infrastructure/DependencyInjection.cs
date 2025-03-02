using Library.Application.Interfaces;
using Library.Infrastructure.Data;
using Library.Infrastructure.Extensions;
using Library.Infrastructure.Services;
using Library.Infrastructure.Services.EmailService;
using Library.Infrastructure.Services.AccountService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfiguration"));
            services.AddIdentityExtensions();
            services.AddAuthExtensions();

            services.AddScoped<AppIdentityDbInitialiser>();             
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<EmailSender>(); 
            return services;
        }
    }
}
