using COMS.Domain.Entities;
using COMS.Domain.Interfaces;
using COMS.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace COMS.Infra.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ComsDbContext _context;

        public ProductRepository(ComsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetById(int productId)
        {
            return await _context.Products.Where(u => u.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<Product> Add(Product Product)
        {
            _context.Products.Add(Product);
            return Product;
        }

        public async Task<Product> Update(Product Product)
        {
            _context.Products.Update(Product);
            return Product;
        }

        public async Task<Product> Remove(int productId)
        {
            var ProductDb = await _context.Products.Where(u => u.Id == productId).FirstOrDefaultAsync();
            _context.Products.Remove(ProductDb);
            return ProductDb;
        }

        public async Task<bool> Exists(int productId)
        {
            return _context.Products.Any(c => c.Id == productId);
        }       

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
