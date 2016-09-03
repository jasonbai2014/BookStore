using BookStore.Web.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Controllers
{
    public class BookController : Controller
    {
        IBookRepository bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }
        
        public ActionResult List()
        {
            return View(this.bookRepository.Books);
        }
    }
}