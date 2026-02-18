using Projet.Entities;

namespace Projet.BLL.Contracts
{
    public interface IOrderBLL
    {
        void ValidateStock(Order order);
        void ApplyStockReduction(Order order);
    }
}
