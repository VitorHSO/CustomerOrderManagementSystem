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

            // Seed Data
            #region :: Customer
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "/lxTyiQAzysKgZjreVi0OQ==", Email = "hyRu1a/MmuqWXtbkEJbuxtg9tpuZxDEkl7yxvn1ism4=", Phone = "lRxujHigli6mGZ8o4HWdtpttwCcC2BkBVcPf0/DmVuo=", EncryptionIV = "VVsWXlPNDIyfoQ/+z2Re4Q==" },
                new Customer { Id = 2, Name = "g3mTV/4c7qXT4b8J1pxsdg==", Email = "oWvXCQFX7kNrkSPCKK8xgQ4xb5l6mKnFbzEX+rrsxCA=", Phone = "RaYry7PWufqvqTKWfJEjt5fK/9WsoSn2KqnjrpCVAx8=", EncryptionIV = "O2Crb57pW9boXtCepoB88w==" }
            );
            #endregion

            #region :: Product
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "RTX 4090", Description = "Placa de Vídeo RTX 4090", Price = 12000 },
                new Product { Id = 2, Name = "Ryzen 7 7800X3D", Description = "Processador AMD Ryzen 7 7800X3D", Price = 3799 },
                new Product { Id = 3, Name = "Fonte XPG Kyber, 850W", Description = "Fonte XPG Kyber, 850W, 80 Plus Gold, Com Cabo, Preto - KYBER850G-BKCBR", Price = 439 }
            );
            #endregion

            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added && entry.Entity is IPreCreatable preCreatableEntity)
                {
                    preCreatableEntity.PreCreate();
                }
            }

            return base.SaveChanges();
        }
    }
}
