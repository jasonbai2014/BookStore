using BookStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Web.Abstract
{
    /// <summary>
    /// This interface defines a property required by a book repository
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// This gets a list of books
        /// </summary>
        IEnumerable<Book> Books { get; }
    }
}
