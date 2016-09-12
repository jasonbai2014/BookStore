using BookStore.Web.Infrastructure;
using BookStore.Web.Models;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Controllers
{
    /// <summary>
    /// This is a class used for accounts
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// This user manager manages all the accounts of this app
        /// </summary>
        private UserManager UserManager
        {
            get { return HttpContext.GetOwinContext().Get<UserManager>(); }
        }

        /// <summary>
        /// This is used to manage authentication
        /// </summary>
        private IAuthenticationManager AuthManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        [AllowAnonymous]
        public ViewResult Signin()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signin(SigninInfo account)
        {
            if (ModelState.IsValid)
            {
                User user = await UserManager.FindAsync(account.Name, account.Password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid name or password");
                } else
                {
                    ClaimsIdentity identity = await UserManager.CreateIdentityAsync(user, 
                        DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

                    return RedirectToAction("List", "Book");
                }  
            }

            return View(account);
        }

        [AllowAnonymous]
        public ViewResult Signup()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signup(SignupInfo account)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = account.Name, Email = account.Email};
                IdentityResult result = await UserManager.CreateAsync(user, account.Password);

                if (result.Succeeded)
                {
                    User createdUser = await UserManager.FindAsync(account.Name, account.Password);
                    ClaimsIdentity identity = await UserManager.CreateIdentityAsync(createdUser, 
                        DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

                    return RedirectToAction("List", "Book");
                } else
                {
                    AddErrorMessage(result);
                }
            }

            return View(account);
        } 

        public RedirectToRouteResult Signout()
        {
            AuthManager.SignOut();
            return RedirectToAction("List", "Book");
        }

        private void AddErrorMessage(IdentityResult result)
        {
            foreach(String error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}