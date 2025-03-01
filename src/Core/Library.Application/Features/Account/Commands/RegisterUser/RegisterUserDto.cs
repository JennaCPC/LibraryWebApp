namespace Library.Application.Features.Account.Commands.RegisterUser
{
    public record RegisterUserDto
    (
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string ClientUri
    );
}
