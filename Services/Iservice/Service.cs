using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TUQA_Shop.Data;

namespace TUQA_Shop.Services.Iservice
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbset;
        public Service(ApplicationDbContext context)
        {
            this._context = context;
            _dbset = context.Set<T>();
        }
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? expression, Expression<Func<T, object>>?[] includes = null, bool IsTracked = true)
        {
            IQueryable<T> entity = _dbset;

            if (expression is not null)
            {
                entity = entity.Where(expression);
            }

            if (includes is not null)
            {
                foreach(var item in includes)
                {
                    entity = entity.Include(item);
                }
            }
            return await entity.ToListAsync();
        }

        public async Task<T?> GetOne(Expression<Func<T, bool>>? expression= null, Expression<Func<T, object>>?[] includes = null, bool IsTracked = true)
        {
            return  _dbset.FirstOrDefault(expression);
        }

        public async Task<bool> RemoveAsync(int id, CancellationToken cancellationToken = default)
        {
            T? entityInDb = _dbset.Find(id);
            if (entityInDb == null)
                return false;

            _dbset.Remove(entityInDb);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
