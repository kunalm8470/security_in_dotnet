using Core.Entities;
using Core.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly AuthenticationConfiguration _authConfig;
        public AccessTokenService(AuthenticationConfiguration authConfig)
        {
            _authConfig = authConfig;
        }

        public string Generate(User user)
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
            SigningCredentials credentials = new(new RsaSecurityKey(privateKey), SecurityAlgorithms.RsaSha512);

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
    }
}
