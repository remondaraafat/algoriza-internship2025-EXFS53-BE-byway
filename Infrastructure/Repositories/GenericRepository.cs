using APICoursePlatform.RepositoryContract;
using Domain.Common;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace APICoursePlatform.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly CoursePlatformContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(CoursePlatformContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            T entity = await _dbSet.FindAsync(id);

            if (entity is IBaseModel baseModel && baseModel.IsDeleted)
                return null;

            return entity;
        }

        public IQueryable<T> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            IQueryable<T> query = _dbSet;

            if (typeof(IBaseModel).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
            }

            // Pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return query;
        }


        public IQueryable<T> GetWithFilterAsync(Expression<Func<T, bool>> predicate, int pageNumber = 1, int pageSize = 10)
        {
            IQueryable<T> query = _dbSet.Where(predicate);

            if (typeof(IBaseModel).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
            }

            // Pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return query;
        }


        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbSet;
            if (typeof(IBaseModel).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e => EF.Property<bool>(e, "IsDeleted") == false).AsNoTracking();
            }
            return await query.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _dbSet;

            if (typeof(IBaseModel).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e => EF.Property<bool>(e, "IsDeleted") == false).AsNoTracking();
            }
            return predicate == null ? await query.CountAsync()
                                     : await query.CountAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(predicate);

            if (entity == null)
                throw new InvalidOperationException("Entity not found.");

            if (typeof(IBaseModel).IsAssignableFrom(typeof(T)))
            {
                
                var propIsDeleted = typeof(T).GetProperty("IsDeleted");
                if (propIsDeleted != null)
                    propIsDeleted.SetValue(entity, true);

                

                _dbSet.Update(entity);
                return;
            }

            _dbSet.Remove(entity);
        }
        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }
        public IQueryable<T> GetQueryable()
        {
            IQueryable<T> query = _dbSet;

            if (typeof(IBaseModel).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
            }

            return query.AsQueryable();
        }


    }
}