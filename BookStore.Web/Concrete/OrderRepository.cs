using BookStore.Web.Abstract;
using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BookStore.Web.Concrete
{
    /// <summary>
    /// This is a repository for orders
    /// </summary>
    public class OrderRepository : AbstractRepository<Order>, IOrderRepository
    {
        /// <summary>
        /// This is a constructor for this class
        /// </summary>
        /// <param name="dbContext">This is a database context instance</param>
        public OrderRepository(StoreDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// This adds an order into this repository
        /// </summary>
        /// <param name="order">This is the order</param>
        /// <returns>The order that is added into the repository</returns>
        public override Order Add(Order order)
        {
            return DbContext.Orders.Add(order);
        }

        /// <summary>
        /// This deletes an order from this repository
        /// </summary>
        /// <param name="order">This is the order that will be removed</param>
        /// <returns>The deleted order</returns>
        public override Order Delete(Order order)
        {
            return DbContext.Orders.Remove(order);
        }

        /// <summary>
        /// This edits an order in this repository
        /// </summary>
        /// <param name="order">This is an edited order</param>
        public override void Edit(Order order)
        {
            DbContext.SetModified(order);
        }

        /// <summary>
        /// This finds an order by its id
        /// </summary>
        /// <param name="id">This is the order id</param>
        /// <returns>The found order or null</returns>
        public override Order FindById(int id)
        {
            return DbContext.Orders.Find(id);
        }

        /// <summary>
        /// This can find orders for a user by using the ID
        /// </summary>
        /// <param name="userId">This is a user ID</param>
        /// <returns>A list of orders</returns>
        public async Task<List<Order>> FindOrdersByUserIDAsync(string userId)
        {
            User user = await ((DbSet<User>)DbContext.Users).FindAsync(userId);
            List<Order> result = new List<Order>();
            
            if (user != null)
            {
                List<ShoppingAddress> addresses = user.Addresses.ToList();

                foreach (ShoppingAddress address in addresses)
                {
                    result.AddRange(address.Orders.ToList());
                }

                result = result.OrderByDescending(x => x.Date).ToList();
            }
            

            return result;
        }

        /// <summary>
        /// This gets all orders in a repository
        /// </summary>
        /// <returns>A set of orders</returns>
        public DbSet<Order> GetOrders()
        {
            return DbContext.Orders;
        }
    }
}