using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.Features.Admin.Queries.GetMembers
{
    public record GetMembersQuery : IRequest<IEnumerable<MemberDto>>;

    public class GetMembersQueryHandler(IAdminService memberService) : IRequestHandler<GetMembersQuery, IEnumerable<MemberDto>>
    {
        public async Task<IEnumerable<MemberDto>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
        {
            return await memberService.GetMembersAsync();
        }
    }
}
