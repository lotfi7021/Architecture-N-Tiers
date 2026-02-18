using Projet.BLL.Contracts;
using Projet.DAL.Contracts;
using Projet.Entities;
using Projet.Enums;
using Projet.Services.Interfaces;

namespace Projet.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderBLL _orderBLL;

        public OrderService(IUnitOfWork unitOfWork, IOrderBLL orderBLL)
        {
            _unitOfWork = unitOfWork;
            _orderBLL = orderBLL;
        }

        // Client
        public async Task<Order> PlaceOrderAsync(Order order)
        {
            foreach (var item in order.OrderItems)
            {
                var product = await _unitOfWork.Repository<Product>()
                    .GetByIdAsync(item.ProductId);

                if (product == null)
                    throw new Exception("Product not found");

                item.Product = product;
                item.UnitPrice = product.Price;
            }

            _orderBLL.ValidateStock(order);

            await _unitOfWork.Repository<Order>().AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return order;
        }


        public async Task<IEnumerable<Order>> GetClientOrdersAsync(int clientId)
            => await _unitOfWork.Repository<Order>()
                .FindAsync(o => o.ClientId == clientId);

        // Admin
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
            => await _unitOfWork.Repository<Order>().GetAllAsync();

        public async Task ApproveOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if (order == null || order.Status != OrderStatus.Pending) return;

            foreach (var item in order.OrderItems)
            {
                item.Product = await _unitOfWork.Repository<Product>()
                    .GetByIdAsync(item.ProductId);
            }

            _orderBLL.ApplyStockReduction(order);

            foreach (var item in order.OrderItems)
            {
                _unitOfWork.Repository<Product>().Update(item.Product);
            }

            order.Status = OrderStatus.Approved;
            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.SaveChangesAsync();
        }


        public async Task RejectOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if (order == null) return;

            order.Status = OrderStatus.Rejected;
            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
