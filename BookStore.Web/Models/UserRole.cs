using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Web.Models
{
    /// <summary>
    /// This is a class for roles of users
    /// </summary>
    public class UserRole : IdentityRole
    {
        /// <summary>
        /// This is a constructor of this class
        /// </summary>
        public UserRole() : base()
        {

        }

        /// <summary>
        /// This is a constructor taking a name of a user role
        /// </summary>
        /// <param name="name">name of a user role</param>
        public UserRole(String name) : base(name)
        {

        }
    }
}