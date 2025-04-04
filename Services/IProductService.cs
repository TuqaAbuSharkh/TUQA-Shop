using System.Linq.Expressions;
using TUQA_Shop.models;

namespace TUQA_Shop.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();

        Product Get(Expression<Func<Product, bool>> expression);

        Product Add(Product product);

        bool update(int id, Product product);

        bool Delete(int id);
    }
}
