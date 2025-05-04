using System.Linq.Expressions;
using TUQA_Shop.models;
using TUQA_Shop.DTOs;


namespace TUQA_Shop.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll(string? query,int page,int limit);

        Product Get(Expression<Func<Product, bool>> expression);

        Product Add(ProductRequest product);

        bool update(int id, ProductUpdateRequest product);

        bool Delete(int id);
    }
}
