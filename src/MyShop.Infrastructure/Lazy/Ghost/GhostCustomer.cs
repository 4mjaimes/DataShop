using MyShop.Domain.Models;
using MyShop.Infrastructure.Lazy.Proxies;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyShop.Infrastructure.Lazy.Ghost
{
    public class GhostCustomer : CustomerProxy
    {
        private readonly Func<Customer> load;
        private LoadStatus status;
        public GhostCustomer(Func<Customer> load) : base()
        {
            this.load = load;
            status = LoadStatus.GHOST;
        }
    }
    enum LoadStatus { GHOST, LOADING, LOADED }
}
