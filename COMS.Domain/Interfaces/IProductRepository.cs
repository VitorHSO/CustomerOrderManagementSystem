using COMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMS.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(int productId);
        Task<Product> Add(Product Product);
        Task<Product> Update(Product Product);
        Task<Product> Remove(int productId);
        Task<bool> Exists(int productId);
        Task<bool> SaveChangesAsync();
    }
}
