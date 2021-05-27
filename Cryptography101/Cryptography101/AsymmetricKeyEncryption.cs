using System;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography101
{
    public class AsymmetricKeyEncryption
    {
        public RSAParameters privateKey;
        public RSAParameters publicKey;
        public RSACryptoServiceProvider csp;

        public AsymmetricKeyEncryption()
        {
            csp = new RSACryptoServiceProvider(2048)
            {
                PersistKeyInCsp = false
            };

            publicKey = csp.ExportParameters(false);
            privateKey = csp.ExportParameters(true);
        }

        public string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherTextBytes = csp.Encrypt(plainTextBytes, RSAEncryptionPadding.Pkcs1);

            return Convert.ToBase64String(cipherTextBytes);
        }

        public string Decrypt(string encryptedText)
        {
            byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText);
            byte[] plainTextBytes = csp.Decrypt(encryptedTextBytes, RSAEncryptionPadding.Pkcs1);

            return Encoding.UTF8.GetString(plainTextBytes);
        }

        ~AsymmetricKeyEncryption()
        {
            csp.Dispose();
        }
    }
}
