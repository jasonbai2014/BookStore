using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.HtmlHelpers
{
    /// <summary>
    /// This is a class for a html extension helper method for rating stars
    /// </summary>
    public static class RatingStarsHelper
    {
        /// <summary>
        /// This is a html extension helper method for rating stars 
        /// </summary>
        /// <param name="html">HtmlHelper class</param>
        /// <param name="rating">This is a rating of a book</param>
        /// <returns>A string that has a list span tags for rating stars</returns>
        public static MvcHtmlString ShowRatingStars(this HtmlHelper html, double rating)
        {
            TagBuilder outerSpanTag = new TagBuilder("span");
            outerSpanTag.MergeAttribute("title", rating.ToString("F1") + " out of 5 stars");
            int starNum = (int) rating;

            // for full stars
            for (int i = 0; i < starNum; i++)
            {
                outerSpanTag.InnerHtml += CreateStarSpan(StarType.Full);
            }

            // for half star
            if (rating > starNum)
            {
                outerSpanTag.InnerHtml += CreateStarSpan(StarType.HalfFull);
            }

            int emptyStarNum = (int)(5 - rating);
            // for empty star
            for (int i = 0; i < emptyStarNum; i++)
            {
                outerSpanTag.InnerHtml += CreateStarSpan(StarType.Empty);
            }

            return MvcHtmlString.Create(outerSpanTag.ToString());
        }

        /// <summary>
        /// This is a helper method that create star span tag
        /// </summary>
        /// <param name="type">This is type of star</param>
        /// <returns>A string containing a span tag for a star</returns>
        private static String CreateStarSpan(StarType type)
        {
            TagBuilder nestedSpan = new TagBuilder("span");
            nestedSpan.AddCssClass("glyphicon");
            nestedSpan.AddCssClass("glyphicon-star");

            if (type == StarType.Empty)
            {
                nestedSpan.AddCssClass("empty");
            } else if (type == StarType.HalfFull) {
                nestedSpan.AddCssClass("half");
            }

            return nestedSpan.ToString();
        }

        /// <summary>
        /// This is an enum type defining star type
        /// </summary>
        private enum StarType
        {
            Full, HalfFull, Empty
        }
    }
}