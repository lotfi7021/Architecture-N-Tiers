using Moq;
using Projet.DAL.Contracts;
using Projet.Entities;
using Projet.Services;
using System.Threading.Tasks;
using Xunit;

namespace Parapharmacy.Tests.Services
{
    public class ProductServiceSimpleTests
    {
        private readonly Mock<IUnitOfWork> _uowMock = new();
        private readonly Mock<IRepository<Product>> _repoMock = new();
        private readonly ProductService _service;

        public ProductServiceSimpleTests()
        {
            _uowMock.Setup(u => u.Repository<Product>())
                    .Returns(_repoMock.Object);

            _service = new ProductService(_uowMock.Object);
        }

        [Fact]
        public async Task AddAsync_Should_Call_Add_And_Save()
        {
           
            var newProduct = new Product { Name = "Test Paracetamol", Price = 12.5m };

            
            var result = await _service.AddAsync(newProduct);

            
            _repoMock.Verify(r => r.AddAsync(newProduct), Times.Once());
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once());
            Assert.Same(newProduct, result);
        }

        [Fact]
        public async Task DeleteAsync_Should_Do_Nothing_When_Product_Not_Found()
        {
            
            _repoMock.Setup(r => r.GetByIdAsync(999))
                     .ReturnsAsync((Product)null);

            
            await _service.DeleteAsync(999);

            
            _repoMock.Verify(r => r.Remove(It.IsAny<Product>()), Times.Never());
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Never());
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_When_Product_Exists()
        {
            
            var existing = new Product { Id = 7, Name = "Ibuprofen" };
            _repoMock.Setup(r => r.GetByIdAsync(7))
                     .ReturnsAsync(existing);

            
            await _service.DeleteAsync(7);

            
            _repoMock.Verify(r => r.Remove(existing), Times.Once());
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once());
        }
    }
}