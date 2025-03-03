using Library.Application.Interfaces;
using Library.Shared.Utilities;
using MediatR;

namespace Library.Application.Features.Account.Commands.ForgotPassword
{
    public record ForgotPasswordCommand(ForgotPasswordDto Data) : IRequest<Result>;

    public class ForgotPasswordCommandHandler(IAccountService AccountService) : IRequestHandler<ForgotPasswordCommand, Result>
    {
        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Data.Email)) return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.EmailInputError("Please enter your email address. ")]);
            return await AccountService.ForgotPassword(request.Data);
        }
    }
}
