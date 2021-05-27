using System;

namespace Cryptography101
{
    class Program
    {
        static void Main(string[] args)
        {
            string plainText = "Hello world!!!";

            Console.WriteLine("Cryptography 101");
            Console.WriteLine("1. Hashing");
            Console.WriteLine("2. Symmetric Key Encrpytion");
            Console.WriteLine("3. Asymmetric Key Encrpytion");
            Console.WriteLine("Enter choice (1-3)");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid choice");
                return;
            }

            switch (choice)
            {
                case 1:
                    Console.WriteLine(Hashing.Hash(plainText));
                    break;

                case 2:
                    (string encrypted, string key, string IV) = SymmetricKeyEncryption.Encrypt(plainText);

                    Console.WriteLine("Encrypted - {0}", encrypted);
                    Console.WriteLine("Decrypted - {0}", SymmetricKeyEncryption.Decrypt(encrypted, key, IV));
                    break;

                case 3:
                    AsymmetricKeyEncryption enc = new AsymmetricKeyEncryption();
                    string encryptedText = enc.Encrypt(plainText);

                    Console.WriteLine("Encrypted - {0}", encryptedText);
                    Console.WriteLine("Decrypted - {0}", enc.Decrypt(encryptedText));
                    break;

                default:
                    Console.WriteLine("No input provided!!!");
                    break;
            }
        }
    }
}
