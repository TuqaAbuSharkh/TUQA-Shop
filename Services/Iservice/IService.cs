using System.Linq.Expressions;

namespace TUQA_Shop.Services.Iservice
{
    public interface IService<T> where T :class
    {
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? expression= null, Expression<Func<T, object>>?[] includes = null, bool IsTracked = true);

       Task< T?> GetOne(Expression<Func<T, bool>> expression, Expression<Func<T, object>>?[] includes = null ,bool IsTracked = true);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        Task<bool> RemoveAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CommitAsync( CancellationToken cancellationToken = default);
    }

}
