using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TUQA_Shop.models;
using TUQA_Shop.Services;

namespace TUQA_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly UserManager<ApplicationUser> userManager;


        public CartController(ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            this.cartService = cartService;
            this.userManager = userManager;
        }

        [HttpPost("{ProductId}")]
        public async Task<IActionResult> AddToCart([FromQuery]int count,[FromRoute]int ProductId)
        {
            var appUser = userManager.GetUserId(User);
            var cart = new Cart()
            {
                ProductId = ProductId,
                Count = count,
                ApplicationUserId = appUser
            };
            await cartService.AddAsync(cart);
            return Ok(cart);
        }
    }
}
