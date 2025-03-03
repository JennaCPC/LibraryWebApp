using Library.Application.Interfaces;
using Library.Shared.Utilities;
using MediatR;


namespace Library.Application.Features.Account.Commands.RegisterUser
{
    public record RegisterUserCommand(RegisterUserDto UserRegisterDto) : IRequest<Result>;
    public class RegisterUserCommandHandler(IAccountService AccountService) : IRequestHandler<RegisterUserCommand, Result>
    {
        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var userRegisterDto = request.UserRegisterDto;
            var errors = RegisterUserValidator.ValidateRegistrationInput(userRegisterDto);

            if (errors.Count == 0) return await AccountService.RegisterAsync(userRegisterDto);            
            
            return Result.Failure(ResultErrorCode.BAD_REQUEST, errors);            
        }
    }
}
