namespace Library.Shared.Utilities
{

    public interface IError
    {
        string Type { get; }
        string Msg { get; }
    }

    public static class ErrorGenerator
    {
        class Error(string type, string msg) : IError
        {
            public string Type { get; private set; } = type;
            public string Msg { get; private set; } = msg;
        }
        public static IError GeneralError(string msg) { return new Error("root", msg); }
        public static IError EmailInputError(string msg) { return new Error("email", msg); }
        public static IError PasswordInputError(string msg) { return new Error("password", msg); }
        public static IError ConfirmPasswordInputError(string msg) { return new Error("confirmPassword", msg); }
        public static IError FirstNameInputError(string msg) { return new Error("firstName", msg); }
        public static IError LastNameInputError(string msg) { return new Error("lastName", msg); }
    }

}
