using Shared.Entities;

namespace COMS.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public Customer() : base()
        {
            EncryptionIV = GenerateEncryptionIV();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string EncryptionIV { get; set; }
    }
}
