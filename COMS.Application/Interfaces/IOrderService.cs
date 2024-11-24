using COMS.Application.DTOs.Order;
using Shared.Services;

namespace COMS.Application.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResult<IEnumerable<OrderDetailedDTO>>> GetAll();
        Task<ServiceResult<OrderDetailedDTO>> GetById(int orderId);
        Task<ServiceResult<OrderDetailedDTO>> Add(OrderDTO orderDTO);
        Task<ServiceResult<OrderDetailedDTO>> Update(int orderId, OrderDTO orderDTO);
        Task<ServiceResult<OrderDetailedDTO>> Remove(int orderId);
    }
}
