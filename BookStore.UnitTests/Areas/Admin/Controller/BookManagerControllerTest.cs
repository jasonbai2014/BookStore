using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Web.Concrete;
using System.Linq;
using BookStore.Web.Models;
using Moq;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using BookStore.Web.Areas.Admin.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.UnitTests.Areas.Admin.Controller
{
    [TestClass]
    public class BookManagerControllerTest
    {
        private BookRepository bookRepo;

        private Mock<StoreDbContext> mockContext;

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
                new Book {BookID = 9, Name = "Web API", Category = "Programming", Price = 52.5M},
                new Book {BookID = 10, Name = "HTML and CSS", Category = "Programming", Price = 17.19M},
                new Book {BookID = 11, Name = "Avengers", Category = "Comics", Price = 14.49M },
                new Book {BookID = 12, Name = "1000 Places to See", Category = "Travel", Price = 23.95M }
            }.AsQueryable();

            Mock<DbSet<Book>> mockSet = new Mock<DbSet<Book>>();
            mockSet.As<IDbAsyncEnumerable<Book>>().Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<Book>(data.GetEnumerator()));

            mockSet.As<IQueryable<Book>>().Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<Book>(data.Provider));

            mockSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            mockContext = new Mock<StoreDbContext>();
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
            BookManageController ctrl = new BookManageController(bookRepo);
            PagingInfo result = (PagingInfo)(await ctrl.Index(2)).Model;
            Book[] books = result.Books.ToArray();

            Assert.IsTrue(result.TotalPages == 2);
            Assert.IsTrue(result.CurPage == 2, "2");
            Assert.IsTrue(books.Length == 2, "3");
            Assert.AreEqual("Avengers", books[0].Name);
            Assert.AreEqual("1000 Places to See", books[1].Name);
        }

        [TestMethod]
        public async Task Can_Edit_Book()
        {
            BookManageController ctrl = new BookManageController(bookRepo);
            Book book = new Book { Name = "C# programming" };
            ActionResult result = await ctrl.Edit(book);

            mockContext.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Cannot_Edit_Book_With_Invalid_Data()
        {
            BookManageController ctrl = new BookManageController(bookRepo);
            Book book = new Book { Name = "C# programming" };
            ctrl.ModelState.AddModelError("error", "error message");
            ActionResult result = await ctrl.Edit(book);

            mockContext.Verify(x => x.SaveChangesAsync(), Times.Never);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
