using onlineBookStore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace onlineBookStore.DAO.Interfaces
{
    public interface IBookDAO
    {
        List<Book> GetBooksByGenre(int genreId);
        List<Book> SearchBooksByTitle(string keyword);
        Book GetBookById(int bookId);
        List<Book> GetAllBooks();
        bool UpdateBookStock(int bookId, int newStock); // Admin
    }
}
