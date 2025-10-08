using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TUQA_Shop.models;
using TUQA_Shop.Services;
using System.Security.Claims;
using Mapster;
using TUQA_Shop.DTOs;

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
        public async Task<IActionResult> AddToCart([FromRoute]int ProductId,CancellationToken cancellationToken)
        {
            var appUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = cartService.AddToCart(appUser, ProductId, cancellationToken);

            return Ok();
        }
        [HttpGet("")]

       public async Task<IActionResult> GetUserCartAsync(string UserId)
        {

            var appUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartItems = await cartService.GetUserCartAsync(UserId);
            var result = cartItems.Select(e=>e.Product).Adapt<IEnumerable<CartResponse>>();
            var totalPrice = cartItems.Sum(e => e.Product.Price * e.Count);

            return Ok(new {result,totalPrice});

        }
    }
}
