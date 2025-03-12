namespace Library.Application.Features.Member.Commands.UpdateName
{
    public record UpdateNameDto
    (
        string Email, 
        string FirstName, 
        string LastName
    ); 
}
