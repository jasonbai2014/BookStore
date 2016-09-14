using BookStore.Web.Abstract;
using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BookStore.Web.Concrete
{
    /// <summary>
    /// This is a class for address repository
    /// </summary>
    public class AddressRepository : AbstractRepository<ShoppingAddress>, IAddressRepository
    {
        /// <summary>
        /// This is a constructor for this class
        /// </summary>
        /// <param name="dbContext">This is a database context instance</param>
        public AddressRepository(StoreDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// This adds an address into this repository
        /// </summary>
        /// <param name="address">This is the address</param>
        /// <returns>The address that is added into the repository</returns>
        public override ShoppingAddress Add(ShoppingAddress address)
        {
            return DbContext.Addresses.Add(address);
        }

        /// <summary>
        /// This deletes an address from this repository
        /// </summary>
        /// <param name="address">This is the address that will be removed</param>
        /// <returns>The deleted address</returns>
        public override ShoppingAddress Delete(ShoppingAddress address)
        {
            return DbContext.Addresses.Remove(address);
        }

        /// <summary>
        /// This edits an address in this repository
        /// </summary>
        /// <param name="address">This is an edited address</param>
        public override void Edit(ShoppingAddress address)
        {
            DbContext.SetModified(address);
        }

        /// <summary>
        /// This finds an address by its id
        /// </summary>
        /// <param name="id">This is an address id</param>
        /// <returns>The found address or null</returns>
        public override ShoppingAddress FindById(int id)
        {
            return DbContext.Addresses.Find(id);
        }

        /// <summary>
        /// This can find addresses for a user by using the ID
        /// </summary>
        /// <param name="userId">This is a user ID</param>
        /// <returns>A list of user shopping addresses</returns>
        public async Task<List<ShoppingAddress>> FindAddressesByUserIDAsync(string userId)
        {
            return await DbContext.Addresses.Where(x => x.UserID == userId).OrderBy(x => x.ShoppingAddressID).ToListAsync();
        }

        /// <summary>
        /// This gets all addresses in a repository
        /// </summary>
        /// <returns>A set of addresses</returns>
        public DbSet<ShoppingAddress> GetAddresses()
        {
            return DbContext.Addresses;
        }
    }
}