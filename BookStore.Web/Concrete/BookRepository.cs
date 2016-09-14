using BookStore.Web.Abstract;
using BookStore.Web.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;


namespace BookStore.Web.Concrete
{
    /// <summary>
    /// This is a class providing books for this bookstore
    /// </summary>
    public class BookRepository : AbstractRepository<Book>, IBookRepository
    {
        /// <summary>
        /// This is a constructor for this class
        /// </summary>
        /// <param name="dbContext">This is a database context instance</param>
        public BookRepository(StoreDbContext dbContext) : base(dbContext)
        {

        }

        /// <summary>
        /// This adds a book into a repository
        /// </summary>
        /// <param name="book">This is the book</param>
        /// <returns>The book that is added into the repository</returns>
        public override Book Add(Book book)
        {
            return DbContext.Books.Add(book);
        }

        /// <summary>
        /// This deletes a book from a repository
        /// </summary>
        /// <param name="book">This is the book that will be removed</param>
        /// <returns>The deleted book</returns>
        public override Book Delete(Book book)
        {
            return DbContext.Books.Remove(book);
        }

        /// <summary>
        /// This edits a book in a repository
        /// </summary>
        /// <param name="book">This is an edited book</param>
        public override void Edit(Book book)
        {
            DbContext.SetModified(book);
        }

        /// <summary>
        /// This finds a book by its id
        /// </summary>
        /// <param name="id">This is a book's id</param>
        /// <returns>The found book or null</returns>
        public override Book FindById(int id)
        {
            return DbContext.Books.Find(id);
        }

        /// <summary>
        /// This gets all books in a repository
        /// </summary>
        /// <returns>A set of books</returns>
        public DbSet<Book> GetBooks()
        {
            return DbContext.Books;
        }

        /// <summary>
        /// This method returns books by their cateogory, title, or both
        /// </summary>
        /// <param name="category">This is a category of books</param>
        /// <param name="title">This is a piece of text in book titles</param>
        /// <returns>An IQueryable<Book> that contains books matching the specified condition(s)</returns> 
        public IQueryable<Book> SearchBooks(string category, string title)
        {
            IQueryable<Book> selectedBooks = DbContext.Books
                .Where(x => category == null || category == x.Category);

            if (!String.IsNullOrEmpty(title))
            {
                selectedBooks = selectedBooks.Where(x => x.Name.ToLower().Contains(title.ToLower()));
            }

            return selectedBooks.OrderBy(x => x.BookID);
        }

        /// <summary>
        /// This gets a list of categories from books in a book repository
        /// </summary>
        /// <returns>A list of book category name strings</returns>
        public String[] GetCategories()
        {
            return DbContext.Books.Select(x => x.Category).Distinct().OrderBy(x => x).ToArray();
        }
    }
}