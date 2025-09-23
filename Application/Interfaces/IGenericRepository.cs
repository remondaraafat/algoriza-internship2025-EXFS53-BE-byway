using System.Linq.Expressions;

namespace APICoursePlatform.RepositoryContract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        IQueryable<T> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        IQueryable<T> GetWithFilterAsync(Expression<Func<T, bool>> predicate, int pageNumber = 1, int pageSize = 10);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        Task AddAsync(T entity);
        void Update(T entity);
        IQueryable<T> GetQueryable();
        Task DeleteAsync(Expression<Func<T, bool>> predicate);

    }
}
