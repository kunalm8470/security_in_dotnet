using Core.Entities;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IRefreshTokenService
    {
        public string Generate();
        public bool Validate(RefreshToken token);
        public Task<RefreshToken> FetchTokenAsync(string token);
        public Task PersistTokenAsync(string refreshToken, int userId);
        public Task<bool> TryDeleteTokenAsync(string refreshToken);
        public Task DeleteTokenByUserAsync(int userId);
    }
}
