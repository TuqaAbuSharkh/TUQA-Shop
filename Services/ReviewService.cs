using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;
using TUQA_Shop.Data;
using TUQA_Shop.models;
using TUQA_Shop.Services.Iservice;

namespace TUQA_Shop.Services
{
    public class ReviewService :Service<Review> , IReviewService
    {
        private readonly ApplicationDbContext context;

        public ReviewService(ApplicationDbContext C):base(C)
        {
            context = C;
        }

        public async Task<bool> updateAsync(int id, Review review, CancellationToken cancellationToken = default)
        {

            Review? ReviewIndb = context.Reviews.Find(id);
            if (ReviewIndb is null)
                return false;

            ReviewIndb.Comment = review.Comment;
            ReviewIndb.Rate = review.Rate;
            await context.SaveChangesAsync(cancellationToken);
            return true;

        }

            
    }
}
