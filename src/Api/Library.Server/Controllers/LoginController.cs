using Library.Application.Features.Account.LoginUser;
using Library.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.Server.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IResult> Login([FromBody] LoginUserDto loginDto)
        {
            var (result, claimsPrincipal) = await sender.Send(new LoginUserQuery(loginDto));

            if (claimsPrincipal != null)
            {
                await HttpContext.SignInAsync(Cookies.CookieAuth, claimsPrincipal);
                return Results.Ok(new
                {
                    email = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value,
                    fullName = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value,
                    role = claimsPrincipal.FindFirst(ClaimTypes.Role)?.Value
                });
            }

            return Results.Problem(statusCode: (int)result.Status, extensions: new Dictionary<string, object?> { { "errors", result.Errors } });
        }

        [HttpGet]
        public bool? IsLoggedIn()
        {
            var result = HttpContext.User.Identity?.IsAuthenticated;
            return result;
        }
    }
}
