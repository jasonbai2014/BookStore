using BookStore.Web.Abstract;
using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        /// <param name="category">This is current category of books shown on a page</param>
        /// <param name="searchText">This is a piece of text used to search books</param>
        /// <returns>A view result to display books from the given page number</returns>
        public async Task<ViewResult> List(String category, String searchText, int page = 1)
        {
            IQueryable<Book> books = this.bookRepository.SearchBooks(category, searchText);
            int bookNum = await books.CountAsync();
            IEnumerable<Book> booksOnCurPage = await books.Skip((page - 1) * BooksPerPage).Take(BooksPerPage).ToListAsync();

            PagingInfo pagingInfo = new PagingInfo
            {
                CurPage = page,
                TotalPages = (int)Math.Ceiling(1.0 * bookNum / BooksPerPage),
                CurCategory = category,
                SearchText = searchText,
                Books = booksOnCurPage
            };

            return View(pagingInfo);
        }

        /// <summary>
        /// This is used to initialize a search on books
        /// </summary>
        /// <param name="category">This is a book category</param>
        /// <param name="searchText">This is a piece of text used to search books</param>
        /// <returns>A route result that redirects to the List action</returns>
        public RedirectToRouteResult Search(String category, String searchText)
        {
            return RedirectToAction("List", new { category = category, searchText = searchText, page = 1 });
        }

        /// <summary>
        /// This action method shows details of a book
        /// </summary>
        /// <param name="bookId">This is the ID of the required book</param>
        /// <returns>A view result to show details of the book</returns>
        public async Task<ViewResult> Detail(int bookId)
        {
            Book selectedBook = await this.bookRepository.FindById(bookId);

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