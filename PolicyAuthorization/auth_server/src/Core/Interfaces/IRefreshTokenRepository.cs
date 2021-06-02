using Ardalis.Specification;
using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken, int>
    {
        Task DeleteAsync(ISpecification<RefreshToken> spec); 
        Task DeleteAllAsync(int userId);
    }
}
