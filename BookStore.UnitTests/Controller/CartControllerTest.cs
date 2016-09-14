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

            mockSet.As<IQueryable<Book>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Book>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Book>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Book>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).
                Returns<object[]>(x => data.FirstOrDefault(b => b.BookID == (int)x[0]));

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
        public void Can_Add_To_Cart()
        {
            CartController cartCtrl = new CartController(bookRepo);
            Cart cart = new Cart();
            RedirectToRouteResult result = cartCtrl.AddToCart(cart, 2, 3);

            Assert.AreEqual("Summary", result.RouteValues["action"]);
            Assert.AreEqual(2, cart.Items[0].Book.BookID);
            Assert.AreEqual(3, cart.Items[0].Quantity);
        }

        [TestMethod]
        public void Can_Remove_From_Cart()
        {
            CartController cartCtrl = new CartController(bookRepo);
            Cart cart = new Cart();
            cart.Add(new Book { BookID = 2, Name = "Star Wars", Category = "Comics", Price = 8.92M }, 2);
            cart.Add(new Book { BookID = 8, Name = "Web development", Category = "Programming", Price = 20.2M }, 1);
            RedirectToRouteResult result = cartCtrl.RemoveFromCart(cart, 2);

            Assert.AreEqual(1, cart.Items.Count());
            Assert.AreEqual("Summary", result.RouteValues["action"]);
            Assert.AreEqual("Web development", (string)cart.Items[0].Book.Name);
            Assert.AreEqual(1, cart.Items[0].Quantity);
        }

        [TestMethod]
        public void Can_Checkout()
        {
            CartController cartCtrl = new CartController(bookRepo);
            RedirectToRouteResult result = cartCtrl.CheckOut("wuehfuw3838yy28r3");

            Assert.AreEqual("List", result.RouteValues["action"]);
            Assert.AreEqual("Address", result.RouteValues["controller"]);
            Assert.AreEqual("wuehfuw3838yy28r3", result.RouteValues["userId"]);
        }
    }
}
