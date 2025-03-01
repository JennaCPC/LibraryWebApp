namespace Library.Application.Features.Account.Commands.ConfirmEmail
{
    public record ResendConfirmEmailDto
    (
        string Email,
        string ClientUri
    );
}
