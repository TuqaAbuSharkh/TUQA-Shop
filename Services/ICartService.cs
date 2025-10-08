using TUQA_Shop.models;
using TUQA_Shop.Services.Iservice;

namespace TUQA_Shop.Services
{
    public interface ICartService : IService<Cart>
    {
        Task<Cart> AddToCart(string userId, int ProductId, CancellationToken cancellationToken);
        Task<IEnumerable<Cart>> GetUserCartAsync(string UserId);

        Task<bool> RemoveRangeAsync(List<Cart> items, CancellationToken cancellationToken = default);
    }
}
