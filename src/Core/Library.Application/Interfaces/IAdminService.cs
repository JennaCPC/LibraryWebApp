using Library.Application.Features.Admin.Queries.GetMembers;
using Library.Shared.Pagination;
using Library.Shared.Utilities;

namespace Library.Application.Interfaces
{
    public interface IAdminService
    {
        Task<(IEnumerable<MemberDto> members, MetaData metaData)> GetMembersAsync(MembersPaginationParameters membersPaginationParams);

        Task<Result> UpdateMemberActiveStatusAsync(string email); 
    }
}
