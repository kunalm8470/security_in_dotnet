using Ardalis.Specification;
using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken, int>
    {
        public Task DeleteAllAsync(ISpecification<RefreshToken> spec);
    }
}
