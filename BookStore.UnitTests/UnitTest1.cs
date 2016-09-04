using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.Web.Abstract;
using BookStore.Web.Models;
using BookStore.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookStore.Web.HtmlHelpers;

namespace BookStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            bookRepo.Setup(x => x.Books).Returns(new Book[]
            {
                new Book {BookID = 1, Name = "Book 1" },
                new Book {BookID = 2, Name = "Book 2" },
                new Book {BookID = 3, Name = "Book 3" },
                new Book {BookID = 4, Name = "Book 4" },
                new Book {BookID = 5, Name = "Book 5" },
                new Book {BookID = 6, Name = "Book 6" },
                new Book {BookID = 7, Name = "Book 7" },
                new Book {BookID = 8, Name = "Book 8" },
                new Book {BookID = 9, Name = "Book 9" }
            });

            BookController bookCtrl = new BookController(bookRepo.Object);
            PagingInfo result = (PagingInfo) bookCtrl.List(2).Model;
            Book[] books = result.Books.ToArray();

            Assert.IsTrue(result.TotalPages == 2, "Didn't have correct total pages");
            Assert.IsTrue(result.CurPage == 2, "Didn't have correct current page");
            Assert.IsTrue(books.Length == 3, "Didn't get right amount of books on current page");
            Assert.AreEqual("Book 7", books[0].Name, "Didn't get correct book on current page");
            Assert.AreEqual("Book 8", books[1].Name, "Didn't get correct book on current page");
            Assert.AreEqual("Book 9", books[2].Name, "Didn't get correct book on current page");
        }

        [TestMethod]
        public void Can_Create_Page_Links()
        {
            Func<int, String> linkCreator = x => "page" + x;
            HtmlHelper helper = null;
            MvcHtmlString result =  helper.PageLinks(2, 3, linkCreator);
            Assert.AreEqual(@"<nav><ul class=""pagination""><li><a href=""page1"">1</a></li>" + 
                @"<li class=""active""><a href=""page2"">2</a></li><li><a href=""page3"">3</a></li></ul></nav>", 
                result.ToString(), "Didn't generate correct page links");
        }

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
        public void Can_Show_Book_Details()
        {
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            bookRepo.Setup(x => x.Books).Returns(new Book[]
            {
                new Book {BookID = 1, Name = "Book 1" },
                new Book {BookID = 2, Name = "Book 2" },
                new Book {BookID = 3, Name = "Book 3" },
                new Book {BookID = 4, Name = "Book 4" },
                new Book {BookID = 5, Name = "Book 5" },
                new Book {BookID = 6, Name = "Book 6" },
                new Book {BookID = 7, Name = "Book 7" },
                new Book {BookID = 8, Name = "Book 8" },
                new Book {BookID = 9, Name = "Book 9" }
            });

            BookController bookCtrl = new BookController(bookRepo.Object);
            Book book1 = (Book)bookCtrl.Detail(3).Model;
            Book book2 = (Book)bookCtrl.Detail(5).Model;
            Book book3 = (Book)bookCtrl.Detail(9).Model;

            Assert.AreEqual("Book 3", book1.Name, "Didn't get right book details");
            Assert.AreEqual("Book 5", book2.Name, "Didn't get right book details");
            Assert.AreEqual("Book 9", book3.Name, "Didn't get right book details");
        }

        [TestMethod]
        public void Can_Generate_Error_For_Book_Details()
        {
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            bookRepo.Setup(x => x.Books).Returns(new Book[]
            {
                new Book {BookID = 1, Name = "Book 1" },
                new Book {BookID = 2, Name = "Book 2" },
                new Book {BookID = 3, Name = "Book 3" },
                new Book {BookID = 4, Name = "Book 4" },
                new Book {BookID = 5, Name = "Book 5" },
                new Book {BookID = 6, Name = "Book 6" },
            });

            BookController bookCtrl = new BookController(bookRepo.Object);
            var model = bookCtrl.Detail(8);
            Error error = model.ViewData.Model as Error;
            Assert.IsNotNull(error, "Didn't get error instance");
            Assert.AreEqual("Can't Find the Book", error.Message, "Didn't carry right error message");
        }
    }
}
