using BookStore.Web.Abstract;
using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Web.Abstract
{
    /// <summary>
    /// This is an interface for order repository
    /// </summary>
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// This gets all orders in a repository
        /// </summary>
        /// <returns>A set of orders</returns>
        DbSet<Order> GetOrders();
    }
}
