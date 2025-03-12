using Library.Application.Interfaces;
using Library.Shared.Utilities;
using MediatR;

namespace Library.Application.Features.Account.Commands.ConfirmEmail
{
    public record ResendConfirmEmailCommand(ResendConfirmEmailDto Data) : IRequest<Result>;

    public class ResendConfirmEmailCommandHandler(IAccountService accountService) : IRequestHandler<ResendConfirmEmailCommand, Result>
    {
        public async Task<Result> Handle(ResendConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Data.Email)) return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.EmailInputError("Please enter your email address. ")]);
            return await accountService.ResendConfirmationEmailAsync(request.Data);
        }
    }
}
