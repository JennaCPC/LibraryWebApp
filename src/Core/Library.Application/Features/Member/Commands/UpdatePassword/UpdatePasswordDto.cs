namespace Library.Application.Features.Member.Commands.UpdatePassword
{
    public record UpdatePasswordDto
    (
        string Email,
        string Password, 
        string NewPassword
    ); 
}
