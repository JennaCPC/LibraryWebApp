using Library.Application.Features.Account.Commands.RegisterUser;
using Library.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Library.Shared.Utilities;

namespace Library.Infrastructure.Services.AccountService
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
                UserName = registerDto.Email
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                await SendConfirmationEmailAsync(user, registerDto.ClientUri);
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

            if (identityErrors.Where(e => e.Code == "InvalidEmail").Any())
            {
                errors.Add(ErrorGenerator.EmailInputError("Please enter a valid email"));
            }
            else if (identityErrors.Where(e => e.Code == "DuplicateEmail" || e.Code == "DuplicateUserName").Any())
            {
                errors.Add(ErrorGenerator.EmailInputError("Email is already registered"));
            }

            if (identityErrors.Where(e => e.Code.Contains("Password")).Any()) { errors.Add(ErrorGenerator.PasswordInputError("Password is not complex enough")); };

            if (errors.Count != 0)
            {
                return Result.Failure(ResultErrorCode.BAD_REQUEST, errors);
            }
            else
            {
                foreach (var error in identityErrors)
                {
                    errors.Add(ErrorGenerator.GeneralError(error.Description));
                }
                return Result.Failure(ResultErrorCode.INTERNAL_ERROR, errors);

            }
        }
    }
}
