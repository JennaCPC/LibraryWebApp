using Library.Application.Features.Account.Commands.ForgotPassword;
using Library.Application.Features.Account.Commands.ResetPassword;
using Library.Infrastructure.Services.EmailService;
using Library.Shared.Utilities;

namespace Library.Infrastructure.Services.Account
{
    public partial class AccountService
    {
        public async Task<Result> ForgotPassword(ForgotPasswordDto data)
        {
            var user = await userManager.FindByEmailAsync(data.Email);
            if (user == null)
                return Result.Failure(ResultErrorCode.NOT_FOUND, [ErrorGenerator.GeneralError("Account not found")]);

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = GetCallbackUrl(user.Email, token, data.ClientUri);

            var message = new EmailMessage([user.Email], "Reset Password", templateProvider.PasswordReset(callbackUrl));

            await emailSender.SendEmailAsync(message);

            return Result.Success();
        }

        public async Task<Result> ResetPassword(ResetPasswordDto data)
        {
            var user = await userManager.FindByEmailAsync(data.Email);
            if (user == null)
                return Result.Failure(ResultErrorCode.NOT_FOUND, [ErrorGenerator.GeneralError("Account not found")]);

            var result = await userManager.ResetPasswordAsync(user, TokenCodec.DecodeToken(data.Token), data.Password);
            if (!result.Succeeded)
            {
                var err = IdentityErrorService.GetPasswordErrorMsg(result.Errors);

                var errors = err.Type switch
                {
                    IdentityErrorType.Wrong_Password => [ErrorGenerator.PasswordInputError(err.Msg)],
                    IdentityErrorType.Invalid_Password => [ErrorGenerator.GeneralError(err.Msg)],
                    _ => IdentityErrorService.GetGeneralErrors(result.Errors)
                };
              
                return Result.Failure(ResultErrorCode.INTERNAL_ERROR, errors);            
            }
            return Result.Success();
        }
    }
}
