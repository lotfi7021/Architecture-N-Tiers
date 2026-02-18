using Projet.DAL.Contracts;
using Projet.Entities;
using Projet.Services.Interfaces;

namespace Projet.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Public (No Login)
        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _unitOfWork.Repository<Product>().GetAllAsync();

        public async Task<IEnumerable<Product>> SearchAsync(string keyword)
            => await _unitOfWork.Repository<Product>()
                .FindAsync(p => p.Name.Contains(keyword));

        // Admin only
        public async Task<Product> AddAsync(Product product)
        {
            await _unitOfWork.Repository<Product>().AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _unitOfWork.Repository<Product>().Update(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int productId)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
            if (product == null) return;

            _unitOfWork.Repository<Product>().Remove(product);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
