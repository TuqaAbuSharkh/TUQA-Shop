using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;
using TUQA_Shop.Data;
using TUQA_Shop.models;
using TUQA_Shop.Services.Iservice;

namespace TUQA_Shop.Services
{
    public class OrderItemService :Service<OrderItem> , IOrderItemService
    {
        private readonly ApplicationDbContext context;

        public OrderItemService(ApplicationDbContext C):base(C)
        {
            context = C;
        }

        public async Task<List<OrderItem>> AddRangeAsync(List<OrderItem> entities, CancellationToken cancellationToken = default)
        {
            await context.AddRangeAsync(entities, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return entities;
        }


    }
}
