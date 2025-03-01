using Library.Application.Features.Account.Commands.RegisterUser;
using Library.Application.Interfaces;
using Library.Application.Models;
using MediatR;

namespace Library.Application.Features.Account.Commands.ResetPassword
{
    public record ResetPasswordCommand(ResetPasswordDto Data) : IRequest<Result>;

    public class ResetPasswordCommandHandler(IIdentityService identityService) : IRequestHandler<ResetPasswordCommand, Result>
    {
        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var errors = ResetPasswordValidator.ValidateResetPasswordInput(request.Data);

            if (errors.Count == 0) return await identityService.ResetPassword(request.Data);

            return Result.Failure(ResultErrorCode.BAD_REQUEST, errors);            
        }
    }
}
