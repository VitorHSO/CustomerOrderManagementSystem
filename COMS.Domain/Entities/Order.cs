using Shared.Entities;

namespace COMS.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order() : base()
        {
            OrderDate = DateTime.UtcNow;
        }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
