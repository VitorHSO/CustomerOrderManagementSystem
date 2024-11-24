using COMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace COMS.Infra.Data.EntitiesConfig
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("OrderId").IsRequired();
            builder.Property(c => c.CustomerId).IsRequired();
            builder.Property(c => c.OrderDate).IsRequired();
        }
    }
}
