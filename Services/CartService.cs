using Microsoft.EntityFrameworkCore;
using TUQA_Shop.Data;
using TUQA_Shop.models;
using TUQA_Shop.Services.Iservice;

namespace TUQA_Shop.Services
{
    public class CartService : Service<Cart>, ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<Cart> AddToCart(string userId, int ProductId, CancellationToken cancellationToken)
        {
            var exsistingCartItems = await _context.Carts.FirstOrDefaultAsync(e => e.ApplicationUserId == userId && e.ProductId == ProductId);

            if (exsistingCartItems is not null)
            {
                exsistingCartItems.Count += 1;
            }
            else
            {
                exsistingCartItems = new Cart
                {
                    ProductId = ProductId,
                    ApplicationUserId = userId,
                    Count = 1
                };
                await _context.Carts.AddAsync(exsistingCartItems, cancellationToken);

            }
            await _context.SaveChangesAsync();
            return exsistingCartItems;
        }


        public async Task<IEnumerable<Cart>> GetUserCartAsync(string UserId)
        {
            return await GetAsync(e=> e.ApplicationUserId==UserId,includes: [c=> c.Product]);
        }


        public async Task<bool> RemoveRangeAsync(List<Cart> items, CancellationToken cancellationToken = default)
        {
            _context.RemoveRange(items);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }


    }
}
