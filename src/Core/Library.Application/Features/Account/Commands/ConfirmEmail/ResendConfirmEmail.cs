using Library.Application.Interfaces;
using Library.Application.Models;
using MediatR;

namespace Library.Application.Features.Account.Commands.ConfirmEmail
{
    public record ResendConfirmEmailCommand(ResendConfirmEmailDto Data) : IRequest<Result>;

    public class ResendConfirmEmailCommandHandler(IIdentityService identityService) : IRequestHandler<ResendConfirmEmailCommand, Result>
    {
        public async Task<Result> Handle(ResendConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Data.Email)) return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.EmailInputError("Please enter your email address. ")]);
            return await identityService.ResendConfirmationEmailAsync(request.Data);
        }
    }
}
