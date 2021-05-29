using System;

namespace SymmetricKeyEncryption
{
    class Program
    {
        static void Main(string[] args)
        {
            string plainText = "Hello World!!!";
            SymmetricKeyEncryption s = new SymmetricKeyEncryption();
            string encryptedText = s.Encrypt(plainText);
            string decryptedText = s.Decrypt(encryptedText);
        }
    }
}
