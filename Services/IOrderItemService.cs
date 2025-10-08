using TUQA_Shop.models;
using TUQA_Shop.Services.Iservice;


namespace TUQA_Shop.Services
{
    public interface IOrderItemService : IService<OrderItem>
    {
        Task<List<OrderItem>> AddRangeAsync(List<OrderItem> entities, CancellationToken cancellationToken = default);
    }
}
