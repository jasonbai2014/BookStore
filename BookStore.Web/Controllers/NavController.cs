using BookStore.Web.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Controllers
{
    /// <summary>
    /// This is a class for navigation menu
    /// </summary>
    public class NavController : Controller
    {
        /// <summary>
        /// This is a book repository obtained from a database
        /// </summary>
        private IBookRepository bookRepository;

        /// <summary>
        /// This is a constructor
        /// </summary>
        /// <param name="bookRepository">This is the book repository</param>
        public NavController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        /// <summary>
        /// This creates a menu to show a list of book categories
        /// </summary>
        /// <param name="category">select book category</param>
        /// <returns>A partial view for the menu</returns>
        public PartialViewResult Menu(String category)
        {
            String[] categories = bookRepository.Books.Select(x => x.Category).Distinct().OrderBy(x => x).ToArray();
            ViewBag.selectedCategory = category;
            return PartialView(categories);
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