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

        ///// <summary>
        ///// This is used to manage user roles
        ///// </summary>
        //private UserRoleManager RoleManager
        //{
        //    get { return HttpContext.GetOwinContext().Get<UserRoleManager>(); }
        //}

        /// <summary>
        /// This shows a sign in page
        /// </summary>
        /// <param name="returnUrl">This is a return url</param>
        /// <returns>A ActionResult for sign in page or redirects user to
        /// home page if the user has already signed in</returns>
        [AllowAnonymous]
        public ActionResult Signin(String returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("List", "Book");
            }

            ViewBag.returnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// This allows a user to sign in if the user has an account
        /// </summary>
        /// <param name="account">This is account information that the user types in</param>
        /// <param name="returnUrl">This is a return url</param>
        /// <returns>Redirects to the home page if succeeded, otherwise returns back to the sign in page</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signin(SigninInfo account, String returnUrl)
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

                    if (String.IsNullOrEmpty(returnUrl))
                    {
                        return RedirectToAction("List", "Book");
                    } else
                    {
                        // if a return url exist, user will be sent to the page 
                        return Redirect(returnUrl);
                    }
                }  
            }

            return View(account);
        }

        /// <summary>
        /// This shows a page for user to sign up an account
        /// </summary>
        /// <param name="returnUrl">This is a return url</param>
        /// <returns>A ActionResult for sign in page or redirects user to
        /// home page if the user has already signed in</returns>
        [AllowAnonymous]
        public ActionResult Signup(String returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("List", "Book");
            }

            ViewBag.returnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// This allows a user to register an account
        /// </summary>
        /// <param name="account">This is account information that the user types in</param>
        /// <param name="returnUrl">This is a return url</param>
        /// <returns>Redirects to the home page if succeeded, otherwise returns back to the sign up page</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signup(SignupInfo account, String returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = new User { UserName = account.Name, Email = account.Email};
                IdentityResult result = await UserManager.CreateAsync(user, account.Password);

                if (result.Succeeded)
                {
                    User createdUser = await UserManager.FindAsync(account.Name, account.Password);
                    // assign the user to customer role
                    IdentityResult roleResult = await UserManager.AddToRoleAsync(createdUser.Id, "Customer");

                    if (!roleResult.Succeeded)
                    {
                        AddErrorMessage(roleResult);
                    } else
                    {
                        ClaimsIdentity identity = await UserManager.CreateIdentityAsync(createdUser,
                        DefaultAuthenticationTypes.ApplicationCookie);
                        AuthManager.SignOut();
                        AuthManager.SignIn(new AuthenticationProperties { IsPersistent = true }, identity);

                        if (String.IsNullOrEmpty(returnUrl))
                        {
                            return RedirectToAction("List", "Book");
                        } else
                        {
                            // if a return url exist, user will be sent to the page 
                            return Redirect(returnUrl);
                        }
                    }            
                } else
                {
                    AddErrorMessage(result);
                }
            }

            return View(account);
        }

        /// <summary>
        /// This allows user to sign out
        /// </summary>
        /// <returns>Redirects to home page</returns>
        [Authorize]
        public RedirectToRouteResult Signout()
        {
            AuthManager.SignOut();
            return RedirectToAction("List", "Book");
        }

        /// <summary>
        /// This is a helper method used to handle error messages from IdentityResult
        /// </summary>
        /// <param name="result">This is an IdentityResult carrying error messages</param>
        private void AddErrorMessage(IdentityResult result)
        {
            foreach(String error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}