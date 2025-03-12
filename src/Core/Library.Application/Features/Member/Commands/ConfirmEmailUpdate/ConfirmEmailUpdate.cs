using Library.Application.Interfaces;
using Library.Shared.Utilities;
using MediatR;

namespace Library.Application.Features.Member.Commands.ConfirmEmailUpdate
{
    public record ConfirmEmailUpdateCommand(ConfirmEmailUpdateDto Data) : IRequest<Result>;

    public class ConfirmEmailUpdateCommandHandler(IMemberService memberService) : IRequestHandler<ConfirmEmailUpdateCommand, Result>
    {
        public async Task<Result> Handle(ConfirmEmailUpdateCommand request, CancellationToken cancellationToken)
        {
            return await memberService.ConfirmEmailUpdateAsync(request.Data); 
        }
    }
}
