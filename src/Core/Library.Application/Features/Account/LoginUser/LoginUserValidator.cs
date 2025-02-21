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
        public static List<Error> ValidateLoginInput(LoginUserDto user)
        {
            var errors = new List<Error>();

            if (string.IsNullOrEmpty(user.Email)) errors.Add(new Error("email", "Please enter your email"));
            if (string.IsNullOrEmpty(user.Password)) errors.Add(new Error("password", "Please enter a password"));

            return errors;
        }
    }
    

}
