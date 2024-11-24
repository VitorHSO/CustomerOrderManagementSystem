using COMS.Domain.Entities;

namespace COMS.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAll();
        Task<Order> GetById(int orderId);
        Task<Order> Add(Order Order);
        Task<Order> Update(Order Order);
        Task<Order> Remove(int orderId);
        Task<bool> Exists(int orderId);
        Task<bool> SaveChangesAsync();
    }
}
