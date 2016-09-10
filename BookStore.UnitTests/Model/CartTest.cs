using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Web.Models;
using System.Collections.Generic;

namespace BookStore.UnitTests.Model
{
    [TestClass]
    public class CartTest
    {
        private List<Book> data;

        [TestInitialize]
        public void TestInitializer()
        {
            data = new List<Book>()
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
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            data = null;
        }

        [TestMethod]
        public void Can_Add_Item()
        {
            Cart cart = new Cart();
            cart.Add(data[2], 2);
            cart.Add(data[4], 3);
            cart.Add(data[0], 1);

            Assert.AreEqual(3, cart.Items.Count);
            Assert.AreEqual(data[2], cart.Items[0].Book);
            Assert.AreEqual(2, cart.Items[0].Quantity);
            Assert.AreEqual(data[4], cart.Items[1].Book);
            Assert.AreEqual(3, cart.Items[1].Quantity);
            Assert.AreEqual(data[0], cart.Items[2].Book);
            Assert.AreEqual(1, cart.Items[2].Quantity);
        }

        [TestMethod]
        public void Can_Add_Exist_Item()
        {
            Cart cart = new Cart();
            cart.Add(data[1], 2);
            cart.Add(data[5], 5);
            cart.Add(data[6], 7);
            cart.Add(data[1], 6);

            Assert.AreEqual(3, cart.Items.Count);
            Assert.AreEqual(8, cart.Items[0].Quantity);
        }

        [TestMethod]
        public void Can_Remove_Item()
        {
            Cart cart = new Cart();
            cart.Add(data[2], 3);
            cart.Add(data[1], 2);
            cart.Add(data[3], 8);

            cart.Revome(data[2]);

            Assert.AreEqual(2, cart.Items.Count);
            Assert.IsNull(cart.Items.Find(x => x.Book.BookID == data[2].BookID));
        }

        [TestMethod]
        public void Can_Clear_Items()
        {
            Cart cart = new Cart();
            cart.Add(data[7], 3);
            cart.Add(data[3], 2);
            cart.Add(data[8], 8);
            cart.clear();

            Assert.AreEqual(0, cart.Items.Count);
        }

        [TestMethod]
        public void Can_Calculate_The_Total()
        {
            Cart cart = new Cart();
            cart.Add(data[2], 3);
            cart.Add(data[7], 2);
            cart.Add(data[5], 3);

            Assert.AreEqual(159.8M, cart.CalculateTotal(), "Didn't get the right total price");
        }
    }
}
