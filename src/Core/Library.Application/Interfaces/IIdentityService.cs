using Library.Application.Features.Account.LoginUser;
using System.Security.Claims;
using Library.Application.Features.Account.RegisterUser;
using Library.Application.Models;

namespace Library.Application.Interfaces
{
    public interface IIdentityService
    {        
        Task<Result> RegisterAsync(RegisterUserDto registerDTO);
        Task<(Result, ClaimsPrincipal?)> LoginAsync(LoginUserDto loginDto); 
    }
}
