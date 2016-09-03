using BookStore.Web.Abstract;
using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Web.Concrete
{
    public class BookRepository : IBookRepository
    {
        private StoreDbContext dbContext = new StoreDbContext();

        public IEnumerable<Book> Books
        {
            get { return dbContext.Books; }
        }
    }
}