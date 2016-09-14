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
        /// This can find orders for a user by using the ID
        /// </summary>
        /// <param name="userId">This is a user ID</param>
        /// <returns>A list of orders</returns>
        Task<List<Order>> FindOrdersByUserIDAsync(string userId);

        /// <summary>
        /// This gets all orders in a repository
        /// </summary>
        /// <returns>A set of orders</returns>
        DbSet<Order> GetOrders();
    }
}
