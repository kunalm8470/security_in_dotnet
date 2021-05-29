namespace AsymmetricKeyEncryption
{
    class Program
    {
        static void Main(string[] args)
        {
            string plainText = "Hello world!!!!";
            AsymmetricKeyEncryption a = new AsymmetricKeyEncryption();
            string encrypted = a.Encrypt(plainText);
            string decrypted = a.Decrypt(encrypted);
        }
    }
}
