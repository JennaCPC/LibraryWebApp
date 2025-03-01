using Library.Domain.Constants;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Library.Application.Features.Account.Commands.RegisterUser;
using Library.Application.Features.Account.Queries.LoginUser;
using Library.Application.Features.Account.Commands.ConfirmEmail;
using Library.Application.Features.Account.Commands.ForgotPassword;
using Library.Application.Features.Account.Commands.ResetPassword;

namespace Library.Server.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController(ISender sender) : ControllerBase
    {
        #region Login Controller
        [HttpPost("login")]
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

            return Results.Problem(statusCode: (int)result.Code, extensions: new Dictionary<string, object?> { { "errors", result.Errors } });
        }

        [HttpGet("login")]
        public bool? IsLoggedIn()
        {
            var result = HttpContext.User.Identity?.IsAuthenticated;
            return result;
        }
        #endregion Login Implementation

        #region Logout Controller
        [HttpPost("logout")]
        public async void Logout()
        {
            await HttpContext.SignOutAsync(Cookies.CookieAuth);
        }
        #endregion Logout Controller

        #region Register Controller
        [HttpPost("register")]
        public async Task<IResult> RegisterUser([FromBody] RegisterUserDto registerDto)
        {
            var result = await sender.Send(new RegisterUserCommand(registerDto));
            if (result.IsSuccess)
            {
                return Results.Ok(new { message = "Account registered!" });
            }
            else
            {
                return Results.Problem(
                    statusCode: (int)result.Code,
                    extensions: new Dictionary<string, object?> { { "errors", result.Errors } }
                );
            }
        }
        #endregion Register Controller

        #region Confirm Controller
        [HttpGet("confirm")]
        public async Task<IResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var result = await sender.Send(new ConfirmEmailCommand(email, token));
            if (result.IsSuccess) return Results.Ok(new { message = "Email confirmed" });
            else return Results.Problem(statusCode: (int)result.Code, extensions: new Dictionary<string, object?> { { "errors", result.Errors } });
        }

        [HttpPost("confirm/resend")]
        public async Task<IResult> ResendConfirmation([FromBody] ResendConfirmEmailDto data)
        {
            var result = await sender.Send(new ResendConfirmEmailCommand(data));
            if (result.IsSuccess) return Results.Ok(new { message = "New confirmation email sent." });
            else return Results.Problem(statusCode: (int)result.Code, extensions: new Dictionary<string, object?> { { "errors", result.Errors } });
        }
        #endregion Confirm Controller

        #region ForgotPassword Implementation
        [HttpPost("forgotpassword")]
        public async Task<IResult> ForgotPassword([FromBody] ForgotPasswordDto data)
        {
            var result = await sender.Send(new ForgotPasswordCommand(data));
            if (result.IsSuccess) return Results.Ok(new { message = "A reset password email has been sent to your inbox." });
            else return Results.Problem(statusCode: (int)result.Code, extensions: new Dictionary<string, object?> { { "errors", result.Errors } });
        }

        [HttpPost("resetpassword")]
        public async Task<IResult> ResetPassword([FromBody] ResetPasswordDto data)
        {
            var result = await sender.Send(new ResetPasswordCommand(data));
            if (result.IsSuccess) return Results.Ok(new { message = "Password has been resetted." });
            else return Results.Problem(statusCode: (int)result.Code, extensions: new Dictionary<string, object?> { { "errors", result.Errors } });

        }
        #endregion

    }
}
