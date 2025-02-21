
namespace Library.Domain.Constants
{
    public abstract class Roles
    {
        public const string Admin = nameof(Admin);
        public const string Member = nameof(Member);
        public static readonly string[] RoleTypes = { Admin, Member };
    }
}
