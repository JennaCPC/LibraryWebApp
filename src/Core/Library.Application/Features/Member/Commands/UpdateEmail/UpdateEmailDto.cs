namespace Library.Application.Features.Member.Commands.UpdateEmail
{
    public record UpdateEmailDto
    (
       string Email,
       string NewEmail,
       string ClientUri
    );
}
