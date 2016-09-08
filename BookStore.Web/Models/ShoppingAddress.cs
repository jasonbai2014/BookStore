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
        /// This is name of receiver
        /// </summary>
        [Required(ErrorMessage = "Please enter a name")]
        [Display(Name = "Full Name")]
        public String Name { get; set; }

        /// <summary>
        /// This is the first address line
        /// </summary>
        [Required(ErrorMessage = "Please enter the first address line")]
        [Display(Name = "Address line 1")]
        public String Line1 { get; set; }

        /// <summary>
        /// This is the second address line
        /// </summary>
        [Display(Name = "Address line 2")]
        public String Line2 { get; set; }

        /// <summary>
        /// This is name of city
        /// </summary>
        [Required(ErrorMessage = "Please enter a city name")]
        public String City { get; set; }

        /// <summary>
        /// This is name of state
        /// </summary>
        [Required(ErrorMessage = "Please enter a state name")]
        public String State { get; set; }

        /// <summary>
        /// This is zip code
        /// </summary>
        [Required(ErrorMessage = "Please enter a zip code")]
        public String Zip { get; set; }

        /// <summary>
        /// This is name of country
        /// </summary>
        [Required(ErrorMessage = "Please enter a country name")]
        public String Country { get; set; }
    }
}