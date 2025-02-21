using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Library.Application.Features.Account.LoginUser;
using Library.Application.Features.Account.RegisterUser;
using Library.Application.Interfaces;
using Library.Application.Models;
using Library.Domain.Constants;
using Library.Infrastructure.Models;
using Library.Infrastructure.Services.EmailService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;


namespace Library.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<UserModel> userManager;
        private readonly EmailSender emailSender;

        public IdentityService(UserManager<UserModel> userManager, EmailSender emailSender)
        {
            this.userManager = userManager; 
            this.emailSender = emailSender;
        }

        public async Task<Result> RegisterAsync(RegisterUserDto registerDto)
        {
            UserModel user = new()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {                
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var param = new Dictionary<string, string?>
                {                   
                    {"email", user.Email },
                    {"token", token }
                }; 
                var callback = QueryHelpers.AddQueryString(registerDto.ClientUri, param);
                var message = new EmailMessage([user.Email], "Confirm Email",  HtmlEncoder.Default.Encode(callback) ); 
                await emailSender.SendEmailAsync(message);
                await userManager.AddToRoleAsync(user, "Member");
                return Result.Success(); 
            }
            else
            {
                return ProcessIdentityErrors(result.Errors);
            }
        }

        public Result ProcessIdentityErrors(IEnumerable<IdentityError> identityErrors)
        {
            List<Error> errors = []; 

            if (identityErrors.Where(e => e.Code == "InvalidEmail").Any())
            {
                errors.Add(new Error("email", "Please enter a valid email"));
            }
            else if (identityErrors.Where(e => e.Code == "DuplicateEmail" || e.Code == "DuplicateUserName").Any())
            {
                errors.Add(new Error("email", "Email is already registered"));
            }

            if (identityErrors.Where(e => e.Code.Contains("Password")).Any()) { errors.Add(new Error("password", "Password is not complex enough")); };

            if (errors.Count != 0)
            {
                return Result.Failure(ErrorStatus.BAD_REQUEST, errors);
            }
            else
            {
                foreach (var error in identityErrors)
                {
                    errors.Add(new Error(error.Code, error.Description));
                }
                return Result.Failure(ErrorStatus.INTERNAL_ERROR, errors);

            }
        }

        public async Task<Result> ConfirmEmailAsync(string email, string token)
        {
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token)); 

            var user = await userManager.FindByEmailAsync(email);

            if (user is null) return Result.Failure(ErrorStatus.BAD_REQUEST, [new Error("email", "Invalid Email Confirmation Request")]); 

            var confirmResult = await userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded) return Result.Failure(ErrorStatus.BAD_REQUEST, [new Error("email", "Invalid Email Confirmation Request")]);

            return Result.Success(); 
        }

        public async Task<(Result, ClaimsPrincipal?)> LoginAsync(LoginUserDto loginDto)
        {
            List<Error> errors = [];

            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                errors.Add(new Error("email", "Email cannot be found"));
                return (Result.Failure(ErrorStatus.NOT_FOUND, errors), default);
            }
            else if (await userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var claims = new List<Claim> {
                        new(ClaimTypes.Email, user.Email.ToString()),
                        new(ClaimTypes.Name, user.FirstName.ToString() + " " + user.LastName.ToString())
                };

                var roles = await userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, Cookies.CookieAuth);
                ClaimsPrincipal claimsPrincipal = new(identity);

                return (Result.Success(), claimsPrincipal);                 
            }
            else
            {
                errors.Add(new Error("password", "Password is incorrect"));
                return (Result.Failure(ErrorStatus.UNAUTHORIZED, errors), default);
            }
        }

    }
}
