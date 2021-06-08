using Ardalis.Specification;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class RefreshTokenRepository : EfRepository<RefreshToken, int>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(UserContext context) : base(context)
        {
        }

        public async Task DeleteAllAsync(ISpecification<RefreshToken> spec)
        {
            IQueryable<RefreshToken> specificationResult = ApplySpecification(spec);
            _context.RefreshTokens.RemoveRange(await specificationResult.ToListAsync());
            await _context.SaveChangesAsync();
        }
    }
}
