using COMS.Domain.Entities;

namespace COMS.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> GetById(int customerId);
        Task<Customer> Add(Customer Customer);
        Task<Customer> Update(Customer Customer);
        Task<Customer> Remove(int customerId);
        Task<bool> Exists(int customerId);
        Task<bool> SaveChangesAsync();
    }
}
