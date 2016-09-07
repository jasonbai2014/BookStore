using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.Web.Abstract;
using BookStore.Web.Models;
using BookStore.Web.Controllers;
using System.Web.Mvc;

namespace BookStore.UnitTests.Controller
{
    [TestClass]
    public class CartControllerTest
    {
        [TestMethod]
        public void Can_Show_Summary()
        {
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            bookRepo.Setup(x => x.Books).Returns(new Book[]
            {
                new Book { BookID = 1, Name = "book 1", Price = 25.2M },
                new Book { BookID = 5, Name = "book 5", Price = 10.6M },
                new Book { BookID = 9, Name = "book 9", Price = 66.7M },
            });

            CartController cartCtrl = new CartController(bookRepo.Object);
            Cart cart = new Cart();
            cartCtrl.AddToCart(cart, 5, 1);
            cartCtrl.AddToCart(cart, 9, 1);
            Cart cartModel = (Cart)cartCtrl.Summary(cart).Model;

            Assert.AreEqual(2, cartModel.Items.Count, "Didn't show right results");
            Assert.AreEqual(5, cartModel.Items[0].Book.BookID, "Didn't show right results");
            Assert.AreEqual(9, cartModel.Items[1].Book.BookID, "Didn't show right results");
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            bookRepo.Setup(x => x.Books).Returns(new Book[]
            {
                new Book { BookID = 1, Name = "book 1", Price = 25.2M },
                new Book { BookID = 2, Name = "book 2", Price = 50.6M },
                new Book { BookID = 3, Name = "book 3", Price = 36.7M },
            });

            CartController cartCtrl = new CartController(bookRepo.Object);
            Cart cart = new Cart();

            cartCtrl.AddToCart(cart, 2, 3);
            Assert.AreEqual(1, cart.Items.Count, "Didn't add into the cart");
            Assert.AreEqual(2, cart.Items[0].Book.BookID, "Didn't add into the cart");
            Assert.AreEqual(3, cart.Items[0].Quantity, "Didn't add into the cart");
        }

        [TestMethod]
        public void Can_Add_To_Cart_Redirect()
        {
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            bookRepo.Setup(x => x.Books).Returns(new Book[]
            {
                new Book { BookID = 2, Name = "book 2", Price = 50.6M },
            });

            CartController cartCtrl = new CartController(bookRepo.Object);
            Cart cart = new Cart();

            RedirectToRouteResult result = cartCtrl.AddToCart(cart, 2, 3);
            Assert.AreEqual("Summary", result.RouteValues["action"], "Didn't redirect");
        }

        [TestMethod]
        public void Can_Remove_From_Cart()
        {
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            bookRepo.Setup(x => x.Books).Returns(new Book[]
            {
                new Book { BookID = 1, Name = "book 1", Price = 25.2M },
                new Book { BookID = 2, Name = "book 2", Price = 50.6M },
                new Book { BookID = 3, Name = "book 3", Price = 36.7M },
            });

            CartController cartCtrl = new CartController(bookRepo.Object);
            Cart cart = new Cart();
            cartCtrl.AddToCart(cart, 1, 2);
            cartCtrl.AddToCart(cart, 3, 2);
            cartCtrl.RemoveFromCart(cart, 1);

            Assert.AreEqual(1, cart.Items.Count, "Didn't remove from the cart");
            Assert.AreEqual(3, cart.Items[0].Book.BookID, "Didn't remove the right book");
            Assert.AreEqual(2, cart.Items[0].Quantity, "Didn't remove the right book");
        }

        [TestMethod]
        public void Can_Remove_From_Cart_Redirect()
        {
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            bookRepo.Setup(x => x.Books).Returns(new Book[]
            {
                new Book { BookID = 1, Name = "book 1", Price = 25.2M }
            });

            CartController cartCtrl = new CartController(bookRepo.Object);
            Cart cart = new Cart();
            cartCtrl.AddToCart(cart, 1, 2);

            RedirectToRouteResult result = cartCtrl.RemoveFromCart(cart, 1);
            Assert.AreEqual("Summary", result.RouteValues["action"], "Didn't redirect");
        }
    }
}
