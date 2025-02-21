using Library.Domain.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Controllers
{
    [Route("api/logout")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        [HttpPost]
        public async void Logout()
        {
            await HttpContext.SignOutAsync(Cookies.CookieAuth);
        }
    }
}
