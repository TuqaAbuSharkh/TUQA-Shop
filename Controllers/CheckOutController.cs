using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using TUQA_Shop.DTOs;
using TUQA_Shop.models;
using TUQA_Shop.Services;

namespace TUQA_Shop.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CheckOutController : ControllerBase
    {
        private readonly ICartService cartService;
        private readonly IOrderService orderService;
        private readonly IEmailSender emailSender;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOrderItemService orderItemService;

        public CheckOutController(ICartService cartService, IOrderService orderService,
            IEmailSender emailSender,UserManager<ApplicationUser> userManager, IOrderItemService orderItemService)
        {
            this.cartService = cartService;
            this.orderService = orderService;
            this.emailSender = emailSender;
            this.userManager = userManager;
            this.orderItemService = orderItemService;
        }
        [HttpGet("Pay")]
        public async Task<IActionResult> Pay([FromBody] PaymentRequest request)
        {
            var appUser = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var carts = await cartService.GetAsync(e => e.ApplicationUserId == appUser,[ e=>e.Product]);

            if (carts is not null)
            {
                Order order = new()
                {
                    OrderStatus = OrderStatus.Pending,
                    OrderDate = DateTime.Now,
                    TotalPrice = carts.Sum( e => e.Product.Price * e.Count),
                    ApplicationUserId = appUser,
                };

                if(request.PaymentMethod == "Cash")
                {
                    order.PaymentMethodType = PaymentMethodType.Cash;
                    await orderService.AddAsync(order);
                    return RedirectToAction(nameof(Success), new { orderId = order.Id });
                }else if(request.PaymentMethod == "Visa")
                {

                    order.PaymentMethodType = PaymentMethodType.Visa;

                    var options = new SessionCreateOptions
                    {
                        PaymentMethodTypes = new List<string> { "card" },
                        LineItems = new List<SessionLineItemOptions>(),

                        Mode = "Payment",
                        SuccessUrl = $"{Request.Scheme}://{Request.Host}/api/checkout/success/{order.Id}",
                        CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
                    };
                    foreach (var cart in carts)
                    {
                        options.LineItems.Add(
                            new SessionLineItemOptions
                            {
                                PriceData = new SessionLineItemPriceDataOptions
                                {
                                    Currency = "USD",
                                    ProductData = new SessionLineItemPriceDataProductDataOptions
                                    {
                                        Name = cart.Product.Name,
                                        Description = cart.Product.Description,
                                    },
                                    UnitAmount = (long)cart.Product.Price * 100,
                                },
                                Quantity = cart.Count,
                            }
                            );
                    }

                    var service = new SessionService();
                    var session = service.Create(options);
                    order.SessionId = session.Id;
                   await orderService.CommitAsync();
                    await orderService.AddAsync(order);

                    return Ok(new { session.Url });
                }
                else
                {
                    return BadRequest(new { message = "Invalid Payment Method !!" });
                }

                   

            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("Success/{orderId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Success([FromRoute] int orderId)
        {
            var order = await orderService.GetOne(e => e.Id == orderId);
            var appUser = await userManager.FindByIdAsync(order.ApplicationUserId);
            var subject = "";
            var body = "";
            var carts = await cartService.GetAsync(e => e.ApplicationUserId == appUser.Id, [e=> e.Product]);

            List<OrderItem> orderItems = [];
            foreach(var item in carts)
            {
                orderItems.Add(new()
                {
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    TotalPrice = item.Product.Price * item.Count,
                });
                item.Product.Quantity -= item.Count;

            }
            await orderItemService.AddRangeAsync(orderItems);
            await cartService.RemoveRangeAsync(carts.ToList());
            await orderService.CommitAsync();



            if (order.PaymentMethodType == PaymentMethodType.Cash)
            {
                subject = "Order Recived - Cash Payment";
                body = $"<h1> welcom ..{appUser.FirstName}</h1> <p> your Order with {orderId} has been placed successfuly <p/>";
            }
            else
            {
                order.OrderStatus = OrderStatus.Approved;
                var service = new SessionService();
                var session = service.Get(order.SessionId);
                order.TransactionId = session.PaymentIntentId;
                await orderService.CommitAsync();


                subject = "Order Payment Success ";
                body = $"<h1> welcom ..{appUser.FirstName}</h1> <p>Thank you! <p/>";

            }

            await emailSender.SendEmailAsync(appUser.Email,subject ,body );

            return Ok(new { message = "Done" });
        }
    }
}
