using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TUQA_Shop.DTOs;
using TUQA_Shop.models;
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

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromBody]RegisterRequestUser request)
        {
            var applicationUser = request.Adapt<ApplicationUser>();
            var result = await userManager.CreateAsync(applicationUser, request.Password);

            if (result.Succeeded)
            {
                await emailSender.SendEmailAsync(applicationUser.Email, "Welcom", $"<h1> welcom ..{applicationUser.FirstName}</h1>");
                await userManager.AddToRoleAsync(applicationUser, StaticData.Customer); 
                await signInManager.SignInAsync(applicationUser, false);
                return NoContent();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestUser request)
        {
            var  applicationUser = await userManager.FindByEmailAsync(request.Email);
            if (applicationUser != null)
            {
                var result =await userManager.CheckPasswordAsync(applicationUser, request.Password);
                if (result)
                {
                    await signInManager.SignInAsync(applicationUser,request.RemmberMe);
                    return NoContent();
                }

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

            if(applicationUser != null)
            {
                var result = await userManager.ChangePasswordAsync(applicationUser, request.OldPassword, request.NewPassword);

                if (result.Succeeded)
                    return NoContent();
                else
                    return BadRequest(result.Errors);
            }
            return BadRequest(new { message = "invalid data !" });
        }
    }
}
