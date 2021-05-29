using Hashing.Interfaces;
using Hashing.Models;
using System;

namespace Hashing
{
    class Program
    {
        static void Main(string[] args)
        {
            string plainText = "Hello world!!!";
            IHasher shaHasher = new ShaHasher();
            IHasher bcryptHasher = new BcryptHasher();

            Console.WriteLine("SHA 512 hash - {0}", shaHasher.Hash(plainText));
            Console.WriteLine("Bcrypt hash - {0}", bcryptHasher.Hash(plainText));
        }
    }
}
