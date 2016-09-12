using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Web.Models
{
    /// <summary>
    /// This is a class for all users of this web app
    /// </summary>
    public class User : IdentityUser
    {
        /// <summary>
        /// This is a list of shopping addresses of this user
        /// </summary>
        public virtual ICollection<ShoppingAddress> Addresses { get; set; }
    }
}