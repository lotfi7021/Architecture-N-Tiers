using System.ComponentModel.DataAnnotations;

namespace Projet.DTOs.Product
{
    public class ProductCreateRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public string Category { get; set; } = null!;
    }
}
