using BookStore.Web.Abstract;
using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository bookRepository;

        public const int BooksPerPage = 6;

        public BookController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }
        
        public ViewResult List(int page = 1)
        {
            IEnumerable<Book> selectedBooks = this.bookRepository.Books.OrderBy(x => x.BookID).
                Skip((page - 1) * BooksPerPage).Take(BooksPerPage);

            PagingInfo pagingInfo = new PagingInfo
            {
                CurPage = page,
                TotalPages = (int) Math.Ceiling(1.0 * this.bookRepository.Books.Count() / BooksPerPage),
                Books = selectedBooks
            };

            return View(pagingInfo);
        }
    }
}