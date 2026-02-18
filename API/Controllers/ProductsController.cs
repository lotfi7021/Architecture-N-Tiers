using Microsoft.AspNetCore.Mvc;
using Projet.DTOs.Product;
using Projet.Entities;
using Projet.Services.Interfaces;

namespace Projet.Service.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            return Ok(products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                ImageUrl = p.ImageUrl,
                Category = p.Category
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductCreateRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                ImageUrl = request.ImageUrl,
                Category = request.Category
            };

            var created = await _productService.AddAsync(product);

            return Ok(created.Id);
        }
    }
}
