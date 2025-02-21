using Library.Domain.Constants;
using Library.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure.Data
{
    public class AppIdentityDbInitialiser            
    {
        private readonly UserManager<UserModel> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AppIdentityDbInitialiser(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager) {
            this.userManager = userManager;
            this.roleManager = roleManager;
        } 
        public async Task SetUpRoles()
        {
            var roles = Roles.RoleTypes;
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public async Task SetUpPrimaryAdmin()
        {
            string email = "admin@admin.com";
            string password = "Qwerty123?";
            if (await userManager.FindByEmailAsync(email) == null)
            {
                UserModel user = new()
                {
                    FirstName = "Main",
                    LastName = "Admin",
                    Email = email,
                    UserName = email
                };

                await userManager.CreateAsync(user, password);

                await userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
    
}
