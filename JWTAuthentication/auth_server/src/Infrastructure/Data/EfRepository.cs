using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EfRepository<TEnt, TKey> : IRepository<TEnt, TKey> where TEnt : class where TKey : struct
    {
        protected readonly UserContext _context;
        public EfRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<TEnt> AddAsync(TEnt entity)
        {
            await _context.Set<TEnt>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<int> CountAsync(ISpecification<TEnt> spec)
        {
            IQueryable<TEnt> specificationResult = ApplySpecification(spec);
            return await specificationResult.CountAsync();
        }

        public async Task DeleteAsync(TEnt entity)
        {
            _context.Set<TEnt>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<TEnt> FirstOrDefaultAsync(ISpecification<TEnt> spec)
        {
            IQueryable<TEnt> specificationResult = ApplySpecification(spec);
            return await specificationResult.FirstOrDefaultAsync();
        }

        public async Task<TEnt> GetByIdAsync(TKey id)
        {
            return await _context.Set<TEnt>().FindAsync(id);
        }

        public async Task<IReadOnlyList<TEnt>> ListAsync(ISpecification<TEnt> spec)
        {
            IQueryable<TEnt> specificationResult = ApplySpecification(spec);
            return await specificationResult.ToListAsync();
        }

        public async Task UpdateAsync(TEnt entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        protected IQueryable<TEnt> ApplySpecification(ISpecification<TEnt> spec)
        {
            SpecificationEvaluator evaluator = new();
            return evaluator.GetQuery(_context.Set<TEnt>().AsQueryable(), spec);
        }
    }
}
