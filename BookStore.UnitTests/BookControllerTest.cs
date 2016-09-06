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
    public class BookControllerTest
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
            PagingInfo result = (PagingInfo) bookCtrl.List(null, 2).Model;
            Book[] books = result.Books.ToArray();

            Assert.IsTrue(result.TotalPages == 2, "Didn't have correct total pages");
            Assert.IsTrue(result.CurPage == 2, "Didn't have correct current page");
            Assert.IsTrue(books.Length == 3, "Didn't get right amount of books on current page");
            Assert.AreEqual("Book 7", books[0].Name, "Didn't get correct book on current page");
            Assert.AreEqual("Book 8", books[1].Name, "Didn't get correct book on current page");
            Assert.AreEqual("Book 9", books[2].Name, "Didn't get correct book on current page");
        }

        [TestMethod]
        public void Can_Show_Right_Category_Books()
        {
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            bookRepo.Setup(x => x.Books).Returns(new Book[]
            {
                new Book {BookID = 1, Name = "Book 1", Category = "Programming" },
                new Book {BookID = 2, Name = "Book 2", Category = "History" },
                new Book {BookID = 3, Name = "Book 3", Category = "History" },
                new Book {BookID = 4, Name = "Book 4", Category = "Programming" },
                new Book {BookID = 5, Name = "Book 5", Category = "Comics" },
                new Book {BookID = 6, Name = "Book 6", Category = "Travel" },
                new Book {BookID = 7, Name = "Book 7", Category = "Travel" },
                new Book {BookID = 8, Name = "Book 8", Category = "Travel" },
                new Book {BookID = 9, Name = "Book 9", Category = "Programming" }
            });

            BookController bookCtrl = new BookController(bookRepo.Object);
            PagingInfo result = (PagingInfo)bookCtrl.List("Programming", 1).Model;
            Book[] books = result.Books.ToArray();

            Assert.AreEqual(1, result.TotalPages, "Didn't get right total pages");
            Assert.AreEqual(3, books.Length, "Didn't get right amount of books");
            Assert.AreEqual("Book 1", books[0].Name, "Didn't get the right type of book");
            Assert.AreEqual("Book 4", books[1].Name, "Didn't get the right type of book");
            Assert.AreEqual("Book 9", books[2].Name, "Didn't get the right type of book");
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
