using Hashing.Interfaces;

namespace Hashing.Models
{
    public class BcryptHasher : IHasher
    {
        public string Hash(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text,
                workFactor: 12,
                enhancedEntropy: true
            );
        }
    }
}
