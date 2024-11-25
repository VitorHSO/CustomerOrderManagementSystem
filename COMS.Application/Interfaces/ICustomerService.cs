using COMS.Application.DTOs.Customer;
using COMS.Application.DTOs.Transaction;
using Shared.Services;

namespace COMS.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<ServiceResult<IEnumerable<CustomerDetailedDTO>>> GetAll();
        Task<ServiceResult<CustomerDetailedDTO>> GetById(int customerId);
        Task<TransactionDTO> Add(CustomerDTO customerDTO);
        Task<TransactionDTO> Update(int customerId, CustomerDTO customerDTO);
        Task<TransactionDTO> Remove(int customerId);
    }
}
