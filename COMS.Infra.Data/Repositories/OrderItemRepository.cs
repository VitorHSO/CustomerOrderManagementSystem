using COMS.Domain.Entities;
using COMS.Domain.Interfaces;
using COMS.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace COMS.Infra.Data.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ComsDbContext _context;

        public OrderItemRepository(ComsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetAll()
        {
            return await _context.OrderItems
                .Include(oi => oi.Order)
                    .ThenInclude(oi => oi.Customer)
                .Include(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<OrderItem> GetById(int orderItemId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Order)
                    .ThenInclude(oi => oi.Customer)
                .Include(oi => oi.Product)
                .Where(u => u.Id == orderItemId).FirstOrDefaultAsync();
        }

        public async Task<OrderItem> Add(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            return orderItem;
        }

        public async Task<OrderItem> Update(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            return orderItem;
        }

        public async Task<OrderItem> Remove(int orderItemId)
        {
            var OrderItemDb = await _context.OrderItems.Where(u => u.Id == orderItemId).FirstOrDefaultAsync();
            _context.OrderItems.Remove(OrderItemDb);
            return OrderItemDb;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
