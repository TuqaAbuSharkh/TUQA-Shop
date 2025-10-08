using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TUQA_Shop.DTOs;
using TUQA_Shop.models;
using TUQA_Shop.Services;
using TUQA_Shop.Utility;

namespace TUQA_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{StaticData.SuperAdmin}")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var users = await userService.GetAsync();
            return Ok(users.Adapt<IEnumerable<UserResponse>>());
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetByid(string id)
        {
            var users = await userService.GetOne(u =>u.Id == id);

            return Ok(users.Adapt<UserResponse>());

        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> ChangeRole([FromRoute]string userId,[FromQuery] string roleName)
        {
            var result = await userService.ChangeRole(userId, roleName);
            return Ok(result);
        }

        [HttpPut("LockUnlock/{userId}")]

        public async Task<IActionResult> LockUnlock(string userId)
        {
            var result = await userService.LockUnlock(userId);

            if (result == true)
            {
                return Ok();
            }
            else return BadRequest();
        }
    }
}
