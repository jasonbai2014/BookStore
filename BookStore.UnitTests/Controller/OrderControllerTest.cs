using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Web.Concrete;
using BookStore.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Moq;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using BookStore.Web.Controllers;
using System.Web.Mvc;

namespace BookStore.UnitTests.Controller
{
    [TestClass]
    public class OrderControllerTest
    {
        private OrderRepository orderRepo;

        private Mock<StoreDbContext> mockContext;

        private Mock<DbSet<Order>> orderMockSet;

        private IQueryable<Order> orderData;

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

            orderData = new List<Order>()
            {
                new Order { OrderID = 1, ShoppingAddressID = 1, Date = Convert.ToDateTime("01/02/2016"), Status = OrderStatus.Processing },
                new Order { OrderID = 2, ShoppingAddressID = 5, Date = Convert.ToDateTime("03/02/2016"), Status = OrderStatus.Received },
                new Order { OrderID = 3, ShoppingAddressID = 1, Date = Convert.ToDateTime("05/10/2016"), Status = OrderStatus.Processing},
                new Order { OrderID = 4, ShoppingAddressID = 2, Date = Convert.ToDateTime("08/12/2016"), Status = OrderStatus.Shipped }
            }.AsQueryable();

            orderMockSet = new Mock<DbSet<Order>>();
            orderMockSet.Setup(x => x.Add(It.IsAny<Order>())).Verifiable();
            orderMockSet.Setup(x => x.Find(It.IsAny<Object[]>())).
                Returns<Object[]>(x => orderData.FirstOrDefault(o => o.OrderID == (int)x[0]));

            mockContext = new Mock<StoreDbContext>();
            mockContext.Setup(x => x.Users).Returns(userMockSet.Object);
            mockContext.Setup(x => x.Orders).Returns(orderMockSet.Object);

            orderRepo = new OrderRepository(mockContext.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            orderRepo = null;
        }

        [TestMethod]
        public async Task Can_List_Orders_With_UserId()
        {
            OrderController orderCtrl = new OrderController(orderRepo);
            ViewResult result = await orderCtrl.List("123456");
            Order[] orders = ((IEnumerable<Order>)result.Model).ToArray();

            Assert.AreEqual(3, orders.Length);
            Assert.AreEqual(4, orders[0].OrderID);
            Assert.AreEqual(3, orders[1].OrderID);
            Assert.AreEqual(1, orders[2].OrderID);
        }

        [TestMethod]
        public async Task Cannot_List_Orders_With_Invalid_UserId()
        {
            OrderController orderCtrl = new OrderController(orderRepo);
            ViewResult result = await orderCtrl.List("166666");
            Order[] orders = ((IEnumerable<Order>)result.Model).ToArray();

            Assert.AreEqual(0, orders.Length);
        }

        [TestMethod]
        public async Task Can_Process_Order()
        {
            Cart cart = new Cart();
            cart.Add(new Book { BookID = 12, Name = "Harry Potter" }, 1);
            cart.Add(new Book { BookID = 16, Name = "King of the Ring" }, 1);
            OrderController orderCtrl = new OrderController(orderRepo);
            ViewResult result = await orderCtrl.Process(cart, 1);

            Assert.AreEqual(0, cart.Items.Count());
            mockContext.Verify(x => x.SaveChangesAsync(), Times.Once);
            orderMockSet.Verify(x => x.Add(It.IsAny<Order>()), Times.Exactly(2));
            Assert.AreEqual("Completed", result.ViewName);
        }

        [TestMethod]
        public async Task Can_Cancel_Order()
        {
            OrderController orderCtrl = new OrderController(orderRepo);
            RedirectToRouteResult result = await orderCtrl.Cancel("123456", 1);
            Order canceledOrder = orderData.First(x => x.OrderID == 1);

            Assert.AreEqual(OrderStatus.Cancelled, canceledOrder.Status);
            mockContext.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.AreEqual("List", result.RouteValues["action"]);
            Assert.AreEqual("123456", result.RouteValues["userId"]);
        }
    }
}
