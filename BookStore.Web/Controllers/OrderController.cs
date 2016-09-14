using BookStore.Web.Abstract;
using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web.Controllers
{
    /// <summary>
    /// This is a class for handling orders
    /// </summary>
    [Authorize]
    public class OrderController : Controller
    {
        /// <summary>
        /// This is an order repository
        /// </summary>
        private IOrderRepository orderRepository;

        /// <summary>
        /// This is a constructor of this class
        /// </summary>
        /// <param name="orderRepository">This is an order repository</param>
        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        /// <summary>
        /// This shows a list of orders for a user by using the user's id
        /// </summary>
        /// <param name="userId">This is a user's id</param>
        /// <returns>A ViewResult for showing a list of orders</returns>
        public async Task<ViewResult> List(String userId)
        {
            IEnumerable<Order> orders = await orderRepository.FindOrdersByUserIDAsync(userId);
            ViewBag.userId = userId;
            return View(orders);
        }

        /// <summary>
        /// This allows a user to confirm orders before letting 
        /// the app process the orders
        /// </summary>
        /// <param name="cart">This is a shopping cart containing orders</param>
        /// <param name="addressId">This is an id of user's address</param>
        /// <returns>ViewResult showing user's order</returns>
        public ViewResult Confirm(Cart cart, int addressId)
        {
            ViewBag.addressId = addressId;
            return View(cart);
        }

        /// <summary>
        /// This processes a user's orders
        /// </summary>
        /// <param name="cart">This is a shopping cart containing orders</param>
        /// <param name="addressId">This is an id of user's address</param>
        /// <returns>ViewResult showing a thank you message</returns>
        [HttpPost]
        public async Task<ViewResult> Process(Cart cart, int addressId)
        {
            foreach (LineItem item in cart.Items)
            {
                orderRepository.Add(new Order
                {
                    Quantity = item.Quantity,
                    Date = DateTime.Now,
                    Status = OrderStatus.Received,
                    TotalPrice = item.LineItemPrice,
                    BookID = item.Book.BookID,
                    ShoppingAddressID = addressId
                });
            }

            await orderRepository.Save();
            cart.clear();

            return View("Completed");
        }

        /// <summary>
        /// This allows a user to cancel an order
        /// </summary>
        /// <param name="userId">This is a user's id</param>
        /// <param name="orderId">This is an order's id</param>
        /// <returns>Redirects to the order list view</returns>
        [HttpPost]
        public async Task<RedirectToRouteResult> Cancel(String userId, int orderId)
        {
            Order order = orderRepository.FindById(orderId);
            order.Status = OrderStatus.Cancelled;
            orderRepository.Edit(order);
            await orderRepository.Save();

            return RedirectToAction("List", new { userId = userId });
        }

        /// <summary>
        /// This shows a order's details
        /// </summary>
        /// <param name="orderId">This is an order's id</param>
        /// <returns>ViewResult showing an order's details</returns>
        public ViewResult Detail(int orderId)
        {
            Order order = orderRepository.FindById(orderId);
            return View(order);
        }

        /// <summary>
        /// This release both managed and unmanaged resources
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; 
        /// false to release only unmanaged resources</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                orderRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}