using Library.Application.Features.Admin.Queries.GetMembers;
using Library.Application.Interfaces;
using Library.Domain.Constants;
using Library.Infrastructure.Models;
using Library.Infrastructure.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Library.Shared.Utilities;
using Library.Shared.Pagination;

namespace Library.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<UserModel> userManager;
        private readonly EmailSender emailSender;

        public AdminService(UserManager<UserModel> userManager, EmailSender emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }
                
        public async Task<(IEnumerable<MemberDto> members, MetaData? metaData)> GetMembersAsync(MembersPaginationParameters membersPaginationParams)
        {
            var totalMembers = await userManager.GetUsersInRoleAsync(Roles.Member);            

            List<MemberDto> memberDtos = [];

            if (totalMembers != null)
            {
                var count = totalMembers.Count;
                foreach (var member in totalMembers)
                {
                    memberDtos.Add(ToMemberDto(member)); 
                }
                var members = memberDtos.Skip((membersPaginationParams.PageNumber - 1) * membersPaginationParams.PageSize)
                                    .Take(membersPaginationParams.PageSize);

                var pagedMembersList = PagedList<MemberDto>.ToPagedList(members, count, membersPaginationParams.PageNumber, membersPaginationParams.PageSize); 

                return (pagedMembersList, pagedMembersList.MetaData); 
            }
            
            return ([], null);
        }             

        public async Task<Result> UpdateMemberActiveStatusAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null) {
                user.Active = !user.Active;
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return Result.Success(); 
                return Result.Failure(ResultErrorCode.INTERNAL_ERROR, ProcessIdentityErrors(result.Errors));
            }
            return Result.Failure(ResultErrorCode.NOT_FOUND, [ErrorGenerator.GeneralError("Member not found")]); 
        }
        
        public static List<IError> ProcessIdentityErrors(IEnumerable<IdentityError> identityErrors)
        {
            List<IError> errors = []; 
            foreach (var error in identityErrors)
            {
                errors.Add(ErrorGenerator.GeneralError(error.Description)); 
            }
            return errors;
        }
      

        private MemberDto ToMemberDto(UserModel user)
        {
            return new MemberDto(
                Id: user.Id,
                Email: user.Email,
                FirstName: user.FirstName,
                LastName: user.LastName
            ); 
        }
    }
}
