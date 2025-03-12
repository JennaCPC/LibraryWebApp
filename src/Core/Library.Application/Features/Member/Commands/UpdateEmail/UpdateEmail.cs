using Library.Application.Interfaces;
using Library.Shared.Utilities;
using MediatR;

namespace Library.Application.Features.Member.Commands.UpdateEmail
{
    public record UpdateEmailCommand(UpdateEmailDto UpdateEmailDto) : IRequest<Result>;

    public class UpdateEmailCommandHandler(IMemberService memberService) : IRequestHandler<UpdateEmailCommand, Result>
    {
        public async Task<Result> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
        {
            return await memberService.UpdateEmailAsync(request.UpdateEmailDto);
        }
    }
}
