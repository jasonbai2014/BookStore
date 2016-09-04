using BookStore.Web.Abstract;
using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Controllers
{
    /// <summary>
    /// This is a class that handles operations for books
    /// </summary>
    public class BookController : Controller
    {
        /// <summary>
        /// This is a book repository obtained from a database
        /// </summary>
        private IBookRepository bookRepository;

        /// <summary>
        /// This is number of books shown per page
        /// </summary>
        public const int BooksPerPage = 6;

        /// <summary>
        /// This is a constructor
        /// </summary>
        /// <param name="bookRepository">This is the book repository</param>
        public BookController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }
        
        /// <summary>
        /// This action method shows books based on a page number
        /// </summary>
        /// <param name="page">This is the page number</param>
        /// <returns>A view result to display books from the given page number</returns>
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

        /// <summary>
        /// This action method shows details of a book
        /// </summary>
        /// <param name="bookId">This is the ID of the required book</param>
        /// <returns>A view result to show details of the book</returns>
        public ViewResult Detail(int bookId)
        {
            Book selectedBook = this.bookRepository.Books.SingleOrDefault(x => x.BookID == bookId);

            if (selectedBook == null)
            {
                return View("error", new Error { Message = "Can't Find the Book" });
            }

            return View(selectedBook);
        }

        /// <summary>
        /// This release both managed and unmanaged resources
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; 
        /// false to release only unmanaged resources</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bookRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}