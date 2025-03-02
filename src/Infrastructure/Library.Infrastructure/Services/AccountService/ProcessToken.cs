using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace Library.Infrastructure.Services.AccountService
{
    public partial class AccountService
    {
        public static string EncodeToken(string token) => WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        public static string DecodeToken(string token) => Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
    }
}
