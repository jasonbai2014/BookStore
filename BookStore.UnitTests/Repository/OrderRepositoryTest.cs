using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Web.Concrete;
using BookStore.Web.Models;
using System.Data.Entity.Infrastructure;
using Moq;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.UnitTests.Repository
{
    [TestClass]
    public class OrderRepositoryTest
    {
        private OrderRepository orderRepo;

        [TestInitialize]
        public void TestInitializer()
        {
            ShoppingAddress address1 = new ShoppingAddress
            {
                ShoppingAddressID = 1,
                Name = "Jason",
                UserID = "123456",
                Line1 = "Orchard Rd",
                City = "Seattle",
                State = "WA",
                Country = "US",
                Zip = "91823",
                Orders = new List<Order>() { new Order { OrderID = 1, ShoppingAddressID = 1, Date = Convert.ToDateTime("01/02/2016") },
                new Order { OrderID = 3, ShoppingAddressID = 1, Date = Convert.ToDateTime("05/10/2016") }}
            };

            ShoppingAddress address2 = new ShoppingAddress
            {
                ShoppingAddressID = 2,
                Name = "Jason",
                UserID = "123456",
                Line1 = "Smith St",
                City = "Tacoma",
                State = "WA",
                Country = "US",
                Zip = "94323",
                Orders = new List<Order>() { new Order { OrderID = 4, ShoppingAddressID = 2, Date = Convert.ToDateTime("08/12/2016") } }
            };

            ShoppingAddress address3 = new ShoppingAddress
            {
                ShoppingAddressID = 5,
                UserID = "556293",
                Name = "Smith",
                Line1 = "Lake Way",
                City = "San Jose",
                State = "CA",
                Country = "US",
                Zip = "34823",
                Orders = new List<Order>() { new Order { OrderID = 2, ShoppingAddressID = 5, Date = Convert.ToDateTime("03/02/2016") } }
            };

            IQueryable<User> userData = new List<User>()
            {
                new User { Id = "123456", UserName = "Jason", Addresses = new List<ShoppingAddress>() { address1, address2} },
                new User { Id = "556293", UserName = "Smith", Addresses = new List<ShoppingAddress>() { address3 } }
            }.AsQueryable();

            Mock<DbSet<User>> userMockSet = new Mock<DbSet<User>>();

            userMockSet.As<IDbAsyncEnumerable<User>>().Setup(m => m.GetAsyncEnumerator())
            .Returns(new TestDbAsyncEnumerator<User>(userData.GetEnumerator()));
            userMockSet.As<IQueryable<User>>().Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<User>(userData.Provider));
            userMockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userData.Expression);
            userMockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            userMockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(userData.GetEnumerator());

            userMockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).
                Returns<object[]>(x => {
                    return Task.FromResult(userData.FirstOrDefault(b => b.Id == (String)x[0]));
                });

            Mock<StoreDbContext> mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(x => x.Users).Returns(userMockSet.Object);
            orderRepo = new OrderRepository(mockContext.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            orderRepo = null;
        }

        [TestMethod]
        public async Task Can_Find_Orders_By_UserID()
        {
            List<Order> orders = await orderRepo.FindOrdersByUserIDAsync("123456");
            Order[] orderArr = orders.ToArray();

            Assert.AreEqual(3, orderArr.Length);
            Assert.AreEqual(4, orderArr[0].OrderID);
            Assert.AreEqual(3, orderArr[1].OrderID);
            Assert.AreEqual(1, orderArr[2].OrderID);
        }
    }
}
