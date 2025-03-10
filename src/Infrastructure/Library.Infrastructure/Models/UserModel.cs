using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Library.Infrastructure.Models
{
    public class UserModel : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; } = string.Empty;

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; } = string.Empty;

        public bool Active { get; set; } = true; 

        public DateTime StartDate { get; set; } = DateTime.Today;

        public DateTime? EndDate { get; set; }
    }
}
