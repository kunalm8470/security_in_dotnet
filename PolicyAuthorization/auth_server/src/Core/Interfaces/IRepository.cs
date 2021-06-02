using Ardalis.Specification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository<TEnt, TKey> where TEnt : class where TKey : struct
    {
        Task<TEnt> GetByIdAsync(TKey id);
        Task<int> CountAsync(ISpecification<TEnt> spec);
        Task<TEnt> FirstOrDefaultAsync(ISpecification<TEnt> spec);
        Task<IReadOnlyList<TEnt>> ListAsync(ISpecification<TEnt> spec);
        Task<TEnt> AddAsync(TEnt entity);
        Task UpdateAsync(TEnt entity);
        Task DeleteAsync(TEnt entity);
    }
}
