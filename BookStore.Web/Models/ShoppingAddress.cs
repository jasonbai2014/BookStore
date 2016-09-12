using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Web.Models
{
    /// <summary>
    /// This is a class for shopping address
    /// </summary>
    public class ShoppingAddress
    {
        /// <summary>
        /// This is an ID for one shopping address
        /// </summary>
        public int ShoppingAddressID { get; set; }

        /// <summary>
        /// This is an ID of an user who owns this shopping address
        /// </summary>
        public String UserID { get; set; }

        /// <summary>
        /// This is name of receiver
        /// </summary>
        [Required(ErrorMessage = "Please enter a name")]
        [StringLength(256, ErrorMessage = "Please make sure less than 256 characters")]
        [Display(Name = "Full Name")]
        public String Name { get; set; }

        /// <summary>
        /// This is the first address line
        /// </summary>
        [Required(ErrorMessage = "Please enter the first address line")]
        [StringLength(256, ErrorMessage = "Please make sure less than 256 characters")]
        [Display(Name = "Address line 1")]
        public String Line1 { get; set; }

        /// <summary>
        /// This is the second address line
        /// </summary>
        [Display(Name = "Address line 2")]
        [StringLength(256, ErrorMessage = "Please make sure less than 256 characters")]
        public String Line2 { get; set; }

        /// <summary>
        /// This is name of city
        /// </summary>
        [Required(ErrorMessage = "Please enter a city name")]
        [StringLength(128, ErrorMessage = "Please make sure less than 128 characters")]
        public String City { get; set; }

        /// <summary>
        /// This is name of state
        /// </summary>
        [Required(ErrorMessage = "Please enter a state name")]
        [StringLength(64, ErrorMessage = "Please make sure less than 64 characters")]
        public String State { get; set; }

        /// <summary>
        /// This is zip code
        /// </summary>
        [Required(ErrorMessage = "Please enter a zip code")]
        [RegularExpression("^[0-9]{5}(?:-[0-9]{4})?$", ErrorMessage = "Please enter a valid zip code")] // US postal format
        [StringLength(16, ErrorMessage = "Please make sure less than 16 characters")]
        public String Zip { get; set; }

        /// <summary>
        /// This is name of country
        /// </summary>
        [Required(ErrorMessage = "Please enter a country name")]
        [StringLength(64, ErrorMessage = "Please make sure less than 64 characters")]
        public String Country { get; set; }

        /// <summary>
        /// This is the user who uses this shopping address
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// This is a list of orders shipped to this address
        /// </summary>
        public virtual ICollection<Order> Orders { get; set; }
    }
}