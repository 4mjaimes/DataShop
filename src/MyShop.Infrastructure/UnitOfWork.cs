using MyShop.Domain.Models;
using MyShop.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private ShoppingContext context;
        public UnitOfWork(ShoppingContext context)
        {
            this.context = context;
        }

        private IRepository<Customer> customerRepository;
        private IRepository<Order> orderRepository;
        private IRepository<Product> productRepository;

        public IRepository<Customer> CustomerRepository
        {
            get
            {
                if (customerRepository == null)
                {
                    customerRepository = new CustomerRepository(context);
                }

                return customerRepository;
            }
        }

        public IRepository<Order> OrderRepository
        {
            get
            {
                if (orderRepository == null)
                {
                    orderRepository = new OrderRepository(context);
                }

                return orderRepository;
            }
        }

        public IRepository<Product> ProductRepository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new ProductRepository(context);
                }

                return productRepository;
            }
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
