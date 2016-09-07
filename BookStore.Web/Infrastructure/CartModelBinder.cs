using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Infrastructure
{
    /// <summary>
    /// This is a model binder for Cart class
    /// </summary>
    public class CartModelBinder : IModelBinder
    {
        /// <summary>
        /// This is a key to get a cart instance from current session
        /// </summary>
        private const String SessionKey = "Cart";

        /// <summary>
        /// This gets a cart instance upon request
        /// </summary>
        /// <param name="controllerContext">This is context of a controller</param>
        /// <param name="bindingContext">This is context of a model</param>
        /// <returns>a cart object</returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext.HttpContext.Session[SessionKey] == null)
            {
                Cart cart = new Cart();
                controllerContext.HttpContext.Session[SessionKey] = cart;
                return cart;
            } else
            {
                return controllerContext.HttpContext.Session[SessionKey];
            }
        }
    }
}