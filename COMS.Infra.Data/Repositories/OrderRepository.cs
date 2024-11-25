using COMS.Domain.Entities;
using COMS.Domain.Interfaces;
using COMS.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace COMS.Infra.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ComsDbContext _context;

        public OrderRepository(ComsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<Order> GetById(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.Id == orderId)
                .FirstOrDefaultAsync();
        }

        public async Task<Order> Add(Order order)
        {
            _context.Orders.Add(order);
            return order;
        }

        public async Task<Order> Update(Order order)
        {
            _context.Orders.Update(order);
            return order;
        }

        public async Task<Order> Remove(int orderId)
        {
            var orderDb = await _context.Orders.Where(u => u.Id == orderId).FirstOrDefaultAsync();
            _context.Orders.Remove(orderDb);
            return orderDb;
        }

        public async Task<bool> Exists(int orderId)
        {
            return _context.Orders.Any(c => c.Id == orderId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
