using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Web.Models
{
    /// <summary>
    /// This is a view model for sign up information
    /// </summary>
    public class SignupInfo
    {
        /// <summary>
        /// This is an account name
        /// </summary>
        [Required(ErrorMessage = "Please enter an account name")]
        [Display(Name = "Account name")]
        public String Name { get; set; }

        /// <summary>
        /// This is a user's email address
        /// </summary>
        [Required(ErrorMessage = "Please enter an email address")]
        public String Email { get; set; }

        /// <summary>
        /// This is user's password
        /// </summary>
        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(12, ErrorMessage = "Please make sure your password is at least 12 characters")]
        public String Password { get; set; }
    }
}