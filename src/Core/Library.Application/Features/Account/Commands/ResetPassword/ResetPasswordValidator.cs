using Library.Application.Models;

namespace Library.Application.Features.Account.Commands.ResetPassword
{
    public static class ResetPasswordValidator
    {
        public static List<IError> ValidateResetPasswordInput(ResetPasswordDto data)
        {
            var errors = new List<IError>();
            if (string.IsNullOrEmpty(data.Email) || string.IsNullOrEmpty(data.Token)) return [ErrorGenerator.GeneralError("Incomplete request")]; 
            
            if (string.IsNullOrEmpty(data.Password)) errors.Add(ErrorGenerator.PasswordInputError("Please enter a new password"));
            else if (string.IsNullOrEmpty(data.ConfirmPassword)) errors.Add(ErrorGenerator.ConfirmPasswordInputError("Please re-enter password"));

            else if (data.Password != data.ConfirmPassword) errors.Add(ErrorGenerator.GeneralError("Passwords do not match"));             

            return errors;
        }

    }
}
