using Library.Application.Features.Member;
using Library.Application.Models;

namespace Library.Application.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberDto>> GetMembersAsync();
        Task<Result> AddMemberAsync(MemberDto member);
        Task<Result> EditMemberAsync(MemberDto member);
        Task<Result> DeleteMemberAsync(string id);

    }
}
