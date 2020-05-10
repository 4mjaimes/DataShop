using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Models;
using MyShop.Infrastructure;
using MyShop.Infrastructure.Repositories;
using MyShop.Web.Models;

namespace MyShop.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork uow;

        public OrderController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public IActionResult Index()
        {
            var orders = uow.OrderRepository.Find(order => order.OrderDate > DateTime.UtcNow.AddDays(-1));
            return View(orders);
        }

        public IActionResult Create()
        {
            var products = uow.ProductRepository.All();

            return View(products);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderModel model)
        {
            if (!model.LineItems.Any()) return BadRequest("Please submit line items");

            if (string.IsNullOrWhiteSpace(model.Customer.Name)) return BadRequest("Customer needs a name");

            var customer = uow.CustomerRepository
                            .Find(c => c.Name == model.Customer.Name)
                            .FirstOrDefault();

            if (customer != null) 
            {
                customer.ShippingAddress = model.Customer.ShippingAddress;
                customer.PostalCode = model.Customer.PostalCode;
                customer.City = model.Customer.City;

                uow.CustomerRepository.Update(customer);
            }
            else
            {
                customer = new Customer
                {
                    Name = model.Customer.Name,
                    ShippingAddress = model.Customer.ShippingAddress,
                    City = model.Customer.City,
                    PostalCode = model.Customer.PostalCode,
                    Country = model.Customer.Country
                };
            }

            var order = new Order
            {
                LineItems = model.LineItems
                    .Select(line => new LineItem { ProductId = line.ProductId, Quantity = line.Quantity })
                    .ToList(),

                Customer = customer
            };

            uow.OrderRepository.Add(order);

            uow.SaveChanges();

            return Ok("Order Created");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
