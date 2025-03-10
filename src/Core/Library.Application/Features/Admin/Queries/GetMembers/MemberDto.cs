namespace Library.Application.Features.Admin.Queries.GetMembers
{
    public record MemberDto
    (
        string Id,
        string Email,
        string FirstName,
        string LastName, 
        string StartDate
    );
}
