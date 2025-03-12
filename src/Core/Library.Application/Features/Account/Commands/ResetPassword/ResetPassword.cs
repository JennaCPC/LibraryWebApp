using Library.Application.Interfaces;
using Library.Shared.Utilities;
using MediatR;

namespace Library.Application.Features.Account.Commands.ResetPassword
{
    public record ResetPasswordCommand(ResetPasswordDto Data) : IRequest<Result>;

    public class ResetPasswordCommandHandler(IAccountService accountService) : IRequestHandler<ResetPasswordCommand, Result>
    {
        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var errors = ResetPasswordValidator.ValidateResetPasswordInput(request.Data);

            if (errors.Count == 0) return await accountService.ResetPassword(request.Data);

            return Result.Failure(ResultErrorCode.BAD_REQUEST, errors);            
        }
    }
}
