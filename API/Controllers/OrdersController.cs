using Microsoft.AspNetCore.Mvc;
using Projet.DTOs.Order;
using Projet.Entities;
using Projet.Services.Interfaces;

namespace Projet.Service.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(OrderCreateRequest request)
        {
            var order = new Order
            {
                ClientId = int.Parse(User.FindFirst("nameidentifier")!.Value),
                OrderItems = request.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            var created = await _orderService.PlaceOrderAsync(order);

            return Ok(new OrderResponse
            {
                Id = created.Id,
                Status = created.Status,
                CreatedAt = created.CreatedAt,
                Items = created.OrderItems.Select(i => new OrderItemResponse
                {
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            });
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            await _orderService.ApproveOrderAsync(id);
            return Ok();
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            await _orderService.RejectOrderAsync(id);
            return Ok();
        }
    }
}
