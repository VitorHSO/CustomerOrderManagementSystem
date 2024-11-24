using COMS.Application.DTOs.Customer;
using Shared.Services;

namespace COMS.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<ServiceResult<IEnumerable<CustomerDetailedDTO>>> GetAll();
        Task<ServiceResult<CustomerDetailedDTO>> GetById(int customerId);
        Task<ServiceResult<CustomerDetailedDTO>> Add(CustomerDTO customerDTO);
        Task<ServiceResult<CustomerDetailedDTO>> Update(int customerId, CustomerDTO customerDTO);
        Task<ServiceResult<CustomerDetailedDTO>> Remove(int customerId);
    }
}
