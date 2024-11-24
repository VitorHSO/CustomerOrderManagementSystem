using COMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMS.Domain.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetAll();
        Task<OrderItem> GetById(int OrderItemId);
        Task<OrderItem> Add(OrderItem OrderItem);
        Task<OrderItem> Update(OrderItem OrderItem);
        Task<OrderItem> Remove(int OrderItemId);
        Task<bool> SaveChangesAsync();
    }
}
