using backend.DataAccess.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace backend.DataAccess
{
    public class DataRepository<T> : IDataRepository<T> where T : class
    {
        private readonly DataContext _db;
        private readonly DbSet<T> table;

        public DataRepository(DataContext db)
        {
            _db = db;
            table = _db.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await table.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsyncInclude(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = table;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (criteria != null)
            {
                query = query.Where(criteria);
            }

            return await query.ToListAsync();
        }


        public async Task<T> GetByIdAsync(int id)
        {
            return await table.FindAsync(id);
        }

        public async Task<T> GetByIdAsyncInclude(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = table;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(criteria);
        }

        public async Task<T> GetByCustomCriteria(Expression<Func<T, bool>> criteria)
        {
            return await table.FirstOrDefaultAsync(criteria);
        }

        public async Task AddAsync(T entity)
        {
            await table.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
        }

        public async Task DeleteAsync(T entity)
        {
            table.Remove(entity);
        }

        public async Task<bool> Save()
        {
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
