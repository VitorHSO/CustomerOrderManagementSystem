using COMS.Application.DTOs.OrderItem;

namespace COMS.Application.DTOs.Order
{
    public class OrderDetailedDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public virtual ICollection<OrderItemDetailedDTO> OrderItems { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
