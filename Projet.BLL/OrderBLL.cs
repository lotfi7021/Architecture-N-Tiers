using Projet.BLL.Contracts;
using Projet.Entities;

namespace Projet.BLL
{
    public class OrderBLL : IOrderBLL
    {
        public void ValidateStock(Order order)
        {
            foreach (var item in order.OrderItems)
            {
                if (item.Product.StockQuantity < item.Quantity)
                {
                    throw new Exception("Insufficient stock for product");
                }
            }
        }

        public void ApplyStockReduction(Order order)
        {
            foreach (var item in order.OrderItems)
            {
                item.Product.StockQuantity -= item.Quantity;
            }
        }
    }
}
