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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<(Result, ClaimsPrincipal?)> LoginAsync(LoginUserDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) 
                return (Result.Failure(ResultErrorCode.NOT_FOUND, [ErrorGenerator.EmailInputError("Email cannot be found")]), default);   
            
            if (!user.EmailConfirmed) 
                return (Result.Failure(ResultErrorCode.UNAUTHORIZED, [ErrorGenerator.GeneralError("Email address hasn't been confirmed yet")]), default); 
            
            if (await userManager.CheckPasswordAsync(user, loginDto.Password))
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
                return (Result.Failure(ResultErrorCode.UNAUTHORIZED, [ErrorGenerator.PasswordInputError("Password is incorrect")]), default);
            }
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
                await SendConfirmationEmailAsync(user, registerDto.ClientUri);
                await userManager.AddToRoleAsync(user, "Member");
                return Result.Success();
            }
            else
            {
                return ProcessRegisterIdentityErrors(result.Errors);
            }
            
        }

        public static Result ProcessRegisterIdentityErrors(IEnumerable<IdentityError> identityErrors)
        {
            List<IError> errors = [];

            if (identityErrors.Where(e => e.Code == "InvalidEmail").Any())
            {
                errors.Add(ErrorGenerator.EmailInputError("Please enter a valid email"));
            }
            else if (identityErrors.Where(e => e.Code == "DuplicateEmail" || e.Code == "DuplicateUserName").Any())
            {
                errors.Add(ErrorGenerator.EmailInputError("Email is already registered"));
            }

            if (identityErrors.Where(e => e.Code.Contains("Password")).Any()) { errors.Add(ErrorGenerator.PasswordInputError("Password is not complex enough")); };

            if (errors.Count != 0)
            {
                return Result.Failure(ResultErrorCode.BAD_REQUEST, errors);
            }
            else
            {
                foreach (var error in identityErrors)
                {
                    errors.Add(ErrorGenerator.GeneralError(error.Description));
                }
                return Result.Failure(ResultErrorCode.INTERNAL_ERROR, errors);

            }
        }

        public async Task SendConfirmationEmailAsync(UserModel user, string clientUri)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var param = new Dictionary<string, string?>
                {
                    {"email", user.Email },
                    {"token", token }
                };
            var callback = QueryHelpers.AddQueryString(clientUri, param);
            var message = new EmailMessage([user.Email], "Confirm Email", HtmlEncoder.Default.Encode(callback));
            await emailSender.SendEmailAsync(message);
        }

        public async Task<Result> ResendConfirmationEmailAsync(string email, string clientUri)
        {
            var (isConfirmed, user) = await IsConfirmedUser(email); 
            if (user != null)
            {
                if (isConfirmed)
                {
                    return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.GeneralError("Email has already been confirmed. ")]);                     
                }
                else
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    await SendConfirmationEmailAsync(user, clientUri);
                    return Result.Success();
                }                
            }
            return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.GeneralError("Account not found")]);
        }

        public async Task<(bool, UserModel?)> IsConfirmedUser(string email)         
        {
            var user = await userManager.FindByEmailAsync(email);
            return (user != null) ?  (user.EmailConfirmed, user) : (false, null); 
        }

        public async Task<Result> ConfirmEmailAsync(string email, string token)
        {
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token)); 

            var user = await userManager.FindByEmailAsync(email);

            if (user is null) return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.TokenError("Invalid Email Confirmation Request")]);

            if (user.EmailConfirmed) return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.TokenError("Email has already been confirmed")]); 

            var confirmResult = await userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
            {  
                return Result.Failure(ResultErrorCode.BAD_REQUEST, [ErrorGenerator.TokenError("Invalid token")]);
            }
            return Result.Success();

        }
      

    }
}
