using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;
using TUQA_Shop.Data;
using TUQA_Shop.models;
using TUQA_Shop.Services.Iservice;

namespace TUQA_Shop.Services
{
    public class CategoryService :Service<Category> , ICategoryService
    {
        private readonly ApplicationDbContext context;

        public CategoryService(ApplicationDbContext C):base(C)
        {
            context = C;
        }

       public async Task<bool> UpdateToggleAsync(int id, CancellationToken cancellationToken = default)
    {
        Category? categoryIndb = context.Categories.Find(id);
        if (categoryIndb is null)
            return false;

        categoryIndb.Status = !categoryIndb.Status;
         await context.SaveChangesAsync(cancellationToken);
        return true;
    }
        public async Task<bool> updateAsync(int id, Category category, CancellationToken cancellationToken = default)
        {

            Category? categoryIndb = context.Categories.Find(id);
            if (categoryIndb is null)
                return false;

            categoryIndb.Name = category.Name;
            categoryIndb.Description = category.Description;
            await context.SaveChangesAsync(cancellationToken);
            return true;

        }

            
    }
}
