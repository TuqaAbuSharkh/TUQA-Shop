using System.Linq.Expressions;
using TUQA_Shop.models;
using TUQA_Shop.Services.Iservice;


namespace TUQA_Shop.Services
{
    public interface ICategoryService :IService<Category>
    {
        public Task<bool> UpdateToggleAsync(int id, CancellationToken cancellationToken = default);
        public Task<bool> updateAsync(int id, Category category, CancellationToken cancellationToken = default);

    }
}
