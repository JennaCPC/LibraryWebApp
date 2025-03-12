using Library.Application.Interfaces;
using Library.Shared.Utilities;
using MediatR;

namespace Library.Application.Features.Admin.Commands.UpdateMemberActiveStatus
{
    public record UpdateMemberActiveStatusCommand(string Email) : IRequest<Result>;

    public class UpdateMemberActiveStatusCommandHandler(IAdminService adminService) : IRequestHandler<UpdateMemberActiveStatusCommand, Result>
    {
        public async Task<Result> Handle(UpdateMemberActiveStatusCommand request, CancellationToken cancellationToken)
        {
            return await adminService.UpdateMemberActiveStatusAsync(request.Email); 
        }
    }
}
