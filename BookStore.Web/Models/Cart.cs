using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Web.Models
{
    /// <summary>
    /// This is a class for a shipping cart
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// This is a list of line items in this shopping cart
        /// </summary>
        private List<LineItem> items;

        public List<LineItem> Items { get { return items; } }

        /// <summary>
        /// This is a constructor of this class
        /// </summary>
        public Cart()
        {
            items = new List<LineItem>();
        }

        /// <summary>
        /// This adds an item into this cart
        /// </summary>
        /// <param name="book">This is the book ordered</param>
        /// <param name="quantity">This is number of the book ordered</param>
        public void Add(Book book, int quantity)
        {
            // checks whether or not the book has been ordered
            LineItem item = items.SingleOrDefault(x => x.Book.BookID == book.BookID);

            if (item == null)
            {
                items.Add(new LineItem() { Book = book, Quantity = quantity });
            } else
            {
                // increments the quantity if the item is already in the cart
                item.Quantity += quantity;
            } 
        }

        /// <summary>
        /// This removes a line item from this shopping cart
        /// </summary>
        /// <param name="book">This is the book customer wants to remove from the cart</param>
        public void Revome(Book book)
        {
            items.RemoveAll(x => x.Book.BookID == book.BookID);
        }

        /// <summary>
        /// This removes all the line items from this cart
        /// </summary>
        public void clear()
        {
            items.Clear();
        }

        /// <summary>
        /// This calculates total price of the items in this cart
        /// </summary>
        /// <returns>total price</returns>
        public decimal CalculateTotal()
        {
            decimal total = 0;

            foreach(LineItem item in items)
            {
                total += item.Book.Price * item.Quantity;
            }

            return total;
        }
    }

    /// <summary>
    /// This is a class for a line item
    /// </summary>
    public class LineItem
    {
        /// <summary>
        /// This is the ordered book
        /// </summary>
        public Book Book { get; set; }

        /// <summary>
        /// This is number of the book ordered
        /// </summary>
        public int Quantity { get; set; }
    }
}