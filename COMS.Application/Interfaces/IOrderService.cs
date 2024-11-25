using COMS.Application.DTOs.Order;
using COMS.Application.DTOs.Transaction;
using Shared.Services;

namespace COMS.Application.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResult<IEnumerable<OrderDetailedDTO>>> GetAll();
        Task<ServiceResult<OrderDetailedDTO>> GetById(int orderId);
        Task<TransactionDTO> Add(OrderDTO orderDTO);
        Task<TransactionDTO> Update(int orderId, OrderDTO orderDTO);
        Task<TransactionDTO> Remove(int orderId);
    }
}
