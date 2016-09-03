using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Web.Models
{
    /// <summary>
    /// This is a model class for books
    /// </summary>
    public class Book
    {
        /// <summary>
        /// This is book id
        /// </summary>
        public int BookID { get; set; }

        /// <summary>
        /// This is an url pointing to a location on the server where cover
        /// image of the book is kept
        /// </summary>
        public String CoverUrl { get; set; }

        /// <summary>
        /// This is book name
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// This is category of this book
        /// </summary>
        public String Category { get; set; }

        /// <summary>
        /// This is book description
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// This is author name
        /// </summary>
        public String Author { get; set; }

        /// <summary>
        /// This is book price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// This is rating of the book
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// This is number of reviews for the book
        /// </summary>
        public int TotalReviews { get; set; }

        /// <summary>
        /// This is total pages of the book
        /// </summary>
        public int pages { get; set; }

        /// <summary>
        /// This is publisher of the book
        /// </summary>
        public String Publisher { get; set; }

        /// <summary>
        /// This is ISBN of the book
        /// </summary>
        public String ISBN { get; set; }

        /// <summary>
        /// This is a list of reviews for the book
        /// </summary>
        public virtual ICollection<Review> Reviews { get; set; }

    }
}