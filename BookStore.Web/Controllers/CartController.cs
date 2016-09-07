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
    /// This is a controller class handling operations on shopping cart
    /// </summary>
    public class CartController : Controller
    {
        /// <summary>
        /// This is a book repository obtained from a database
        /// </summary>
        private IBookRepository bookRepository;

        /// <summary>
        /// This is a constructor
        /// </summary>
        /// <param name="bookRepository">This is the book repository</param>
        public CartController(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        /// <summary>
        /// This shows a list of line items on a page
        /// </summary>
        /// <param name="cart">This is the cart containing the line items</param>
        /// <returns>A view result to display a list of line items</returns>
        public ViewResult Summary(Cart cart)
        {
            return View(cart);
        }

        /// <summary>
        /// This adds a book into a shopping cart
        /// </summary>
        /// <param name="cart">This is the cart</param>
        /// <param name="bookId">This is id of the book</param>
        /// <param name="quantity">This is quantity of the book</param>
        /// <returns>A route result that redirects to the summary action</returns>
        public RedirectToRouteResult AddToCart(Cart cart, int bookId, int quantity)
        {
            Book book = bookRepository.Books.SingleOrDefault(x => x.BookID == bookId);

            if (book != null)
            {
                cart.Add(book, quantity);
            }

            return RedirectToAction("Summary");
        }

        /// <summary>
        /// This removes a book from a shopping cart
        /// </summary>
        /// <param name="cart">This is the cart</param>
        /// <param name="bookId">This is id of the book</param>
        /// <returns>A route result that redirects to the summary action</returns>
        public RedirectToRouteResult RemoveFromCart(Cart cart, int bookId)
        {
            Book book = bookRepository.Books.SingleOrDefault(x => x.BookID == bookId);

            if (book != null)
            {
                cart.Revome(book);
            }

            return RedirectToAction("Summary");
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