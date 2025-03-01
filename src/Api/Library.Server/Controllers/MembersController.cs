using Library.Application.Features.Member.GetMembers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Controllers
{
    [Route("api/members")]
    [ApiController]
    public class MembersController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IResult> GetMembers()
        {
            var members = await sender.Send(new GetMembersQuery()); 
            return Results.Ok(members);
        }        
    }
}
