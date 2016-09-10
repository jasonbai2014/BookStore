using BookStore.Web.Abstract;
using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Areas.Admin.Controllers
{
    public class BookManageController : Controller
    {
        private IBookRepository bookRepository;

        private const int BooksPerPage = 10;

        public BookManageController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public async Task<ViewResult> Index(int page = 1)
        {
            int bookNum = await bookRepository.GetBooks().CountAsync();
            IEnumerable<Book> books = bookRepository.GetBooks().OrderBy(x => x.BookID).
                Skip((page - 1) * BooksPerPage).Take(BooksPerPage);

            return View(new PagingInfo {
                CurPage = page,
                TotalPages = (int) Math.Ceiling(bookNum * 1.0 / BooksPerPage),
                Books = books
            });
        }
    }
}