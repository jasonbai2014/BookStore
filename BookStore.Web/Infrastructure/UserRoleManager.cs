using BookStore.Web.Concrete;
using BookStore.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Web.Infrastructure
{
    /// <summary>
    /// This is a class used to manage roles of users
    /// </summary>
    public class UserRoleManager : RoleManager<UserRole>, IDisposable
    {
        /// <summary>
        /// This is a constructor
        /// </summary>
        /// <param name="store">This is role store</param>
        public UserRoleManager(RoleStore<UserRole> store) : base(store)
        {

        }

        /// <summary>
        /// This is used to create a UserRoleManager instance
        /// </summary>
        /// <param name="options">This is for configuration options for a 
        /// IdentityFactoryMiddleware</param>
        /// <param name="context">This wraps OWIN environment dictionary and 
        /// provides strongly typed accessors.</param>
        /// <returns>A UserRoleManager instance</returns>
        public static UserRoleManager Create(IdentityFactoryOptions<UserRoleManager> options, IOwinContext context)
        {
            StoreDbContext dbContext = context.Get<StoreDbContext>();   
            UserRoleManager manager = new UserRoleManager(new RoleStore<UserRole>(dbContext));
            return manager;
        }
    }
}