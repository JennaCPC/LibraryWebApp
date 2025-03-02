using Library.Application.Features.Account.Commands.ConfirmEmail;
using Library.Application.Models;
using Library.Infrastructure.Models;

namespace Library.Infrastructure.Services.AccountService
{
    public partial class AccountService
    {
        public async Task<Result> ConfirmEmailAsync(string email, string token)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user is null) return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.GeneralError("Invalid Email Confirmation Request")]);

            if (user.EmailConfirmed) return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.GeneralError("Email has already been confirmed")]);

            var confirmResult = await userManager.ConfirmEmailAsync(user, DecodeToken(token));
            if (!confirmResult.Succeeded)
            {
                return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.GeneralError("Invalid token")]);
            }
            return Result.Success();

        }

        public async Task SendConfirmationEmailAsync(UserModel user, string clientUri)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmailWithTokenAsync(user.Email!, clientUri, EncodeToken(token), "Confirm Email");
        }

        public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmEmailDto data)
        {
            var (isConfirmed, user) = await IsConfirmedUser(data.Email);
            if (user != null)
            {
                if (isConfirmed)
                {
                    return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.GeneralError("Email has already been confirmed. ")]);
                }
                else
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    await SendConfirmationEmailAsync(user, data.ClientUri);
                    return Result.Success();
                }
            }
            return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.GeneralError("Account not found")]);
        }

        public async Task<(bool, UserModel?)> IsConfirmedUser(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null ? (user.EmailConfirmed, user) : (false, null);
        }        

    }
}
