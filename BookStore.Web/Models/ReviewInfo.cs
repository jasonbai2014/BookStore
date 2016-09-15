using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Web.Models
{
    /// <summary>
    /// This is a view model used for review pagination
    /// </summary>
    public class ReviewInfo
    {
        /// <summary>
        /// This is current page number
        /// </summary>
        public int CurPage { get; set; }

        /// <summary>
        /// This is total amount of pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// This is an Id of a book which reviews belong to
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// This is a list of reviews shown on current page
        /// </summary>
        public IEnumerable<Review> Reviews { get; set; }
    }
}