using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BookStore.Web.Concrete
{
    /// <summary>
    /// This is a database context class for this bookstore
    /// </summary>
    public class StoreDbContext : DbContext
    {
        /// <summary>
        /// This is a constructor of this class
        /// </summary>
        public StoreDbContext() : base("StoreDbContext")
        {

        }

        /// <summary>
        /// This returns a set of books from a database for this bookstore
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// This returns a set of reviews for the books 
        /// </summary>
        public DbSet<Review> Reviews { get; set; }
    }
}