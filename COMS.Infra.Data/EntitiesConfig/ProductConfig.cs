using COMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace COMS.Infra.Data.EntitiesConfig
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("ProductId").IsRequired();
            builder.Property(c => c.Name).HasMaxLength(255).IsRequired();
            builder.Property(c => c.Description).HasColumnType("VARCHAR(MAX)").IsRequired();
            builder.Property(c => c.Price).HasColumnType("decimal(18,2)").IsRequired();
        }
    }
}
