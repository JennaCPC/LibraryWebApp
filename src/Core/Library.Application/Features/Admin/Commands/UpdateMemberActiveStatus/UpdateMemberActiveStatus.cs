using Library.Application.Features.Admin.Queries.GetMembers;
using Library.Application.Interfaces;
using Library.Application.Models;
using MediatR;

namespace Library.Application.Features.Admin.Commands.UpdateMemberActiveStatus
{
    public record UpdateMemberActiveStatusCommand(string Email) : IRequest<Result>;

    public class UpdateMemberActiveStatusCommandHandler(IAdminService memberService) : IRequestHandler<UpdateMemberActiveStatusCommand, Result>
    {
        public async Task<Result> Handle(UpdateMemberActiveStatusCommand request, CancellationToken cancellationToken)
        {
            return await memberService.UpdateMemberActiveStatusAsync(request.Email); 
        }
    }
}
