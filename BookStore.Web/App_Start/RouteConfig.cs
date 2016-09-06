using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "BookCategoryPage",
                url: "{controller}/{category}/Page{page}",
                defaults: new { controller = "Book", action = "List" }
            );

            routes.MapRoute(
                name: "AllBookPage",
                url: "{controller}/Page{page}",
                defaults: new { controller = "Book", action = "List" }
            );

            routes.MapRoute(
                name: "BookDetailPage",
                url: "{controller}/{action}/Book{bookId}",
                defaults: new { controller = "Book", action = "Detail" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Book", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}
