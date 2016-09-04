using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.HtmlHelpers
{
    /// <summary>
    /// This is a class for a html extension helper method for pagination
    /// </summary>
    public static class PagingHelper
    {
        /// <summary>
        /// This is a html extension helper method for pagination 
        /// </summary>
        /// <param name="html">HtmlHelper class</param>
        /// <param name="curPage">This is current page</param>
        /// <param name="totalPage">This is total pages</param>
        /// <param name="pageUrl">This is used to generate a link</param>
        /// <returns>A String containing links for pages</returns>
        public static MvcHtmlString PageLinks(this HtmlHelper html, int curPage, int totalPage, Func<int, String> pageUrl)
        {
            TagBuilder navTag = new TagBuilder("nav");
            TagBuilder ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("pagination");

            for (int i = 1; i <= totalPage; i++)
            {
                TagBuilder liTag = new TagBuilder("li");
                TagBuilder aTag = new TagBuilder("a");
                aTag.MergeAttribute("href", pageUrl(i));

                if (i == curPage)
                {
                    liTag.AddCssClass("active");
                }

                aTag.InnerHtml = i.ToString();
                liTag.InnerHtml = aTag.ToString();
                ulTag.InnerHtml += liTag.ToString();
            }

            navTag.InnerHtml = ulTag.ToString();

            return MvcHtmlString.Create(navTag.ToString());
        }
    }
}