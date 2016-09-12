using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [DefaultValue("/Images/default.jpg")]
        public String CoverUrl { get; set; }

        /// <summary>
        /// This is book name
        /// </summary>
        [StringLength(60, ErrorMessage = "Please make sure less than 60 characters")]
        [Required(ErrorMessage = "Please enter a name")]
        public String Name { get; set; }

        /// <summary>
        /// This is category of this book
        /// </summary>
        [Required(ErrorMessage = "Please enter a category")]
        [StringLength(16, ErrorMessage = "Please make sure less than 16 characters")]
        public String Category { get; set; }

        /// <summary>
        /// This is book description
        /// </summary>
        [Required(ErrorMessage = "Please enter description")]
        [StringLength(600, ErrorMessage = "Please make sure less than 600 characters")]
        [DataType(DataType.MultilineText)]
        public String Description { get; set; }

        /// <summary>
        /// This is author name
        /// </summary>
        [Required(ErrorMessage = "Please enter an author name")]
        [RegularExpression(@"(([A-Z]\.?\s?)*([A-Z][a-z]+\.?\s?)+([A-Z]\.?\s?[a-z]*)*)", 
            ErrorMessage = "Please enter a valid name")]
        [StringLength(60, ErrorMessage = "Please make sure less than 60 characters")]
        public String Author { get; set; }

        /// <summary>
        /// This is book price
        /// </summary>
        [Required(ErrorMessage = "Please enter a price")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Please make sure enter a number with at most 2 decimal places")]
        [DataType(DataType.Currency)]
        [Range(0.01, Double.MaxValue, ErrorMessage = "Please enter a valid number")]
        public decimal Price { get; set; }

        /// <summary>
        /// This is rating of the book
        /// </summary>
        [DefaultValue(0.0)]
        public double Rating { get; set; }

        /// <summary>
        /// This is number of reviews for the book
        /// </summary>
        [DefaultValue(0.0)]
        public int TotalReviews { get; set; }

        /// <summary>
        /// This is total pages of the book
        /// </summary>
        [Required(ErrorMessage = "Please enter page number")]
        [RegularExpression("^(0|[1-9][0-9]*)$", ErrorMessage = "Please enter an integer")]
        [Range(1, int.MaxValue, ErrorMessage = "Please a valid number")]
        public int pages { get; set; }

        /// <summary>
        /// This is publisher of the book
        /// </summary>
        [Required(ErrorMessage = "Please enter a publisher name")]
        [StringLength(60, ErrorMessage = "Please make sure less than 60 characters")]
        public String Publisher { get; set; }

        /// <summary>
        /// This is ISBN of the book
        /// </summary>
        [Required(ErrorMessage = "Please enter an ISBN")]
        [StringLength(16, MinimumLength = 10, ErrorMessage = "Please enter a right ISBN")]
        public String ISBN { get; set; }

        /// <summary>
        /// This is a list of reviews for the book
        /// </summary>
        public virtual ICollection<Review> Reviews { get; set; }

        /// <summary>
        /// This is a list orders for this book
        /// </summary>
        public virtual ICollection<Order> Orders { get; set; }
    }
}