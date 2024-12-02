﻿using Shared.Interfaces;
using System.Security.Cryptography;

namespace Shared.Entities
{
    public abstract class BaseEntity : IPreCreatable
    {
        public int Id { get; set; }

        protected BaseEntity()
        {
            //// Usa o número de ticks para gerar um número único com base no tempo.
            //Id = (int)(DateTime.UtcNow.Ticks % int.MaxValue);
        }

        public void PreCreate()
        {
            //if (Id == 0)
            //{
            //    Id = (int)(DateTime.UtcNow.Ticks % int.MaxValue);
            //}
        }

        protected string GenerateEncryptionIV()
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateIV();

                // Converte as chaves em strings base64 para facilitar o armazenamento
                string encryptionIV = Convert.ToBase64String(aesAlg.IV);

                return encryptionIV;
            }
        }
    }
}
