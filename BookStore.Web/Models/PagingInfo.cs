using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Web.Models
{
    /// <summary>
    /// This is a view model used for pagination
    /// </summary>
    public class PagingInfo
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
        /// This is a list of books shown on current page
        /// </summary>
        public IEnumerable<Book> Books { get; set; }
    }
}