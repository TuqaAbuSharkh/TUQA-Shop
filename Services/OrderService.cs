using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;
using TUQA_Shop.Data;
using TUQA_Shop.models;
using TUQA_Shop.Services.Iservice;

namespace TUQA_Shop.Services
{
    public class OrderService :Service<Order> , IOrderService
    {
        private readonly ApplicationDbContext context;

        public OrderService(ApplicationDbContext C):base(C)
        {
            context = C;
        }

      

            
    }
}
