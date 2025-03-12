using Microsoft.AspNetCore.Mvc;
using MediatR;
using Library.Application.Features.Member.Commands.UpdateEmail;
using Library.Application.Features.Member.Commands.ConfirmEmailUpdate;
using Library.Application.Features.Member.Commands.UpdatePassword;
using Library.Application.Features.Member.Commands.UpdateName;

namespace Library.Server.Controllers
{
    [Route("api/member")]
    [ApiController]
    public class MemberController(ISender sender) : Controller
    {
        [HttpPost("update/email")]
        public async Task<IResult> UpdateEmail([FromBody] UpdateEmailDto updateEmailDto)
        {            
            var result = await sender.Send(new UpdateEmailCommand(updateEmailDto));

            if (result.IsSuccess)
            {
                return Results.Ok(new { message = "Please confirm new email. " });
            }

            return Results.Problem(statusCode: (int)result.Code, extensions: new Dictionary<string, object?> { { "errors", result.Errors } });
        }

        [HttpGet("update/confirm")]
        public async Task<IResult> ConfirmEmail([FromQuery] ConfirmEmailUpdateDto data)
        {
            
            var result = await sender.Send(new ConfirmEmailUpdateCommand(data));
            if (result.IsSuccess) return Results.Ok(new { message = "Email confirmed" });
            else return Results.Problem(statusCode: (int)result.Code, extensions: new Dictionary<string, object?> { { "errors", result.Errors } });
        }

        [HttpPost("update/password")]
        public async Task<IResult> UpdatePassword([FromBody] UpdatePasswordDto data)
        {
            var result = await sender.Send(new UpdatePasswordCommand(data));

            if (result.IsSuccess)
            {
                return Results.Ok(new { message = "Password successfully updated. " });
            }

            return Results.Problem(statusCode: (int)result.Code, extensions: new Dictionary<string, object?> { { "errors", result.Errors } });
        }

        [HttpPost("update/name")]
        public async Task<IResult> UpdateName([FromBody] UpdateNameDto data)
        {
            var result = await sender.Send(new UpdateNameCommand(data));

            if (result.IsSuccess)
            {
                return Results.Ok(new { message = "Name successfully updated. " });
            }

            return Results.Problem(statusCode: (int)result.Code, extensions: new Dictionary<string, object?> { { "errors", result.Errors } });
        }

    }
}
