using Library.Application.Interfaces;
using Library.Shared.Utilities;
using MediatR;

namespace Library.Application.Features.Account.Commands.ConfirmEmail
{
    public record ConfirmEmailCommand(string Email, string Token) : IRequest<Result>;

    public class ConfirmEmailCommandHandler(IAccountService AccountService) : IRequestHandler<ConfirmEmailCommand, Result>
    {
        public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            return await AccountService.ConfirmEmailAsync(request.Email, request.Token);
        }
    }
}
