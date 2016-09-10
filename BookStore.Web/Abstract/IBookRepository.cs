using BookStore.Web.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace BookStore.Web.Abstract
{
    /// <summary>
    /// This is an interface for book repository
    /// </summary>
    public interface IBookRepository : IRepository<Book>
    {
        /// <summary>
        /// This method returns books by their cateogory, title, or both
        /// </summary>
        /// <param name="category">This is a category of books</param>
        /// <param name="title">This is a piece of text in book titles</param>
        /// <returns>An IQueryable<Book> that contains books matching the specified condition(s)</returns>  
        IQueryable<Book> SearchBooks(String category, String title);

        /// <summary>
        /// This gets a list of categories from books in a book repository
        /// </summary>
        /// <returns>A list of book category name strings</returns>
        String[] GetCategories();

        /// <summary>
        /// This gets all books in a repository
        /// </summary>
        /// <returns>A set of books</returns>
        DbSet<Book> GetBooks();
    }
}
