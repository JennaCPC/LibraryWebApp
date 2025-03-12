using Library.Application.Features.Member.Commands.ConfirmEmailUpdate;
using Library.Application.Features.Member.Commands.UpdateEmail;
using Library.Application.Features.Member.Commands.UpdateName;
using Library.Application.Features.Member.Commands.UpdatePassword;
using Library.Shared.Utilities;

namespace Library.Application.Interfaces
{
    public interface IMemberService
    {
        Task<Result> UpdateEmailAsync(UpdateEmailDto updateEmailDto);
        Task<Result> ConfirmEmailUpdateAsync(ConfirmEmailUpdateDto data);
        Task<Result> UpdatePasswordAsync(UpdatePasswordDto data);
        Task<Result> UpdateNameAsync(UpdateNameDto data); 
    }
}
