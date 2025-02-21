using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.Features.Member.GetMembers
{
    public record GetMembersQuery : IRequest<IEnumerable<MemberDto>>;

    public class GetMembersQueryHandler(IMemberService memberService) : IRequestHandler<GetMembersQuery, IEnumerable<MemberDto>>
    {
        public async Task<IEnumerable<MemberDto>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
        {
            return await memberService.GetMembersAsync(); 
        }
    }
}
