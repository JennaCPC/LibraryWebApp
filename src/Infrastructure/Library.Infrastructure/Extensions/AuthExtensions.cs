using Library.Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuthExtensions(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

            services.AddAuthentication(Cookies.CookieAuth).AddCookie(Cookies.CookieAuth, options =>
            {
                options.Cookie.Name = Cookies.CookieAuth;
            });

            services.AddAuthorization();

            return services;
        }

    }
}
