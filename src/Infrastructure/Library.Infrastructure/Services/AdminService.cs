using Library.Application.Features.Admin.Queries.GetMembers;
using Library.Application.Interfaces;
using Library.Domain.Constants;
using Library.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Library.Shared.Utilities;
using Library.Shared.Pagination;


namespace Library.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<UserModel> userManager;

        public AdminService(UserManager<UserModel> userManager)
        {
            this.userManager = userManager;
        }
                
        public async Task<(IEnumerable<MemberDto> members, MetaData? metaData)> GetMembersAsync(MembersPaginationParameters membersPaginationParams)
        {
            var totalMembers = await userManager.GetUsersInRoleAsync(Roles.Member);            

            List<MemberDto> memberDtos = [];

            if (totalMembers != null)
            {                
                var filteredAndSortedMembers = FilterAndSortMembers(totalMembers, membersPaginationParams);               

                foreach (var member in filteredAndSortedMembers)
                {
                    memberDtos.Add(ToMemberDto(member)); 
                }

                var count = memberDtos.Count;

                var pagedMemberDtos = memberDtos.Skip((membersPaginationParams.PageNumber - 1) * membersPaginationParams.PageSize)
                                    .Take(membersPaginationParams.PageSize);

                var pagedMembersList = PagedList<MemberDto>.ToPagedList(pagedMemberDtos, count, membersPaginationParams.PageNumber, membersPaginationParams.PageSize); 

                return (pagedMembersList, pagedMembersList.MetaData); 
            }
            
            return ([], null);
        }    
        
        public static IEnumerable<UserModel> FilterAndSortMembers(IEnumerable<UserModel> members, MembersPaginationParameters parameters)
        {
            (string searchTerm, bool? isActive, string orderBy) = parameters;
            var filteredMembers = FilterMembers(members, searchTerm, isActive);
            return SortMembers(filteredMembers, orderBy); 
        }

        public static IEnumerable<UserModel> FilterMembers(IEnumerable<UserModel> members, string searchTerm, bool? isActive)
        {            
            if (!string.IsNullOrEmpty(searchTerm))
            {
                members = members.Where(member => member.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                 member.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                                 member.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();   
            }
            if (isActive != null)
            {
                members = members.Where(member => member.Active == isActive);
            }
            return members;
        }

        public static IEnumerable<UserModel> SortMembers(IEnumerable<UserModel> members, string orderBy)
        {
            if (!string.IsNullOrEmpty(orderBy))
            {
                var orderParam = orderBy.Trim().Split(' ');     // get order property and sorting direction
                var orderParamName = orderParam[0];
                var orderParamDir = orderParam.ElementAtOrDefault(1) ?? ""; 
                var userProps = typeof(UserModel).GetProperties();
                
                var orderProp = userProps.FirstOrDefault(prop => prop.Name.Equals(orderParamName, StringComparison.InvariantCultureIgnoreCase));
              
                if (orderProp != null)
                {
                    members = orderParamDir.Equals("desc") ? members.OrderByDescending(m => orderProp.GetValue(m, null)) : 
                                                             members.OrderBy(m => orderProp.GetValue(m, null)); 
                }                
            }           
            return members; 
        }

        public async Task<Result> UpdateMemberActiveStatusAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null) {
                user.Active = !user.Active;
                user.EndDate = user.Active ? null : DateTime.UtcNow;  
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

        private static MemberDto ToMemberDto(UserModel user)
        {            
            return new MemberDto(
                Id: user.Id,
                Email: user.Email,
                FirstName: user.FirstName,
                LastName: user.LastName,
                StartDate: user.StartDate.ToLocalTime().ToString()
            ); 
        }
    }
}
