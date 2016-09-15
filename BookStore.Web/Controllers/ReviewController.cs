using BookStore.Web.Concrete;
using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Controllers
{
    /// <summary>
    /// This is a controller for user reviews
    /// </summary>
    public class ReviewController : Controller
    {
        /// <summary>
        /// This is a book repository
        /// </summary>
        private BookRepository bookRepository;

        /// <summary>
        /// This is number of reviews on one page
        /// </summary>
        private const int ReviewsPerPage = 6;

        /// <summary>
        /// This is a constructor of this class
        /// </summary>
        /// <param name="bookRepository">This is a book repository instance</param>
        public ReviewController(BookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        /// <summary>
        /// This displays a list of reviews on a page
        /// </summary>
        /// <param name="bookId">This is an Id of a book which reviews belong to</param>
        /// <param name="page">This is a page number</param>
        /// <returns>A ViewResult showing a list of reviews from the book</returns>
        public ViewResult List(int bookId, int page = 1)
        {
            Book book = bookRepository.FindById(bookId);
            int totalPages = (int) Math.Ceiling(1.0 * book.Reviews.Count() / ReviewsPerPage);
            IEnumerable<Review> reviews = book.Reviews.OrderByDescending(x => x.ReviewID).
                Skip((page - 1) * ReviewsPerPage).Take(ReviewsPerPage).ToList();

            return View(new ReviewInfo
            {
                CurPage = page,
                TotalPages = totalPages,
                BookId = bookId,
                Reviews = reviews
            });
        }

        /// <summary>
        /// This shows a part of reviews from a book
        /// </summary>
        /// <param name="bookId">This is an Id of a book which reviews belong to</param>
        /// <returns>A ViewResult showing a list of reviews from the book</returns>
        public ViewResult Partial(int bookId)
        {
            Book book = bookRepository.FindById(bookId);
            IEnumerable<Review> reviews = book.Reviews.OrderByDescending(x => x.ReviewID).
                Take(ReviewsPerPage).ToList();

            return View(reviews);
        }

        /// <summary>
        /// This allows a user to write a review for a book
        /// </summary>
        /// <param name="bookId">This is an Id of a book</param>
        /// <returns>A ViewResult with a form for user to write a review</returns>
        [Authorize]
        public ViewResult Add(int bookId)
        {
            ViewBag.bookId = bookId;
            return View();
        }

        /// <summary>
        /// This adds user's review into a database
        /// </summary>
        /// <param name="bookId">This is an Id of a book which the review belongs to</param>
        /// <param name="review">This is the user's review</param>
        /// <returns>Redirects to book detail page if review data is valid</returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Add(int bookId, Review review)
        {
            if (ModelState.IsValid)
            {
                Book book = bookRepository.FindById(bookId);
                book.Reviews.Add(review);

                double originRating = book.Rating;
                int originReviews = book.TotalReviews;
                double originRatingTotal = originRating * originReviews;
                book.TotalReviews = originReviews + 1;
                book.Rating = 1.0 * (originRatingTotal + review.Rating) / book.TotalReviews;
                await bookRepository.Save();

                return RedirectToAction("Detail", "Book", new { bookId = bookId });
            }

            return View(review);
        }
    }
}