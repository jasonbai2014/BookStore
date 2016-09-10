using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.Web.Models;
using BookStore.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BookStore.Web.Concrete;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;


namespace BookStore.UnitTests.Controller
{
    [TestClass]
    public class BookControllerTest
    {
        private BookRepository bookRepo;

        [TestInitialize]
        public void TestInitializer()
        {
            IQueryable<Book> data = new List<Book>()
            {
                new Book {BookID = 1, Name = "SQL programming", Category = "Programming", Price = 12.1M},
                new Book {BookID = 2, Name = "Star Wars", Category = "Comics", Price = 8.92M},
                new Book {BookID = 3, Name = "America History", Category = "History", Price = 21.8M},
                new Book {BookID = 4, Name = "JavaScript programming", Category = "Programming", Price = 32.5M},
                new Book {BookID = 5, Name = "Iron Man", Category = "Comics", Price = 21.9M},
                new Book {BookID = 6, Name = "Road Trip", Category = "Travel", Price = 18.0M},
                new Book {BookID = 7, Name = "National Parks",Category = "Travel", Price = 10.5M},
                new Book {BookID = 8, Name = "Web development", Category = "Programming", Price = 20.2M},
                new Book {BookID = 9, Name = "Web API", Category = "Programming", Price = 52.5M}
            }.AsQueryable();

            Mock<DbSet<Book>> mockSet = new Mock<DbSet<Book>>();
            mockSet.As<IDbAsyncEnumerable<Book>>().Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Book>(data.GetEnumerator()));

            mockSet.As<IQueryable<Book>>().Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Book>(data.Provider));

            mockSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            Mock<StoreDbContext> mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(x => x.Books).Returns(mockSet.Object);

            bookRepo = new BookRepository(mockContext.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            bookRepo = null;
        }

        [TestMethod]
        public async Task Can_Paginate()
        {
            BookController bookCtrl = new BookController(bookRepo);
            PagingInfo result = (PagingInfo)(await bookCtrl.List(null, null, 2)).Model;
            Book[] books = result.Books.ToArray();

            Assert.IsTrue(result.TotalPages == 2);
            Assert.IsTrue(result.CurPage == 2);
            Assert.IsTrue(books.Length == 3);
            Assert.AreEqual("National Parks", books[0].Name);
            Assert.AreEqual("Web development", books[1].Name);
            Assert.AreEqual("Web API", books[2].Name);
        }

        [TestMethod]
        public async Task Can_Show_Right_Category_Books()
        {
            BookController bookCtrl = new BookController(bookRepo);
            PagingInfo result = (PagingInfo)(await bookCtrl.List("Programming", null, 1)).Model;
            Book[] books = result.Books.ToArray();

            Assert.AreEqual(1, result.TotalPages);
            Assert.AreEqual(4, books.Length);
            Assert.AreEqual("SQL programming", books[0].Name);
            Assert.AreEqual("JavaScript programming", books[1].Name);
            Assert.AreEqual("Web development", books[2].Name);
            Assert.AreEqual("Web API", books[3].Name);
        }

        [TestMethod]
        public async Task Can_Search_Books()
        {
            BookController bookCtrl = new BookController(bookRepo);
            PagingInfo result = (PagingInfo)(await bookCtrl.List(null, "programming")).Model;
            Book[] books = result.Books.ToArray();

            Assert.AreEqual(2, books.Length);
            Assert.AreEqual("SQL programming", books[0].Name);
            Assert.AreEqual("JavaScript programming", books[1].Name);
        }

        [TestMethod]
        public async Task Can_Search_Books_With_Category()
        {
            BookController bookCtrl = new BookController(bookRepo);
            PagingInfo result = (PagingInfo)(await bookCtrl.List("Programming", "web")).Model;
            Book[] books = result.Books.ToArray();

            Assert.AreEqual(2, books.Length);
            Assert.AreEqual("Web development", books[0].Name);
            Assert.AreEqual("Web API", books[1].Name);
        }

        [TestMethod]
        public void Can_Redirect_Search_Page()
        {
            BookController bookCtrl = new BookController(bookRepo);
            RedirectToRouteResult result = bookCtrl.Search("Comics", "star");

            Assert.AreEqual("List", result.RouteValues["action"]);
            Assert.AreEqual("Comics", result.RouteValues["category"]);
            Assert.AreEqual("star", result.RouteValues["searchText"]);
        }
    }
}
