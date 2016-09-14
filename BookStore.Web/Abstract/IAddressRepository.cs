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
    /// This is an interface for ShoppingAddress repository
    /// </summary>
    public interface IAddressRepository : IRepository<ShoppingAddress>
    {
        /// <summary>
        /// This can find addresses for a user by using the ID
        /// </summary>
        /// <param name="userId">This is a user ID</param>
        /// <returns>A list of user shopping addresses</returns>
        Task<List<ShoppingAddress>> FindAddressesByUserIDAsync(String userId);

        /// <summary>
        /// This gets all addresses in a repository
        /// </summary>
        /// <returns>A set of addresses</returns>
        DbSet<ShoppingAddress> GetAddresses();
    } 
}
