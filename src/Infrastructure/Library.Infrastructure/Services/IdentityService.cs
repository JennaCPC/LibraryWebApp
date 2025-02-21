using System.Security.Claims;
using Library.Application.Features.Account.LoginUser;
using Library.Application.Features.Account.RegisterUser;
using Library.Application.Interfaces;
using Library.Application.Models;
using Library.Domain.Constants;
using Library.Infrastructure.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;


namespace Library.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<UserModel> userManager;

        public IdentityService(UserManager<UserModel> userManager, IAuthenticationService authService)
        {
            this.userManager = userManager; 
        }

        public async Task<Result> RegisterAsync(RegisterUserDto registerDto)
        {
            List<Error> errors = [];

            UserModel user = new()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Member");
                return Result.Success(); 
            }
            else
            {
                var identityErrors = result.Errors;

                if (identityErrors.Where(e => e.Code == "InvalidEmail").Any())
                {
                    errors.Add(new Error("email", "Please enter a valid email"));
                }
                else if (identityErrors.Where(e => e.Code == "DuplicateEmail" || e.Code == "DuplicateUserName").Any())
                {
                    errors.Add(new Error("email", "Email is already registered"));
                }

                if (identityErrors.Where(e => e.Code.Contains("Password")).Any()) { errors.Add(new Error("password", "Password is not complex enough")); };

                if (errors.Count != 0)
                {
                    return Result.Failure(ErrorStatus.INVALID_INPUT, errors);
                }
                else
                {
                    foreach (var error in identityErrors)
                    {
                        errors.Add(new Error(error.Code, error.Description));
                    }
                    return Result.Failure(ErrorStatus.INTERNAL_ERROR, errors);  

                }
            }
        }

        public async Task<(Result, ClaimsPrincipal?)> LoginAsync(LoginUserDto loginDto)
        {
            List<Error> errors = [];

            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                errors.Add(new Error("email", "Email cannot be found"));
                return (Result.Failure(ErrorStatus.NOT_FOUND, errors), default);
            }
            else if (await userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var claims = new List<Claim> {
                        new(ClaimTypes.Email, user.Email.ToString()),
                        new(ClaimTypes.Name, user.FirstName.ToString() + " " + user.LastName.ToString())
                };

                var roles = await userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, Cookies.CookieAuth);
                ClaimsPrincipal claimsPrincipal = new(identity);

                return (Result.Success(), claimsPrincipal);                 
            }
            else
            {
                errors.Add(new Error("password", "Password is incorrect"));
                return (Result.Failure(ErrorStatus.UNAUTHORIZED, errors), default);
            }
        }

    }
}
