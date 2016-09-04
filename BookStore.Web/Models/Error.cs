using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Models
{
    /// <summary>
    /// This is a view model class for error message
    /// </summary>
    public class Error
    {
        /// <summary>
        /// This is an error message
        /// </summary>
        public String Message { get; set; }

        public static explicit operator Error(ViewResult v)
        {
            throw new NotImplementedException();
        }
    }
}