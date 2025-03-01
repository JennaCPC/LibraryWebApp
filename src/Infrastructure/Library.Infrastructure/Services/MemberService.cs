
using Library.Application.Features.Member;
using Library.Application.Interfaces;
using Library.Application.Models;
using Library.Domain.Constants;
using Library.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure.Services
{
    public class MemberService : IMemberService
    {
        private readonly UserManager<UserModel> userManager;

        public MemberService(UserManager<UserModel> userManager)
        {
            this.userManager = userManager;
        }
                
        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            var members = await userManager.GetUsersInRoleAsync(Roles.Member);

            List<MemberDto> memberDtos = [];

            if (members != null)
            {
                foreach (var member in members)
                {
                    memberDtos.Add(ToMemberDto(member)); 
                }
            }
            return memberDtos;
        }

        public async Task<Result> AddMemberAsync(MemberDto member)
        {

            //RegisterUserDto registerDto = new (
            //    Email: member.Email, 
            //    FirstName: member.FirstName, 
            //    LastName: member.LastName, 
            //    Password: 
            //)

            //Result result = await identityService.RegisterAsync()
            throw new NotImplementedException();
        }

        public Task<Result> DeleteMemberAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Result> EditMemberAsync(MemberDto member)
        {
            throw new NotImplementedException();
        }


        //public async Task<MemberDto?> GetMemberByEmailAsync(string email)
        //{
        //    var member = await userManager.FindByEmailAsync(email);           
        //    return member != null ? ToMemberDto(member) : null;
        //}

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
