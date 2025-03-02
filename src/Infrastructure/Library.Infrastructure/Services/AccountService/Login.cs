using System.Security.Claims;
using Library.Application.Features.Account.Queries.LoginUser;
using Library.Application.Models;
using Library.Domain.Constants;

namespace Library.Infrastructure.Services.AccountService
{
    public partial class AccountService
    {
        public async Task<(Result, ClaimsPrincipal?)> LoginAsync(LoginUserDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null || !user.Active)
                return (Result.Failure(ResultErrorCode.NOT_FOUND, [ErrorGenerator.EmailInputError("Account cannot be found")]), default);

            if (!user.EmailConfirmed)
                return (Result.Failure(ResultErrorCode.UNAUTHORIZED, [ErrorGenerator.GeneralError("Email address hasn't been confirmed yet")]), default);

            if (await userManager.CheckPasswordAsync(user, loginDto.Password))
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
                return (Result.Failure(ResultErrorCode.UNAUTHORIZED, [ErrorGenerator.PasswordInputError("Password is incorrect")]), default);
            }
        }
    }
}
