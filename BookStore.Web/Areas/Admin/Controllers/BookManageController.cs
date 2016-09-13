using BookStore.Web.Abstract;
using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// This class is used by an administrator to manager books
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class BookManageController : Controller
    {
        /// <summary>
        /// This is a book repository
        /// </summary>
        private IBookRepository bookRepository;

        /// <summary>
        /// This is number of books shown on one page
        /// </summary>
        private const int BooksPerPage = 10;

        /// <summary>
        /// This is a constructor
        /// </summary>
        /// <param name="bookRepository">This is a book repository</param>
        public BookManageController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        /// <summary>
        /// This shows books from the book repository
        /// </summary>
        /// <param name="page">This is a page number</param>
        /// <returns>A ViewResult showing books on a page</returns>
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

        /// <summary>
        /// This shows a page allowing an administrator to edit a selected book
        /// </summary>
        /// <param name="bookId">This is id of the selected book</param>
        /// <returns>A ViewResult containing a form for editing book information</returns>
        public async Task<ViewResult> Edit(int bookId)
        {
            Book book = await bookRepository.FindById(bookId);
            return View(book);
        }

        /// <summary>
        /// This saves a book's edited information or redirects to the editing form 
        /// if at least one data is invalid
        /// </summary>
        /// <param name="book">This is an edited book</param>
        /// <returns>A ActionResult that is either a ViewResult or RedirectToRouteResult</returns>
        [HttpPost]
        public async Task<ActionResult> Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                bookRepository.Edit(book);
                await bookRepository.Save();
                TempData["message"] = "The changes have been saved";
                return RedirectToAction("Index");
            } else
            {
                return View(book);
            }
        }

        /// <summary>
        /// This shows a page allowing an administrator to create a book
        /// </summary>
        /// <returns>A ViewResult containing a form used to add a new book into the repository</returns>
        public ViewResult Create()
        {
            return View(new Book());
        }

        /// <summary>
        /// This adds a book into the repository or redirects to the form page
        /// if at least one data is invalid
        /// </summary>
        /// <param name="book">This is the book created</param>
        /// <returns>A ActionResult that is either a ViewResult or RedirectToRouteResult</returns>
        [HttpPost]
        public async Task<ActionResult> Create(Book book, HttpPostedFileBase file)
        {
            if (ModelState.IsValid && file != null)
            {
                // saves cover image to a server
                String fileName = book.ISBN + Path.GetExtension(file.FileName);
                String path = Path.Combine(Server.MapPath("~/Images"), fileName);
                file.SaveAs(path);
                book.CoverUrl = "/Images/" + fileName;

                bookRepository.Add(book);
                await bookRepository.Save();
                TempData["Message"] = String.Format("{0} has been saved", book.Name);
                return RedirectToAction("Index");
            } else
            {
                return View(book);
            }
        }

        /// <summary>
        /// This deletes a selected book from the book repository
        /// </summary>
        /// <param name="bookId">This is id of the selected book</param>
        /// <returns>A RedirectToRouteResult that redirects to the index page</returns>
        [HttpPost]
        public async Task<RedirectToRouteResult> Delete(int bookId)
        {
            Book selectedBook = await bookRepository.FindById(bookId);
            Book deletedBook = bookRepository.Delete(selectedBook);

            // deletes cover image of the book from server
            String filePath = Request.MapPath(selectedBook.CoverUrl);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            await bookRepository.Save();
            TempData["message"] = String.Format("{0} has been deleted", deletedBook.Name);

            return RedirectToAction("Index");
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