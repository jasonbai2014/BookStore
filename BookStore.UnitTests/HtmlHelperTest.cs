using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using BookStore.Web.HtmlHelpers;

namespace BookStore.UnitTests
{

    [TestClass]
    public class HtmlHelperTest
    {

        [TestMethod]
        public void Can_Generate_Rating_Stars()
        {
            HtmlHelper helper = null;
            MvcHtmlString result = helper.ShowRatingStars(3.9);
            Assert.AreEqual(@"<span title=""3.9 out of 5 stars""><span class=""glyphicon-star glyphicon""></span>" +
                @"<span class=""glyphicon-star glyphicon""></span><span class=""glyphicon-star glyphicon""></span>" +
                @"<span class=""half glyphicon-star glyphicon""></span><span class=""empty glyphicon-star glyphicon""></span></span>",
                result.ToString(), "Didn't generate correct rating stars label");
        }

        [TestMethod]
        public void Can_Create_Page_Links()
        {
            Func<int, String> linkCreator = x => "page" + x;
            HtmlHelper helper = null;
            MvcHtmlString result = helper.PageLinks(2, 3, linkCreator);
            Assert.AreEqual(@"<nav><ul class=""pagination""><li><a href=""page1"">1</a></li>" +
                @"<li class=""active""><a href=""page2"">2</a></li><li><a href=""page3"">3</a></li></ul></nav>",
                result.ToString(), "Didn't generate correct page links");
        }
    }
}
