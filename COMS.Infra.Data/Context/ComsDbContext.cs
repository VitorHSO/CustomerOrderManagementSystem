using COMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Shared.Interfaces;

namespace COMS.Infra.Data.Context
{
    public class ComsDbContext : DbContext
    {
        private IConfiguration _configuration;

        public ComsDbContext(IConfiguration configuration, DbContextOptions<ComsDbContext> options) : base(options)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Para cada propriedade do tipo string
                var stringProperties = entity.GetProperties()
                    .Where(p => p.ClrType == typeof(string));

                foreach (var property in stringProperties)
                {
                    // Definir o MaxLength padrão de 255, se não estiver configurado
                    if (!property.GetMaxLength().HasValue)
                    {
                        property.SetMaxLength(255);
                    }

                    // Se a propriedade não estiver explicitamente marcada como IsRequired, configurá-la como nullable
                    if (property.IsNullable == false && !property.IsPrimaryKey())
                    {
                        property.IsNullable = true;
                    }
                }
            }

            // Aplicar outras configurações específicas
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ComsDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added && entry.Entity is IPreCreatable preCreatableEntity)
                {
                    preCreatableEntity.PreCreate(); // Chama o PreCreate antes de salvar
                }
            }

            return base.SaveChanges();
        }
    }
}
