using Library.Application.Models;

namespace Library.Application.Features.Account.RegisterUser
{
    public static class RegisterUserValidator
    {
        public static List<Error> ValidateRegistrationInput(RegisterUserDto user)
        {
            var errors = new List<Error>();

            if (string.IsNullOrEmpty(user.FirstName)) errors.Add(new Error("firstName", "Please enter your first name"));
            if (string.IsNullOrEmpty(user.LastName)) errors.Add(new Error("lastName", "Please enter your last name"));
            if (string.IsNullOrEmpty(user.Email)) errors.Add(new Error("email", "Please enter your email"));
            if (string.IsNullOrEmpty(user.Password)) errors.Add(new Error("password", "Please enter a password"));

            return errors;
        }


    }
}
