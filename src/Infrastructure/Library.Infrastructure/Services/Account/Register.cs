using Library.Application.Features.Account.Commands.RegisterUser;
using Library.Infrastructure.Models;
using Library.Infrastructure.Services.EmailService;
using Library.Shared.Utilities;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure.Services.Account
{
    public partial class AccountService
    {
        public async Task<Result> RegisterAsync(RegisterUserDto registerDto)
        {
            UserModel user = new()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                StartDate = DateTime.UtcNow
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                await SendEmailConfirmationAsync(user, registerDto.ClientUri);
                await userManager.AddToRoleAsync(user, "Member");
                return Result.Success();
            }
            else
            {
                return ProcessRegisterIdentityErrors(result.Errors);
            }

        }

        public static Result ProcessRegisterIdentityErrors(IEnumerable<IdentityError> identityErrors)
        {
            List<IError> errors = [];

            var emailError = IdentityErrorService.GetEmailErrorMsg(identityErrors);
            var passwordError = IdentityErrorService.GetPasswordErrorMsg(identityErrors);

            if (emailError.Type != IdentityErrorType.None)
                errors.Add(ErrorGenerator.EmailInputError(emailError.Msg));

            if (passwordError.Type != IdentityErrorType.None)
                errors.Add(ErrorGenerator.PasswordInputError(passwordError.Msg));

            if (errors.Count > 0)
            {
                return Result.Failure(ResultErrorCode.BAD_REQUEST, errors);
            }

            return Result.Failure(ResultErrorCode.INTERNAL_ERROR, IdentityErrorService.GetGeneralErrors(identityErrors));

        }

        public async Task SendEmailConfirmationAsync(UserModel user, string clientUri)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = GetCallbackUrl(user.Email, token, clientUri);

            var message = new EmailMessage([user.Email], "Confirm Email", templateProvider.EmailConfirmation(callbackUrl));

            await emailSender.SendEmailAsync(message);
        }

    }
}
