using Library.Application.Interfaces;
using Library.Shared.Utilities;
using MediatR;

namespace Library.Application.Features.Member.Commands.UpdateName
{
    public record UpdateNameCommand(UpdateNameDto UpdateNameDto) : IRequest<Result>;

    public class UpdateNameCommandHandler(IMemberService memberService) : IRequestHandler<UpdateNameCommand, Result>
    {
        public async Task<Result> Handle(UpdateNameCommand request, CancellationToken cancellationToken)
        {
            return await memberService.UpdateNameAsync(request.UpdateNameDto);
        }
    }
}
