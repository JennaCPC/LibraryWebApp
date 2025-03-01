
namespace Library.Application.Features.Account.Commands.ResetPassword
{
    public record ResetPasswordDto(
        string Email, 
        string Token, 
        string Password, 
        string ConfirmPassword
    ); 
}
