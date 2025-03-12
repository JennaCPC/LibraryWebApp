using Library.Application.Interfaces;
using Library.Shared.Utilities;
using MediatR;

namespace Library.Application.Features.Member.Commands.UpdatePassword
{
    public record UpdatePasswordCommand(UpdatePasswordDto UpdatePasswordDto) : IRequest<Result>;

    public class UpdatePasswordCommandHandler(IMemberService memberService) : IRequestHandler<UpdatePasswordCommand, Result>
    {
        public async Task<Result> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            return await memberService.UpdatePasswordAsync(request.UpdatePasswordDto);
        }
    }
}
