using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        Task<RefreshToken> FetchRefreshToken(string token);
        Task PersistRefreshToken(string refreshToken, int userId);
        Task DeleteRefreshToken(string refreshToken);
        Task DeleteRefreshTokenForUser(int userId);
        bool ValidateRefreshToken(RefreshToken token);
    }
}
