using DataAccessLayer.Abstract;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Concrete.EfCore
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order>, IOrderRepository
    {
        public EfCoreOrderRepository(ETicaretUygulamasiContext ctx) : base(ctx)
        {

        }
        private ETicaretUygulamasiContext Context
        {
            get
            {
                return genericContext as ETicaretUygulamasiContext;
            }
        }
        public List<Order> GetOrders(string userId)
        {

            var orders = Context.Orders.Include(i => i.OrderItems).ThenInclude(i => i.Product).AsQueryable();

            if (!string.IsNullOrEmpty(userId))
            {
                orders = orders.Where(i => i.UserId == userId);
            }

            return orders.ToList();

        }
    }
}
