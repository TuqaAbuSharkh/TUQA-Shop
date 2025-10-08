using TUQA_Shop.models;
using TUQA_Shop.Services.Iservice;

namespace TUQA_Shop.Services
{
    public interface ICartService : IService<Cart>
    {
<<<<<<< HEAD
        Task<Cart> AddToCart(string userId, int ProductId, CancellationToken cancellationToken);
        Task<IEnumerable<Cart>> GetUserCartAsync(string UserId);

        Task<bool> RemoveRangeAsync(List<Cart> items, CancellationToken cancellationToken = default);
=======
>>>>>>> c834fd62c84bfde81c178f6e24c295094fbd524a
    }
}
