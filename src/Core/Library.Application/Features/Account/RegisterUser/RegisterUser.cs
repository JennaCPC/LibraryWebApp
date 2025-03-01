using Library.Application.Interfaces;
using Library.Application.Models;
using MediatR;


namespace Library.Application.Features.Account.RegisterUser
{
    public record RegisterUserCommand(RegisterUserDto UserRegisterDto) : IRequest<Result>;
    public class RegisterUserCommandHandler(IIdentityService identityService) : IRequestHandler<RegisterUserCommand, Result>
    {
        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userRegisterDto = request.UserRegisterDto;
            var errors = RegisterUserValidator.ValidateRegistrationInput(userRegisterDto);

            if (errors.Count == 0)
            {
                return await identityService.RegisterAsync(userRegisterDto);
            }
            else
            {
                return Result.Failure(ResultErrorCode.BAD_REQUEST, errors);
            }

        }
    }
}
