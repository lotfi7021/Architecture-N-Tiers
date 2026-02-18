using Projet.Enums;

namespace Projet.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public User Client { get; set; } = null!;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
