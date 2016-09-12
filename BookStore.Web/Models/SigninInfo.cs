using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Web.Models
{
    /// <summary>
    /// This is a class for sign in information
    /// </summary>
    public class SigninInfo
    {
        /// <summary>
        /// This is an account name
        /// </summary>
        [Required(ErrorMessage = "Please enter an account name")]
        [Display(Name = "Account name")]
        public String Name { get; set; }

        /// <summary>
        /// This is user's password
        /// </summary>
        [Required(ErrorMessage = "Please enter your password")]
        public String Password { get; set; }
    }
}