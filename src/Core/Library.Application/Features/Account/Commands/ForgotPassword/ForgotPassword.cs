using Library.Application.Interfaces;
using Library.Application.Models;
using MediatR;

namespace Library.Application.Features.Account.Commands.ForgotPassword
{
    public record ForgotPasswordCommand(ForgotPasswordDto Data) : IRequest<Result>;

    public class ForgotPasswordCommandHandler(IIdentityService identityService) : IRequestHandler<ForgotPasswordCommand, Result>
    {
        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Data.Email)) return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.EmailInputError("Please enter your email address. ")]);
            return await identityService.ForgotPassword(request.Data);
        }
    }
}
