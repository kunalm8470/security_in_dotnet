using Ardalis.Specification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository<TEnt, TKey> where TEnt : class where TKey : struct
    {
        public Task<TEnt> GetByIdAsync(TKey id);
        public Task<int> CountAsync(ISpecification<TEnt> spec);
        public Task<TEnt> FirstOrDefaultAsync(ISpecification<TEnt> spec);
        public Task<IReadOnlyList<TEnt>> ListAsync(ISpecification<TEnt> spec);
        public Task<TEnt> AddAsync(TEnt entity);
        public Task UpdateAsync(TEnt entity);
        public Task DeleteAsync(TEnt entity);
    }
}
