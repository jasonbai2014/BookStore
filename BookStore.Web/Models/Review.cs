using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Web.Models
{
    /// <summary>
    /// This is a model for a book review
    /// </summary>
    public class Review
    {
        /// <summary>
        /// This is review id
        /// </summary>
        public int ReviewID { get; set; }

        /// <summary>
        /// This is an id of a book for which this review is written
        /// </summary>
        public int BookID { get; set; }

        /// <summary>
        /// This is reviewer name
        /// </summary>
        [StringLength(256)]
        public String Reviewer { get; set; }

        /// <summary>
        /// This is a rating 
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// This is review text
        /// </summary>
        [Required(ErrorMessage = "Please enter your review")]
        [StringLength(1200, ErrorMessage = "Please make sure less than 1200 characters")]
        public String ReviewText { get; set; }

        /// <summary>
        /// This is the date this review is posted
        /// </summary>
        public DateTime PostDate { get; set; }

        /// <summary>
        /// This is the book this review belongs to
        /// </summary>
        public virtual Book Book { get; set; }
    }
}