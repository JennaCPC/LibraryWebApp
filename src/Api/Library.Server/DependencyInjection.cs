using Library.Application;
using Library.Infrastructure;

namespace Library.Server
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServerDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationDI() 
                .AddInfrastructureDI(configuration); 
            return services;
        }
    }
}
