using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TUQA_Shop.DTOs;
using TUQA_Shop.models;
using TUQA_Shop.Services;
using TUQA_Shop.Utility;

namespace TUQA_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly PasswordResetCodeService passwordResetCodeService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, PasswordResetCodeService passwordResetCodeService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.passwordResetCodeService = passwordResetCodeService;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromBody] RegisterRequestUser request)
        {
            var applicationUser = request.Adapt<ApplicationUser>();
            var result = await userManager.CreateAsync(applicationUser, request.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(applicationUser, StaticData.Customer);
                var token = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                var emailConfirm = Url.Action(nameof(ConfirmEmail), "Account", new { token, userId = applicationUser.Id },
                    protocol: Request.Scheme,
                    host: Request.Host.Value
                    );

                await emailSender.SendEmailAsync(applicationUser.Email, "Confirm email",
                    $"<h1> welcom ..{applicationUser.FirstName}</h1> <p> t-shop , new account <p/>" +
                    $"<a href='{emailConfirm}'> click here </a>");

                return NoContent();
            }
            return BadRequest(result.Errors);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {
            var user = await userManager.FindByEmailAsync(userId);
            if (user is not null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return Ok(new { message = "email confirmed " });
                }
                else
                {
                    return BadRequest(result.Errors);
                }

            }
            return NotFound();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestUser request)
        {
            var applicationUser = await userManager.FindByEmailAsync(request.Email);
            if (applicationUser != null)
            {

                var result = await signInManager.PasswordSignInAsync(applicationUser, request.Password, request.RemmberMe, false);
                List<Claim> claims = new();
                claims.Add(new(ClaimTypes.Name, applicationUser.UserName));
                var userRole = await userManager.GetRolesAsync(applicationUser);
                if (userRole.Count > 0)
                {
                    foreach (var item in userRole)
                        claims.Add(new(ClaimTypes.Role, item));
                }
                if (result.Succeeded)
                {
                    SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("8YZWvXgEuu2eWg9ughTYJ0lr3BRJgIWG"));
                    SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.Sha256);

                    var jwtToken = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: signingCredentials
                        );
                    string token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                    return Ok(new { token });
                }
                if (result.IsLockedOut)
                    return BadRequest(new { message = "your account is locked ,please try again later ." });
                if (result.IsNotAllowed)
                    return BadRequest(new { message = "email not confirmed ." });
            }
            return BadRequest(new { message = "invalid email or password !" });
        }

        [HttpGet("Logout")]

        public async Task<IActionResult> logout()
        {
            await signInManager.SignOutAsync();
            return NoContent();
        }

        [Authorize]
        [HttpPost("ChangePassword")]

        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var applicationUser = await userManager.GetUserAsync(User);

            if (applicationUser != null)
            {
                var result = await userManager.ChangePasswordAsync(applicationUser, request.OldPassword, request.NewPassword);

                if (result.Succeeded)
                    return NoContent();
                else
                    return BadRequest(result.Errors);
            }
            return BadRequest(new { message = "invalid data !" });
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(forgetPassword request)
        {
            var appUser = await userManager.FindByEmailAsync(request.Email);
            if (appUser is not null)
            {
                var code = new Random().Next(1000, 9999).ToString();
                await passwordResetCodeService.AddAsync(new()
                {
                    ApplicationUserId = appUser.Id,
                    Code = code,
                    ExpirationCode = DateTime.Now.AddMinutes(30),
                });
                await emailSender.SendEmailAsync(appUser.Email, "Reset Password",
                   $"<h1> welcom ..{appUser.FirstName}</h1> <p> t-shop , Reset Password <p/>" +
                   $" Code is {code}");
                return Ok(new { message = "Reset code sent " });
            }
            else
            {
                return BadRequest(new { message = "user not found !" });
            }
        }

        [HttpPatch("SendCode")]
        public async Task<IActionResult> SendCode([FromBody] SendCodeRequest request)
        {
            var appUser = await userManager.FindByEmailAsync(request.Email);
            if (appUser is not null)
            {
                var resetCode = (await passwordResetCodeService.GetAsync(e => e.ApplicationUserId == appUser.Id))
                     .OrderByDescending(e => e.ExpirationCode).FirstOrDefault();

                if (resetCode is not null && resetCode.Code == request.Code && resetCode.ExpirationCode > DateTime.Now)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(appUser);
                    var result = await userManager.ResetPasswordAsync(appUser, token, request.Password);
                    if (result.Succeeded)
                    {
                        await emailSender.SendEmailAsync(appUser.Email, "change Password",
                  $"<h1> welcom ..{appUser.FirstName}</h1> <p> t-shop , your Password is changed <p/>");

                        await passwordResetCodeService.RemoveAsync(resetCode.Id);
                        return Ok(new { message = "Password is changed successfully " });
                    }
                    else
                    {
                        return BadRequest(result.Errors);
                    }

                }
                else
                {
                    return BadRequest(new { message = "Invalid code !" });

                }
            }

            return BadRequest(new { message = "user not found !" });
        }



    }
}
