using Projet.Entities;

namespace Projet.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(Order order);
        Task<IEnumerable<Order>> GetClientOrdersAsync(int clientId);

        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task ApproveOrderAsync(int orderId);
        Task RejectOrderAsync(int orderId);
    }
}
