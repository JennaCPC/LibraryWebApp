using Library.Application.Interfaces;
using Library.Application.Models;
using MediatR;

namespace Library.Application.Features.Account.ConfirmEmail
{
    public record ConfirmEmailQuery(string email, string token) : IRequest<Result>;

    public class ConfirmEmailQueryHandler(IIdentityService identityService) : IRequestHandler<ConfirmEmailQuery, Result>
    {
        public async Task<Result> Handle(ConfirmEmailQuery request, CancellationToken cancellationToken)
        {
           return await identityService.ConfirmEmailAsync(request.email, request.token);
            
        }
    }
}
