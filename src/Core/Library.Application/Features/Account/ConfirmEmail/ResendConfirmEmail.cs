using Library.Application.Interfaces;
using Library.Application.Models;
using MediatR;

namespace Library.Application.Features.Account.ConfirmEmail
{
    public record ResendConfirmEmailQuery(string Email, string ClientUri) : IRequest<Result>;

    public class ResendConfirmEmailQueryHandler(IIdentityService identityService) : IRequestHandler<ResendConfirmEmailQuery, Result>
    {
        public async Task<Result> Handle(ResendConfirmEmailQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email)) return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.EmailInputError("Please enter your email address. ")]);
            return await identityService.ResendConfirmationEmailAsync(request.Email, request.ClientUri);
        }
    }
}
