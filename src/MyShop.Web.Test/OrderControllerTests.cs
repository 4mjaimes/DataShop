using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyShop.Domain.Models;
using MyShop.Infrastructure;
using MyShop.Infrastructure.Repositories;
using MyShop.Web.Controllers;
using MyShop.Web.Models;
using System;

namespace MyShop.Web.Test
{
    [TestClass]
    public class OrderControllerTests
    {
        [TestMethod]
        public void CanCreateOrderWithCorrectModel()
        {
            // ARRANGE 
            var uow = new Mock<IUnitOfWork>();
            var productRepository = new Mock<IRepository<Product>>();
            var customerRepository = new Mock<IRepository<Customer>>();

            var orderController = new OrderController(uow.Object);

            var createOrderModel = new CreateOrderModel
            {
                Customer = new CustomerModel
                {
                    Name = "Filip Ekberg",
                    ShippingAddress = "Test address",
                    City = "Gothenburg",
                    PostalCode = "43317",
                    Country = "Sweden"
                },
                LineItems = new[]
               {
                    new LineItemModel { ProductId = Guid.NewGuid(), Quantity = 10 },
                    new LineItemModel { ProductId = Guid.NewGuid(), Quantity = 2 },
                }
            };

            // ACT

            orderController.Create(createOrderModel);

            // ASSERT

            uow.Verify(r => r.OrderRepository.Add(It.IsAny<Order>()),
            Times.AtLeastOnce());
        }
    }
}
