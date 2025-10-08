using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TUQA_Shop.DTOs;
using TUQA_Shop.models;
using TUQA_Shop.Services;

namespace TUQA_Shop.Controllers
{
    [Route("api/products/{productId}/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IOrderItemService orderItemService;
        private readonly IReviewService reviewService;

        public ReviewsController(IOrderItemService orderItemService, IReviewService reviewService)
        {
            this.orderItemService = orderItemService;
            this.reviewService = reviewService;
        }


        [HttpPost]
        public async Task<IActionResult> Create(int productId,[FromBody] ReviewRequest request)
        {
            var appUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var hasOrder =( await orderItemService.GetAsync(e=>e.ProductId == productId && e.Order.ApplicationUserId == appUser)).Any();


            if (hasOrder)
            {
                var hasReview =  (await reviewService.GetAsync(e=>e.ProductId== productId && e.ApplicationUserId == appUser)).Any();

                if (!hasReview)
                {
                    var review = request.Adapt<Review>();
                    review.ApplicationUserId = appUser;
                    review.ReviewDate = DateTime.Now;
                    await reviewService.AddAsync(review);

                    return Ok(new { message = " review added successfully " });
                }
                return BadRequest(new { message = "one comment is allowed!" });

            }
            return BadRequest(new { message = "you can only review the product you already ordered !" });
        }
    }
}
