using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Web.Concrete;
using BookStore.Web.Models;
using System.Linq;
using System.Collections.Generic;
using Moq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace BookStore.UnitTests.Repository
{
    [TestClass]
    public class AddressRepositoryTest
    {
        private AddressRepository addressRepo;

        [TestInitialize]
        public void TestInitializer()
        {
            IQueryable<ShoppingAddress> data = new List<ShoppingAddress>()
            {
                new ShoppingAddress {ShoppingAddressID = 1, UserID = "#421521", Line1 = "Orchard Rd", City = "Seattle",
                    State = "WA", Country = "US", Zip = "91823" },
                new ShoppingAddress {ShoppingAddressID = 2, UserID = "#374821", Line1 = "Smith St", City = "Tacoma",
                    State = "WA", Country = "US", Zip = "94323" },
                new ShoppingAddress {ShoppingAddressID = 3, UserID = "#211521", Line1 = "W 49 St", City = "Portland",
                    State = "OR", Country = "US", Zip = "96594" },
                new ShoppingAddress {ShoppingAddressID = 4, UserID = "#421521", Line1 = "Sunset St", City = "Bellingham",
                    State = "WA", Country = "US", Zip = "98226" },
                new ShoppingAddress {ShoppingAddressID = 5, UserID = "#289182", Line1 = "Lake Way", City = "San Jose",
                    State = "CA", Country = "US", Zip = "34823" },
                new ShoppingAddress {ShoppingAddressID = 6, UserID = "#421521", Line1 = "Chestnut Rd", City = "New York",
                    State = "NY", Country = "US", Zip = "65823" }
            }.AsQueryable();

            Mock<DbSet<ShoppingAddress>> mockSet = new Mock<DbSet<ShoppingAddress>>();
            mockSet.As<IDbAsyncEnumerable<ShoppingAddress>>().Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<ShoppingAddress>(data.GetEnumerator()));

            mockSet.As<IQueryable<ShoppingAddress>>().Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<ShoppingAddress>(data.Provider));

            mockSet.As<IQueryable<ShoppingAddress>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<ShoppingAddress>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<ShoppingAddress>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            Mock<StoreDbContext> mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(x => x.Addresses).Returns(mockSet.Object);
            addressRepo = new AddressRepository(mockContext.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            addressRepo = null;
        }

        [TestMethod]
        public async Task Can_Find_Addresses_By_UserID()
        {
            List<ShoppingAddress> addresses = await addressRepo.FindAddressesByUserIDAsync("#421521");
            ShoppingAddress[] addArr = addresses.ToArray();

            Assert.AreEqual(3, addArr.Length);
            Assert.AreEqual("Orchard Rd", addArr[0].Line1);
            Assert.AreEqual("Sunset St", addArr[1].Line1);
            Assert.AreEqual("Chestnut Rd", addArr[2].Line1);
        }

        [TestMethod]
        public async Task Cannot_Find_Addresses_With_Invalid_UserID()
        {
            List<ShoppingAddress> addresses = await addressRepo.FindAddressesByUserIDAsync("#888521");
            Assert.AreEqual(0, addresses.Count());
        }
    }
}
