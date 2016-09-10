using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Web.Concrete;
using Moq;
using BookStore.Web.Models;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;

namespace BookStore.UnitTests.Repository
{
    [TestClass]
    public class BookRepositoryTest
    {
        private BookRepository bookRepo;

        [TestInitialize]
        public void TestInitializer()
        {
            IQueryable<Book> data = new List<Book>()
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
        public void Can_Search_Books_By_Category()
        {
            Book[] books = bookRepo.SearchBooks("Programming", null).ToArray();

            Assert.AreEqual(3, books.Length);
            Assert.AreEqual("Book 1", books[0].Name);
            Assert.AreEqual("Book 4", books[1].Name);
            Assert.AreEqual("Book 9", books[2].Name);
        }

        [TestMethod]
        public void Can_Search_Books_By_Name()
        {
            Book[] books = bookRepo.SearchBooks(null, "6").ToArray();

            Assert.AreEqual(1, books.Length);
            Assert.AreEqual("Book 6", books[0].Name);
        }

        [TestMethod]
        public void Can_Search_Books_By_Category_And_Name()
        {
            Book[] books = bookRepo.SearchBooks("Travel", "book").ToArray();

            Assert.AreEqual(3, books.Length);
            Assert.AreEqual("Book 6", books[0].Name);
            Assert.AreEqual("Book 7", books[1].Name);
            Assert.AreEqual("Book 8", books[2].Name);
        }

        [TestMethod]
        public void Can_Get_Categories()
        {
            String[] categories = bookRepo.GetCategories();

            Assert.AreEqual(4, categories.Length);
            Assert.AreEqual("Comics", categories[0]);
            Assert.AreEqual("History", categories[1]);
            Assert.AreEqual("Programming", categories[2]);
            Assert.AreEqual("Travel", categories[3]);
        }
    }
}
