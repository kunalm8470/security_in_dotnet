using System;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography101
{
    public static class SymmetricKeyEncryption
    {
        public static string Decrypt(string encryptedText, string key, string IV)
        {
            byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText);

            using Aes cipher = Aes.Create();
            cipher.Key = Convert.FromBase64String(key);
            cipher.IV = Convert.FromBase64String(IV);
            cipher.Mode = CipherMode.CBC;
            cipher.Padding = PaddingMode.ISO10126;

            using ICryptoTransform cryptTransform = cipher.CreateDecryptor();

            byte[] plainTextBytes = cryptTransform.TransformFinalBlock(encryptedTextBytes, 0, encryptedTextBytes.Length);
            return Encoding.UTF8.GetString(plainTextBytes);
        }

        public static (string encrypted, string key, string IV) Encrypt(string plainText)
        {
            /*
             *  Defaults to 
             *  
             *  keysize 256
             *  
             *  CBC mode (i.e break the plain text into blocks 
             *  and encrypt each block separately with using IV for the first block
             *  and passing the next IV from the output of first block and so on
             *  
            */
            using Aes cipher = Aes.Create();
            cipher.Mode = CipherMode.CBC;
            cipher.Padding = PaddingMode.ISO10126;

            using ICryptoTransform cryptTransform = cipher.CreateEncryptor();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherTextBytes = cryptTransform.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

            return (
                encrypted: Convert.ToBase64String(cipherTextBytes),
                key: Convert.ToBase64String(cipher.Key),
                IV: Convert.ToBase64String(cipher.IV)
           );
        }
    }
}
