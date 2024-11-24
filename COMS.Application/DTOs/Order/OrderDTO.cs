using COMS.Application.DTOs.OrderItem;

namespace COMS.Application.DTOs.Order
{
    public class OrderDTO
    {
        public int CustomerId { get; set; }
        public virtual ICollection<OrderItemDTO> OrderItems { get; set; }
    }
}
