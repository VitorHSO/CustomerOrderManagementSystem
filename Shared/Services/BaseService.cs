using System.Security.Cryptography;

namespace Shared.Services
{
    public abstract class BaseService
    {
        private readonly string EncryptionKey = "YaOI3HmanVzi/QVCiFpjX2Z3XOMtZp5VClq3tvJUngY=";

        protected string Encrypt(string field, string encryptionIV)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(EncryptionKey);
                aesAlg.IV = Convert.FromBase64String(encryptionIV);

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(field);
                    }

                    var encrypted = msEncrypt.ToArray();
                    return Convert.ToBase64String(encrypted); // Retorna como string base64
                }
            }
        }

        protected string Decrypt(string encryptedField, string encryptionIV)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(EncryptionKey);
                aesAlg.IV = Convert.FromBase64String(encryptionIV);

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedField)))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd(); // Retorna o valor descriptografado
                }
            }
        }

        protected string HashPassword(string password)
        {
            // Gera o hash da senha usando BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return passwordHash;

            // Estudar no futuro implementação com Argon2 para melhorar performance e resistência a ataques de GPU.
        }
    }
}
