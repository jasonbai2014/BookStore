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
        /// <returns>A viewresult to display a list of line items</returns>
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
        /// <returns>A routeresult that redirects to the summary action</returns>
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
        /// <returns>A routeresult that redirects to the summary action</returns>
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
        /// This gives a short summary about the cart such as number of items in the cart
        /// </summary>
        /// <param name="cart">This is the shopping cart</param>
        /// <returns>A partial view to show the summary</returns>
        public PartialViewResult Brief(Cart cart)
        {
            return PartialView(cart);
        }

        /// <summary>
        /// This allows a user to check out items in a cart
        /// </summary>
        /// <returns>A viewresult to show a form for shopping address</returns>
        public ViewResult CheckOut()
        {
            return View();
        }

        [HttpPost]
        public ViewResult CheckOut(Cart cart, ShoppingAddress address)
        {
            if (ModelState.IsValid)
            {
                // TO-DO: need to add more codes to deal with order processing here
                cart.clear();
                return View("Completed");
            } else
            {
                return View(address);
            }
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