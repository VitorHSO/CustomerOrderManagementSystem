using COMS.Application.DTOs.OrderItem;
using Shared.Services;

namespace COMS.Application.Interfaces
{
    public interface IOrderItemService
    {
        Task<ServiceResult<IEnumerable<OrderItemDetailedDTO>>> GetAll();
        Task<ServiceResult<OrderItemDetailedDTO>> GetById(int orderItemId);
        Task<ServiceResult<OrderItemDetailedDTO>> Add(OrderItemDTO orderItemDTO);
        Task<ServiceResult<OrderItemDetailedDTO>> Update(int orderItemId, OrderItemDTO orderItemDTO);
        Task<ServiceResult<OrderItemDetailedDTO>> Remove(int orderItemId);
    }
}
