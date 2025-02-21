namespace Library.Application.Features.Account.RegisterUser
{
    public record RegisterUserDto
    (
        string FirstName,
        string LastName,
        string Email,
        string? Password
    );
}
