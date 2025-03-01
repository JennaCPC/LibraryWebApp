using Library.Application.Models;

namespace Library.Application.Features.Account.Commands.RegisterUser
{
    public static class RegisterUserValidator
    {
        public static List<IError> ValidateRegistrationInput(RegisterUserDto user)
        {
            var errors = new List<IError>();

            if (string.IsNullOrEmpty(user.FirstName)) errors.Add(ErrorGenerator.FirstNameInputError("Please enter your first name"));
            if (string.IsNullOrEmpty(user.LastName)) errors.Add(ErrorGenerator.LastNameInputError("Please enter your last name"));
            if (string.IsNullOrEmpty(user.Email)) errors.Add(ErrorGenerator.EmailInputError("Please enter your email"));
            if (string.IsNullOrEmpty(user.Password)) errors.Add(ErrorGenerator.PasswordInputError("Please enter a password"));

            return errors;
        }


    }
}
