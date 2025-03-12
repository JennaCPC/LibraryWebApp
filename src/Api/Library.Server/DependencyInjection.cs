using Library.Application;
using Library.Application.Interfaces;
using Library.Infrastructure;
using Library.Server.Common;

namespace Library.Server
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServerDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationDI() 
                .AddInfrastructureDI(configuration);
            services.AddScoped<IEmailTemplateProvider, EmailTemplateProvider>(); 
            return services;
        }
    }
}
