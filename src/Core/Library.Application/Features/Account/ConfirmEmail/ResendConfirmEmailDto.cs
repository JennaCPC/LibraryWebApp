namespace Library.Application.Features.Account.ConfirmEmail
{
    public record ResendConfirmEmailDto
    (
        string Email,
        string ClientUri
    );
}
