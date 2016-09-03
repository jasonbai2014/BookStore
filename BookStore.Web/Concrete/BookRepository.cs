using BookStore.Web.Abstract;
using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Web.Concrete
{
    /// <summary>
    /// This is a class providing books for this bookstore
    /// </summary>
    public class BookRepository : IBookRepository
    {
        /// <summary>
        /// This is a database context instance that keeps data of books
        /// </summary>
        private StoreDbContext dbContext = new StoreDbContext();

        /// <summary>
        /// This is a property that returns a list of books from the database
        /// </summary>
        public IEnumerable<Book> Books
        {
            get { return dbContext.Books; }
        }
    }
}