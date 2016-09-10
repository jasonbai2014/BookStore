using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.Web.Models;
using BookStore.Web.Controllers;
using System.Web.Mvc;
using BookStore.Web.Concrete;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BookStore.UnitTests.Controller
{
    [TestClass]
    public class CartControllerTest
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
        public void Can_Show_Summary()
        {
            CartController cartCtrl = new CartController(bookRepo);
            Cart cart = new Cart();
            cart.Add(new Book { BookID = 1, Name = "SQL programming", Category = "Programming", Price = 12.1M }, 2);
            cart.Add(new Book { BookID = 7, Name = "National Parks", Category = "Travel", Price = 10.5M }, 1);
            Cart cartModel = (Cart)cartCtrl.Summary(cart).Model;

            Assert.AreEqual(2, cartModel.Items.Count);
            Assert.AreEqual(1, cartModel.Items[0].Book.BookID);
            Assert.AreEqual(7, cartModel.Items[1].Book.BookID);
        }

        [TestMethod]
        public void Can_Checkout()
        {
            CartController cartCtrl = new CartController(bookRepo);
            Cart cart = new Cart();
            cart.Add(new Book { BookID = 6, Name = "Road Trip", Category = "Travel", Price = 18.0M }, 1);
            cart.Add(new Book { BookID = 9, Name = "Web API", Category = "Programming", Price = 52.5M }, 1);

            ViewResult result = cartCtrl.CheckOut(cart, new ShoppingAddress());

            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(0, cart.Items.Count);
        }

        [TestMethod]
        public void Cannot_Checkout_With_Invalid_Address()
        {
            CartController cartCtrl = new CartController(bookRepo);
            Cart cart = new Cart();
            cart.Add(new Book { BookID = 3, Name = "America History", Category = "History", Price = 21.8M }, 3);
            cartCtrl.ModelState.AddModelError("error", "Invalid address");

            ViewResult result = cartCtrl.CheckOut(cart, new ShoppingAddress());
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
            Assert.AreEqual(1, cart.Items.Count);
            Assert.AreEqual("", result.ViewName);
        }
    }
}
