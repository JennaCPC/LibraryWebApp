using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Application.Models;

namespace Library.Application.Features.Account.LoginUser
{
    public static class LoginUserValidator
    {
        public static List<IError> ValidateLoginInput(LoginUserDto user)
        {
            var errors = new List<IError>();

            if (string.IsNullOrEmpty(user.Email)) errors.Add(ErrorGenerator.EmailInputError("Please enter your email"));
            if (string.IsNullOrEmpty(user.Password)) errors.Add(ErrorGenerator.PasswordInputError("Please enter a password"));

            return errors;
        }
    }
    

}
