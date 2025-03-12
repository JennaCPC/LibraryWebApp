namespace Library.Application.Features.Member.Commands.ConfirmEmailUpdate
{
    public record ConfirmEmailUpdateDto
    (
       string Email,
       string NewEmail,
       string Token
    );
}
