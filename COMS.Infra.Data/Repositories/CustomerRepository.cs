using COMS.Domain.Entities;
using COMS.Domain.Interfaces;
using COMS.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace COMS.Infra.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ComsDbContext _context;

        public CustomerRepository(ComsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetById(int customerId)
        {
            return await _context.Customers.Where(u => u.Id == customerId).FirstOrDefaultAsync();
        }

        public async Task<Customer> Add(Customer Customer)
        {
            _context.Customers.Add(Customer);
            return Customer;
        }

        public async Task<Customer> Update(Customer Customer)
        {
            _context.Customers.Update(Customer);
            return Customer;
        }

        public async Task<Customer> Remove(int customerId)
        {
            var CustomerDb = await _context.Customers.Where(u => u.Id == customerId).FirstOrDefaultAsync();
            _context.Customers.Remove(CustomerDb);
            return CustomerDb;
        }

        public async Task<bool> Exists(int customerId)
        {
            return _context.Customers.Any(c => c.Id == customerId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
