using System.Linq.Expressions;
using TUQA_Shop.models;

namespace TUQA_Shop.Services
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();

        Category Get(Expression<Func<Category, bool>> expression);

        Category Add(Category category);

        bool update(int id, Category category);

        bool Delete(int id);
    }
}
