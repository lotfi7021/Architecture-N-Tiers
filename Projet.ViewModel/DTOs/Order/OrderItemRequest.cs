using System.ComponentModel.DataAnnotations;

namespace Projet.DTOs.Order
{
    public class OrderItemRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
