using Library.Application.Interfaces;
using Library.Shared.Pagination;
using MediatR;

namespace Library.Application.Features.Admin.Queries.GetMembers
{
    public record GetMembersQuery(MembersPaginationParameters MemberPaginationParams) : IRequest<(IEnumerable<MemberDto> members, MetaData? metaData)>;

    public class GetMembersQueryHandler(IAdminService adminService) : IRequestHandler<GetMembersQuery, (IEnumerable<MemberDto> members, MetaData? metaData)>
    {
        public async Task<(IEnumerable<MemberDto> members, MetaData? metaData)> Handle(GetMembersQuery request, CancellationToken cancellationToken)
        {
            return await adminService.GetMembersAsync(request.MemberPaginationParams);           
        }
    }
}
