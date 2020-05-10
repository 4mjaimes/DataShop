using MyShop.Domain.Models;
using MyShop.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Infrastructure
{
    public interface IUnitOfWork
    {
        IRepository<Customer> CustomerRepository { get; }
        IRepository<Order> OrderRepository { get; }
        IRepository<Product> ProductRepository { get; }
        void SaveChanges();
    }
}
