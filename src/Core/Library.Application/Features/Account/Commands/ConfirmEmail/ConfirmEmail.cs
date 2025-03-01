using Library.Application.Interfaces;
using Library.Application.Models;
using MediatR;

namespace Library.Application.Features.Account.Commands.ConfirmEmail
{
    public record ConfirmEmailCommand(string Email, string Token) : IRequest<Result>;

    public class ConfirmEmailCommandHandler(IIdentityService identityService) : IRequestHandler<ConfirmEmailCommand, Result>
    {
        public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            return await identityService.ConfirmEmailAsync(request.Email, request.Token);
        }
    }
}
