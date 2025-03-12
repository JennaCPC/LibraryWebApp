
using Library.Shared.Utilities;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure.Services
{
    public interface IIdentityErrorMsg
    {
        IdentityErrorType Type { get; }
        string Msg { get; }
    }

    public enum IdentityErrorType
    {
        None,
        Invalid_Email,
        Duplicate_Email,
        Invalid_Password,
        Wrong_Password
    }

    public class IdentityErrorService
    {    

        class IdentityErrorMsg (IdentityErrorType type, string msg) : IIdentityErrorMsg
        {
            public IdentityErrorType Type { get; private set; } = type;
            public string Msg { get; private set; } = msg;
        }

        public static IIdentityErrorMsg GetEmailErrorMsg(IEnumerable<IdentityError> identityErrors)
        {
            if (identityErrors.Where(e => e.Code == "InvalidEmail").Any())
            {
                return new IdentityErrorMsg(IdentityErrorType.Invalid_Email, "Please enter a valid email");
            }
            else if (identityErrors.Where(e => e.Code == "DuplicateEmail" || e.Code == "DuplicateUserName").Any())
            {
                return new IdentityErrorMsg(IdentityErrorType.Duplicate_Email, "Email is already registered");
            }

            return new IdentityErrorMsg(IdentityErrorType.None, "");           
        }

        public static IIdentityErrorMsg GetPasswordErrorMsg(IEnumerable<IdentityError> identityErrors)
        {
            if (identityErrors.Where(e => e.Code.Contains("PasswordMismatch")).Any())
                return new IdentityErrorMsg(IdentityErrorType.Wrong_Password, "Wrong Password");

            else if (identityErrors.Where(e => e.Code.Contains("Password")).Any())
                return new IdentityErrorMsg(IdentityErrorType.Invalid_Password, "Password is not complex enough");

            return new IdentityErrorMsg(IdentityErrorType.None, ""); 
        }

        public static List<IError> GetGeneralErrors(IEnumerable<IdentityError> identityErrors)
        {
            List<IError> errors = [];
            foreach (var error in identityErrors)
            {
                errors.Add(ErrorGenerator.GeneralError(error.Description));
            }
            return errors;
        }
    }
}
