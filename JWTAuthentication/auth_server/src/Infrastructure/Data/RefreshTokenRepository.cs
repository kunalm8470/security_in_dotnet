using Ardalis.Specification;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class RefreshTokenRepository : EfRepository<RefreshToken, int>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(UserContext context) : base(context)
        {

        }

        public async Task DeleteAllAsync(int userId)
        {
            IEnumerable<RefreshToken> refreshTokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            _context.RefreshTokens.RemoveRange(refreshTokens);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ISpecification<RefreshToken> spec)
        {
            IQueryable<RefreshToken> specificationResult = ApplySpecification(spec);
            RefreshToken found = await specificationResult.FirstOrDefaultAsync();
            if (found == default)
                return;

            _context.RefreshTokens.Remove(found);
            await _context.SaveChangesAsync();
        }
    }
}
