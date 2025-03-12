using System.Security.Claims;
using Library.Application.Interfaces;
using Library.Shared.Utilities;
using MediatR;

namespace Library.Application.Features.Account.Queries.LoginUser
{
    public record LoginUserQuery(LoginUserDto LoginDto) : IRequest<(Result, ClaimsPrincipal?)>;

    public class LoginUserQueryHandler(IAccountService accountService) : IRequestHandler<LoginUserQuery, (Result, ClaimsPrincipal?)>
    {
        public async Task<(Result, ClaimsPrincipal?)> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var loginDto = request.LoginDto;
            var errors = LoginUserValidator.ValidateLoginInput(loginDto);

            if (errors.Count == 0) return await accountService.LoginAsync(loginDto);            

            return (Result.Failure(ResultErrorCode.BAD_REQUEST, errors), default);          

        }
    }
}
