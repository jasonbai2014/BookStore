﻿using BookStore.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BookStore.Web.Concrete
{
    /// <summary>
    /// This is a database context class for this bookstore
    /// </summary>
    public class StoreDbContext : IdentityDbContext<User>
    {
        /// <summary>
        /// This is a constructor of this class
        /// </summary>
        public StoreDbContext() : base("StoreDbContext")
        {

        }

        /// <summary>
        /// This returns a set of books from a database for this bookstore
        /// </summary>
        public virtual DbSet<Book> Books { get; set; }

        /// <summary>
        /// This returns a set of reviews for the books 
        /// </summary>
        public virtual DbSet<Review> Reviews { get; set; }

        /// <summary>
        /// This returns a set of orders for the books
        /// </summary>
        public virtual DbSet<Order> Orders { get; set; }

        /// <summary>
        /// This returns a set of shopping addresses for users
        /// </summary>
        public virtual DbSet<ShoppingAddress> Addresses { get; set; }

        /// <summary>
        /// This edits an entity in a database
        /// </summary>
        /// <param name="entity">This is an edited entity</param>
        public virtual void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// This creates a bookstore dbcontext instance
        /// </summary>
        /// <returns>A dbcontext instance</returns>
        public static StoreDbContext Create()
        {
            return new StoreDbContext();
        }

    }
}