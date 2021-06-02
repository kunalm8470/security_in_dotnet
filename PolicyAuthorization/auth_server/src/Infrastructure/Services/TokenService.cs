using Core.Entities;
using Core.Interfaces;
using Core.Services;
using Core.Specifications;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly AuthenticationConfiguration _authConfig;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public TokenService(AuthenticationConfiguration authConfig,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _authConfig = authConfig;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<RefreshToken> FetchRefreshToken(string token)
        {
            return await _refreshTokenRepository.FirstOrDefaultAsync(new RefreshTokenWithUserSpecification(token));
        }

        public async Task DeleteRefreshToken(string refreshToken)
        {
            await _refreshTokenRepository.DeleteAsync(new DeleteRefreshTokenByValueSpecification(refreshToken));
        }

        public async Task DeleteRefreshTokenForUser(int userId)
        {
            await _refreshTokenRepository.DeleteAllAsync(userId);
        }

        public string GenerateAccessToken(User user)
        {
            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Birthdate, user.DateOfBirth.ToString("yyyy-MM-dd"))
            };

            RSA privateKey = RSA.Create();
            privateKey.ImportFromPem(_authConfig.AccessTokenKeys.PrivateKey);

            SecurityKey key = new RsaSecurityKey(privateKey);
            SigningCredentials credentials = new(key, SecurityAlgorithms.RsaSha512);

            JwtSecurityToken token = new(
                    issuer: _authConfig.Issuer,
                    audience: _authConfig.Audience,
                    claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddMinutes(_authConfig.AccessTokenExpirationMinutes),
                    signingCredentials: credentials
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            RSA privateKey = RSA.Create();
            privateKey.ImportFromPem(_authConfig.RefreshTokenKeys.PrivateKey);

            SecurityKey key = new RsaSecurityKey(privateKey);
            SigningCredentials credentials = new(key, SecurityAlgorithms.RsaSha512);

            JwtSecurityToken token = new(
                    issuer: _authConfig.Issuer,
                    audience: _authConfig.Audience,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddMinutes(_authConfig.RefreshTokenExpirationMinutes),
                    signingCredentials: credentials
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task PersistRefreshToken(string refreshToken, int userId)
        {
            RefreshToken token = new RefreshToken
            {
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.Now.AddMinutes(_authConfig.RefreshTokenExpirationMinutes).ToUniversalTime(),
                UserId = userId
            };
            await _refreshTokenRepository.AddAsync(token);
        }

        public bool ValidateRefreshToken(RefreshToken token)
        {
            if (token.ExpireAt > DateTime.UtcNow)
                return false;

            RSA publicKey = RSA.Create();
            publicKey.ImportFromPem(_authConfig.RefreshTokenKeys.PublicKey);

            SecurityKey key = new RsaSecurityKey(publicKey);

            TokenValidationParameters validationParameters = new()
            {
                IssuerSigningKey = key,
                ValidIssuer = _authConfig.Issuer,
                ValidAudience = _authConfig.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                JwtSecurityTokenHandler tokenHandler = new();
                tokenHandler.ValidateToken(token.Token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
