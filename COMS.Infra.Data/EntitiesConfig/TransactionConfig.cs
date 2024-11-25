using COMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace COMS.Infra.Data.EntitiesConfig
{
    public class TransactionConfig : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("TransactionId").IsRequired();
            builder.Property(c => c.Status).HasMaxLength(255).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(255).IsRequired();
            builder.Property(c => c.ModelRequest).HasColumnType("VARCHAR(MAX)").IsRequired();
        }
    }
}
