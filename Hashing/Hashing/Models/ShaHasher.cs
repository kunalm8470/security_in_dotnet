using Hashing.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Hashing.Models
{
    public class ShaHasher : IHasher
    {
        /// <summary>
        /// Create hash from SHA-2 family (SHA512)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Hash(string text)
        {
            SHA512 hashSvc = SHA512.Create();
            byte[] hash = hashSvc.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder result = new();
            for (int i = 0; i < hash.Length; i++)
            {
                // Format in hexadecimal - https://stackoverflow.com/questions/20750062/what-is-the-meaning-of-tostringx2
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}
