using System.Security.Claims;
using Library.Application.Models;
using Library.Application.Features.Account.Commands.RegisterUser;
using Library.Application.Features.Account.Queries.LoginUser;
using Library.Application.Features.Account.Commands.ConfirmEmail;
using Library.Application.Features.Account.Commands.ForgotPassword;
using Library.Application.Features.Account.Commands.ResetPassword;

namespace Library.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<Result> RegisterAsync(RegisterUserDto registerDTO);
        Task<(Result, ClaimsPrincipal?)> LoginAsync(LoginUserDto loginDto);
        Task<Result> ConfirmEmailAsync(string email, string token);
        Task<Result> ResendConfirmationEmailAsync(ResendConfirmEmailDto data);
        Task<Result> ForgotPassword(ForgotPasswordDto data);
        Task<Result> ResetPassword(ResetPasswordDto data);
    }
}
