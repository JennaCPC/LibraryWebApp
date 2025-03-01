using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Library.Application.Features.Account.RegisterUser;
using Library.Application.Interfaces;
using Library.Application.Models;
using MediatR;

namespace Library.Application.Features.Account.LoginUser
{
    public record LoginUserQuery(LoginUserDto LoginDto) : IRequest<(Result, ClaimsPrincipal?)>;

    public class LoginUserQueryHandler(IIdentityService identityService): IRequestHandler<LoginUserQuery, (Result, ClaimsPrincipal?)>
    {
        public async Task<(Result, ClaimsPrincipal?)> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var loginDto = request.LoginDto;
            var errors = LoginUserValidator.ValidateLoginInput(loginDto);

            if (errors.Count == 0)
            {
                return await identityService.LoginAsync(loginDto);                
            }
            else
            {
                return (Result.Failure(ResultErrorCode.BAD_REQUEST, errors), default);
            }

        }
    }
}
