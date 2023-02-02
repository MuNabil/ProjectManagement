using DataAccess_EF.Data;
using Domain.Constants;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess_EF.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>>? GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T>? GetById(int? id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public async Task<int> Count()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<int> Count(Expression<Func<T, bool>> criteria)
        {
            return await _context.Set<T>().CountAsync(criteria);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public async Task<T>? Find(Expression<Func<T, bool>> criteria, string[]? includes = null)
        {
            IQueryable<T> query = TableWithInclude(includes);

            return await query.SingleOrDefaultAsync(criteria);
        }

        public async Task<IEnumerable<T>>? FindAll(Expression<Func<T, bool>> criteria, int? skip, int? take, string[]? includes = null,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = TableWithInclude(includes);

            query = query.Where(criteria);

            if (skip.HasValue)
                query = query.Take(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);


            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Descending)
                    query = query.OrderByDescending(orderBy);
                else
                    query = query.OrderBy(orderBy);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>>? FindAll(Expression<Func<T, bool>> criteria, string[]? includes = null)
        {
            IQueryable<T> query = TableWithInclude(includes);

            return await query.Where(criteria).ToListAsync();
        }

        private IQueryable<T> TableWithInclude(string[]? includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
            return query;
        }
    }
}
