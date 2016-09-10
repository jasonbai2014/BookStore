using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Web.Models;
using Moq;
using BookStore.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using BookStore.Web.Concrete;
using System.Data.Entity;

namespace BookStore.UnitTests.Controller
{
    [TestClass]
    public class NavControllerTest
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
            mockSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(data.Provider);
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
        public void Can_Get_Nav_Categories()
        {
            NavController navCtrl = new NavController(bookRepo);
            String[] categories = ((IEnumerable<String>)navCtrl.Menu(null).Model).ToArray();

            Assert.AreEqual(4, categories.Length);
            Assert.AreEqual("Comics", categories[0]);
            Assert.AreEqual("History", categories[1]);
            Assert.AreEqual("Programming", categories[2]);
            Assert.AreEqual("Travel", categories[3]);
        }

        [TestMethod]
        public void Can_Highlight_Nav_Selected_Category()
        {
            NavController navCtrl = new NavController(bookRepo);
            String result = navCtrl.Menu("Programming").ViewBag.selectedCategory;

            Assert.AreEqual("Programming", result);
        }
    }
}
