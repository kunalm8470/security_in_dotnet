using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace AsymmetricKeyEncryption
{
    public class AsymmetricKeyEncryption
    {
        private readonly RSA _privateKey;
        private readonly RSA _publicKey;
        public AsymmetricKeyEncryption()
        {
            string privateKey = File.ReadAllText("..\\..\\..\\Keys\\private_key.pem");
            string publicKey = File.ReadAllText("..\\..\\..\\Keys\\public_key.pem");

            _privateKey = RSA.Create();
            _privateKey.ImportFromPem(privateKey.ToCharArray());

            _publicKey = RSA.Create();
            _publicKey.ImportFromPem(publicKey.ToCharArray());
        }

        public string Encrypt(string text)
        {
            byte[] encrypted = _publicKey.Encrypt(Encoding.UTF8.GetBytes(text), RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string encrypted)
        {
            byte[] decrypted = _privateKey.Decrypt(Convert.FromBase64String(encrypted), RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(decrypted, 0, decrypted.Length);
        }
    }
}
