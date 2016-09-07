using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Web.Models;

namespace BookStore.UnitTests.Model
{
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public void Can_Add_Item()
        {
            Book book1 = new Book { BookID = 1, Name = "book 1", Price = 25.2M };
            Book book2 = new Book { BookID = 2, Name = "book 2", Price = 50.6M };
            Book book3 = new Book { BookID = 3, Name = "book 3", Price = 36.7M };

            Cart cart = new Cart();
            cart.Add(book1, 2);
            cart.Add(book2, 3);
            cart.Add(book3, 1);

            Assert.AreEqual(3, cart.Items.Count, "Didn't add all items");
            Assert.AreEqual(book1, cart.Items[0].Book, "Didn't add all items");
            Assert.AreEqual(book2, cart.Items[1].Book, "Didn't add all items");
            Assert.AreEqual(book3, cart.Items[2].Book, "Didn't add all items");
        }

        [TestMethod]
        public void Can_Add_Exist_Item()
        {
            Book book1 = new Book { BookID = 1, Name = "book 1", Price = 25.2M };
            Book book2 = new Book { BookID = 2, Name = "book 2", Price = 50.6M };
            Book book3 = new Book { BookID = 3, Name = "book 3", Price = 36.7M };

            Cart cart = new Cart();
            cart.Add(book1, 2);
            cart.Add(book2, 5);
            cart.Add(book3, 7);
            cart.Add(book1, 6);

            Assert.AreEqual(3, cart.Items.Count, "Didn't increment existing item quantity");
            Assert.AreEqual(8, cart.Items[0].Quantity, "Didn't increment existing item quantity");
        }

        [TestMethod]
        public void Can_Remove_Item()
        {
            Book book1 = new Book { BookID = 1, Name = "book 1", Price = 25.2M };
            Book book2 = new Book { BookID = 2, Name = "book 2", Price = 50.6M };
            Book book3 = new Book { BookID = 3, Name = "book 3", Price = 36.7M };

            Cart cart = new Cart();
            cart.Add(book2, 3);
            cart.Add(book1, 2);
            cart.Add(book3, 8);

            cart.Revome(book2);

            Assert.AreEqual(2, cart.Items.Count, "Didn't remove the book");
            Assert.IsNull(cart.Items.Find(x => x.Book.BookID == book2.BookID), "Didn't remove the right book");
        }

        [TestMethod]
        public void Can_Clear_Items()
        {
            Book book1 = new Book { BookID = 1, Name = "book 1", Price = 25.2M };
            Book book2 = new Book { BookID = 2, Name = "book 2", Price = 50.6M };
            Book book3 = new Book { BookID = 3, Name = "book 3", Price = 36.7M };

            Cart cart = new Cart();
            cart.Add(book2, 3);
            cart.Add(book1, 2);
            cart.Add(book3, 8);
            cart.clear();

            Assert.AreEqual(0, cart.Items.Count, "Didn't remove all the items");
        }

        [TestMethod]
        public void Can_Calculate_The_Total()
        {
            Book book1 = new Book { BookID = 1, Name = "book 1", Price = 25.2M };
            Book book2 = new Book { BookID = 2, Name = "book 2", Price = 50.6M };
            Book book3 = new Book { BookID = 3, Name = "book 3", Price = 36.7M };

            Cart cart = new Cart();
            cart.Add(book2, 3);
            cart.Add(book1, 2);
            cart.Add(book3, 3);

            Assert.AreEqual(312.3M, cart.CalculateTotal(), "Didn't get the right total price");
        }
    }
}
