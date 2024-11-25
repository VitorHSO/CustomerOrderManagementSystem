using COMS.Application.DTOs.OrderItem;
using COMS.Application.DTOs.Transaction;
using Shared.Services;

namespace COMS.Application.Interfaces
{
    public interface IOrderItemService
    {
        Task<ServiceResult<IEnumerable<OrderItemDetailedDTO>>> GetAll();
        Task<ServiceResult<OrderItemDetailedDTO>> GetById(int orderItemId);
        Task<TransactionDTO> Add(OrderItemDTO orderItemDTO);
        Task<TransactionDTO> Update(int orderItemId, OrderItemDTO orderItemDTO);
        Task<TransactionDTO> Remove(int orderItemId);
    }
}
