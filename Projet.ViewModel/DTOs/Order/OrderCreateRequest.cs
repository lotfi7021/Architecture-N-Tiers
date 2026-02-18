using System.ComponentModel.DataAnnotations;

namespace Projet.DTOs.Order
{
    public class OrderCreateRequest
    {
        [Required]
        [MinLength(1)]
        public List<OrderItemRequest> Items { get; set; } = new();
    }
}
