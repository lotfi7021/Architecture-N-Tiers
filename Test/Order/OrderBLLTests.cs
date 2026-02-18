using Xunit;
using Projet.BLL;
using Projet.Entities;
using System;
using System.Collections.Generic;

namespace Parapharmacy.Tests
{
    public class OrderBLLTests
    {
        private readonly OrderBLL bll = new OrderBLL();

        [Fact]
        public void ValidateStock_does_not_throw_when_stock_is_ok()
        {
            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Quantity = 2, Product = new Product { StockQuantity = 10 } }
                }
            };

            
            bll.ValidateStock(order);
        }

        [Fact]
        public void ValidateStock_throws_when_stock_is_not_enough()
        {
            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Quantity = 10, Product = new Product { StockQuantity = 3 } }
                }
            };

            Assert.Throws<Exception>(() => bll.ValidateStock(order));
        }

        [Fact]
        public void ApplyStockReduction_reduces_stock()
        {
            var product = new Product { StockQuantity = 10 };

            var order = new Order
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Quantity = 3, Product = product }
                }
            };

            bll.ApplyStockReduction(order);

            Assert.Equal(7, product.StockQuantity);
        }
    }
}