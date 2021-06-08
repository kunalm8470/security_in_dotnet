using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Services;
using Core.Specifications;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AuthenticationConfiguration _authConfig;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public RefreshTokenService(AuthenticationConfiguration authConfig,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _authConfig = authConfig;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<RefreshToken> FetchTokenAsync(string token)
        {
            return await _refreshTokenRepository.FirstOrDefaultAsync(new RefreshTokenWithUserSpecification(token));
        }

        public async Task<bool> TryDeleteTokenAsync(string refreshToken)
        {
            RefreshToken found = await _refreshTokenRepository.FirstOrDefaultAsync(new RefreshTokenWithUserSpecification(refreshToken));
            if (found == default)
                return false;

            await _refreshTokenRepository.DeleteAsync(found);
            return true;
        }

        public async Task DeleteTokenByUserAsync(int userId)
        {
            await _refreshTokenRepository.DeleteAllAsync(new DeleteRefreshTokensForUserSpecification(userId));
        }

        public string Generate()
        {
            RSA privateKey = RSA.Create();
            privateKey.ImportFromPem(_authConfig.RefreshTokenKeys.PrivateKey);
            SigningCredentials credentials = new(new RsaSecurityKey(privateKey), SecurityAlgorithms.RsaSha512);

            JwtSecurityToken token = new(
                issuer: _authConfig.Issuer,
                audience: _authConfig.Audience,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_authConfig.RefreshTokenExpirationMinutes),
                signingCredentials: credentials
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task PersistTokenAsync(string refreshToken, int userId)
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

        public bool Validate(RefreshToken token)
        {
            if (token.ExpireAt > DateTime.UtcNow)
                throw new RefreshTokenExpiredException();

            RSA publicKey = RSA.Create();
            publicKey.ImportFromPem(_authConfig.RefreshTokenKeys.PublicKey);

            TokenValidationParameters validationParameters = new()
            {
                IssuerSigningKey = new RsaSecurityKey(publicKey),
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
                tokenHandler.ValidateToken(token.Token, validationParameters, out _);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
