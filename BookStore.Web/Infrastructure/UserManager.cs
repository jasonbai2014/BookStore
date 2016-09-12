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
    public class UserManager : UserManager<User>
    {
        /// <summary>
        /// This is a class used to manage user accounts
        /// </summary>
        /// <param name="store">This is user store</param>
        public UserManager(IUserStore<User> store) : base(store)
        {

        }

        /// <summary>
        /// This is used to create a UserManager instance
        /// </summary>
        /// <param name="options">This is for configuration options for a 
        /// IdentityFactoryMiddleware</param>
        /// <param name="context">This wraps OWIN environment dictionary and 
        /// provides strongly typed accessors.</param>
        /// <returns>A UserManager instance</returns>
        public static UserManager Create(IdentityFactoryOptions<UserManager> options, IOwinContext context)
        {
            StoreDbContext dbContext = context.Get<StoreDbContext>();
            UserManager manager = new UserManager(new UserStore<User>(dbContext));

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 12,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            manager.UserValidator = new UserValidator<User>(manager)
            {
               AllowOnlyAlphanumericUserNames = true,
               RequireUniqueEmail = true
            };

            return manager;
        }
    }
}