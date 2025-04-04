using System.Linq.Expressions;
using TUQA_Shop.models;

namespace TUQA_Shop.Services
{
    public interface IBrandService
    {
        IEnumerable<Brand> GetAll();

        Brand Get(Expression<Func<Brand, bool>> expression);

        Brand Add(Brand category);

        bool update(int id, Brand category);

        bool Delete(int id);
    }
}
