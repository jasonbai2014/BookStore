using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookStore.Web.Models
{
    /// <summary>
    /// This is a class for an order
    /// </summary>
    public class Order
    {
        /// <summary>
        /// This is an order ID
        /// </summary>
        public int OrderID { get; set; }

        /// <summary>
        /// This is number of items in this order
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// This is order date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// This is status of this order
        /// </summary>
        public String Status { get; set; }

        /// <summary>
        /// This is total price of this order
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// This is an ordered book's ID
        /// </summary>
        public int BookID { get; set; }

        /// <summary>
        /// This is a shopping address's ID
        /// </summary>
        public int ShoppingAddressID { get; set; }

        /// <summary>
        /// This is a shopping address for this order
        /// </summary>
        public virtual ShoppingAddress Address { get; set; }

        /// <summary>
        /// This is a book ordered
        /// </summary>
        public virtual Book Book { get; set; }
    }
}