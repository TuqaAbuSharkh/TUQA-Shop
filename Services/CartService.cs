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
    }
}
