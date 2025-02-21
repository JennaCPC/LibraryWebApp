using Library.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.Extensions
{
    public static class InitialiserExtensions
    {
        public static async Task InitialiseDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<AppIdentityDbInitialiser>();

            await initialiser.SetUpRoles();

            await initialiser.SetUpPrimaryAdmin();
        }
    }

}
