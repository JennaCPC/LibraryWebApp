using Library.Application.Features.Admin.Commands.UpdateMemberActiveStatus;
using Library.Application.Features.Admin.Queries.GetMembers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController(ISender sender) : ControllerBase
    {
        [HttpGet("members")]
        public async Task<IResult> GetMembers()
        {
            var members = await sender.Send(new GetMembersQuery()); 
            return Results.Ok(members);
        }

        [HttpPost("members")]
        public async Task<IResult> UpdateMemberActiveStatus([FromBody] string email)
        {
            var result = await sender.Send(new UpdateMemberActiveStatusCommand(email));
            if (result.IsSuccess) return Results.Ok(new { message = "Member status has been updated. " });
            else return Results.Problem(statusCode: (int)result.Code, extensions: new Dictionary<string, object?> { { "errors", result.Errors } });
        }

    }
}
