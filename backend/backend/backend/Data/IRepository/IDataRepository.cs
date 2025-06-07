using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace backend.DataAccess.IRepository
{
    public interface IDataRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsyncInclude(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsyncInclude(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes);
        Task<T> GetByCustomCriteria(Expression<Func<T, bool>> criteria);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> Save();
    }
}
