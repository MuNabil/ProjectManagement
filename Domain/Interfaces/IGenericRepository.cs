using Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T>? GetById(int? id);
        Task<IEnumerable<T>>? GetAll();
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task<int> Count();
        Task<int> Count(Expression<Func<T, bool>> criteria);
        Task<T>? Find(Expression<Func<T, bool>> criteria, string[]? includes = null);
        public Task<IEnumerable<T>>? FindAll(Expression<Func<T, bool>> criteria, string[]? includes = null);
        Task<IEnumerable<T>>? FindAll(Expression<Func<T, bool>> criteria,
            int? skip, int? take, string[]? includes = null,
            Expression<Func<T, object>>? orderBy = null, string orderByDirection = OrderBy.Ascending);

    }
}
