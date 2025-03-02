using Library.Application.Features.Admin.Queries.GetMembers;
using Library.Application.Models;

namespace Library.Application.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<MemberDto>> GetMembersAsync();

        Task<Result> UpdateMemberActiveStatusAsync(string email); 
    }
}
