using Library.Application.Features.Account.Commands.ForgotPassword;
using Library.Application.Features.Account.Commands.ResetPassword;
using Library.Shared.Utilities;

namespace Library.Infrastructure.Services.AccountService
{
    public partial class AccountService
    {        
        public async Task<Result> ForgotPassword(ForgotPasswordDto data)
        {
            var user = await userManager.FindByEmailAsync(data.Email);
            if (user == null)
                return Result.Failure(ResultErrorCode.NOT_FOUND, [ErrorGenerator.GeneralError("Account not found")]);
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            await SendEmailWithTokenAsync(data.Email, EncodeToken(token), data.ClientUri, "Reset Password");
            return Result.Success();
        }

        public async Task<Result> ResetPassword(ResetPasswordDto data)
        {
            var user = await userManager.FindByEmailAsync(data.Email);
            if (user == null)
                return Result.Failure(ResultErrorCode.NOT_FOUND, [ErrorGenerator.GeneralError("Account not found")]);
            
            var result = await userManager.ResetPasswordAsync(user, DecodeToken(data.Token), data.Password);
            if (!result.Succeeded)
            {
                var identityErrors = result.Errors;
                if (identityErrors.Where(e => e.Code.Contains("Password")).Any())
                {
                    return Result.Failure(ResultErrorCode.INTERNAL_ERROR, [ErrorGenerator.PasswordInputError("Password is not complex enough")]);
                }
                else
                {
                    List<IError> errors = [];
                    foreach (var error in identityErrors)
                    {
                        errors.Add(ErrorGenerator.GeneralError(error.Description));
                    }
                    return Result.Failure(ResultErrorCode.INTERNAL_ERROR, errors);
                }
            }
            return Result.Success();
        }
    }
}
