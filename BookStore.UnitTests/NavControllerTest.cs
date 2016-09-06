using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Web.Abstract;
using BookStore.Web.Models;
using Moq;
using BookStore.Web.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.UnitTests
{
    [TestClass]
    public class NavControllerTest
    {
        [TestMethod]
        public void Can_Get_Categories()
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

            NavController navCtrl = new NavController(bookRepo.Object);
            String[] categories = ((IEnumerable<String>)navCtrl.Menu(null).Model).ToArray();

            Assert.AreEqual(4, categories.Length, "Didn't generate right number of categories");
            Assert.AreEqual("Comics", categories[0], "Didn't sort categories correctly");
            Assert.AreEqual("History", categories[1], "Didn't sort categories correctly");
            Assert.AreEqual("Programming", categories[2], "Didn't sort categories correctly");
            Assert.AreEqual("Travel", categories[3], "Didn't sort categories correctly");
        }
        
        [TestMethod]
        public void Can_Highlight_Selected_Category()
        {
            Mock<IBookRepository> bookRepo = new Mock<IBookRepository>();
            bookRepo.Setup(x => x.Books).Returns(new Book[]
            {
                new Book {BookID = 1, Name = "Book 1", Category = "Programming" },
                new Book {BookID = 3, Name = "Book 3", Category = "History" },
                new Book {BookID = 5, Name = "Book 5", Category = "Comics" },
                new Book {BookID = 6, Name = "Book 6", Category = "Travel" },
            });

            NavController navCtrl = new NavController(bookRepo.Object);
            String result = navCtrl.Menu("Programming").ViewBag.selectedCategory;

            Assert.AreEqual("Programming", result, "Didn't highlight selected category");
        }
    }
}
