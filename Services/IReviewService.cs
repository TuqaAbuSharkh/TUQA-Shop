using System.Linq.Expressions;
using TUQA_Shop.models;
using TUQA_Shop.Services.Iservice;


namespace TUQA_Shop.Services
{
    public interface IReviewService :IService<Review>
    {
        public Task<bool> updateAsync(int id, Review review, CancellationToken cancellationToken = default);

    }
}
