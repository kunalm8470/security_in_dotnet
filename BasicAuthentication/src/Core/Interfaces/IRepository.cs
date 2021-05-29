using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository<TEnt, TKey> where TEnt : class
    {
        Task<IEnumerable<TEnt>> GetAllAsync(int page, int limit);
        Task<IEnumerable<TEnt>> GetAllAsync(Expression<Func<TEnt, bool>> predicate, int page, int limit);
        Task<TEnt> FirstOrDefaultAsync(Expression<Func<TEnt, bool>> predicate);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEnt, bool>> predicate);
        Task<TEnt> AddAsync(TEnt entity);
        Task<TEnt> UpdateAsync(TEnt entity);
        Task<TEnt> DeleteAsync(TKey id);
    }
}
