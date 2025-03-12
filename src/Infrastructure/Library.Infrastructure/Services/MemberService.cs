using Library.Application.Features.Member.Commands.ConfirmEmailUpdate;
using Library.Application.Features.Member.Commands.UpdateEmail;
using Library.Application.Features.Member.Commands.UpdateName;
using Library.Application.Features.Member.Commands.UpdatePassword;
using Library.Application.Interfaces;
using Library.Infrastructure.Models;
using Library.Infrastructure.Services.EmailService;
using Library.Shared.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Library.Infrastructure.Services
{
    public class MemberService : IMemberService
    {
        private readonly UserManager<UserModel> userManager;
        private readonly EmailSender emailSender;
        private readonly IEmailTemplateProvider templateProvider; 

        public MemberService(UserManager<UserModel> userManager, EmailSender emailSender, IEmailTemplateProvider templateProvider) 
        { 
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.templateProvider = templateProvider;
        }

        public async Task<Result> UpdateEmailAsync(UpdateEmailDto updateEmailDto)
        {
            var user = await userManager.FindByEmailAsync(updateEmailDto.Email);
            if (user is null)
                return Result.Failure(ResultErrorCode.NOT_FOUND, [ErrorGenerator.GeneralError("Error updating email address; user not found")]);

            var userWithNewEmail = await userManager.FindByEmailAsync(updateEmailDto.NewEmail); 

            if (userWithNewEmail is null)
            {
                await SendUpdateEmailConfirmationAsync(user, updateEmailDto.NewEmail, updateEmailDto.ClientUri); 
                return Result.Success();
            }
            else
            {
                return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.EmailInputError("Email already registered")]); 
            }            
        }

        public async Task SendUpdateEmailConfirmationAsync(UserModel user, string newEmail, string clientUri)
        {
            var token = await userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var param = new Dictionary<string, string?>
                {
                    {"email", user.Email },
                    {"newEmail", newEmail }, 
                    {"token", TokenCodec.EncodeToken(token) }
                };
            var callbackUrl = QueryHelpers.AddQueryString(clientUri, param);            

            var message = new EmailMessage([newEmail], "Confirm Email", templateProvider.EmailConfirmation(callbackUrl));

            await emailSender.SendEmailAsync(message);
        }

        public async Task<Result> ConfirmEmailUpdateAsync(ConfirmEmailUpdateDto data)
        {          
            var user = await userManager.FindByEmailAsync(data.Email);

            if (user is null) return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.GeneralError("Invalid Email Update Request")]);

            user.UserName = data.NewEmail;

            var result = await userManager.ChangeEmailAsync(user, data.NewEmail, TokenCodec.DecodeToken(data.Token));

            if (!result.Succeeded)
            {
                return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.GeneralError("Invalid token")]);
            }
            return Result.Success();
            
        }

        public async Task<Result> UpdatePasswordAsync(UpdatePasswordDto data)
        {
            var user = await userManager.FindByEmailAsync(data.Email);

            if (user is null) return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.GeneralError("Invalid Password Update Request")]);

            var result = await userManager.ChangePasswordAsync(user, data.Password, data.NewPassword);

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

        public async Task<Result> UpdateNameAsync(UpdateNameDto data)
        {
            var user = await userManager.FindByEmailAsync(data.Email);

            if (user is null) return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.GeneralError("Invalid Name Update Request")]); 

            user.FirstName = data.FirstName;
            user.LastName = data.LastName;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Result.Failure(ResultErrorCode.INTERNAL_ERROR, IdentityErrorService.GetGeneralErrors(result.Errors));
            }
            return Result.Success(); 
        }

    }
}
