using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Web.Concrete;
using System.Linq;
using BookStore.Web.Models;
using System.Collections.Generic;
using System.Data.Entity;
using Moq;
using System.Data.Entity.Infrastructure;
using BookStore.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.UnitTests.Controller
{
    [TestClass]
    public class AddressControllerTest
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

            mockSet.Setup(m => m.Find(It.IsAny<object[]>())).
                Returns<object[]>(x => data.FirstOrDefault(b => b.ShoppingAddressID == (int)x[0]));

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
        public async Task Can_Show_Existing_Addresses()
        {
            AddressController addressCtrl = new AddressController(addressRepo);
            ViewResult result = (await addressCtrl.List("#421521")) as ViewResult;

            Assert.IsNotNull(result);

            ShoppingAddress[] addresses = ((IEnumerable <ShoppingAddress>) result.Model).ToArray();

            Assert.AreEqual(3, addresses.Length);
            Assert.AreEqual(1, addresses[0].ShoppingAddressID);
            Assert.AreEqual(4, addresses[1].ShoppingAddressID);
            Assert.AreEqual(6, addresses[2].ShoppingAddressID);
        }

        [TestMethod]
        public async Task Can_Redirect_When_No_Exisiting_Addresses()
        {
            AddressController addressCtrl = new AddressController(addressRepo);
            RedirectToRouteResult result = await addressCtrl.List("#111111") as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Add", result.RouteValues["action"]);
        }

        [TestMethod]
        public async Task Can_Add_Valid_Address()
        {
            AddressController addressCtrl = new AddressController(addressRepo);
            RedirectToRouteResult result = await addressCtrl.Add(new ShoppingAddress
            {
                ShoppingAddressID = 12,
                UserID = "#939521",
                Line1 = "Pine Rd",
                City = "New York",
                State = "NY",
                Country = "US",
                Zip = "77823"
            }) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Confirm", result.RouteValues["action"]);
            Assert.AreEqual("Order", result.RouteValues["controller"]);
            Assert.AreEqual(12, result.RouteValues["addressId"]);
        }

        [TestMethod]
        public async Task Cannot_Add_Invalid_Address()
        {
            AddressController addressCtrl = new AddressController(addressRepo);
            addressCtrl.ModelState.AddModelError("error", "error message");
            ViewResult result = await addressCtrl.Add(new ShoppingAddress
            {
                ShoppingAddressID = 11,
                UserID = "#939521",
                Line1 = "Pine Rd"
            }) as ViewResult;

            Assert.IsNotNull(result);

            ShoppingAddress address = (ShoppingAddress)result.Model;

            Assert.AreEqual(11, address.ShoppingAddressID);
        }

        [TestMethod]
        public async Task Can_Delete_Address()
        {
            AddressController addressCtrl = new AddressController(addressRepo);
            RedirectToRouteResult result = (RedirectToRouteResult) await addressCtrl.DeleteAddress(4);

            Assert.AreEqual("List", result.RouteValues["action"]);
            Assert.AreEqual("#421521", result.RouteValues["userId"]);
        }
    }
}
