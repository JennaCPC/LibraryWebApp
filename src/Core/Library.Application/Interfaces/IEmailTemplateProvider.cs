namespace Library.Application.Interfaces
{
    public interface IEmailTemplateProvider
    {
        string EmailConfirmation(string link);
        string PasswordReset(string link);
    }
}
