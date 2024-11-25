using COMS.Application.DTOs.Transaction;
using Shared.Services;

namespace COMS.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<ServiceResult<IEnumerable<TransactionDTO>>> GetAll();
        Task<ServiceResult<TransactionDetailedDTO>> GetById(int transactionId);
    }
}
