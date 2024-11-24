using COMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace COMS.Infra.Data.EntitiesConfig
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("CustomerId").IsRequired();
            builder.Property(c => c.Name).HasMaxLength(255).IsRequired();
            builder.Property(c => c.Email).HasMaxLength(255).IsRequired();
            builder.Property(u => u.Phone).HasMaxLength(255).IsRequired();
        }
    }
}
