using Library.Application.Features.Account.ConfirmEmail;
using Library.Application.Features.Account.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Controllers
{
    [Route("api")]
    [ApiController]
    public class RegisterController(ISender sender) : ControllerBase
    {
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
                    statusCode: (int)result.Status,
                    extensions: new Dictionary<string, object?> { { "errors", result.Errors } }
                ); 
            }
        }

        [HttpGet("emailconfirmation")]
        public async Task<IResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var result = await sender.Send(new ConfirmEmailQuery(email, token));
            if (result.IsSuccess) return Results.Ok(new { message = "Email confirmed" });
            else return Results.Problem(statusCode: (int)result.Status, extensions: new Dictionary<string, object?> { { "errors", result.Errors } }); 
        }
    }
}
