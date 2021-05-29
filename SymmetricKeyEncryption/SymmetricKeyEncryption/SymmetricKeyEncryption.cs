using System;
using System.Security.Cryptography;
using System.Text;

namespace SymmetricKeyEncryption
{
    public class SymmetricKeyEncryption
    {
        private readonly Aes cipher;
        public SymmetricKeyEncryption()
        {
            /*
             *  Defaults to 
             *  
             *  keysize 256
             *  
             *  CBC mode (i.e break the plain text into blocks 
             *  and encrypt each block separately with using IV for the first block
             *  and passing the next IV from the output of first block and so on)
            */
            cipher = Aes.Create();
            cipher.Mode = CipherMode.CBC;
            cipher.Padding = PaddingMode.ISO10126;
        }

        public string Decrypt(string encryptedText)
        {
            byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText);
            using ICryptoTransform cryptTransform = cipher.CreateDecryptor();

            byte[] plainTextBytes = cryptTransform.TransformFinalBlock(encryptedTextBytes, 0, encryptedTextBytes.Length);
            return Encoding.UTF8.GetString(plainTextBytes);
        }

        public string Encrypt(string plainText)
        {
            using ICryptoTransform cryptTransform = cipher.CreateEncryptor();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherTextBytes = cryptTransform.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);

            return Convert.ToBase64String(cipherTextBytes);
        }
    }
}
