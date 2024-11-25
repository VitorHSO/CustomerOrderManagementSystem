using COMS.Domain.Entities;
using COMS.Domain.Interfaces;
using COMS.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace COMS.Infra.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ComsDbContext _context;

        public TransactionRepository(ComsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetAll()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<Transaction> GetById(int transactionId)
        {
            return await _context.Transactions.Where(u => u.Id == transactionId).FirstOrDefaultAsync();
        }

        public async Task<Transaction> Add(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
    }
}
