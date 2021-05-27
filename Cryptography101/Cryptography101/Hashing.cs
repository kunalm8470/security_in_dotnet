using System.Security.Cryptography;
using System.Text;

namespace Cryptography101
{
    public static class Hashing
    {
        /// <summary>
        /// Create hash from SHA-2 family (SHA512)
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Hash(string plainText)
        {
            SHA512 hashSvc = SHA512.Create();
            byte[] hash = hashSvc.ComputeHash(Encoding.UTF8.GetBytes(plainText));
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}
