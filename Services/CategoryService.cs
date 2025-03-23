using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TUQA_Shop.Data;
using TUQA_Shop.models;

namespace TUQA_Shop.Services
{
    public class CategoryService : ICategoryService
    {
        ApplicationDbContext context;

        public CategoryService(ApplicationDbContext C)
        {
            context = C;
        }

        Category ICategoryService.Add(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();

            return category;
        }

        bool ICategoryService.Delete(int id)
        {
            Category? category = context.Categories.Find(id);
            if (category is null)
                return false;

            context.Categories.Remove(category);
            context.SaveChanges();
            return true;
        }

        Category ICategoryService.Get(Expression<Func<Category, bool>> expression)
        {
            return context.Categories.FirstOrDefault(expression);
        }

        IEnumerable<Category> ICategoryService.GetAll()
        {
            return context.Categories.ToList();
        }

        bool ICategoryService.update(int id, Category category)
        {
            Category? categoryInDatabase = context.Categories.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (categoryInDatabase is null)
                return false;

            category.Id = id;
            context.Categories.Update(category);
            context.SaveChanges();
            return true;

        }
    }
}
